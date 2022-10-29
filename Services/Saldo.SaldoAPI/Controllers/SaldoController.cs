using Microsoft.AspNetCore.Mvc;
using Saldo.SaldoAPI.Data;
using Saldo.SaldoAPI.Repositories.Interfaces;
using Saldo.SaldoAPI.Services.Interfaces;
using Saldo.SaldoAPI.Entities;
using Polly;


namespace Saldo.SaldoAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class SaldoController : ControllerBase
{
    private readonly ISaldoService _service;
    private readonly ILogger<SaldoController> _logger;

    public SaldoController(ISaldoService service, ILogger<SaldoController> logger)
    {
        this._logger = logger;
        this._service = service;
    }

     
    [Route("[action]/{userid}", Name = "Get")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SaldoTotal>))]
    public async Task<ActionResult<IEnumerable<SaldoTotal>>> Get(int userid)
    {
        this._logger.LogInformation("endpoint GET -> Saldo.Get()");
        try
        {
            var Result = await _service.Get(userid); 
            this._logger.LogInformation("endpoint GET -> Saldo.Get() -> IsCompletedSuccessfully");
            return Ok(Result);
        }
        catch (Exception ex)
        {
            this._logger.LogError("endpoint GET -> Transacao.Get() -> Error/Exception");
            this._logger.LogError(ex.Message);
            return BadRequest();
        }
    }

}
