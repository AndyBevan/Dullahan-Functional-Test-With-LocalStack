using System;
using System.Net;
using System.Threading.Tasks;
using Dullahan.LocalStack.Sample.Core.Dtos;
using Dullahan.LocalStack.Sample.FunctionalTests.Extensions;
using Dullahan.LocalStack.Sample.FunctionalTests.Fixtures;
using Dullahan.LocalStack.Sample.FunctionalTests.Routes;
using Xunit;

namespace Dullahan.LocalStack.Sample.FunctionalTests.Scenarios
{
    [Collection(nameof(ApiTestCollection))]
    public class MovieScenarios(TestServerFixture testServerFixture) : BaseScenario(testServerFixture)
    {
        [Fact]
        public async Task AddMovie_Should_Return_400_From_ServiceValidation()
        {
            AddMovieRequestViewModel model = new();
            var response = await HttpClient.PostAsync(requestUri: MovieRoutes.Root, model: model);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task AddMovie_If_Request_Valid_Should_Return_200()
        {
            var requestViewModel = new AddMovieRequestViewModel
            {
                DirectorId = TestConstants.GetDirectorId(),
                MovieName = "Terminator"
            };

            var response = await HttpClient.PostAsync(MovieRoutes.Root, requestViewModel);

            var responseModel = await response.Content.GetAsync<AddMovieResponseViewModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(await CheckMovieByMovieIdAsync(responseModel.MovieId));
        }

        [Fact]
        public async Task GetMovie_If_Id_Null_Should_Return_400()
        {
            var response = await HttpClient.GetAsync(MovieRoutes.GetMovieById(Guid.Empty));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetMovie_If_Guid_Not_Found_Should_Return_404()
        {
            var response = await HttpClient.GetAsync(MovieRoutes.GetMovieById(Guid.NewGuid()));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetPostById_If_Found_Should_Return_200()
        {
            var response = await HttpClient.GetAsync(MovieRoutes.GetMovieById(TestConstants.GetMovieId()));

            GetMovieResponseViewModel responseModel = await response.Content.GetAsync<GetMovieResponseViewModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal(responseModel.MovieId, TestConstants.GetMovieId());
        }

        [Fact]
        public async Task CommentMovie_If_Missing_Comment_Should_Return_400()
        {
            CommentMovieRequestModel model = new()
            {
                MovieId = TestConstants.GetMovieId()
            };

            var response = await HttpClient.PostAsync(MovieRoutes.CommentMovie(), model);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CommentMovie_If_Valid_Should_Return_200()
        {
            CommentMovieRequestModel model = new()
            {
                MovieId = TestConstants.GetMovieId(),
                Comment = "Great Movie!"
            };

            var response = await HttpClient.PostAsync(MovieRoutes.CommentMovie(), model);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.True(await IsItemInQueueAsync(TestConstants.GetMovieId()));
        }
    }
}
