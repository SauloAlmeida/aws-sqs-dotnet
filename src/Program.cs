using AWSSQSDotnet.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IQueueService, QueueService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapPost("/publish", async ([FromBody] MessageInput input, IQueueService queue) =>
{
    await queue.Pub(input.ToString());

    return Results.NoContent();
});

app.Run();

public class MessageInput
{
    public string Message { get; set; }

    public override string ToString() => JsonConvert.SerializeObject(this);
}