using CashRegister.Interfaces;
using CashRegister.Models;
using CashRegister.UI;

namespace CashRegister.Handlers
{
    public class ProductHandler
    {
        public List<Product> listOfProducts;
        private FileHandler<Product> _productFileHandler;
        private DisplayList _displayList;
        private Action _returnToMainMenu;

        public ProductHandler(FileHandler<Product> fileHandler, DisplayList displayList)
        {
            _productFileHandler = fileHandler;
            _displayList = displayList;
            listOfProducts = fileHandler.LoadFromFile(ParseProduct);
        }

        /// <summary>
        /// Tar en sträng från produktfilen och delar upp den i komponenter för att skapa ett 
        /// Product-objekt med dess specifika egenskaper. Returnerar ett nytt Product-objekt med värden som är extraherade från strängen
        /// </summary>
        /// <param name="line">En sträng från filen som innehåller egenskaper för ett Product-objekt.</param>
        private Product ParseProduct(string line)
        {
            string[] parts = line.Split(' ');
            int pluNumber = int.Parse(parts[0]);
            string name = parts[1];
            decimal price = decimal.Parse(parts[2]);
            PriceType priceType = (PriceType)Enum.Parse(typeof(PriceType), parts[3]);

            return new Product(pluNumber, name, price, priceType);
        }

        /// <summary>
        /// Formaterar ett Product-objekt som en sträng i rätt format för 
        /// lagring i rabattfilen Products.txt. Denna metod används som en 
        /// delegerad funktion i SaveToFile(), där varje rabattobjekt 
        /// konverteras till strängformat för att skrivas till filen.
        /// </summary>
        private string FormatProduct(Product product)
        {
            return $"{product.PLU} {product.Name} {product.Price} {product.PriceType}";
        }

        /// <summary>
        /// Sparar nuvarnade produkter i listOfProducts till Products.txt
        /// </summary>
        public void SaveProducts()
        {
            listOfProducts.Sort((x, y) => x.PLU.CompareTo(y.PLU));

            _productFileHandler.SaveToFile(listOfProducts, FormatProduct);
        }

        /// <summary>
        /// Gör det möjligt för användaren att ändra en befintlig produkt.
        /// </summary>
        public void ChangeProduct()
        {
            int index = _displayList.BrowseAList(listOfProducts, false);
            Product selectedProduct = listOfProducts[index]; 

            while (true)
            {
                Console.Clear();
                MenuGraphics.ShowMenuGraphics();

                Console.Write("Du har valt:\n");
                Console.WriteLine($"{selectedProduct}\n");

                // PLU Input
                Console.Write("Ange nytt PLU eller tryck Enter för att behålla " +
                    "nuvarande:\n");
                string newPluInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newPluInput) && int.TryParse(newPluInput,
                    out int newPlu))
                {
                    if (listOfProducts.Any(p => p.PLU == newPlu))
                    {
                        Console.WriteLine("Det angivna PLU-numret är redan " +
                            "upptaget. Försök igen.");
                        Thread.Sleep(2000);
                        continue;
                    }
                    selectedProduct.PLU = newPlu;
                }
                else if (string.IsNullOrEmpty(newPluInput))
                {
                    newPlu = selectedProduct.PLU;
                    Console.WriteLine("Du behöll PLU-numret.");
                }
                else
                {
                    Console.WriteLine("Felaktigt värde av PLU, exempel: 125. Försök igen");
                    Thread.Sleep(2000);
                    continue;
                }

