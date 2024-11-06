using CashRegister.Models;
using CashRegister.UI;

namespace CashRegister.Handlers
{
    /// <summary>
    /// Hanterar kassörens input och en shopping session.
    /// </summary>
    public class CustomerHandler
    {
        private List<CustomerItems> _cart;
        private readonly List<Product> _listOfProducts;
        private readonly List<Discount> _listOfDiscounts;
        private Receipt _receipt;
        private Action _returnToMainMenu;

        public CustomerHandler(List<Product> listOfProducts, List<Discount> 
            listOfDiscounts, Receipt receipt)
        {
            _cart = new List<CustomerItems>();
            _listOfDiscounts = listOfDiscounts;
            _listOfProducts = listOfProducts;
            _receipt = receipt;
        }

        /// <summary>
        /// Startar kundens shopping-session, hanterar kommandon och avslutar
        /// vid betalning.
        /// </summary>
        public void StartShopping()
        {
            Customer standardCustomer = new();
            DateTime currentDate = DateTime.Now;

            bool shopping = true;
            while (shopping)
            {
                Console.Clear();
                MenuGraphics.ShowMenuGraphics();

                Console.WriteLine("Kommandon:\n<productid> <antal>\nPAY");
                string command = Console.ReadLine();

                if (command.ToLower() == "pay")
                {
                    decimal total = ProcessCart(currentDate);
                    _receipt.SaveReceipt(_cart, total); 
                    _receipt.DisplayReceipt();
                    shopping = false;
                    Console.ReadKey();
                    _returnToMainMenu.Invoke();
                }
                else
                {
                    ProcessCommand(command);
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// Beräknar totalpriset på varukorgen, inklusive tillämpliga rabatter.
        /// </summary>
        private decimal ProcessCart(DateTime currentDate)
        {
            decimal total = 0;

            foreach (var item in _cart)
                total += item.CalculateToPrice(currentDate, _listOfDiscounts);

            return total;
        }

        /// <summary>
        /// Behandlar användarkommando för att lägga till produkt i 
        /// varukorgen.
        /// </summary>
        private void ProcessCommand(string command)
        {
            var parts = command.Split(' ');
            if (parts.Length == 2 && int.TryParse(parts[0], out int 
                inputProductPLU) && int.TryParse(parts[1], out int 
                inputProductQuantity))
            {
                var product = _listOfProducts.FirstOrDefault
                    (p => p.PLU == inputProductPLU); 

                if (product != null)
                {
                    AddToCart(product, inputProductQuantity);
                    Console.WriteLine($"{inputProductQuantity} x" +
                        $" {product.Name} tillagd i varukorgen.");
                    Console.Beep();
                }
                else
                {
                    Console.WriteLine("PLU kunde inte hittas, försök igen.");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt kommando.");
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Lägger till en produkt i varukorgen eller uppdaterar 
        /// "inputProductQuantity" om produkten redan finns.
        /// </summary>
        private void AddToCart(Product product, int quantity)
        {
            var existingProductInCart = _cart.FirstOrDefault
                (c => c.Product.PLU == product.PLU);

            if (existingProductInCart != null)
                existingProductInCart.Quantity += quantity;

            else
                _cart.Add(new CustomerItems(product, quantity));
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
