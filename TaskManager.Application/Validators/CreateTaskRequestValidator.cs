using FluentValidation;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Validators;

public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.DueDate).GreaterThan(DateTime.UtcNow);
        RuleFor(x => x.Priority).IsInEnum();
    }
}
