using System.Collections.Generic;

namespace Dullahan.LocalStack.Sample.Core.Dtos
{
    public class GetMoviesResponseViewModel
    {
        public IList<GetMovieResponseViewModel> Movies { get; set; }
    }
}
