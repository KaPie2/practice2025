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
    public void Ships_ShouldMoveWithDifferentSpeed()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        fighter.MoveForward();
        cruiser.MoveForward();
    }

    [Fact]
    public void Rotation_ShouldWork()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        fighter.Rotate(90);
        cruiser.Rotate(180);
    }

    [Fact]
    public void Firing_ShouldWork()
    {
        var fighter = new Fighter();
        var cruiser = new Cruiser();
        fighter.Fire();
        cruiser.Fire();
    }
}
