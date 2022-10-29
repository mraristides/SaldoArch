using Saldo.SaldoAPI.Data.Interfaces;
using Saldo.SaldoAPI.Entities;
using Saldo.SaldoAPI.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Saldo.SaldoAPI.Repositories;

public class SaldoRepository : ISaldoRepository
{
    private readonly IDistributedCache _distributedCache;
    private readonly ITransacaoRepository _transacaoRepository;
    private readonly IMemoryCache _memoryCache;
    public SaldoRepository(IDistributedCache distributedCache, ITransacaoRepository transacaoRepository, IMemoryCache memoryCache)
    {
        this._distributedCache = distributedCache;
        this._transacaoRepository = transacaoRepository;
        this._memoryCache = memoryCache;
    }

    public async Task<SaldoTotal> GetRedis(int userid)
    {
        var cacheKey = $"userid-{userid}-saldo";

        var saldoRedis = await _distributedCache.GetStringAsync(cacheKey);
        if (!String.IsNullOrEmpty(saldoRedis)) {
            var saldo = JsonConvert.DeserializeObject<SaldoTotal>(saldoRedis);
            if (saldo != null) {
                Console.WriteLine("Peguei do Redis");
                return saldo;
            }
        } 

        var transacoes = await _transacaoRepository.GetTransacoesByUserId(userid);
        Console.WriteLine("Peguei do Mongo");
        return await _transacaoRepository.SaldoRecargaTotal(userid, transacoes);
    }

    public async Task<SaldoTotal> GetCache(int userid)
    {
        var cacheKey = $"userid-{userid}-saldo";

        SaldoTotal? saldoCache = _memoryCache.Get<SaldoTotal>(cacheKey);
        if (saldoCache is not null) {
            Console.WriteLine("Peguei do Cache 20 Segundos Expira");
            return saldoCache;
        } else {
            var saldoRedis = await GetRedis(userid);
            saldoCache = _memoryCache.GetOrCreate<SaldoTotal>(cacheKey, context => 
            {
                context.SetAbsoluteExpiration(TimeSpan.FromSeconds(20));
                context.SetPriority(CacheItemPriority.High);
                return saldoRedis;
            });
            return saldoRedis;
        }
    }
}
