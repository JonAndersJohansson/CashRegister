using CashRegister.Models;
using System.Text;

namespace CashRegister.Utilities
{
    /// <summary>
    /// Klassen hanterar uträkningar samt kvittots grafik och returnerar allt 
    /// som en string med hjälp av StringBuilder.
    /// </summary>
    public class ReceiptFormatter
    {
        public string FormatReceipt(List<CustomerItems> cart, decimal total, 
            int receiptNumber)
        {
            StringBuilder receipt = new StringBuilder();
            decimal totalTaxAmount = 0;
            decimal totalRounded = Math.Round(total);
            decimal roundedOfAmount = totalRounded - total;

            receipt.AppendLine("");
            receipt.AppendLine("██╗  ██╗  ██████╗  ██████╗  ██████╗ ");
            receipt.AppendLine("██║ ██╔╝██╔═══██╗██╔═══██╗██╔══██╗");
            receipt.AppendLine("█████╔╝ ██║    ██║██║    ██║██████╔╝");
            receipt.AppendLine("██╔═██╗ ██║    ██║██║    ██║██╔═══╝ ");
            receipt.AppendLine("██║  ██╗╚██████╔╝╚██████╔╝██║     ");
            receipt.AppendLine("╚═╝  ╚═╝ ╚═════╝  ╚═════╝  ╚═╝  ®  ");
            receipt.AppendLine("                Lillgatan 7");
            receipt.AppendLine("            89242 Örnsköldsvik");
            receipt.AppendLine("             Tel: 0660-63729");
            receipt.AppendLine("            Orgnr: 5612147854");
            receipt.AppendLine("               www.koop.kom");
            receipt.AppendLine("");
            receipt.AppendLine("                  KVITTO");
            receipt.AppendLine("");
            receipt.AppendLine($"Kvittonr: {receiptNumber}");
            receipt.AppendLine($"Datum: {DateTime.Now}");
            receipt.AppendLine("");
            receipt.AppendLine("-----------------------------------------");
            receipt.AppendLine("Artikel       Antal   a-pris      Total ");
            receipt.AppendLine("-----------------------------------------");

            foreach (var item in cart)
            {
                Product product = item.Product;
                decimal originalLineTotal = product.Price * item.Quantity;
                decimal lineTotal = originalLineTotal - item.DiscountAmount;
                decimal taxAmount = product.GetFoodTaxAmount(lineTotal);

                receipt.AppendFormat("{0,-15} {1,-3}* {2,6} {3,13:C}\n", 
                    product.Name, item.Quantity, product.Price, lineTotal);

                if (item.DiscountAmount > 0)
                {
                    receipt.AppendFormat($"      erhållen rabatt: " +
                        $"{item.DiscountAmount:C}\n");
                    
                }
                totalTaxAmount = taxAmount + totalTaxAmount;
            }
            receipt.AppendLine("");
            receipt.AppendFormat("Öresutjämning:{0,27:C}\n", roundedOfAmount);
            receipt.AppendLine("-----------------------------------------");
            receipt.AppendFormat("Totalt:{0,34:C}\n", totalRounded);
            receipt.AppendFormat("varav moms:{0,30:C}\n", totalTaxAmount);
            receipt.AppendLine("");
            receipt.AppendLine("");
            receipt.AppendLine("               VÄLKOMMER ÅTER");
            receipt.AppendLine("");
            receipt.AppendLine("~~~~~~~~~~~~~~~~~RIV~AV~HÄR~~~~~~~~~~~~~~~~~");

            return receipt.ToString();
        }
    }
}
