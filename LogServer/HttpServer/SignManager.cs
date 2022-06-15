using System.Collections.Concurrent;
using SqlSugar;
using HttpServer.Models;

namespace HttpServer;

public class SignManager
{
    private SqlSugarScope db;
    private ConcurrentDictionary<string, SignProof> signs;

    public SignManager(SqlSugarScope db)
    {
        this.db = db;
        signs = new ConcurrentDictionary<string, SignProof>();
    }

    public SignProof this[string key]
    {
        get => signs[key];
        set => signs[key] = value;
    }

    public async Task<SignProof> Sign(string appkey)
    {
        var app = await db.Queryable<MainLogApp>()
            .Where(i => i.AppKey == appkey)
            .FirstAsync();
        if (app == null)
        {
        }



        return null;
    }
}
