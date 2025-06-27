namespace task04;

public interface ISpaceship
{
    void MoveForward();
    void Rotate(int angle);
    void Fire();
    int Speed { get; }
    int FirePower { get; }
}

public class Cruiser : ISpaceship
{
    public int Speed { get; } = 50;
    public int FirePower { get; } = 100;
    public void MoveForward()
    {
        Console.WriteLine($"Крейсер движется вперед со скоростью {Speed}!");
    }

    public void Rotate(int angle)
    {
        Console.WriteLine($"Крейсер совершил поворот на {angle} градусов");
    }

    public void Fire()
    {
        Console.WriteLine($"БА-БАХ! Нанесен урон {FirePower} единиц");
    }
}

public class Fighter : ISpaceship
{
    public int Speed { get; } = 100;
    public int FirePower { get; } = 50;
    public void MoveForward()
    {
        Console.WriteLine($"Истребитель движется вперед со скоростью {Speed}!");
    }

    public void Rotate(int angle)
    {
        Console.WriteLine($"Истребитель совершил поворот на {angle} градусов");
    }

    public void Fire()
    {
        Console.WriteLine($"ТЫЩЬ! Нанесен урон {FirePower} единиц");
    }
}
