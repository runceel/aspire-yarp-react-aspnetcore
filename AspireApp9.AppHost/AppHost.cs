var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddProject<Projects.AspireApp9_Server>("server")
    .WithHttpHealthCheck("/health")
    .WithHttpEndpoint(port: 5302, isProxied: false)
    .WithHttpsEndpoint(port: 7419, isProxied: false);

var webfrontend = builder.AddViteApp("webfrontend", "../frontend")
    .WithReference(server)
    .WaitFor(server)
    .WithHttpEndpoint(port: 5173, isProxied: false);

var yarp = builder.AddProject<Projects.AspireApp9_Yarp>("yarp")
    .WithReference(server)
    .WithReference(webfrontend)
    .WaitFor(server)
    .WaitFor(webfrontend)
    .WithExternalHttpEndpoints();

server.PublishWithContainerFiles(webfrontend, "wwwroot");

builder.Build().Run();
