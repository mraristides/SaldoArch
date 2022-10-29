using Saldo.SaldoAPI.Entities;
using Saldo.SaldoAPI.Data.Interfaces;
using MongoDB.Driver;

namespace Saldo.SaldoAPI.Data;

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
    }

    public IMongoCollection<Transacao> Transacoes { get; }
}
