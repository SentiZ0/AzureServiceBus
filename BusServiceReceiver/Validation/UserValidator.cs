using BusServiceReceiver.Models;
using FluentValidation;

namespace BusServiceReceiver.Validation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.FirstName).NotNull();
            RuleFor(user => user.LastName).NotNull();
            RuleFor(user => user.Email).NotNull().EmailAddress();
            RuleFor(user => user.Age).NotNull();
        }
    }
}
