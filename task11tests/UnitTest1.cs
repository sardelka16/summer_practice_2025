namespace task11tests;

using task11;
public class CalculatorMakerTests
{
    [Fact]
    public void MakeCalculator_ReturnsValidCalculatorInstance()
    {
        var calculatorMaker = new CalculatorMaker();
        var calculator = calculatorMaker.MakeCalculator();

        Assert.NotNull(calculator);
    }

    [Fact]
    public void MakeCalculator_ReturnsCorrectAddResult()
    {
        var calculatorMaker = new CalculatorMaker();
        var calculator = calculatorMaker.MakeCalculator();
        var addResult = calculator.Add(11, 5);

        Assert.Equal(16, addResult);
    }

    [Fact]
    public void MakeCalculator_ReturnsCorrectMinusResult()
    {
        var calculatorMaker = new CalculatorMaker();
        var calculator = calculatorMaker.MakeCalculator();
        var minusResult = calculator.Minus(11, 5);

        Assert.Equal(6, minusResult);
    }

    [Fact]
    public void MakeCalculator_ReturnsCorrectMulResult()
    {
        var calculatorMaker = new CalculatorMaker();
        var calculator = calculatorMaker.MakeCalculator();
        var mulResult = calculator.Mul(11, 5);

        Assert.Equal(55, mulResult);
    }
    
    [Fact]
    public void MakeCalculator_ReturnsCorrectDivResult()
    {
        var calculatorMaker = new CalculatorMaker();
        var calculator = calculatorMaker.MakeCalculator();
        var divResult = calculator.Div(11, 5);

        Assert.Equal(2 ,divResult);
    }
}
