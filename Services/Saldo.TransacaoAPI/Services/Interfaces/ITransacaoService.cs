using Saldo.TransacaoAPI.Entities;

namespace Saldo.TransacaoAPI.Services.Interfaces;

public interface ITransacaoService
{
    Task<IEnumerable<Transacao>> GetTransacoesByUserId(int userid);
    Task CreateTransacao(Transacao transacao);
    Task SaldoRecargaTotal(int userid);

}
