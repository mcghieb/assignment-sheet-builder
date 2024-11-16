namespace Models;

public class Assignment(string classCode, string name, string url, string dueDate)
{
    public string ClassCode { get; } = classCode;
    public string Name { get; } = name;
    public string Url { get; } = url;
    public string DueDate { get; } = dueDate;
}

public class Assignments
{
    private List<Assignment> assignments { get; } = [];

    public void AddAssignment(Assignment assignment)
    {
        assignments.Add(assignment);
    }
}
