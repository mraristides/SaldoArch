using Saldo.TransacaoAPI.Entities;
using MongoDB.Driver;

namespace Saldo.TransacaoAPI.Data.Interfaces;


public interface ITransacaoContext
{
    IMongoCollection<Transacao> Transacoes { get; }
}
