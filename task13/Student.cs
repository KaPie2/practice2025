using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13;

public class Subject
{
    public string Name { get; set; }
    public int Grade { get; set; }
}

public class Student
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime BirthDate { get; set; }
    public List<Subject>? Grades { get; set; }
}

public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string format = "dd.MM.yyyy";
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => DateTime.ParseExact(reader.GetString(), format, null);

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString(format));
}

public static class StudentSerializer
{
    static readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new CustomDateTimeConverter() }
    };

    public static string Serialize(Student student)
        => JsonSerializer.Serialize(student, options);

    public static Student Deserialize(string json)
        => JsonSerializer.Deserialize<Student>(json, options);

    public static void SaveToFile(string path, Student student)
    {
        File.WriteAllText(path, Serialize(student));
    }

    public static Student LoadFromFile(string path)
    {
        string json = File.ReadAllText(path);
        return Deserialize(json);
    }
}
