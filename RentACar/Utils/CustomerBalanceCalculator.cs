namespace RentACar.Utils;

public static class CustomerBalanceCalculator
{
    public static double CalculateBalanceAfterReservation(double balanceBefore, double pricePerDay, int days)
    {
        return balanceBefore - pricePerDay * days;
    }
    
    public static double CalculateBalanceAfterCancelation(double balanceBefore, double pricePerDay, int days)
    {
        // 25 percent fee
        return balanceBefore + 0.75 * pricePerDay * days;
    }
}