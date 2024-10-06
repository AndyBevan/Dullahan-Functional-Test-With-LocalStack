using System;
using Amazon.DynamoDBv2.DataModel;

namespace Dullahan.LocalStack.Sample.Core.Entities
{
    [DynamoDBTable("Movies")]
    public class MovieEntity
    {
        [DynamoDBHashKey]
        public Guid DirectorId { get; set; }

        [DynamoDBRangeKey]
        public string CreateDate { get; set; }

        public Guid MovieId { get; set; }

        public string MovieName { get; set; }
    }
}
