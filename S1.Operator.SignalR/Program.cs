using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using RandomNameGeneratorLibrary;
using S1.Operator.SignalR.Hubs;
using S1.Operator.SignalR.Services;
using S1.Operator.SignalR.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseSentry();
builder.Services.AddSignalR();
builder.Services.AddSingleton<LastBetHub>();
builder.Services.AddHostedService<GenerateRoundWorker>();
builder.Services.AddSingleton<IPersonNameGenerator, PersonNameGenerator>();
builder.Services.AddSingleton<IGenerateName, GenerateName>();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddCors();

var excludeExtensions = new[] { "html", "ico", "png", "jpg" };
builder.Services
    .AddOpenTelemetry()
    .ConfigureResource(_ => _.AddService(builder.Configuration.GetValue<string>("Otel:ServiceName")))
    .WithTracing(tracerProviderBuilder => tracerProviderBuilder
        .AddSource("Tgs.*")
        .AddSource("TGS.*")
        .AddSource("Valhalla.*")
        .AddAspNetCoreInstrumentation(options => options.Filter =
            httpContext => !httpContext.Request.Path.HasValue
                           || !httpContext.Request.Path.StartsWithSegments("/swagger")
                           && !httpContext.Request.Path.StartsWithSegments("/health")
                           && !httpContext.Request.Path.StartsWithSegments("/profiler/")
                           && !httpContext.Request.Path.StartsWithSegments("/metrics")
                           && !excludeExtensions.Any(
                               ext => httpContext.Request.Path.Value.EndsWith($".{ext}")))
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation(options => options.SetDbStatementForText = true)
        .AddProfileViewExporter()
        .AddOtlpExporter(options => options.Endpoint = builder.Configuration.GetValue<Uri>("Otel:Collector")))
    .WithMetrics(meterBuiler => meterBuiler
        .SetResourceBuilder(ResourceBuilder.CreateDefault()
            .AddEnvironmentVariableDetector())
        .AddRuntimeInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter())
    .Services
    .AddProfileViewer();


var app = builder.Build();

app.UseCors(x =>
        x.AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials());

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseRouting();
app.MapHub<LastBetHub>("/last-bet-hub");
app.UseHealthChecks("/health");

app.Run();
