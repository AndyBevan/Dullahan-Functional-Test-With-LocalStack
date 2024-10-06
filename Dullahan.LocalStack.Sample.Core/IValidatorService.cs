using FluentValidation;
using System.Threading.Tasks;

namespace Dullahan.LocalStack.Sample.Core
{
    public interface IValidatorService
    {
        Task ValidationCheck<T, U>(U model) where T : AbstractValidator<U>, new();
    }
}
