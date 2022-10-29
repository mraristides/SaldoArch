using Saldo.SaldoAPI.Entities;

namespace Saldo.SaldoAPI.Services.Interfaces;

public interface ISaldoService
{
    Task<SaldoTotal> Get(int userid);
}
