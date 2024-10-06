using System;
using Dullahan.LocalStack.Sample.Core.Dtos;
using FluentValidation;

namespace Dullahan.LocalStack.Sample.Core.Validators
{
    public class CommentModelValidator : AbstractValidator<CommentModel>
    {
        public CommentModelValidator()
        {
            RuleFor(b => b.MovieId).NotEqual(Guid.Empty);
            RuleFor(b => b.CommentId).NotEqual(Guid.Empty);
            RuleFor(b => b.CreateDate).NotEmpty();
            RuleFor(b => b.Comment).NotEmpty();
        }
    }
}
