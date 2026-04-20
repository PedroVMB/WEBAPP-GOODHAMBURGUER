using GoodHamburguer.Application.Services;
using Xunit;

namespace GoodHamburguer.Tests;

public class DiscountCalculatorTests
{
    [Fact]
    public void SandwichOnly_ReturnsZeroDiscount()
    {
        var result = DiscountCalculator.Calculate(true, false, false);
        Assert.Equal(0m, result);
    }

    [Fact]
    public void NoSandwich_ReturnsZeroDiscount()
    {
        var result = DiscountCalculator.Calculate(false, false, false);
        Assert.Equal(0m, result);
    }

    [Fact]
    public void FriesOnlyNoSandwich_ReturnsZeroDiscount()
    {
        var result = DiscountCalculator.Calculate(false, true, false);
        Assert.Equal(0m, result);
    }

    [Fact]
    public void SodaOnlyNoSandwich_ReturnsZeroDiscount()
    {
        var result = DiscountCalculator.Calculate(false, false, true);
        Assert.Equal(0m, result);
    }

    [Fact]
    public void FriesAndSodaNoSandwich_ReturnsZeroDiscount()
    {
        var result = DiscountCalculator.Calculate(false, true, true);
        Assert.Equal(0m, result);
    }

    [Fact]
    public void SandwichAndFries_Returns10PercentDiscount()
    {
        var result = DiscountCalculator.Calculate(true, true, false);
        Assert.Equal(10m, result);
    }

    [Fact]
    public void SandwichAndSoda_Returns15PercentDiscount()
    {
        var result = DiscountCalculator.Calculate(true, false, true);
        Assert.Equal(15m, result);
    }

    [Fact]
    public void SandwichAndFriesAndSoda_Returns20PercentDiscount()
    {
        var result = DiscountCalculator.Calculate(true, true, true);
        Assert.Equal(20m, result);
    }
}

