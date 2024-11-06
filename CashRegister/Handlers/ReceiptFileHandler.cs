namespace CashRegister.Handlers
{
    /// <summary>
    /// Hanterar kvittons filnamn, mappstruktur, hittar nästa kvittonummer samt
    /// sparar ner datan till kvittofilen.
    /// </summary>
    public class ReceiptFileHandler
    {
        /// <summary>
        /// Skapar en .txt-fil på rätt plats med rätt namn och dagens datum 
        /// genom att kombinera receiptFilePath och fileName. Om filen inte 
        /// finns så skapas den.
        /// </summary>
        public string CreateReceiptFilePath(string receiptFilePath, 
            string dateString)
        {
            string fileName = $"RECEIPT_{dateString}.txt";
            return Path.Combine(receiptFilePath, fileName);
        }

        /// <summary>
        /// Hittar senast använda kvittonummer genom att söka igenom varje rad
        /// i kvitto-filen efter ordet "Kvittonr:". Om den inte hittas blir 
        /// nummret "1" annars tar den det senaste numret och ökar med 1.
        /// </summary>
        public int GetNextReceiptNumber(string filePath)
        {
            int updatedReceiptNumber = 0;

            if (File.Exists(filePath))
            {
                var lines = File.ReadAllLines(filePath);

                foreach (var line in lines)
                {
                    if (line.StartsWith("Kvittonr:"))
                    {
                        var parts = line.Split(':');
                        if (parts.Length > 1 && int.TryParse(parts[1].Trim(),
                            out int currentReceiptNumber))
                        {
                            if (updatedReceiptNumber <= currentReceiptNumber)
                                updatedReceiptNumber = currentReceiptNumber;  
                        }
                    }
                }
            }
            return updatedReceiptNumber + 1;
        }

        /// <summary>
        /// Skapar filen om den inte finns och skriver allt från receiptContent
        /// på den via StreamWriter append.
        /// </summary>
        public void SaveReceiptToFile(string filePath, string receiptContent)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.Write(receiptContent);
            }
        }
    }
}
