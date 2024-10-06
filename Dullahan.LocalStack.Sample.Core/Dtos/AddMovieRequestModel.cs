using System;

namespace Dullahan.LocalStack.Sample.Core.Dtos
{
    public class AddMovieRequestModel
    {
        public Guid DirectorId { get; set; }

        public Guid MovieId { get; set; }

        public string CreateDate { get; set; }

        public string MovieName { get; set; }
    }
}
