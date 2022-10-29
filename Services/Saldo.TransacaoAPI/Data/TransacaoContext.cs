using Saldo.TransacaoAPI.Entities;
using Saldo.TransacaoAPI.Data.Interfaces;
using MongoDB.Driver;

namespace Saldo.TransacaoAPI.Data;

public class TransacaoContext : ITransacaoContext
{
    public TransacaoContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>
            ("DatabaseSettings:ConnectionString"));
            
        var database = client.GetDatabase(configuration.GetValue<string>
            ("DatabaseSettings:DatabaseName"));

        Transacoes = database.GetCollection<Transacao>(configuration.GetValue<string>
            ("DatabaseSettings:CollectionName"));

        TransacaoContextSeed.SeedData(Transacoes);
    }

    public IMongoCollection<Transacao> Transacoes { get; }
}
