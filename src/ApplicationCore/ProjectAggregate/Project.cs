using ApplicationCore.Interfaces;

namespace ApplicationCore.ProjectAggregate;

public class Project : BaseEntity, IAggregateRoot
{
    public Project(string name, string number, DateTime createdDate)
    {
        Name = name;
        Number = number;
        CreatedDate = createdDate;
    }

    public string Name { get; private set; }

    public string Number { get; private set; }

    public DateTime CreatedDate { get; private set; }
}
