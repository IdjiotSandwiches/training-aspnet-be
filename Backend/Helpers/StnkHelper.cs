namespace Backend.Helpers
{
    public class StnkHelper
    {
        public decimal CalculateTax(decimal carPrice, int carTypeTax, int engineSizeTax, int progresiveTax)
        {
            return (carPrice * carTypeTax / 100) + (carPrice * engineSizeTax / 100) + (carPrice * progresiveTax * 0.5m);
        }
    }
}
