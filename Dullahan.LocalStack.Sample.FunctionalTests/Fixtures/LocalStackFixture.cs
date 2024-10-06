using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using System.Threading.Tasks;
using Xunit;

namespace Dullahan.LocalStack.Sample.FunctionalTests.Fixtures
{
    public class LocalStackFixture : IAsyncLifetime
    {
        private readonly IContainer _localStackContainer;

        public LocalStackFixture()
        {
            var localStackBuilder = new ContainerBuilder()
            .WithImage("localstack/localstack:latest")
            .WithCleanUp(true)
            .WithEnvironment("DEFAULT_REGION", "eu-central-1")
            .WithEnvironment("SERVICES", "dynamodb,sqs")
            .WithPortBinding(4566, 4566)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(4566));


            _localStackContainer = localStackBuilder.Build();
        }
        public async Task InitializeAsync()
        {
            await _localStackContainer.StartAsync();
        }

        public async Task DisposeAsync()
        {
            await _localStackContainer.StopAsync();
        }
    }
}
