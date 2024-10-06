using System;

namespace Dullahan.LocalStack.Sample.Core.Dtos
{
    public class GetMovieResponseModel
    {
        public Guid MovieId { get; set; }

        public Guid DirectorId { get; set; }

        public string CreateDate { get; set; }

        public string MovieName { get; set; }
    }
}
