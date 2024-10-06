using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.SQS;
using Dullahan.LocalStack.Sample.Api;
using Dullahan.LocalStack.Sample.FunctionalTests.Extensions;
using Dullahan.LocalStack.Sample.FunctionalTests.SeedData.DynamoDBSeeders;
using Dullahan.LocalStack.Sample.FunctionalTests.SeedData.SqsSeeders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Dullahan.LocalStack.Sample.FunctionalTests.Fixtures;

public class TestServerFixture : WebApplicationFactory<Startup>
{
    private readonly string _dynamoDbServiceUrl = "http://localhost:4566";
    private readonly string _sqsServiceUrl = "http://localhost:4566";
    private readonly AmazonDynamoDBClient _dynamoDbClient;
    private readonly AmazonSQSClient _sqsClient;

    public TestServerFixture()
    {
        // Create and configure the DynamoDB client
        var dynamoDbConfig = new AmazonDynamoDBConfig
        {
            RegionEndpoint = RegionEndpoint.EUCentral1,
            UseHttp = true,
            ServiceURL = _dynamoDbServiceUrl
        };
        _dynamoDbClient = new AmazonDynamoDBClient("123", "123", dynamoDbConfig);

        // Create and configure the SQS client
        var sqsConfig = new AmazonSQSConfig
        {
            RegionEndpoint = RegionEndpoint.EUCentral1,
            UseHttp = true,
            ServiceURL = _sqsServiceUrl
        };
        _sqsClient = new AmazonSQSClient("123", "123", sqsConfig);
    }

    protected override IHostBuilder CreateHostBuilder()
    {
        var hostBuilder = base.CreateHostBuilder()
            .UseEnvironment("Testing")
            .ConfigureAppConfiguration(builder =>
            {
                var configPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                builder.AddJsonFile(configPath);
            });

        return hostBuilder;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Replace IDynamoDBContext with a scoped DynamoDBContext that uses the pre-configured _dynamoDbClient
            services.RemoveAll<IDynamoDBContext>();
            services.AddScoped<IDynamoDBContext>(provider => new DynamoDBContext(_dynamoDbClient));

            // Replace IAmazonDynamoDB with the pre-configured _dynamoDbClient
            services.RemoveAll<IAmazonDynamoDB>();
            services.AddSingleton<IAmazonDynamoDB>(_dynamoDbClient);

            // Replace IAmazonSQS with the pre-configured _sqsClient
            services.RemoveAll<IAmazonSQS>();
            services.AddSingleton<IAmazonSQS>(_sqsClient);

            // Remove all IHostedService instances if they are not needed during testing
            services.RemoveAll<IHostedService>();

            // Seed data using the configured clients
            DynamoDbSeeder.CreateTable(_dynamoDbClient);
            DynamoDbSeeder.Seed(_dynamoDbClient);
            SqsSeeder.CreateQueue(_sqsClient);
        });

        base.ConfigureWebHost(builder);
    }
}
