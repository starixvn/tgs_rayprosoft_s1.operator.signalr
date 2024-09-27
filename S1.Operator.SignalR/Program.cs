using RandomNameGeneratorLibrary;
using S1.Operator.SignalR.Hubs;
using S1.Operator.SignalR.Workers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddSingleton<LastBetHub>();
builder.Services.AddHostedService<GenerateRoundWorker>();
builder.Services.AddSingleton<IPersonNameGenerator, PersonNameGenerator>();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
builder.Services.AddCors();


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
