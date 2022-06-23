using System.Collections.Concurrent;
using SqlSugar;
using HttpServer.Models;
using HttpServer.Exceptions;

namespace HttpServer;

public class SignManager
{
    private IFreeSql db;
    private ConcurrentDictionary<string, long> tokens;
    private ConcurrentDictionary<string, SignProof> signs;

    public SignManager(IFreeSql db)
    {
        this.db = db;
        tokens = new ConcurrentDictionary<string, long>();
        signs = new ConcurrentDictionary<string, SignProof>();
    }

    public SignProof this[string key]
    {
        get => signs[key];
        set => signs[key] = value;
    }

    public async Task<string> Ack(string account)
    {
        var app = await db.Select<FLogApp>()
            .Where(i => i.Account == account)
            .ToOneAsync();
        if (app == null)
        {
            throw new SignException($"invalid account: {account}");
        }
        var token = Guid.NewGuid().ToString("N");
    }

    public async Task<SignProof> Sign(string account, string signature)
    {
        var app = await db.Select<FLogApp>()
            .Where(i => i.Account == account)
            .ToOneAsync();
        if (app == null)
        {
            throw new SignException($"invalid account: {account}");
        }



        return null;
    }
}
