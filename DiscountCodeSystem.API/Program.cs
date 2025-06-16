using DiscountCodeSystem.API.Extensions;
using DiscountCodeSystem.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();

builder.Services.AddLogging();

var app = builder.Build();



app.MapGrpcService<DiscountCodeGrpcService>();


app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. ");

app.Run();
