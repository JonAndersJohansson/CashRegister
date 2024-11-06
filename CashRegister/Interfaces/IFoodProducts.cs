namespace CashRegister.Interfaces
{
    public enum PriceType
    {
        Styckpris,
        Kilopris
    }

    public interface IFoodProducts
    {
        PriceType PriceType { get; set; }
        decimal GetFoodTaxAmount(decimal total);
    }
}
