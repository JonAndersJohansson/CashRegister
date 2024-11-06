using CashRegister.Handlers;

namespace CashRegister.UI
{
    /// <summary>
    /// Klassen hanterar menyerna MainMenu och AdminMenu.
    /// </summary>
    public class Menus
    {
        private readonly DisplayList _displayList;
        private readonly ProductHandler _productHandler;
        private readonly DiscountHandler _discountHandler;
        private readonly CustomerHandler _customerHandler;
        public Menus(DisplayList displayList, ProductHandler productHandeling, 
            DiscountHandler discountHandeling, CustomerHandler customerHandler)
        {
            _displayList = displayList;
            _productHandler = productHandeling;
            _discountHandler = discountHandeling;
            _customerHandler = customerHandler;
        }

        public readonly List<string> listMainMenu = new List<string>
        {
        "1. Ny kund", "2. Administratörverktyg", "Avsluta"
        };
        public readonly List<string> listAdminMenu = new List<string>
        {
        "1. Ändra befintlig produkt", "2. Lägg till ny produkt", "3. Ta bort " +
            "produkt", "4. Lägg till ny kampanj", "5. Ta bort kampanj"
        };

        /// <summary>
        /// Metoden ger användaren alternativen i huvudmenyn genom DisplayList.
        /// </summary>
        public void MainMenu()
        {
            switch (_displayList.BrowseAList(listMainMenu, true))
            {
                case 0:
                    _customerHandler.StartShopping();
                    break;
                case 1:
                    AdminMenu();
                    break;
                case 2:
                    Environment.Exit(0);
                    return;
                default:
                    Console.WriteLine("Ogiltigt alternativ MainMenu, försök igen.");
                    Thread.Sleep(2000);
                    break;
            }
        }

        /// <summary>
        /// Metoden ger användaren alternativen i admin-menyn genom 
        /// DisplayList.
        /// </summary>
        public void AdminMenu()
        {
            switch (_displayList.BrowseAList(listAdminMenu, false))
            {
                case 0:
                    _productHandler.ChangeProduct();
                    break;
                case 1:
                    _productHandler.AddProduct();
                    break;
                case 2:
                    _productHandler.RemoveProduct();
                    break;
                case 3:
                    _discountHandler.AddDiscount();
                    break;
                case 4:
                    _discountHandler.RemoveDiscount();
                    break;
                default:
                    Console.WriteLine("Ogiltigt alternativ AdminMenu, försök igen.");
                    Thread.Sleep(2000);
                    break;
            }
        }
    }
}