                // Name Input
                Console.Write("Ange nytt namn eller tryck Enter för att behålla " +
                    "nuvarande:\n");
                string newName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newName))
                    selectedProduct.Name = newName;
                else
                    Console.WriteLine("Du behöll namnet.");

                // Price Input
                Console.Write("Ange nytt pris eller tryck Enter för att behålla " +
                    "nuvarande:\nExempel: 29,90\n");
                string newPriceInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newPriceInput) && decimal.TryParse(
                    newPriceInput, out decimal newPrice))
                {
                    selectedProduct.Price = newPrice;
                }
                else if (string.IsNullOrEmpty(newPriceInput))
                {
                    newPrice = selectedProduct.Price;
                    Console.WriteLine("Du behöll priset.");
                }
                else
                {
                    Console.WriteLine("Fel format på pris, Exempel: 29,90. Försök igen.");
                    Thread.Sleep(2000);
                    continue;
                }

                // PriceType input
                Console.WriteLine("Ange önskad pristyp, 1 eller 2 och tryck " +
                    "Enter\n1. Styckpris\n2. Kilopris");
                string changePriceTypeInput = Console.ReadLine();
                if (changePriceTypeInput == "1" || changePriceTypeInput == "2")
                {
                    selectedProduct.PriceType = changePriceTypeInput == "1" ?
                        PriceType.Styckpris : PriceType.Kilopris;
                }
                else
                {
                    Console.WriteLine("Fel värde, välj 1 eller 2. Försök igen.");
                    Thread.Sleep(2000);
                    continue;
                }
                break;
            }
            Console.WriteLine($"Följande produkt har ändrats:\n" +
                $"{selectedProduct}\n\nTryck valfri tangent för att återgå" +
                $" till huvudmenyn.");

            SaveProducts();
            Console.ReadKey();
            _returnToMainMenu.Invoke();
        }

        /// <summary>
        /// Metod som ger användaren möjlighet att skapa en ny produkt.
        /// </summary>
        public void AddProduct()
        {
            Product newProduct = new Product(0, "", 0, PriceType.Styckpris);

            while (true)
            {
                Console.Clear();
                MenuGraphics.ShowMenuGraphics();

                // PLU input
                int lastPlu = listOfProducts.Last().PLU;
                Console.Write($"Skapa ny produkt.\nNuvarade högsta PLU-nummer: " +
                    $"{lastPlu}\n\nAnge önskat PLU och tryck Enter:\n");
                string newPluInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newPluInput) && int.TryParse(
                    newPluInput, out int newPlu))
                {
                    if (listOfProducts.Any(p => p.PLU == newPlu))
                    {
                        Console.WriteLine("Det angivna PLU-numret är redan " +
                            "upptaget. Försök igen.");
                        Thread.Sleep(2000);
                        continue;
                    }
                    else
                        newProduct.PLU = newPlu;
                }
                else
                {
                    Console.WriteLine("Felatigt värde på PLU. Använd heltal, " +
                        "försök igen");
                    Thread.Sleep(2000);
                    continue;
                }

                // Name Input
                Console.Write("Ange nytt namn och tryck Enter:\n");
                string newName = Console.ReadLine();
                if (!string.IsNullOrEmpty(newName))
                    newProduct.Name = newName;
                else
                {
                    Console.WriteLine("Du måste ge produkten ett namn. " +
                        "Försök igen.");
                    Thread.Sleep(2000);
                    continue;
                }

                // Price Input
                Console.Write("Ange pris. Exempel: 29,90. Tryck därefter Enter:\n");
                string newPriceInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(newPriceInput) && decimal.TryParse(
                    newPriceInput, out decimal newPrice))
                {
                    newProduct.Price = newPrice;
                }
                else
                {
                    Console.WriteLine("Felaktigt värde, Exempel: 29,90. Försök igen.");
                    Thread.Sleep(2000);
                    AddProduct();
                }

                // PriceType Input
                Console.WriteLine("Ange önskad pristyp. " +
                    "Välj 1 eller 2 och tryck Enter:");
                Console.WriteLine("1. Styckpris\n2. Kilopris");
                string newPriceTypeInput = Console.ReadLine();
                if (newPriceTypeInput == "1" || newPriceTypeInput == "2")
                {
                    if (newPriceTypeInput == "1")
                        newProduct.PriceType = PriceType.Styckpris;
                    else
                        newProduct.PriceType = PriceType.Kilopris;
                }
                else
                {
                    Console.WriteLine("Felaktigt värde, välj 1 eller 2");
                    Thread.Sleep(2000);
                    continue;
                }
                break;
            }

            listOfProducts.Add(newProduct);
            Console.WriteLine($"Produkten har lagts till:\n{newProduct}\n\n" +
                $"Tryck valfri tangent för att återgå till huvudmenyn.");

            SaveProducts();
            Console.ReadKey();
            _returnToMainMenu.Invoke();

        }

        /// <summary>
        /// Ger användaren möjlighet att ta bort befintlig produkt från listOfProducts och Products.txt.
        /// </summary>
        public void RemoveProduct()
        {
            int selectedIndex = _displayList.BrowseAList(listOfProducts, false);
            Product selectedProduct = listOfProducts[selectedIndex];

            Console.Clear();
            MenuGraphics.ShowMenuGraphics();

            Console.WriteLine($"Följade produkt är borttagen:\n" +
                $"{selectedProduct}\n\nTryck valfri tangent för att " +
                $"återgå till huvudmenyn.");
            listOfProducts.RemoveAt(selectedIndex);

            SaveProducts();
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
