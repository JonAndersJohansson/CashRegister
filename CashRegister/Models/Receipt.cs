using CashRegister.Handlers;
using CashRegister.UI;
using CashRegister.Utilities;

namespace CashRegister.Models
{
    public class Receipt
    {
        private ReceiptFileHandler _receiptFileHandler = new ReceiptFileHandler();
        private ReceiptFormatter _receiptFormatter = new ReceiptFormatter();
        private string _receiptContent;
        private string _receiptFilePath;

        public Receipt(string receiptFilePath)
        {
            _receiptFilePath = receiptFilePath;
        }
        public void SaveReceipt(List<CustomerItems> cart, decimal total)
        {
            string dateString = DateTime.Now.ToString("yyyyMMdd");
            _receiptFilePath = _receiptFileHandler.CreateReceiptFilePath(_receiptFilePath, dateString);
            int receiptNumber = _receiptFileHandler.GetNextReceiptNumber(_receiptFilePath);

            _receiptContent = _receiptFormatter.FormatReceipt(cart, total, receiptNumber);

            _receiptFileHandler.SaveReceiptToFile(_receiptFilePath, _receiptContent);
        }

        public void DisplayReceipt()
        {
            string[] receiptLines = _receiptContent.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            string[] receiptArray = receiptLines.Skip(21).Take(receiptLines.Length - 25).ToArray();
            string receiptToDisplay = string.Join(Environment.NewLine, receiptArray);

            Console.Clear();
            MenuGraphics.ShowMenuGraphics();
            Console.WriteLine("KÖP GENOMFÖRT:");
            Console.WriteLine(receiptToDisplay);
            Console.WriteLine("Kvitto har skrivits ut. Tryck enter för att fortsätta.");
        }
    }
}
