using ApplicationCore.Interfaces;
using ApplicationCore.ProjectAggregate.Specifications;
using FluentValidation;

namespace ApplicationCore.ProjectAggregate.Commands.AddProjectCommand;

public class AddProjectValidator : AbstractValidator<AddProject>
{
    private readonly IReadRepository<Project> _projectReadRepository;

    public AddProjectValidator(IReadRepository<Project> projectReadRepository)
    {
        _projectReadRepository = projectReadRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name can not be empty.");

        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("Number can not be empty.")
            .MustAsync(async (value, cancellationToken) => await UniqueProjectNumber(value, cancellationToken)).WithMessage("A project with this projectNumber exists.");
    }

    private async Task<bool> UniqueProjectNumber(string value, CancellationToken cancellationToken)
    {
        var projectByNumber = new ProjectByNumber(value);
        return !await _projectReadRepository.AnyAsync(projectByNumber, cancellationToken);
    }
}
