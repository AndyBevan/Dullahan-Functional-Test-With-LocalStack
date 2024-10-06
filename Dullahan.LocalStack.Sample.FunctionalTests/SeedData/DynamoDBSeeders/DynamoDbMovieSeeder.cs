using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Dullahan.LocalStack.Sample.Core;
using Dullahan.LocalStack.Sample.Core.Entities;

namespace Dullahan.LocalStack.Sample.FunctionalTests.SeedData.DynamoDBSeeders
{
    public class DynamoDbMovieSeeder : IDynamoDbSeeder
    {
        public void Seed(AmazonDynamoDBClient client)
        {
            MovieEntity model = new()
            {
                MovieId = TestConstants.GetMovieId(),
                DirectorId = TestConstants.GetDirectorId(),
                MovieName = TestConstants.GetMovieName(),
                CreateDate = DateTime.UtcNow.ToComparableDateString()
            };
            DynamoDbSeeder.Add(client, model);
        }

            public void CreateTable(AmazonDynamoDBClient client)
            {
                var postTableCreateRequest = new CreateTableRequest
                {
                    AttributeDefinitions = new List<AttributeDefinition>
                    {
                        new() {
                            AttributeName = nameof(MovieEntity.DirectorId),
                            AttributeType = ScalarAttributeType.S
                        },
                        new() {
                            AttributeName = nameof(MovieEntity.CreateDate),
                            AttributeType = ScalarAttributeType.S
                        },
                        new()
                        {
                            AttributeName = nameof(MovieEntity.MovieId),
                            AttributeType = ScalarAttributeType.S
                        }
                    },
                    TableName = "Movies",
                    KeySchema = new List<KeySchemaElement>()
                    {
                        new()
                        {
                            AttributeName = nameof(MovieEntity.DirectorId),
                            KeyType = KeyType.HASH
                        },
                        new()
                        {
                            AttributeName = nameof(MovieEntity.CreateDate),
                            KeyType = KeyType.RANGE
                        }
                    },
                    GlobalSecondaryIndexes = new List<GlobalSecondaryIndex>
                    {
                        new() {
                            Projection = new Projection
                            {
                                ProjectionType = ProjectionType.ALL
                            },
                            IndexName = Constants.MoiveTableMovieIdGsi,
                            KeySchema = new List<KeySchemaElement>
                            {
                                new() {
                                    AttributeName = nameof(MovieEntity.MovieId),
                                    KeyType = KeyType.HASH
                                }
                            },
                            ProvisionedThroughput = new ProvisionedThroughput
                            {
                                ReadCapacityUnits = 5,
                                WriteCapacityUnits = 5
                            }
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 5,
                        WriteCapacityUnits = 6
                    },
                };
                CreateTableResponse result = client.CreateTableAsync(postTableCreateRequest).GetAwaiter().GetResult();
            }
    }
}
