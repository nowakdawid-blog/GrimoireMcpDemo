using Anthropic;
using Anthropic.Core;
using Microsoft.Agents.AI.DevUI;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;

var builder = WebApplication.CreateBuilder(args);

var apiKey = builder.Configuration["Anthropic:ApiKey"]
    ?? throw new InvalidOperationException(
        "No API key. Set it by: dotnet user-secrets set \"Anthropic:ApiKey\" \"<your-key>\"");

var model = builder.Configuration["Anthropic:Model"] ?? "claude-haiku-4-5-20251001";
var grimoireUrl = builder.Configuration["Grimoire:Url"] ?? "http://localhost:5090/mcp";

IChatClient chatClient = new AnthropicClient(new ClientOptions { ApiKey = apiKey })
    .AsIChatClient(model);

builder.Services.AddChatClient(chatClient);

var grimoire = await McpClient.CreateAsync(
    new HttpClientTransport(new HttpClientTransportOptions
    {
        Name = "Grimoire",
        Endpoint = new Uri(grimoireUrl),
    }));

var spells = await grimoire.ListToolsAsync();

const string systemPrompt = """
    You are a novice mage, a young apprentice assistant. You are helpful, polite,
    and a little nervous. Always answer using simple, plain English. Use short
    sentences and easy words. Avoid fancy or complex language.
    You can only cast spells that are written in your grimoire.
    """;

builder.AddAIAgent("NoviceMage", systemPrompt)
    .WithAITools([.. spells]);

builder.Services.AddOpenAIResponses();
builder.Services.AddOpenAIConversations();
builder.Services.AddDevUI();

var app = builder.Build();

app.MapOpenAIResponses();
app.MapOpenAIConversations();

if (app.Environment.IsDevelopment())
{
    app.MapDevUI();
}

app.Run();
