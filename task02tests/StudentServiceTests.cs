using Xunit;
using task02;

namespace task02tests;

public class StudentServiceTests
{
    private List<Student> _testStudents;
    private StudentService _service;

    public StudentServiceTests()
    {
        _testStudents = new List<Student>
        {
            new() { Name = "Иван", Faculty = "ФИТ", Grades = new List<int> { 5, 4, 5 } },
            new() { Name = "Анна", Faculty = "ФИТ", Grades = new List<int> { 3, 4, 3 } },
            new() { Name = "Петр", Faculty = "Экономика", Grades = new List<int> { 5, 5, 5 } },
            new() { Name = "Инна", Faculty = "ФГО", Grades = new List<int> {} },
            new() { Name = "Антон", Faculty = "ФГО", Grades = new List<int> { 3, 4, 4 } },
            new() { Name = "Кирилл", Faculty = "РТФ", Grades = new List<int> { 4, 4, 4 } }
        };
        _service = new StudentService(_testStudents);
    }

    [Fact]
    public void GetStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsByFaculty("ФИТ").ToList();
        Assert.Equal(2, result.Count);
        Assert.True(result.All(s => s.Faculty == "ФИТ"));
    }

    [Fact]
    public void GetStudentsWithMinAverageGrade_ReturnsCorrectStudents()
    {
        var result = _service.GetStudentsWithMinAverageGrade(3.5).ToList();
        Assert.Equal(4, result.Count);
        Assert.True(result.All(s => s.Grades.Average() >= 3.5));
    }

    [Fact]
    public void GetStudentsOrderedByName_ReturnsCorrectOrder()
    {
        var result = _service.GetStudentsOrderedByName().Select(s => s.Name).ToList();
        Assert.Equal(new[] { "Анна", "Антон", "Иван", "Инна", "Кирилл", "Петр" }, result);
    }

    [Fact]
    public void GroupStudentsByFaculty_ReturnsCorrectStudents()
    {
        var result = _service.GroupStudentsByFaculty();
        Assert.Equal(4, result.Count);
        Assert.Equal(2, result["ФИТ"].Count());
        Assert.Single(result["Экономика"]);
        Assert.Equal(2, result["ФГО"].Count());
        Assert.Single(result["РТФ"]);
    }

    [Fact]
    public void GetFacultyWithHighestAverageGrade_ReturnsCorrectFaculty()
    {
        var result = _service.GetFacultyWithHighestAverageGrade();
        Assert.Equal("Экономика", result);
    }
}
