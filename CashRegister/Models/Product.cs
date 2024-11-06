using CashRegister.Interfaces;

namespace CashRegister.Models
{
    public class Product : IFoodProducts
    {
        public int PLU { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public PriceType PriceType { get; set; }

        public Product(int plu, string name, decimal price, PriceType priceType)
        {
            PLU = plu;
            Name = name;
            Price = price;
            PriceType = priceType;
        }

        public decimal GetFoodTaxAmount(decimal total)
        {
            return total * 0.12m;
        }

        public override string ToString()
        {
            return $"{PLU} - {Name}, Pris: {Price}, Typ: {PriceType}";
        }
    }
}
