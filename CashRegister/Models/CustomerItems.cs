namespace CashRegister.Models
{
    /// <summary>
    /// Tar kundens valda Products och definierar dem efter mängd och rabatter.
    /// Räknar ut priset för varje produkt med dessa egenskaper.
    /// </summary>
    public class CustomerItems
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountAmount { get; private set; }

        public CustomerItems(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
            DiscountAmount = 0;
        }

        /// <summary>
        /// Räknar ur och returnerar totalPrice.
        /// </summary>
        public decimal CalculateToPrice(DateTime currentDate, 
            List<Discount> listOfDiscount)
        {
            decimal totalPrice = Product.Price * Quantity;
            List<Discount> applicableDiscounts = GetApplicableDiscounts
                (currentDate, listOfDiscount);

            if (applicableDiscounts.Any())
            {
                DiscountAmount = CalculateDiscount(applicableDiscounts, 
                    totalPrice);
                totalPrice -= DiscountAmount;
            }
            return totalPrice;
        }

        /// <summary>
        /// Räknar ut och returnerar discountAmount beroende på vilken 
        /// rabattyp som gäller. Säkerställer att rabatten inte är större än priset.
        /// </summary>
        private decimal CalculateDiscount(List<Discount> applicableDiscounts, 
            decimal totalPrice)
        {
            decimal discountAmount = 0;

            foreach (var discount in applicableDiscounts)
            {
                if (discount.DiscountType == DiscountType.Procent)
                    discountAmount += totalPrice * 
                        (discount.DiscountNumber / 100);

                else if (discount.DiscountType == DiscountType.FastBelopp)
                    discountAmount += discount.DiscountNumber * Quantity;
            }
            if (discountAmount > totalPrice)
                discountAmount = totalPrice;

            return discountAmount;
        }

        /// <summary>
        /// Går igenom varje rabatt och kontrollerar om den är applicerbar på 
        /// produkten genom att jämföra PLU-nummer och datum.
        /// </summary>
        private List<Discount> GetApplicableDiscounts(DateTime currentDate, 
            List<Discount> listOfDiscount)
        {
            var applicableDiscounts = new List<Discount>();

            foreach (var discount in listOfDiscount)
            {
                if (discount.PLU == Product.PLU && discount.IsValid(currentDate))
                    applicableDiscounts.Add(discount);
            }
            return applicableDiscounts;
        }
    }
}
