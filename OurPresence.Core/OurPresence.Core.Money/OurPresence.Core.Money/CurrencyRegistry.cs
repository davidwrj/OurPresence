using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace OurPresence.Core.Money
{
    /// <summary>Represent the central thread-safe registry for currencies.</summary>
    internal class CurrencyRegistry
    {
        private static readonly ConcurrentDictionary<string, byte> Namespaces = new ConcurrentDictionary<string, byte> { ["ISO-4217"] = default, ["ISO-4217-HISTORIC"] = default };
        private static readonly ConcurrentDictionary<string, Currency> Currencies = new ConcurrentDictionary<string, Currency>((DefaultCurrencies.Currencies ?? new List<Currency>()).ToDictionary(k => k.Namespace + "::" + k.Code));

        static CurrencyRegistry()
        {
            DefaultCurrencies.EnsureCurrencyTable();
        }

        /// <summary>Tries the get <see cref="Currency"/> of the given code and namespace.</summary>
        /// <param name="code">A currency code, like EUR or USD.</param>
        /// <param name="currency">When this method returns, contains the <see cref="Currency"/> that has the specified code, or the default value of the type if the operation failed.</param>
        /// <returns><b>true</b> if <see cref="CurrencyRegistry"/> contains a <see cref="Currency"/> with the specified code; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The value of 'code' cannot be null or empty.</exception>
        public static bool TryGet(string code, out Currency currency)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            _ = Namespaces.Keys.Select(ns => Currencies.TryGetValue(ns + "::" + code, out var c));
            var found = new List<Currency>();
            foreach (var ns in Namespaces.Keys)
            {
                // don't use string.Format(), string concat much faster in this case!
                if (Currencies.TryGetValue(ns + "::" + code, out var c))
                {
                    found.Add(c);
                }
            }

            currency = found.FirstOrDefault(); // TODO: If more than one, sort by prio.
            return !currency.Equals(default);
        }

        /// <summary>Tries the get <see cref="Currency"/> of the given code and namespace.</summary>
        /// <param name="code">A currency code, like EUR or USD.</param>
        /// <param name="namespace">A namespace, like ISO-4217.</param>
        /// <param name="currency">When this method returns, contains the <see cref="Currency"/> that has the specified code and namespace, or the default value of the type if the operation failed.</param>
        /// <returns><b>true</b> if <see cref="CurrencyRegistry"/> contains a <see cref="Currency"/> with the specified code; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The value of 'code' or 'namespace' cannot be null or empty.</exception>
        public bool TryGet(string code, string @namespace, out Currency currency)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (string.IsNullOrWhiteSpace(@namespace))
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            return Currencies.TryGetValue(@namespace + "::" + code, out currency); // don't use string.Format(), string concat much faster in this case!
        }

        /// <summary>Attempts to add the <see cref="Currency"/> of the given code and namespace.</summary>
        /// <param name="code">A currency code, like EUR or USD.</param>
        /// <param name="namespace">A namespace, like ISO-4217.</param>
        /// <param name="currency">When this method returns, contains the <see cref="Currency"/> that has the specified code and namespace, or the default value of the type if the operation failed.</param>
        /// <returns><b>true</b> if the <see cref="Currency"/> with the specified code is added; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The value of 'code' or 'namespace' cannot be null or empty.</exception>
        public bool TryAdd(string code, string @namespace, Currency currency)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (string.IsNullOrWhiteSpace(@namespace))
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            Namespaces[@namespace] = default;
            var key = @namespace + "::" + code;

            return Currencies.ContainsKey(key) ? false : Currencies.TryAdd(key, currency);
        }

        /// <summary>Attempts to remove the <see cref="Currency"/> of the given code and namespace.</summary>
        /// <param name="code">A currency code, like EUR or USD.</param>
        /// <param name="namespace">A namespace, like ISO-4217.</param>
        /// <param name="currency">When this method returns, contains the <see cref="Currency"/> that has the specified code and namespace, or the default value of the type if the operation failed.</param>
        /// <returns><b>true</b> if the <see cref="Currency"/> with the specified code is removed; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException">The value of 'code' or 'namespace' cannot be null or empty.</exception>
        public bool TryRemove(string code, string @namespace, out Currency currency)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            if (string.IsNullOrWhiteSpace(@namespace))
            {
                throw new ArgumentNullException(nameof(@namespace));
            }

            var key = @namespace + "::" + code;
            if(Currencies.TryRemove(key, out currency))
            {
                if (!Currencies.Any(c => c.Value.Namespace == @namespace))
                {
                    Namespaces.TryRemove(@namespace, out var item);
                }
                return true;
            }
            return false;
        }

        /// <summary>Get all registered currencies.</summary>
        /// <returns>An <see cref="IEnumerable{Currency}"/> of all registered currencies.</returns>
        public IEnumerable<Currency> GetAllCurrencies()
        {
            DefaultCurrencies.EnsureCurrencyTable();
            return DefaultCurrencies.Currencies ?? new List<Currency>();
        }
    }
}
