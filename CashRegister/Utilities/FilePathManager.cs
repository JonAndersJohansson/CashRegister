namespace CashRegister.Utilities
{
    /// <summary>
    /// Klassen hanterar programmets datafilers filvägar.
    /// </summary>
    public static class FilePathManager
    {
        public static string ProductsFilePath => "../../../Data/Products.txt";
        public static string DiscountsFilePath => "../../../Data/Discounts.txt";
        public static string ReceiptFilePath => "../../../Receipts";
    }
}
