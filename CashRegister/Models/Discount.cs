namespace CashRegister.Models
{
    public enum DiscountType
    {
        Procent,
        FastBelopp
    }

    /// <summary>
    /// Definierar vad en rabatt (Discount) är. Hanterar kontroll av 
    /// rabatternas giltighetsdatum.
    /// </summary>
    public class Discount
    {
        public int PLU { get; set; }
        public decimal DiscountNumber { get; set; }
        public DiscountType DiscountType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Discount(int plu, decimal discountNumber, DiscountType discountType, DateTime startDate, DateTime endDate)
        {
            PLU = plu;
            DiscountNumber = discountNumber;
            DiscountType = discountType;
            StartDate = startDate;
            EndDate = endDate;
        }

        /// <summary>
        /// Metoden tar in ett datum som en parameter och kontrollerar det mot 
        /// StartDate och EndDate
        /// </summary>
        public bool IsValid(DateTime date)
        {
            return date >= StartDate && date <= EndDate;
        }

        /// <summary>
        /// Metoden används för att kunna skriva ut Discounts i menyn via DisplayList.
        /// </summary>
        public override string ToString()
        {
            return $"PLU:{PLU}, Rabatt:{DiscountNumber}/{DiscountType}, " +
                $"{StartDate:yy-MM-dd} tom {EndDate:yy-MM-dd}";
        }
    }
}
