using System;
using System.Threading;
using System.Threading.Tasks;
using Dullahan.LocalStack.Sample.Core.Dtos;

namespace Dullahan.LocalStack.Sample.Core.Services.Contracts
{
    public interface IMovieService
    {
        Task<GetMovieResponseModel> GetMovieByIdAsync(Guid movieId, CancellationToken token = default);

        Task<AddMovieResponseModel> AddMovieAsync(AddMovieRequestModel model, CancellationToken token = default);

        Task<bool> CommentMovie(CommentModel model, CancellationToken token = default);
    }
}
