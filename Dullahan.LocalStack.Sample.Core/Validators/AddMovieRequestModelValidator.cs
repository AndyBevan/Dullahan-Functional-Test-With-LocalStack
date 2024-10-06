using System;
using Dullahan.LocalStack.Sample.Core.Dtos;
using FluentValidation;

namespace Dullahan.LocalStack.Sample.Core.Validators
{
    public class AddMovieRequestModelValidator : AbstractValidator<AddMovieRequestModel>
    {
        public AddMovieRequestModelValidator()
        {
            RuleFor(b => b.MovieId).NotEqual(Guid.Empty);
            RuleFor(b => b.DirectorId).NotEqual(Guid.Empty);
            RuleFor(b => b.CreateDate).NotEmpty();
            RuleFor(b => b.MovieName).NotEmpty();
        }
    }
}
