using Saldo.SaldoAPI.Entities;

namespace Saldo.SaldoAPI.Repositories.Interfaces;

public interface ISaldoRepository
{
    Task<SaldoTotal> GetRedis(int userid);
    Task<SaldoTotal> GetCache(int userid);
}
