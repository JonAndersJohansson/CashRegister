using CashRegister.UI;

namespace CashRegister.Handlers
{
    /// <summary>
    /// En generisk klass för att hantera filinläsning och -skrivning av olika typer av objekt.
    /// </summary>
    /// <typeparam name="T">Typen av objekt som ska sparas och läsas in från filen.</typeparam>
    public class FileHandler<T>
    {
        private string _filePath;

        public FileHandler(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Metoden skapar och returnerar en generisk lista genom att hämta data från en fil.
        /// </summary>
        /// <param name="formatFromFile">En delegerad funktion som tar in en "string", formatterar och returnerar den som ett generiskt objekt "T".</param>
        /// <returns></returns>
        public List<T> LoadFromFile(Func<string, T> formatFromFile)
        {
            var loadedList = new List<T>();

            if (!File.Exists(_filePath))
            {
                MenuGraphics.ShowMenuGraphics();

                Console.WriteLine($"ERROR: En viktig datafil saknas för att " +
                    $"programmet skall\nkunna fungera. Vi ber om ursäkt för " +
                    $"detta.\n\nProgrammet kommer att avslutas. När du " +
                    $"startar programmet\nigen kommer en ny tom datafil att " +
                    $"ha skapats på filväg:\n{_filePath}\nDetta möjliggör " +
                    $"att programmet kan användas men viktig data kan " +
                    $"fortfarande saknas. Om detta är ett problem, " +
                    $"kontakta\ntekniker på telnr: 0703792074\n\nTryck " +
                    $"valfri tangent för att avsluta programmet.");
                File.Create(_filePath).Dispose(); 
                Console.ReadKey();
                Environment.Exit(0);
            }

            string[] lines = File.ReadAllLines(_filePath);
            foreach (string line in lines)
            {
                T item = formatFromFile(line);
                loadedList.Add(item);
            }
            return loadedList;
        }

        /// <summary>
        /// Metoden sparar en lista till den specifika filen med hjälp av StreamWriter. 
        /// Tar en generisk lista, som listOfProducts eller listOfDiscounts, samt en delegerad metod för formatering.
        /// </summary>
        /// <param name="listOfObjects">En generisk lista av objekt som ska sparas i filen.</param>
        /// <param name="formatToFile">En delegerad funktion som formaterar varje objekt korrekt till filen som en sträng.</param>
        public void SaveToFile(List<T> listOfObjects, Func<T, string> formatToFile)
        {
            using (StreamWriter writer = new StreamWriter(_filePath))
            {
                foreach (T item in listOfObjects)
                    writer.WriteLine(formatToFile(item));
            }
        }
    }
}
