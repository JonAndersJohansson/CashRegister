using CashRegister.Models;

namespace CashRegister.UI
{
    /// <summary>
    /// Klassen hanterar listor genom att låta användaren stega igenom dem 
    /// och välja ett index.
    /// </summary>
    public class DisplayList
    {
        private Action _returnToMainMenu;

        /// <summary>
        /// Låter användaren stega igenom en lista och välja ett index med 
        /// felhantering. Lägger även till "Huvudmeny" som val ifall inte 
        /// huvudmenyn är den lista som används.
        /// </summary>
        /// <param name="lista">Generisk lista</param>
        /// <param name="isMainMenu">En flagga som visar om valet "Huvudmeny" 
        /// skall användas i menyn.</param>
        public int BrowseAList<T>(List<T> lista, bool isMainMenu)
        {
            int newIndex = 0;
            int selectedIndex = 0;
            bool userInputIsUnsatisfying = true;

            while (userInputIsUnsatisfying)
            {
                Console.Clear();
                MenuGraphics.ShowMenuGraphics();

                ShowHeader(lista);

                DisplayItems(lista, selectedIndex);

                if (!isMainMenu)
                    ShowMainMenuButton(selectedIndex == lista.Count);

                while (userInputIsUnsatisfying)
                {
                    newIndex = HandleUserInput(lista, selectedIndex,
                    isMainMenu);

                    if (newIndex != -1)
                        break;
                }
                if (newIndex != selectedIndex)
                    selectedIndex = newIndex;
                else 
                    userInputIsUnsatisfying = false;

            }
            return selectedIndex;
        }

        /// <summary>
        /// Visar rätt meddelande till användaren beroende på vilken typ av 
        /// lista som används.
        /// </summary>
        private void ShowHeader<T>(List<T> lista)
        {
            if (lista is List<Product>)
                Console.WriteLine("Välj produkt, pil upp/ned, " +
                    "och tryck Enter:");

            else if (lista is List<Discount>)
                Console.WriteLine("Välj rabatt, pil upp/ned, " +
                    "och tryck Enter:");

            else
                Console.WriteLine("Använd upp/ned piltangenterna " +
                    "och tryck Enter:");
            Console.WriteLine();
        }

        /// <summary>
        /// Skriver ut varje index i listan som en string. Valt 
        /// index markeras med egen färg för att visa att det är "markerat". 
        /// Om det finns ett index som har innehållet "Avsluta" så skrivs 
        /// det ut i röd färg.
        /// </summary>
        /// <param name="lista">En generisk lista</param>
        /// <param name="selectedIndex">Det markerade indexet</param>
        private void DisplayItems<T>(List<T> lista, int selectedIndex)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else if (lista[i].ToString() == "Avsluta")
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ResetColor(); 

                Console.WriteLine(lista[i].ToString());
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Om listan INTE är listMainMenu så skall valet "Huvudmeny" finnas
        /// längst ned som ett val. 
        /// </summary>
        /// <param name="isSelected">Boolskt utryck för om "Huvudmeny" är 
        /// markerad.</param>
        private void ShowMainMenuButton(bool isSelected)
        {
            if (isSelected)
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
                Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine("Huvudmeny");
            Console.ResetColor();
        }

        /// <summary>
        /// Metoden hanterar använarens input i form av tangenttryckningarna 
        /// upp/ned/enter.
        /// </summary>
        /// <param name="lista">Generisk lista</param>
        /// <param name="selectedIndex">Valt index</param>
        /// <param name="isMainMenu">FLagga för om listan är 
        /// listMainMenu</param>
        private int HandleUserInput<T>(List<T> lista, int selectedIndex,
            bool isMainMenu)
        {
            var keyInfo = Console.ReadKey(true);

            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedIndex--;

                if (selectedIndex < 0)
                    selectedIndex = isMainMenu ? lista.Count - 1 : lista.Count;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedIndex++;

                if (selectedIndex > (isMainMenu ? lista.Count - 1 : lista.Count))
                    selectedIndex = 0;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (!isMainMenu && selectedIndex == lista.Count)
                    _returnToMainMenu.Invoke();
                else
                    return selectedIndex;
            }
            else if (keyInfo.Key == ConsoleKey.Escape)
                _returnToMainMenu.Invoke();
            else
                return -1;

            return selectedIndex;
        }

        /// <summary>
        /// Registrerar en metod för att återgå till huvudmenyn.
        /// </summary>
        public void ReturnToMainMenu(Action mainMenu)
        {
            _returnToMainMenu = mainMenu;
        }
    }
}
