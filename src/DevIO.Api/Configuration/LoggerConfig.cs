using DevIO.Api.Extensions;
using Elmah.Io.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DevIO.Api.Configuration;

public static class LoggerConfig
{
    public static void AddLoggingConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddElmahIo(o =>
        {
            o.ApiKey = "f8e14a14e9a74e4081603982d197c9a4";
            o.LogId = new Guid("2ca0cca9-1c62-4aa4-8e34-1b725db43690");
        });

        //services.AddLogging(builder =>
        //{
        //    builder.AddElmahIo(o =>
        //    {
        //        o.ApiKey = "f8e14a14e9a74e4081603982d197c9a4";
        //        o.LogId = new Guid("2ca0cca9-1c62-4aa4-8e34-1b725db43690");
        //    });

        //    builder.AddFilter<ElmahIoLoggerProvider>(null, LogLevel.Warning);
        //});

        services.AddHealthChecks()
            .AddElmahIoPublisher(options =>
            {
                options.ApiKey = "f8e14a14e9a74e4081603982d197c9a4";
                options.LogId = new Guid("2ca0cca9-1c62-4aa4-8e34-1b725db43690");
                options.HeartbeatId = "1039ca1902bb47dab0a78d4f8af40a6e";
            })
            .AddCheck("Produtos", new SqlServerHealthCheck(configuration.GetConnectionString("DefaultConnection")))
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "DbSQL");
    }

    public static void UseLoggingConfiguration(this IApplicationBuilder app)
    {
        app.UseElmahIo();
    }
}