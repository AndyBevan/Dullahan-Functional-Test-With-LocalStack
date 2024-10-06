using System;

namespace Dullahan.LocalStack.Sample.Core.Dtos
{
    public class CommentModel
    {
        public Guid MovieId { get; set; }

        public string Comment { get; set; }

        public string CreateDate { get; set; }

        public Guid CommentId { get; set; }
    }
}
