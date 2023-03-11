using AWSSQSDotnet.DTO;
using AWSSQSDotnet.Service;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IQueueService, QueueService>();
builder.Services.AddHostedService<ConsumerBackgroundService>();

var app = builder.Build();

app.MapPost("/publish", async ([FromBody] ModelInput input, IQueueService queue) =>
{
    await queue.PublishAsync(input.ToString());

    return Results.NoContent();
});

app.Run();