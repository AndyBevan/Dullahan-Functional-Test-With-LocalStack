using System;
using System.Threading;
using System.Threading.Tasks;
using Dullahan.LocalStack.Sample.Core.Entities;

namespace Dullahan.LocalStack.Sample.Core.Repositories.Contracts
{
    public interface IMovieRepository : IRepository<MovieEntity>
    {
        Task<MovieEntity> GetMovieByIdAsync(Guid movieId, CancellationToken token = default);
    }
}
