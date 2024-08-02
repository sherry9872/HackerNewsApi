using HackerNewsApi.Services; // Make sure this namespace is included
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection; 

var builder = WebApplication.CreateBuilder(args);

// Configure HTTP client
builder.Services.AddHttpClient<HackerNewsService>(client =>
{
    client.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/");
});

// Register services
builder.Services.AddScoped<IHackerNewsService, HackerNewsService>();

// Add other services
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();