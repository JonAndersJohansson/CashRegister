using CashRegister.Interfaces;

namespace CashRegister.Models
{
    /// <summary>
    /// Definierar en Product och dess egenskaper.
    /// </summary>
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

        /// <summary>
        /// Metod för att räkna ut moms på livsmedel.
        /// </summary>
        public decimal GetFoodTaxAmount(decimal total)
        {
            return total * 0.12m;
        }

        /// <summary>
        /// Metoden används för att kunna skriva ut Products i menyn via DisplayList.
        /// </summary>
        public override string ToString()
        {
            return $"{PLU} - {Name}, Pris: {Price}, Typ: {PriceType}";
        }
    }
}
