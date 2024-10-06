using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DataModel;
using Dullahan.LocalStack.Sample.Core.Entities;
using Dullahan.LocalStack.Sample.Core.Repositories.Contracts;

namespace Dullahan.LocalStack.Sample.Core
{
    public class MovieRepository(IDynamoDBContext context) : Repository<MovieEntity>(context), IMovieRepository
    {
        public async Task<MovieEntity> GetMovieByIdAsync(Guid movieId, CancellationToken token = default)
        {
            DynamoDBOperationConfig operationConfig = new()
            {
                IndexName = Constants.MoiveTableMovieIdGsi,
            };

            List<MovieEntity> result = await _context.QueryAsync<MovieEntity>(movieId, operationConfig)
                .GetRemainingAsync(token);

            return result.FirstOrDefault();
        }
    }
}
