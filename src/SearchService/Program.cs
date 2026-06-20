using System.Net;
using Polly;
using Polly.Extensions.Http;
using SearchService.Datas;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<AuctionSvcHttpClients>().AddPolicyHandler(GetPolicy());
var app = builder.Build();

app.UseAuthorization();
app.MapControllers();

try
{
    await DbInitializer.InintDb(app);
}
catch (Exception ex)
{
    Console.WriteLine($"Error initializing database: {ex.Message}");
}
app.Run();
static IAsyncPolicy<HttpResponseMessage> GetPolicy()
     => HttpPolicyExtensions.HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
