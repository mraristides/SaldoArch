using Saldo.SaldoAPI.Entities;
using MongoDB.Driver;

namespace Saldo.SaldoAPI.Data.Interfaces;

public interface ITransacaoContext
{
    IMongoCollection<Transacao> Transacoes { get; }
}
