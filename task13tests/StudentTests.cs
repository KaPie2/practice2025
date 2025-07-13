using Xunit;
using task13;

namespace task13tests;

public class StudentTests
{
    [Fact]
    public void Serialize_IgnoreNullsAndFormatDate()
    {
        var student = new Student
        {
            FirstName = null,
            LastName = "Паровозов",
            BirthDate = DateTime.Parse("01.01.2006"),
            Grades = null
        };

        string json = StudentSerializer.Serialize(student);
        Assert.DoesNotContain("FirstName", json);
        Assert.DoesNotContain("Grades", json);
        Assert.Contains("LastName", json);
        Assert.Contains("01.01.2006", json);
    }

    [Fact]
    public void SerializeThenDeserialize_RestoresOriginalStudentData()
    {
        var originalStudent = new Student
        {
            FirstName = "Аркадий",
            LastName = "Паровозов",
            BirthDate = DateTime.Parse("09.07.2025"),
            Grades = new List<Subject>
            {
                new Subject { Name = "Математика", Grade = 5 },
                new Subject { Name = "Физика", Grade = 4 }
            }
        };

        string json = StudentSerializer.Serialize(originalStudent);
        var deserializedStudent = StudentSerializer.Deserialize(json);

        Assert.NotNull(deserializedStudent);
        Assert.Equal(originalStudent.FirstName, deserializedStudent.FirstName);
        Assert.Equal(originalStudent.LastName, deserializedStudent.LastName);
        Assert.Equal(originalStudent.BirthDate, deserializedStudent.BirthDate);
        Assert.NotNull(deserializedStudent.Grades);
        Assert.Equal(originalStudent.Grades.Count, deserializedStudent.Grades.Count);
    }

    [Fact]
    public void SaveToFile_And_LoadFromFile_WorkCorrectly()
    {
        var student = new Student
        {
            FirstName = "А",
            LastName = "Б",
            BirthDate = DateTime.Parse("01.01.2000"),
            Grades = new List<Subject>
            {
                new Subject { Name = "История", Grade = 4 }
            }
        };
        
        string path = "test_student.json";

        StudentSerializer.SaveToFile(path, student);
        var loadedStudent = StudentSerializer.LoadFromFile(path);

        Assert.NotNull(loadedStudent);
        Assert.Equal(student.FirstName, loadedStudent.FirstName);
        Assert.Equal(student.LastName, loadedStudent.LastName);
        Assert.Equal(student.BirthDate, loadedStudent.BirthDate);
        Assert.NotNull(loadedStudent.Grades);
        Assert.Equal(student.Grades.Count, loadedStudent.Grades.Count);

        if (File.Exists(path)) File.Delete(path);
    }
}
