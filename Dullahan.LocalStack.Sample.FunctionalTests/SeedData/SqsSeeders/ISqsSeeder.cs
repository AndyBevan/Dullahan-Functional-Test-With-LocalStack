using Amazon.SQS;

namespace Dullahan.LocalStack.Sample.FunctionalTests.SeedData.SqsSeeders
{
    public interface ISqsSeeder
    {
        void CreateQueue(AmazonSQSClient client);
    }
}
