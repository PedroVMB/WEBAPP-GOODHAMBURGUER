namespace GoodHamburguer.Application.Services;

public static class DiscountCalculator
{
    public static decimal Calculate(bool hasSandwich, bool hasFries, bool hasSoda)
    {
        if (!hasSandwich)
            return 0m;

        if (hasFries && hasSoda)
            return 20m;

        if (hasSoda)
            return 15m;

        if (hasFries)
            return 10m;

        return 0m;
    }
}

