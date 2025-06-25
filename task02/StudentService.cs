using System;
using System.Collections.Generic;
using System.Linq;

namespace task02;

public class Student
{
    public string? Name { get; set; }
    public string? Faculty { get; set; }
    public List<int>? Grades { get; set; }
}

public class StudentService
{
    private readonly List<Student> _students;

    public StudentService(List<Student> students) => _students = students;

    public IEnumerable<Student> GetStudentsByFaculty(string faculty)
        => _students.Where(s => s.Faculty == faculty);


    public IEnumerable<Student> GetStudentsWithMinAverageGrade(double minAverageGrade)
        => _students.Where(s => s.Grades.Count > 0 && s.Grades.Average() >= minAverageGrade);

    public IEnumerable<Student> GetStudentsOrderedByName()
        => _students.OrderBy(s => s.Name);

    public ILookup<string, Student> GroupStudentsByFaculty()
        => _students.ToLookup(s => s.Faculty);

    public string GetFacultyWithHighestAverageGrade()
        => _students
        .Where(s => s.Grades.Count > 0)
        .GroupBy(s => s.Faculty)
        .Select(p => new
        {
            Faculty = p.Key,
            AverageGradesFaculty = p.Average(s => s.Grades.Average())
        })
        .OrderByDescending(f => f.AverageGradesFaculty)
        .First().Faculty;
}
