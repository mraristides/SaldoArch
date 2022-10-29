using Saldo.TransacaoAPI.Services.Interfaces;
using Saldo.TransacaoAPI.Repositories.Interfaces;
using Saldo.TransacaoAPI.Entities;
using System.Text;


namespace Saldo.TransacaoAPI.Services;

public class TransacaoService : ITransacaoService
{
    private readonly ITransacaoRepository _repository;
    public TransacaoService(ITransacaoRepository repository)
    {
        this._repository = repository;
    }

    public async Task<IEnumerable<Transacao>> GetTransacoesByUserId(int userid)
    {
        return await _repository.GetTransacoesByUserId(userid);
    }

    public async Task CreateTransacao(Transacao transacao)
    {
        await _repository.CreateTransacao(transacao);
    }

    public async Task SaldoRecargaTotal(int userid)
    {
        var transacoes = await _repository.GetTransacoesByUserId(userid);
        await _repository.SaldoRecargaTotal(userid, transacoes);
    }

}
