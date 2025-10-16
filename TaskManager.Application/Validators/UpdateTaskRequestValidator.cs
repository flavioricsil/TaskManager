using FluentValidation;
using TaskManager.Application.DTOs;

namespace TaskManager.Application.Validators;

public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidator()
    {
        RuleFor(x => x.TaskId).NotEmpty();
        RuleFor(x => x.Status).IsInEnum();
    }
}
