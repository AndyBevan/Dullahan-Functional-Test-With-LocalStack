using System.Collections.Generic;

namespace Dullahan.LocalStack.Sample.Core.Dtos
{
    public class GetMoviesResponseModel
    {
        public IList<GetMovieResponseModel> Movies { get; set; }
    }
}
