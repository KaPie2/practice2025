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
    public int CurrentAngle = 0;
    public int PositionX = 0;
    public int PositionY = 0;
    public int RemainingRockets = 10;
    public void MoveForward()
    {
        double AngleRadians = CurrentAngle * Math.PI / 180;
        PositionX += (int)Math.Round(Speed * Math.Cos(AngleRadians));
        PositionY += (int)Math.Round(Speed * Math.Sin(AngleRadians));
    }

    public void Rotate(int angle)
    {
        CurrentAngle = (CurrentAngle + angle) % 360;
        if (CurrentAngle < 0) CurrentAngle += 360;
    }

    public void Fire()
    {
        if (RemainingRockets > 0) RemainingRockets--;
    }
}

public class Fighter : ISpaceship
{
    public int Speed { get; } = 100;
    public int FirePower { get; } = 50;
    public int CurrentAngle = 0;
    public int PositionX = 0;
    public int PositionY = 0;
    public int RemainingRockets = 10;
    public void MoveForward()
    {
        double AngleRadians = CurrentAngle * Math.PI / 180;
        PositionX += (int)(Speed * Math.Cos(AngleRadians));
        PositionY += (int)(Speed * Math.Sin(AngleRadians));
    }

    public void Rotate(int angle)
    {
        CurrentAngle = (CurrentAngle + angle) % 360;
        if (CurrentAngle < 0) CurrentAngle += 360;
    }

    public void Fire()
    {
        if (RemainingRockets > 0) RemainingRockets--;
    }
}
