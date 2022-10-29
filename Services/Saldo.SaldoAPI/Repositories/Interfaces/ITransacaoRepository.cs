using Saldo.SaldoAPI.Entities;

namespace Saldo.SaldoAPI.Repositories.Interfaces;

public interface ITransacaoRepository
{
    Task<IEnumerable<Transacao>> GetTransacoesByUserId(int userid);
    Task<SaldoTotal> SaldoRecargaTotal(int userid, IEnumerable<Transacao> transacoes);
}
