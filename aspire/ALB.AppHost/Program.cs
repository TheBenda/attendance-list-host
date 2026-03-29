var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgWeb()
    .WithLifetime(ContainerLifetime.Persistent);

var postgresdb = postgres.AddDatabase("postgresdb");

var migrationService = builder.AddProject<Projects.ALB_MigrationService>("MigrationService")
    .WithReference(postgresdb)
    .WaitFor(postgresdb);

var api = builder.AddProject<Projects.ALB_Api>("Api")
    .WithReference(postgresdb)
    .WaitFor(postgresdb)
    .WaitFor(migrationService);

var viteApp = builder.AddViteApp("vite-app", "../../../attendance-list-frontend/")
    .WithReference(api);

api.WithReference(viteApp)
    .WaitFor(viteApp);

var gateway = builder.AddYarp("gateway") 
    .WithConfiguration(yarp =>
    {
        yarp.AddRoute(viteApp);
        yarp.AddRoute("/api/{**catch-all}", api);
    });

builder.Build().Run();