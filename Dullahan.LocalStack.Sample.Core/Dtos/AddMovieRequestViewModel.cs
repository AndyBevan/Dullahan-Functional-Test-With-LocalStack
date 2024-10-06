using System;

namespace Dullahan.LocalStack.Sample.Core.Dtos
{
    public class AddMovieRequestViewModel
    {
        public Guid DirectorId { get; set; }

        public string MovieName { get; set; }
    }
}
