using Saldo.SaldoAPI.Data;
using Saldo.SaldoAPI.Data.Interfaces;
using Saldo.SaldoAPI.Repositories;
using Saldo.SaldoAPI.Repositories.Interfaces;
using Saldo.SaldoAPI.Services;
using Saldo.SaldoAPI.Services.Interfaces;
using Microsoft.Extensions.Http;
using Serilog;
using System.Net.Http.Headers;

namespace Saldo.SaldoAPI;
public class Startup : IStartup
{
    public IConfiguration Configuration { get; }
    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services) 
    {
        
        services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>
            ("RedisSettings:ConnectionString");
            }
        );
        
        services.AddScoped<ITransacaoContext, TransacaoContext>();
        services.AddScoped<ITransacaoRepository, TransacaoRepository>();
        services.AddScoped<ISaldoRepository, SaldoRepository>();
        services.AddScoped<ISaldoService, SaldoService>();

        services.AddCors(options => options.AddDefaultPolicy(builder => {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

        services.AddControllers();

        services.AddMvc(options => {
            options.RespectBrowserAcceptHeader = true;
            options.FormatterMappings.SetMediaTypeMappingForFormat("json","application/json"); 
        });
        services.AddMemoryCache();
        

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(WebApplication app, IWebHostEnvironment environment)
    {
        if (app.Environment.IsDevelopment())
        {
            
        }
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }

}

public static class StartupExtensions
{
    public static WebApplicationBuilder UserStartup<TStartup>(this WebApplicationBuilder WebAppBuilder) where TStartup : IStartup
    {
        var startup = Activator.CreateInstance(typeof(TStartup), WebAppBuilder.Configuration) as IStartup;

        if (startup == null) throw new ArgumentException("Classe Startup.cs inválida");

        Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .ReadFrom.Configuration(WebAppBuilder.Configuration)
                .CreateLogger();
        WebAppBuilder.Host.UseSerilog(Log.Logger);
        
        try
        {
            Log.Information("Inicializando microserviço");
            startup.ConfigureServices(WebAppBuilder.Services);
            var app = WebAppBuilder.Build();
            startup.Configure(app, app.Environment);
            app.Run();

        }   
        catch (Exception ex)
        {
            Log.Fatal(ex, "A Aplicação falhou a iniciar");
        }
        finally
        {
            Log.CloseAndFlush();
        }        

        return WebAppBuilder;
    }

}
