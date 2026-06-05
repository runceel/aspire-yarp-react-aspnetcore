var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add YARP reverse proxy and resolve cluster destinations through Aspire service discovery.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

var app = builder.Build();

app.MapDefaultEndpoints();

// Authentication/authorization middleware can be added here in the future,
// before the request reaches the reverse proxy pipeline.

app.MapReverseProxy();

app.Run();
