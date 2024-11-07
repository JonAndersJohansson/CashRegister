using CashRegister.Models;
using CashRegister.UI;

namespace CashRegister.Handlers
{
    /// <summary>
    /// Hanterar Discounts genom att lägga till, ta bort eller spara dem.
    /// </summary>
    public class DiscountHandler
    {
        public List<Discount> listOfDiscounts;
        private readonly FileHandler<Discount> _discountFileHandler;
        private readonly DisplayList _displayList;
        private readonly ProductHandler _productHandler;
        private Action _returnToMainMenu;

        public DiscountHandler(FileHandler<Discount> fileHandler, 
            DisplayList displayList, ProductHandler productHandler)
        {
            _discountFileHandler = fileHandler;
            _displayList = displayList;
            _productHandler = productHandler;
            listOfDiscounts = fileHandler.LoadFromFile(ParseDiscount);
        }

        /// <summary>
        /// Tar en sträng från rabattfilen och delar upp den i komponenter för 
        /// att skapa ett Discount-objekt med dess specifika egenskaper. Denna 
        /// metod används som en delegerad funktion i LoadFromFile(). 
        /// Returnerar ett nytt Discount-objekt.
        /// </summary>
        /// <param name="line">En string från filen som innehåller data i form 
        /// av egenskaper för ett Discount-objekt.</param>
        private Discount ParseDiscount(string line)
        {
            string[] parts = line.Split(' ');
            int plu = int.Parse(parts[0]);
            decimal discountAmount = decimal.Parse(parts[1]);
            DiscountType discountType = (DiscountType)Enum.Parse
                (typeof(DiscountType), parts[2]);
            DateTime startDate = DateTime.Parse(parts[3]);
            DateTime endDate = DateTime.Parse(parts[4]);

            return new Discount(plu, discountAmount, discountType, 
                startDate, endDate);
        }

        /// <summary>
        /// Formaterar ett Discount-objekt som en sträng i rätt format för 
        /// lagring i rabattfilen Discounts.txt. Denna metod används som en 
        /// delegerad funktion i SaveToFile(), där varje rabattobjekt 
        /// konverteras till strängformat för att skrivas till filen.
        /// </summary>
        private string FormatDiscount(Discount discount)
        {
            return $"{discount.PLU} {discount.DiscountNumber} " +
                $"{discount.DiscountType} {discount.StartDate:yyyy-MM-dd} " +
                $"{discount.EndDate:yyyy-MM-dd}";
        }

        /// <summary>
        /// Sorterar listOfDiscounts efter PLU, använder rätt filePath genom
        /// _discountFileHandler. Sparar nuvarnade rabatter i listOfDiscounts 
        /// till Discounts.txt genom skicka med metoden FormatDiscount i 
        /// metoden SaveToFile.
        /// </summary>
        public void SaveDiscounts()
        {
            listOfDiscounts.Sort((x, y) => x.PLU.CompareTo(y.PLU));

            _discountFileHandler.SaveToFile(listOfDiscounts, FormatDiscount);
        }

        /// <summary>
        /// Skapar ett nytt Discount-objekt genom användarens input ges den 
        /// egenskaperna PLU, DiscountType, DiscountAmount, StartDate, EndDate.
        /// </summary>
        public void AddDiscount()
        {
            int selectedIndex = _displayList.BrowseAList
                (_productHandler.listOfProducts, false);
            Product selectedProduct = 
                _productHandler.listOfProducts[selectedIndex];

            int plu = selectedProduct.PLU;
            DiscountType newDiscountType;
            decimal newDiscountAmountInput;
            DateTime newStartDateInput;
            DateTime newEndDateInput;

            while (true)
            {
                Console.Clear();
                MenuGraphics.ShowMenuGraphics();

                // PLU Input
                Console.WriteLine($"Du har valt: {selectedProduct}\n");

                // DiscountType Input
                Console.WriteLine("Ange önskad rabatttyp, 1 eller 2 och " +
                    "tryck Enter\n1. Procent\n2. Fast belopp");
                string newDiscountTypeInput = Console.ReadLine();
                if (newDiscountTypeInput == "1" || newDiscountTypeInput == "2")
                {
                    if (newDiscountTypeInput == "1")
                        newDiscountType = DiscountType.Procent;
                    else
                        newDiscountType = DiscountType.FastBelopp;
                }
                else
                {
                    Console.WriteLine("Fel värde, välj 1 eller 2. " +
                        "Försök igen.");
                    Thread.Sleep(2000);
                    continue;
                }

                // DiscountNumber Input
                Console.WriteLine("Ange önskad rabatt. Exempel: 20");
                if (!decimal.TryParse(Console.ReadLine(), out 
                    newDiscountAmountInput) || newDiscountAmountInput < 0)
                {
                    Console.WriteLine("Felaktigt format på rabatt, " +
                        "försök igen.");
                    Thread.Sleep(2000);
                    continue;
                }

                // StartDate Input
                Console.WriteLine("Ange startdatum (åååå-mm-dd):");
                if (!DateTime.TryParse(Console.ReadLine(), out 
                    newStartDateInput))
                {
                    Console.WriteLine("Felaktigt format på datum, " +
                        "försök igen.");
                    Thread.Sleep(2000);
                    continue;
                }

                // EndDate Input
                Console.WriteLine("Ange slutdatum (åååå-mm-dd):");
                if (!DateTime.TryParse(Console.ReadLine(), out 
                    newEndDateInput))
                {
                    Console.WriteLine("Felaktigt format på datum, " +
                        "försök igen.");
                    Thread.Sleep(2000);
                    continue;
                }
                if (newStartDateInput > newEndDateInput)
                {
                    Console.WriteLine("Startdatumet är efter slutdatumet. " +
                        "Försök igen.");
                    Thread.Sleep(2000);
                    continue;
                }
                break;
            }

            Discount newDiscount = new Discount(plu, newDiscountAmountInput, 
                newDiscountType, newStartDateInput, newEndDateInput);
            listOfDiscounts.Add(newDiscount);

            Console.Clear();
            MenuGraphics.ShowMenuGraphics();

            Console.WriteLine($"Rabatt tillagd:\n\n{newDiscount}\n\n" +
                $"Tryck på valfri tangent för att återgå till huvudmenyn.");
            SaveDiscounts();

            Console.ReadKey();
            _returnToMainMenu.Invoke();
        }

        /// <summary>
        /// En metod för att kunna ta bort en befintlig rabatt och därefter 
        /// uppdatera rabattlistan och spara den till Discounts.txt.
        /// </summary>
        public void RemoveDiscount()
        {
            int selectedIndex = _displayList.BrowseAList
                (listOfDiscounts, false);
            Discount selectedDiscount = listOfDiscounts[selectedIndex];

            Console.Clear();
            MenuGraphics.ShowMenuGraphics();
            Console.WriteLine($"Följande rabatt är borttagen:\n\n" +
                $"{selectedDiscount}\n\nTryck på valfri tangent för att " +
                $"återgå till huvudmenyn.");

            listOfDiscounts.RemoveAt(selectedIndex);
            SaveDiscounts();

            Console.ReadKey();
            _returnToMainMenu.Invoke();
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
