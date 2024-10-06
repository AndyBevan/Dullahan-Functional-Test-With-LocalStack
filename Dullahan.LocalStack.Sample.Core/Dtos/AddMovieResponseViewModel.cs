using System;

namespace Dullahan.LocalStack.Sample.Core.Dtos
{
    public class AddMovieResponseViewModel
    {
        public Guid DirectorId { get; set; }

        public string CreateDate { get; set; }

        public Guid MovieId { get; set; }
    }
}
