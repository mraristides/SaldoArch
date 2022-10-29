using Microsoft.AspNetCore.Mvc;
using Saldo.TransacaoAPI.Data;
using Saldo.TransacaoAPI.Repositories.Interfaces;
using Saldo.TransacaoAPI.Services.Interfaces;
using Saldo.TransacaoAPI.Entities;
using Polly;


namespace Saldo.TransacaoAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class TransacaoController : ControllerBase
{
    private readonly ITransacaoService _service;
    private readonly ILogger<TransacaoController> _logger;

    public TransacaoController(ITransacaoService service, ILogger<TransacaoController> logger)
    {
            this._logger = logger;
            this._service = service;
    }

    [Route("[action]/{userid}", Name = "GetTransacoesByUserId")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Transacao>))]
    public async Task<ActionResult<IEnumerable<Transacao>>> GetTransacoesByUserId(int userid)
    {
        var transacoes = await _service.GetTransacoesByUserId(userid);
        if (transacoes is null)
        {
            return NotFound();
        }
        return Ok(transacoes);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CreateTransacao([FromBody] Transacao transacao)
    {
        this._logger.LogInformation("endpoint POST -> Transacao.Post()");

        if (transacao is null)
        {
            this._logger.LogInformation("endpoint POST -> Transacao.Post() -> error transacao is null");
            return BadRequest();
        }

        var ExecutePolicyRetry = await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(5))
            .ExecuteAsync(async () => { await _service.CreateTransacao(transacao); } )
            .ContinueWith(t => t)
            .ConfigureAwait(false);

        if (ExecutePolicyRetry.IsCompletedSuccessfully) {
            this._logger.LogInformation("endpoint POST -> Transacao.Post() -> IsCompletedSuccessfully");
            return RedirectToAction(actionName: "SaldoRecargaTotal", new { userid = transacao.userid });
        } else {
            this._logger.LogError("endpoint POST -> Transacao.Post() -> Error/Exception");
            this._logger.LogError(ExecutePolicyRetry.Exception.Message);
            return BadRequest();
        }

    }

    
    [Route("[action]/{userid}", Name = "SaldoRecargaTotal")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SaldoTotal))]
    public async Task<ActionResult> SaldoRecargaTotal(int userid)
    {
        this._logger.LogInformation("endpoint GET -> Transacao.SaldoRecargaTotal()");

        var ExecutePolicyRetry = await Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(3))
            .ExecuteAsync(async () => { await _service.SaldoRecargaTotal(userid); } )
            .ContinueWith(t => t)
            .ConfigureAwait(false);

        if (ExecutePolicyRetry.IsCompletedSuccessfully) {
            this._logger.LogInformation("endpoint GET -> Transacao.SaldoRecargaTotal() -> IsCompletedSuccessfully");
            return Ok();
        } else {
            this._logger.LogError("endpoint GET -> Transacao.SaldoRecargaTotal() -> Error/Exception");
            this._logger.LogError(ExecutePolicyRetry.Exception.Message);
            return BadRequest();
        }
    }

}
