using Xunit;

namespace Dullahan.LocalStack.Sample.FunctionalTests.Fixtures
{
    [CollectionDefinition(nameof(ApiTestCollection))]
    public class ApiTestCollection : ICollectionFixture<LocalStackFixture>, ICollectionFixture<TestServerFixture>
    {

    }
}
