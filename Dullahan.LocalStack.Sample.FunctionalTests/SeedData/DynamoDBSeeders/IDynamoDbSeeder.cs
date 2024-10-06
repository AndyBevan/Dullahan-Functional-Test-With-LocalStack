using Amazon.DynamoDBv2;

namespace Dullahan.LocalStack.Sample.FunctionalTests.SeedData.DynamoDBSeeders
{
    public interface IDynamoDbSeeder
    {
        void Seed(AmazonDynamoDBClient client);

        void CreateTable(AmazonDynamoDBClient client);
    }
}
