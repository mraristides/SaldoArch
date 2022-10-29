using Saldo.TransacaoAPI.Entities;
using MongoDB.Driver;

namespace Saldo.TransacaoAPI.Data;

public class TransacaoContextSeed
{

    public static void SeedData(IMongoCollection<Transacao> transacaoCollection)
    {
        bool existTransacao = transacaoCollection.Find(p => true).Any();
        if (!existTransacao)
        {
            transacaoCollection.InsertManyAsync(GetMyTransacoes());
        }
    }

    public static IEnumerable<Transacao> GetMyTransacoes()
    {
        return new List<Transacao> ()
        {
            new Transacao() {
                id = Guid.NewGuid().ToString(),
                userid = 1,
                date = DateTime.Now,
                value = 100
            },
            new Transacao() {
                id = Guid.NewGuid().ToString(),
                userid = 1,
                date = DateTime.Now,
                value = -50
            },
            new Transacao() {
                id = Guid.NewGuid().ToString(),
                userid = 1,
                date = DateTime.Now,
                value = +33.15
            },
            new Transacao() {
                id = Guid.NewGuid().ToString(),
                userid = 1,
                date = DateTime.Now,
                value = -96.36
            },
            new Transacao() {
                id = Guid.NewGuid().ToString(),
                userid = 1,
                date = DateTime.Now,
                value = 150
            },
            new Transacao() {
                id = Guid.NewGuid().ToString(),
                userid = 1,
                date = DateTime.Now,
                value = -30
            },
        };
    }


}
