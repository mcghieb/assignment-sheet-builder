namespace Models;

public class Assignment(string classCode, string name, string url, DateTime dueDate) : BaseRecord;
{
    [SheetField(
                DisplayName = "Class Code",
                ColumnID = 1,
                FieldType = SheetFieldType.String)]
    public string ClassCode { get; } = classCode;

    [SheetField(
                DisplayName = "Name",
                ColumnID = 2,
                FieldType = SheetFieldType.String)]
    public string Name { get; } = name;

    [SheetField(
                DisplayName = "URL",
                ColumnID = 3,
                FieldType = SheetFieldType.String)]
    public string Url { get; } = url;

    [SheetField(
                DisplayName = "Due Date",
                ColumnID = 4,
                FieldType = SheetFieldType.DateTime)]
    public DateTime DueDate { get; } = dueDate;

    public Assignment() { }

    public Assignment(IList<object> row, int rowId, int minColumnId = 1) 
        : base(row, rowId, minColumnId)


}

public class AssignmentRepository : BaseRepository<Assignment>
{
    public AssignmentRepository() { }

    public AssignmentRepository(SheetHelper<Assignment> sheetsHelper, BaseRepositoryConfiguration config) 
        : base(sheetsHelper, config) { }

    // private List<Assignment> AssignmentsList { get; } = [];
    // private int Count { get; set; }

    // public void AddAssignment(Assignment assignment)
    // {
    //     AssignmentsList.Add(assignment);
    //     Count++;
    // }

    // public List<Assignment> GetAssignments()
    // {
    //     return AssignmentsList;
    // }

    // public int GetCount()
    // {
    //     return Count;
    // }
}

