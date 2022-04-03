using ApplicationCore.Interfaces;
using ApplicationCore.ProjectAggregate;
using ApplicationCore.ProjectAggregate.Specifications;
using FluentValidation;

namespace ApplicationCore.TimeTrackAggregate.Commands.AddTimeTrack;

public class AddTimeTrackValidator : AbstractValidator<AddTimeTrack>
{
    private readonly IReadRepository<Project> _projectReadRepository;

    public AddTimeTrackValidator(IReadRepository<Project> projectReadRepository)
    {
        _projectReadRepository = projectReadRepository;

        RuleFor(x => x.ProjectNumber)
            .NotEmpty().WithMessage("ProjectNumber is required")
            .MustAsync(async (value, cancellationToken) => await ProjectExists(value, cancellationToken))
            .WithMessage("Project must exist");

        RuleFor(x => x.Task)
            .NotEmpty().WithMessage("Task is required")
            .MaximumLength(256).WithMessage("Task must be less than 256 characters");

        RuleFor(x => x.TimeSpent)
            .NotEmpty().WithMessage("TimeSpent is required");

        RuleFor(x => x.Description)
            .MaximumLength(256).When(x => !string.IsNullOrEmpty(x.Description)).WithMessage("Description must be less than 256 characters");
    }

    private async Task<bool> ProjectExists(string value, CancellationToken cancellationToken)
    {
        var projectByNumber = new ProjectByNumber(value);
        return await _projectReadRepository.AnyAsync(projectByNumber, cancellationToken);
    }
}
