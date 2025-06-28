using Xunit;
using Moq;
using task04;

namespace task04tests;

public class SpaceshipTests
{
    [Fact]
    public void Cruiser_ShouldHaveCorrectStats()
    {
        ISpaceship cruiser = new Cruiser();
        Assert.Equal(50, cruiser.Speed);
        Assert.Equal(100, cruiser.FirePower);
    }

    [Fact]
    public void Fighter_ShouldHaveCorrectStats()
    {
        ISpaceship fighter = new Fighter();
        Assert.Equal(100, fighter.Speed);
        Assert.Equal(50, fighter.FirePower);
    }

    [Fact]
    public void Fighter_ShouldBeFasterThanCruiser()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.Speed > cruiser.Speed);
    }

    [Fact]
    public void Fighter_ShouldHaveLessFirePowerThanCruise()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        Assert.True(fighter.FirePower < cruiser.FirePower);
    }

    [Fact]
    public void Cruiser_ShouldMoveCorrectly_At0And90Degrees()
    {
        var cruiser = new Cruiser();

        cruiser.MoveForward();
        Assert.Equal(50, cruiser.PositionX);
        Assert.Equal(0, cruiser.PositionY);

        cruiser.CurrentAngle = 90;
        cruiser.MoveForward();
        Assert.Equal(50, cruiser.PositionX);
        Assert.Equal(50, cruiser.PositionY);
    }

    [Fact]
    public void Fighter_ShouldMoveCorrectly_At0And90Degrees()
    {
        var fighter = new Fighter();

        fighter.MoveForward();
        Assert.Equal(100, fighter.PositionX);
        Assert.Equal(0, fighter.PositionY);

        fighter.CurrentAngle = 90;
        fighter.MoveForward();
        Assert.Equal(100, fighter.PositionX);
        Assert.Equal(100, fighter.PositionY);
    }

    [Fact]
    public void Rotation_ShouldWorkAndNormalizeCorrectly()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        fighter.Rotate(90);
        cruiser.Rotate(180);
        Assert.Equal(90, fighter.CurrentAngle);
        Assert.Equal(180, cruiser.CurrentAngle);

        fighter.Rotate(300);
        Assert.Equal(30, fighter.CurrentAngle);

        cruiser.Rotate(-200);
        Assert.Equal(340, cruiser.CurrentAngle);
    }

    [Fact]
    public void Firing_ShouldDecreaseRocketsByOne_ShouldNotDecreaseCountBelowZero()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();

        Assert.Equal(10, fighter.RemainingRockets);
        Assert.Equal(10, cruiser.RemainingRockets);

        fighter.Fire();
        cruiser.Fire();

        Assert.Equal(9, fighter.RemainingRockets);
        Assert.Equal(9, cruiser.RemainingRockets);

        fighter.RemainingRockets = 0;
        fighter.Fire();
        Assert.Equal(0, fighter.RemainingRockets);
    }

    [Fact]
    public void Movement_ShouldBeAngleDependent_At172Degrees()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();

        fighter.Rotate(172);
        cruiser.Rotate(172);
        fighter.MoveForward();
        cruiser.MoveForward();

        Assert.Equal(-99, fighter.PositionX);
        Assert.Equal(13, fighter.PositionY);
        Assert.Equal(-50, cruiser.PositionX);
        Assert.Equal(7, cruiser.PositionY);
    }
}
