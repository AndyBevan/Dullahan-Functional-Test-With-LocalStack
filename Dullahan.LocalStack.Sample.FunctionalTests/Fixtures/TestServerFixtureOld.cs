using System.IO;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.SQS;
using Dullahan.LocalStack.Sample.Api;
using Dullahan.LocalStack.Sample.FunctionalTests.SeedData.DynamoDBSeeders;
using Dullahan.LocalStack.Sample.FunctionalTests.SeedData.SqsSeeders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using AmazonDynamoDBClient = Amazon.DynamoDBv2.AmazonDynamoDBClient;

namespace Dullahan.LocalStack.Sample.FunctionalTests.Fixtures
{
    public class TestServerFixture2 : WebApplicationFactory<Startup>
    {
        private const string DynamoDbServiceUrl = "http://localhost:4566/";
        private const string SqsServiceUrl = "http://localhost:4566/";

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
            builder.ConfigureTestServices(collection =>
            {
                collection.Remove(new ServiceDescriptor(typeof(IDynamoDBContext),
                    a => a.GetService(typeof(IDynamoDBContext)), ServiceLifetime.Scoped));

                AmazonDynamoDBConfig clientConfig = new()
                {
                    RegionEndpoint = RegionEndpoint.EUCentral1,
                    UseHttp = true,
                    ServiceURL = DynamoDbServiceUrl
                };

                var dynamoDbClient = new AmazonDynamoDBClient("123", "123", clientConfig);

                collection.AddScoped<IDynamoDBContext, DynamoDBContext>(opt =>
                {
                    //AmazonDynamoDBClient client = new AmazonDynamoDBClient("123", "123", clientConfig);
                    AmazonDynamoDBClient client = new();
                    client = dynamoDbClient;
                    var dynamoDbContext = new DynamoDBContext(client);
                    //var dynamoDbContext = new DynamoDBContext(dynamoDbClient);
                    return dynamoDbContext;
                });

                collection.Remove(new ServiceDescriptor(typeof(IAmazonDynamoDB),
                    a => a.GetService(typeof(IAmazonDynamoDB)), ServiceLifetime.Scoped));

                collection.AddAWSService<IAmazonDynamoDB>(options: new AWSOptions
                {
                    Region = RegionEndpoint.EUCentral1,
                    Credentials = new BasicAWSCredentials("123", "123"),
                }, ServiceLifetime.Scoped);

                collection.Remove(new ServiceDescriptor(typeof(IAmazonSQS), a => a.GetService(typeof(IAmazonSQS)),
                    ServiceLifetime.Scoped));

                AmazonSQSConfig sqsConfig = new()
                {
                    RegionEndpoint = RegionEndpoint.EUCentral1,
                    UseHttp = true,
                    ServiceURL = SqsServiceUrl,
                };

                var sqsClient = new AmazonSQSClient("123", "123", sqsConfig);

                collection.AddScoped<IAmazonSQS, AmazonSQSClient>(opt =>
                {
                    AmazonSQSConfig localSqsConfig = new()
                    {
                        RegionEndpoint = RegionEndpoint.EUCentral1,
                        UseHttp = true,
                        ServiceURL = SqsServiceUrl,
                    };

                    var localSqsClient = new AmazonSQSClient("123", "123", localSqsConfig);

                    return localSqsClient;
                });

                collection.RemoveAll(typeof(IHostedService));
                DynamoDbSeeder.CreateTable(dynamoDbClient);
                DynamoDbSeeder.Seed(dynamoDbClient);
                SqsSeeder.CreateQueue(sqsClient);
            });
            base.ConfigureWebHost(builder);
        }
    }
}
