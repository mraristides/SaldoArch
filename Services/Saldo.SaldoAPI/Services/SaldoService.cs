using Saldo.SaldoAPI.Services.Interfaces;
using Saldo.SaldoAPI.Repositories.Interfaces;
using Saldo.SaldoAPI.Repositories;
using Saldo.SaldoAPI.Entities;
using System.Text;
using Newtonsoft.Json;


namespace Saldo.SaldoAPI.Services;

public class SaldoService : ISaldoService
{
    private readonly ISaldoRepository _repository;
    public SaldoService(ISaldoRepository repository)
    {
        this._repository = repository;
    }

    public async Task<SaldoTotal> Get(int userid)
    {
        return await _repository.GetCache(userid);
    }

}
