var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

var redis = builder.AddRedis("cache")
    .WithDataVolume();

var authDb = postgres.AddDatabase(name: "auth-db", databaseName: "odyssey_auth_db");
var homestayDb = postgres.AddDatabase(name: "homestay-db", databaseName: "odyssey_homestay_db");
var tripDb = postgres.AddDatabase(name: "trip-db", databaseName: "odyssey_trip_db");
var notifyDb = postgres.AddDatabase(name: "notify-db", databaseName: "odyssey_notify_db");

#region need
//var apiService = builder.AddProject<Projects.Odyssey_ApiService>("apiservice")
//    .WithHttpHealthCheck("/health");

//builder.AddProject<Projects.Odyssey_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithHttpHealthCheck("/health")
//    .WithReference(cache)
//    .WaitFor(cache)
//    .WithReference(apiService)
//    .WaitFor(apiService);
#endregion

var authApi = builder.AddProject<Projects.AuthService_API>("authservice-api")
    .WithReference(authDb)
    .WaitFor(authDb)
    .WithReference(redis)
    .WaitFor(redis)
    .WithHttpHealthCheck("/health");

var homestayApi = builder.AddProject<Projects.HomestayService_API>("homestayservice-api")
    .WithReference(homestayDb)
    .WaitFor(homestayDb)
    .WithReference(redis)
    .WaitFor(redis)
    .WithReference(authApi)
    .WaitFor(authApi)
    .WithHttpHealthCheck("/health");

var tripApi = builder.AddProject<Projects.TripService_API>("tripservice-api")
    .WithReference(tripDb)
    .WaitFor(tripDb)
    .WithReference(redis)
    .WaitFor(redis)
    .WithReference(authApi)
    .WaitFor(authApi)
    .WithHttpHealthCheck("/health");

var aiGuideApi = builder.AddProject<Projects.AIGuideService_API>("aiguideservice-api")
    .WithReference(redis)
    .WaitFor(redis)
    .WithReference(authApi)
    .WaitFor(authApi)
    .WithHttpHealthCheck("/health");

var notifyApi = builder.AddProject<Projects.NotificationService_API>("notificationservice-api")
    .WithReference(notifyDb)
    .WaitFor(notifyDb)
    .WithReference(redis)
    .WaitFor(redis)
    .WithHttpHealthCheck("/health");

builder.Build().Run();
