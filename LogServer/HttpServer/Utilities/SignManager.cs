using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using SqlSugar;
using HttpServer.Models;
using HttpServer.Exceptions;

namespace HttpServer.Utilities;

public class SignManager
{
    private IFreeSql db;
    private ILogger<SignManager> logger;
    private ConcurrentDictionary<string, SignToken> tokens;
    private ConcurrentDictionary<string, SignProof> signs;

    public int TokenCount { get => tokens.Count; }
    public int SignCount { get => signs.Count; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public SignManager(IFreeSql db, ILogger<SignManager> logger)
    {
        this.db = db;
        this.logger=logger;
        tokens = new ConcurrentDictionary<string, SignToken>();
        signs = new ConcurrentDictionary<string, SignProof>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public SignProof this[string key]
    {
        get => signs[key];
        set => signs[key] = value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    /// <exception cref="SignException"></exception>
    public async Task<string> Ack(string account)
    {
        var app = await db.Select<FLogApp>()
            .Where(i => i.Account == account)
            .ToOneAsync();
        if (app == null)
        {
            throw new SignException($"invalid account: {account}");
        }
        var head = Guid.NewGuid().ToString("N");
        var tail = new byte[32];
        Random.Shared.NextBytes(tail);
        var token = $"{head}-{Convert.ToHexString(tail)}";
        var signature = Encrypt(app.Secret, token);
        tokens[token] = new SignToken
        {
            AppId = app.Id,
            CreateAt = DateTime.Now,
            ExpireAt = DateTime.Now.AddMinutes(1),
        };
        return signature;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <param name="signature"></param>
    /// <returns></returns>
    /// <exception cref="SignException"></exception>
    public async Task<string> Sign(string token)
    {
        if (!tokens.ContainsKey(token))
        {
            throw new SignException($"invalid token: {token}");
        }
        var tinfo = tokens[token];
        var secret = new byte[32];
        Random.Shared.NextBytes(secret);
        await Task.CompletedTask;
        var head = Guid.NewGuid().ToString("N");
        var tail = new byte[32];
        Random.Shared.NextBytes(tail);
        var proof = $"{head}-{Convert.ToHexString(tail)}";
        signs[proof] = new SignProof
        {
            AppId = tinfo.AppId,
            CreateAt = DateTime.Now,
            ExpireAt = DateTime.Now.AddHours(24),
            Secret = Convert.ToHexString(secret),
        };
        return proof;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int ClearTokens()
    {
        var now = DateTime.Now;
        var keys = tokens
            .Where(t => t.Value.ExpireAt <= now)
            .Select(t => t.Key)
            .ToArray();

        // logger.LogInformation($"ttttt: {keys.GetType().FullName} => {keys.Count()}");

        SignToken? token;
        foreach (var key in keys)
        {
            tokens.Remove(key, out token);
        }
        return keys.Length;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int ClearSigns()
    {
        var now = DateTime.Now;
        var keys = signs
            .Where(s => s.Value.ExpireAt <= now)
            .Select(s => s.Key)
            .ToArray();

        SignProof? proof;
        foreach (var key in keys)
        {
            signs.Remove(key, out proof);
        }

        return keys.Length;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="db"></param>
    public static void Init(IFreeSql db)
    {
        var ac = db.Queryable<FLogApp>().Count();
        System.Diagnostics.Debug.WriteLine($"sign manager init: {ac}");
        if (ac == 0)
        {
            var sercet = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes("123456789")));
            db.Insert(new FLogApp
            {
                Account = "tester",
                Secret = sercet,
                CreateAt = DateTime.Now,
            }).ExecuteIdentity();
            System.Diagnostics.Debug.WriteLine($"sign manager insert: {sercet}");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="iv"></param>
    /// <returns></returns>
    private static Aes NewAes(byte[] key, byte[] iv)
    {
        var aes = Aes.Create();
        aes.IV = iv;
        aes.Key = key;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7;
        aes.KeySize = 256;
        return aes;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="raw"></param>
    /// <returns></returns>
    public static string Encrypt(string key, string raw)
    {
        var k = Convert.FromHexString(key);
        var iv = new byte[16];
        Random.Shared.NextBytes(iv);
        using var aes = NewAes(k, iv);
        using var encryptor = aes.CreateEncryptor();
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using var writer = new StreamWriter(cs);
        writer.Write(raw);
        var encrypted = ms.ToArray();
        var hash = HMACSHA256.HashData(k, encrypted);
        var result = new byte[iv.Length + hash.Length + encrypted.Length];
        Array.Copy(iv, 0, result, 0, iv.Length);
        Array.Copy(hash, 0, result, iv.Length, hash.Length);
        Array.Copy(encrypted, 0, result, iv.Length +  hash.Length, encrypted.Length);
        return Convert.ToBase64String(result);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="raw"></param>
    /// <returns></returns>
    public static string Decrypt(string key, string raw)
    {
        var k = Convert.FromHexString(key);
        var buffer = Convert.FromBase64String(raw);
        var iv = buffer.Take(16).ToArray();
        var hash = buffer.Skip(16).Take(32).ToArray();
        var encrypted = buffer.Skip(iv.Length + hash.Length).ToArray();
        var vhash = HMACSHA256.HashData(k, encrypted);
        if (!hash.SequenceEqual(vhash))
        {
            throw new Exception("散列校验错误，数据被篡改。");
        }
        var aes = NewAes(k, iv);
        using var decryptor = aes.CreateDecryptor();
        using var ms = new MemoryStream(encrypted);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var reader = new StreamReader(cs);
        var plaintext = reader.ReadToEnd();
        return plaintext;
    }
}
