using Ardalis.Specification;

namespace ApplicationCore.ProjectAggregate.Specifications;

public class ProjectByNumber : Specification<Project>, ISingleResultSpecification
{
    public ProjectByNumber(string number)
    {
        Query.Where(x => x.Number == number);
    }
}
