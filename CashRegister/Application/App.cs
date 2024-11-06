using CashRegister.Handlers;
using CashRegister.Models;
using CashRegister.UI;
using CashRegister.Utilities;

namespace CashRegister.Application
{
    /// <summary>
    /// Programmets start där de flesta instanser av klasserna instanseras 
    /// och injiceras vidare in i andra klassers konstruktorer.
    /// </summary>
    public class App
    {
        private readonly ProductHandler _productHandler;
        private readonly DiscountHandler _discountHandler;
        private readonly CustomerHandler _customerHandler;
        private readonly Menus _menus;
        private readonly DisplayList _displayList;
        private readonly FileHandler<Product> _productFileHandler;
        private readonly FileHandler<Discount> _discountFileHandler;
        private readonly Receipt _receipt;
        

        public App()
        {
            _productFileHandler = new FileHandler<Product>(FilePathManager.ProductsFilePath);
            _discountFileHandler = new FileHandler<Discount>(FilePathManager.DiscountsFilePath);

            _displayList = new DisplayList();
            _productHandler = new ProductHandler(_productFileHandler, _displayList);
            _discountHandler = new DiscountHandler(_discountFileHandler, _displayList, _productHandler);
            _receipt = new Receipt(FilePathManager.ReceiptFilePath);
            _customerHandler = new CustomerHandler(_productHandler.listOfProducts, _discountHandler.listOfDiscounts, _receipt);
            _menus = new Menus(_displayList, _productHandler, _discountHandler, _customerHandler);

            _productHandler.ReturnToMainMenu(_menus.MainMenu);
            _discountHandler.ReturnToMainMenu(_menus.MainMenu);
            _displayList.ReturnToMainMenu(_menus.MainMenu);
            _customerHandler.ReturnToMainMenu(_menus.MainMenu);
        }

        /// <summary>
        /// Startar programmet.
        /// </summary>
        public void Run()
        {
            _menus.MainMenu();
        }
    }
}
