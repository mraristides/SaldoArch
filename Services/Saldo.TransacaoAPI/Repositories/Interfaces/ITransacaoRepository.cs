using Saldo.TransacaoAPI.Entities;

namespace Saldo.TransacaoAPI.Repositories.Interfaces;

public interface ITransacaoRepository
{
    Task<IEnumerable<Transacao>> GetTransacoesByUserId(int userid);
    Task CreateTransacao(Transacao transacao);
    Task SaldoRecargaTotal(int userid, IEnumerable<Transacao> transacoes);

}
