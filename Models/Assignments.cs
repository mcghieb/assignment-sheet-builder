namespace Models;

public class Assignment(string classCode, string name, string url, DateTime dueDate)
{
    public string ClassCode { get; } = classCode;
    public string Name { get; } = name;
    public string Url { get; } = url;
    public DateTime DueDate { get; } = dueDate;
}

public class Assignments
{
    private List<Assignment> AssignmentsList { get; } = [];
    private int Count { get; set; }

    public void AddAssignment(Assignment assignment)
    {
        AssignmentsList.Add(assignment);
        Count++;
    }

    public List<Assignment> GetAssignments()
    {
        return AssignmentsList;
    }
}
