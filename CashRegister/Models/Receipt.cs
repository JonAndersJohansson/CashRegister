using CashRegister.Handlers;
using CashRegister.UI;
using CashRegister.Utilities;

namespace CashRegister.Models
{
    /// <summary>
    /// Representerar ett kvitto som hanterar formatering, lagring och visning 
    /// av kvittoinformation.
    /// </summary>
    public class Receipt
    {
        private ReceiptFileHandler _receiptFileHandler = 
            new ReceiptFileHandler();
        private ReceiptFormatter _receiptFormatter = new ReceiptFormatter();
        private string _receiptContent;
        private string _receiptFilePath;

        public Receipt(string receiptFilePath)
        {
            _receiptFilePath = receiptFilePath;
        }

        /// <summary>
        /// Genererar och sparar kvittot för de köpta varorna och det totala 
        /// beloppet till en textfil.
        /// </summary>
        /// <param name="cart">En lista över kundens köpta artiklar.</param>
        /// <param name="total">Det totala beloppet för köpet.</param>
        public void SaveReceipt(List<CustomerItems> cart, decimal total)
        {
            string dateString = DateTime.Now.ToString("yyyyMMdd");
            string fullFilePath = _receiptFileHandler.CreateReceiptFilePath
                (_receiptFilePath, dateString);
            int receiptNumber = _receiptFileHandler.GetNextReceiptNumber
                (fullFilePath);

            _receiptContent = _receiptFormatter.FormatReceipt(cart, total, 
                receiptNumber);
            _receiptFileHandler.SaveReceiptToFile(fullFilePath, 
                _receiptContent);
        }

        /// <summary>
        /// Visar kvittot på skärmen med en formaterad layout och grafik.
        /// </summary>
        public void DisplayReceipt()
        {
            string[] receiptLines = _receiptContent.Split(new[] 
            { Environment.NewLine }, StringSplitOptions.None);
            string[] receiptArray = receiptLines.Skip(21).
                Take(receiptLines.Length - 25).ToArray();
            string receiptToDisplay = string.Join(Environment.NewLine, 
                receiptArray);

            Console.Clear();
            MenuGraphics.ShowMenuGraphics();
            Console.WriteLine("KÖP GENOMFÖRT:");
            Console.WriteLine(receiptToDisplay);
            Console.WriteLine("Kvitto har skrivits ut. " +
                "Tryck enter för att fortsätta.");
        }
    }
}
