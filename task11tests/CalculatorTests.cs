using Xunit;
using task11;

namespace task11tests;

public class CalculatorTests
{
    private const string CalculatorCode = @"
    using task11;

    public class Calculator : ICalculator
    {
    public int Add(int a, int b) => a + b;
    public int Minus(int a, int b) => a - b;
    public int Mul(int a, int b) => a * b;
    public int Div(int a, int b) => a / b;
    }
    ";

    [Fact]
    public void Add_WorksCorrectly()
    {
        var calculator = CalculatorLoader.CreateCalculator(CalculatorCode);
        Assert.Equal(7, calculator.Add(3, 4));
    }

    [Fact]
    public void Minus_WorksCorrectly()
    {
        var calculator = CalculatorLoader.CreateCalculator(CalculatorCode);
        Assert.Equal(2, calculator.Minus(5, 3));
    }

    [Fact]
    public void Mul_WorksCorrectly()
    {
        var calculator = CalculatorLoader.CreateCalculator(CalculatorCode);
        Assert.Equal(15, calculator.Mul(3, 5));
    }

    [Fact]
    public void Div_WorksCorrectly()
    {
        var calculator = CalculatorLoader.CreateCalculator(CalculatorCode);
        Assert.Equal(4, calculator.Div(12, 3));
    }

    [Fact]
    public void Div_ByZero_ThrowsDivideByZeroException()
    {
        var calculator = CalculatorLoader.CreateCalculator(CalculatorCode);
        Assert.Throws<DivideByZeroException>(() => calculator.Div(5, 0));
    }
}
