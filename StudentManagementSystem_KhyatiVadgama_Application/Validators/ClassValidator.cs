using FluentValidation;
using StudentManagementSystem_KhyatiVadgama_Application.DTOs;

namespace StudentManagementSystem_KhyatiVadgama_Application.Validators
{
    public class ClassValidator : AbstractValidator<CreateClassRequest>
    {
        public ClassValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.Description)
                .MaximumLength(100)
                .WithMessage("Description cannot be more than 100 characters");
        }
    }
}
