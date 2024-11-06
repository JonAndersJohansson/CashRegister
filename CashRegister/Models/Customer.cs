namespace CashRegister.Models
{
    /// <summary>
    /// Definerar en Customer.
    /// </summary>
    public class Customer
    {
        public string Name { get; set; }
        public bool IsMember { get; set; }

        public Customer()
        {
            Name = "StandardCustomer";
            IsMember = false;
        }
    }
}
