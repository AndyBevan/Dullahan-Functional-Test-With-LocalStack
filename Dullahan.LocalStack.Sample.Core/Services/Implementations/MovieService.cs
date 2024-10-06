using System;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Amazon.SQS;
using Amazon.SQS.Model;
using Dullahan.LocalStack.Sample.Core.Dtos;
using Dullahan.LocalStack.Sample.Core.Entities;
using Dullahan.LocalStack.Sample.Core.Repositories.Contracts;
using Dullahan.LocalStack.Sample.Core.Services.Contracts;
using Dullahan.LocalStack.Sample.Core.Validators;
using Mapster;
using Microsoft.Extensions.Options;

namespace Dullahan.LocalStack.Sample.Core.Services.Implementations
{
    public class MovieService(IMovieRepository movieRepository, IValidatorService validatorService, IAmazonSQS amazonSqs, IOptions<SqsQueueConfig> options) : IMovieService
    {
        private readonly IMovieRepository _movieRepository = movieRepository;
        private readonly IValidatorService _validatorService = validatorService;
        private readonly IAmazonSQS _amazonSqs = amazonSqs;
        private readonly SqsQueueConfig _sqsQueueConfig = options.Value;

        public async Task<GetMovieResponseModel> GetMovieByIdAsync(Guid movieId, CancellationToken token)
        {
            Contract.Requires<Exception>(movieId != Guid.Empty, nameof(movieId));

            MovieEntity movieEntity = await _movieRepository.GetMovieByIdAsync(movieId, token);

            GetMovieResponseModel responseModel = await movieEntity.BuildAdapter().AdaptToTypeAsync<GetMovieResponseModel>();

            return responseModel;
        }

        public async Task<AddMovieResponseModel> AddMovieAsync(AddMovieRequestModel model, CancellationToken token = default)
        {
            try
            {
                await _validatorService.ValidationCheck<AddMovieRequestModelValidator, AddMovieRequestModel>(model);

                MovieEntity entity = await model.BuildAdapter().AdaptToTypeAsync<MovieEntity>();

                await _movieRepository.AddAsync(entity, token);

                AddMovieResponseModel responseModel = await entity.BuildAdapter().AdaptToTypeAsync<AddMovieResponseModel>();

                return responseModel;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> CommentMovie(CommentModel model, CancellationToken token = default)
        {
            try
            {
                await _validatorService.ValidationCheck<CommentModelValidator, CommentModel>(model);

                string serializedObject = JsonSerializer.Serialize(model);

                string queueName = _sqsQueueConfig.QueueName;

                GetQueueUrlResponse response = await _amazonSqs.GetQueueUrlAsync(queueName, token);

                var sendMessageRequest = new SendMessageRequest
                {
                    QueueUrl = response.QueueUrl,
                    MessageGroupId = model.MovieId.ToString(),
                    MessageDeduplicationId = Guid.NewGuid().ToString(),
                    MessageBody = serializedObject
                };

                var messageResponse = await _amazonSqs.SendMessageAsync(sendMessageRequest, token);

                if (messageResponse.HttpStatusCode != HttpStatusCode.OK)
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
