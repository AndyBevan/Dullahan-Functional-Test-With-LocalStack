using System;

namespace Dullahan.LocalStack.Sample.Core.Dtos
{
    public class CommentMovieRequestModel
    {
        public Guid MovieId { get; set; }

        public string Comment { get; set; }
    }
}
