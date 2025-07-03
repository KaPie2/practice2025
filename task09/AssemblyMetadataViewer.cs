using System.Reflection;

namespace task09;

public class AssemblyMetadataViewer
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Ошибка: не указан путь к файлу .dll");
            return;
        }

        string dll_path = args[0];

        if (!File.Exists(dll_path))
        {
            Console.WriteLine("Ошибка: файл не найден");
            return;
        }

        Assembly assembly = Assembly.LoadFrom(dll_path);

        foreach (Type type in assembly.GetTypes())
        {
            if (!type.IsClass) continue;
            Console.WriteLine($"Класс: {type.FullName}");

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                if (method.IsSpecialName) continue;
                Console.WriteLine($"  Метод: {method.Name}");
                foreach (var parameter in method.GetParameters())
                {
                    Console.WriteLine($"    Имя и тип параметра: {parameter.Name} - {parameter.ParameterType.Name}");
                }
            }

            foreach (var attribute in type.GetCustomAttributes())
            {
                Console.WriteLine($"  Атрибут класса: {attribute.GetType().Name}");
            }

            foreach (var constructor in type.GetConstructors())
            {
                Console.WriteLine("  Конструктор:");
                foreach (var parameter in constructor.GetParameters())
                {
                    Console.WriteLine($"    Имя и тип параметра: {parameter.Name} - {parameter.ParameterType.Name}");
                }
            }

            Console.WriteLine();
        }
    }
}
