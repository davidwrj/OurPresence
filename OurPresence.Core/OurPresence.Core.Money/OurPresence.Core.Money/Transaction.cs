namespace OurPresence.Core.Money
{
    /// <summary>Represents a general transaction done in a webshop.</summary>
    public class Transaction
    {
        /// <summary>Gets or sets the transaction amount of the transaction in the local currency of the customer.</summary>
        public Amount Amount { get; set; }

        /// <summary>Gets or sets the exchange rate to the base currency of the shop at the time of the transaction.</summary>
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>Gets or sets the tax amount of the transaction.</summary>
        public Amount Tax { get; set; }

        /// <summary>Gets or sets the optional discount of the transaction.</summary>
        public Amount Discount { get; set; }
    }
}
