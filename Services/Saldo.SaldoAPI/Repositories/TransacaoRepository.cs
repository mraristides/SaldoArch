using Saldo.SaldoAPI.Data.Interfaces;
using Saldo.SaldoAPI.Entities;
using Saldo.SaldoAPI.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using System.Text;
using Newtonsoft.Json;

namespace Saldo.SaldoAPI.Repositories;

public class TransacaoRepository : ITransacaoRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly ITransacaoContext _context;
    public TransacaoRepository(ITransacaoContext context,IDistributedCache distributedCache)
    {
        this._distributedCache = distributedCache;
        this._context = context;
    }

    public async Task<IEnumerable<Transacao>> GetTransacoesByUserId(int userid)
    {
        FilterDefinition<Transacao> filter = Builders<Transacao>.Filter
            .Eq(p => p.userid,userid);

        return await _context.Transacoes.Find(filter).ToListAsync();
    }

    public async Task<SaldoTotal> SaldoRecargaTotal(int userid, IEnumerable<Transacao> transacoes)
    {
        double saldo = 0;
        if (transacoes != null)
        {
            foreach (Transacao transacao in transacoes)
            {
                saldo += transacao.value;
            }
        }

        var cacheKey = $"userid-{userid}-saldo";
        var userNewSaldo = new SaldoTotal()
        {
            userid = userid, 
            saldo = saldo 
        };
        
        string serializedSaldo = JsonConvert.SerializeObject(userNewSaldo);
        await _distributedCache.SetStringAsync(cacheKey, serializedSaldo);

        return userNewSaldo;
    }
}
