using System;

namespace Dullahan.LocalStack.Sample.Core.Dtos
{
    public class GetMovieResponseViewModel
    {
        public Guid MovieId { get; set; }

        public Guid DirectorId { get; set; }

        public string CreateDate { get; set; }
    }
}
