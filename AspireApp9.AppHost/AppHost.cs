var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddProject<Projects.AspireApp9_Server>("server")
    .WithHttpHealthCheck("/health")
    .WithUrls(context => context.Urls.Clear());

var webfrontend = builder.AddViteApp("webfrontend", "../frontend")
    .WithReference(server)
    .WaitFor(server)
    .WithUrls(context => context.Urls.Clear());

var yarp = builder.AddProject<Projects.AspireApp9_Yarp>("yarp")
    .WithReference(server)
    .WithReference(webfrontend)
    .WaitFor(server)
    .WaitFor(webfrontend)
    .WithExternalHttpEndpoints();

server.PublishWithContainerFiles(webfrontend, "wwwroot");

builder.Build().Run();
