namespace CashRegister.UI
{
    /// <summary>
    /// Statisk klass som hanterar grafik.
    /// </summary>
    static class MenuGraphics
    {
        /// <summary>
        /// Statisk metod som skriver ut grafik i konsollen.
        /// </summary>
        public static void ShowMenuGraphics()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("             ██╗  ██╗ ██████╗  ██████╗ ██████╗ ");
            Console.WriteLine("             ██║ ██╔╝██╔═══██╗██╔═══██╗██╔══██╗");
            Console.WriteLine("             █████╔╝ ██║   ██║██║   ██║██████╔╝");
            Console.WriteLine("             ██╔═██╗ ██║   ██║██║   ██║██╔═══╝ ");
            Console.WriteLine("             ██║  ██╗╚██████╔╝╚██████╔╝██║     ");
            Console.WriteLine("             ╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚═╝  ®  ");
            Console.WriteLine("                    ~   Örnsköldsvik   ~");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }
    }
}
