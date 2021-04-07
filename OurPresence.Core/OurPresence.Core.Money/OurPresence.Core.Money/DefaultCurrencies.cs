using System;
using System.Collections.Generic;

namespace OurPresence.Core.Money
{
    /// <summary>
    /// Static list of default currencies.
    /// </summary>
    internal static class DefaultCurrencies
    {
        /// <summary>
        /// The Malagasy ariary and the Mauritanian ouguiya are technically divided into five subunits (the iraimbilanja and
        /// khoum respectively), rather than by a power of ten. The coins display "1/5" on their face and are referred to as
        /// a "fifth" (Khoum/cinquième). These are not used in practice, but when written out, a single significant digit is
        /// used. E.g. 1.2 UM.
        /// To represent this in decimal we do the following steps: 5 is 10 to the power of log(5) = 0.69897... ~ 0.7.
        /// </summary>
        internal const double Z07 = 0.69897000433601880478626110527551; // Math.Log10(5);

        /// <summary>Used for indication that the number of decimal digits doesn't matter, for example for gold or silver.</summary>
        internal const double NotApplicable = -1;

        private static readonly object s_obj = new();
        private static IList<Currency>? s_currencies;

        internal static IEnumerable<Currency>? Currencies
        {
            get
            {
                EnsureCurrencyTable();
                return s_currencies;
            }
        }

        internal static void EnsureCurrencyTable()
        {
            if (s_currencies is null)
            {
                InitCurrencyTable();
            }
        }

        private static void InitCurrencyTable()
        {
            lock (s_obj)
            {
                s_currencies = new List<Currency>
                {
                    // ISO-4217 currencies (list one)
                    new Currency("AED", "784", 2, "United Arab Emirates dirham", "د.إ"),
                    new Currency("AFN", "971", 2, "Afghan afghani", "؋"),
                    new Currency("ALL", "008", 2, "Albanian lek", "L"),
                    new Currency("AMD", "051", 2, "Armenian dram", "֏"),
                    new Currency("ANG", "532", 2, "Netherlands Antillean guilder", "ƒ"),
                    new Currency("AOA", "973", 2, "Angolan kwanza", "Kz"),
                    new Currency("ARS", "032", 2, "Argentine peso", "$"),
                    new Currency("AUD", "036", 2, "Australian dollar", "$"),
                    new Currency("AWG", "533", 2, "Aruban florin", "ƒ"),
                    new Currency("AZN", "944", 2, "Azerbaijan Manat", "ман"), // AZERBAIJAN
                    new Currency("BAM", "977", 2, "Bosnia and Herzegovina convertible mark", "KM"),
                    new Currency("BBD", "052", 2, "Barbados dollar", "$"),
                    new Currency("BDT", "050", 2, "Bangladeshi taka", "৳"), // or Tk
                    new Currency("BGN", "975", 2, "Bulgarian lev", "лв."),
                    new Currency("BHD", "048", 3, "Bahraini dinar", "BD"), // or د.ب. (switched for unit tests to work)
                    new Currency("BIF", "108", 0, "Burundian franc", "FBu"),
                    new Currency("BMD", "060", 2, "Bermudian dollar", "$"),
                    new Currency("BND", "096", 2, "Brunei dollar", "$"), // or B$
                    new Currency("BOB", "068", 2, "Boliviano", "Bs."), // or BS or $b
                    new Currency("BOV", "984", 2, "Bolivian Mvdol (funds code)", Currency.GenericCurrencySign), // <==== not found
                    new Currency("BRL", "986", 2, "Brazilian real", "R$"),
                    new Currency("BSD", "044", 2, "Bahamian dollar", "$"),
                    new Currency("BTN", "064", 2, "Bhutanese ngultrum", "Nu."),
                    new Currency("BWP", "072", 2, "Botswana pula", "P"),
                    new Currency("BYR", "974", 0, "Belarusian ruble", "Br", validTo: new DateTime(2016, 12, 31), validFrom: new DateTime(2000, 01, 01)),
                    new Currency("BYN", "974", 0, "Belarusian ruble", "Br", validFrom: new DateTime(2006, 06, 01)),
                    new Currency("BZD", "084", 2, "Belize dollar", "BZ$"),
                    new Currency("CAD", "124", 2, "Canadian dollar", "$"),
                    new Currency("CDF", "976", 2, "Congolese franc", "FC"),
                    new Currency("CHE", "947", 2, "WIR Euro (complementary currency)", "CHE"),
                    new Currency("CHF", "756", 2, "Swiss franc", "fr."), // or CHF
                    new Currency("CHW", "948", 2, "WIR Franc (complementary currency)", "CHW"),
                    new Currency("CLF", "990", 4, "Unidad de Fomento (funds code)", "CLF"),
                    new Currency("CLP", "152", 0, "Chilean peso", "$"),
                    new Currency("CNY", "156", 2, "Chinese yuan", "¥"),
                    new Currency("COP", "170", 2, "Colombian peso", "$"),
                    new Currency("COU", "970", 2, "Unidad de Valor Real", Currency.GenericCurrencySign), // ???
                    new Currency("CRC", "188", 2, "Costa Rican colon", "₡"),
                    new Currency("CUC", "931", 2, "Cuban convertible peso", "CUC$"), // $ or CUC
                    new Currency("CUP", "192", 2, "Cuban peso", "$"), // or ₱ (obsolete?)
                    new Currency("CVE", "132", 0, "Cape Verde escudo", "$"),
                    new Currency("CZK", "203", 2, "Czech koruna", "Kč"),
                    new Currency("DJF", "262", 0, "Djiboutian franc", "Fdj"),
                    new Currency("DKK", "208", 2, "Danish krone", "kr."),
                    new Currency("DOP", "214", 2, "Dominican peso", "RD$"), // or $
                    new Currency("DZD", "012", 2, "Algerian dinar", "DA"), // (Latin) or د.ج (Arabic)
                    new Currency("EGP", "818", 2, "Egyptian pound", "LE"), // or E£ or ج.م (Arabic)
                    new Currency("ERN", "232", 2, "Eritrean nakfa", "ERN"),
                    new Currency("ETB", "230", 2, "Ethiopian birr", "Br"), // (Latin) or ብር (Ethiopic)
                    new Currency("EUR", "978", 2, "Euro", "€"),
                    new Currency("FJD", "242", 2, "Fiji dollar", "$"), // or FJ$
                    new Currency("FKP", "238", 2, "Falkland Islands pound", "£"),
                    new Currency("GBP", "826", 2, "Pound sterling", "£"),
                    new Currency("GEL", "981", 2, "Georgian lari", "ლ."), // TODO: new symbol since July 18, 2014 => see http://en.wikipedia.org/wiki/Georgian_lari
                    new Currency("GHS", "936", 2, "Ghanaian cedi", "GH¢"), // or GH₵
                    new Currency("GIP", "292", 2, "Gibraltar pound", "£"),
                    new Currency("GMD", "270", 2, "Gambian dalasi", "D"),
                    new Currency("GNF", "324", 0, "Guinean Franc", "FG"), // (possibly also Fr or GFr)  GUINEA
                    new Currency("GTQ", "320", 2, "Guatemalan quetzal", "Q"),
                    new Currency("GYD", "328", 2, "Guyanese dollar", "$"), // or G$
                    new Currency("HKD", "344", 2, "Hong Kong dollar", "HK$"), // or $
                    new Currency("HNL", "340", 2, "Honduran lempira", "L"),
                    new Currency("HRK", "191", 2, "Croatian kuna", "kn"),
                    new Currency("HTG", "332", 2, "Haitian gourde", "G"),
                    new Currency("HUF", "348", 2, "Hungarian forint", "Ft"),
                    new Currency("IDR", "360", 2, "Indonesian rupiah", "Rp"),
                    new Currency("ILS", "376", 2, "Israeli new shekel", "₪"),
                    new Currency("INR", "356", 2, "Indian rupee", "₹"),
                    new Currency("IQD", "368", 3, "Iraqi dinar", "د.ع"),
                    new Currency("IRR", "364", 0, "Iranian rial", "ريال"),
                    new Currency("ISK", "352", 0, "Icelandic króna", "kr"),
                    new Currency("JMD", "388", 2, "Jamaican dollar", "J$"), // or $
                    new Currency("JOD", "400", 3, "Jordanian dinar", "د.ا.‏"),
                    new Currency("JPY", "392", 0, "Japanese yen", "¥"),
                    new Currency("KES", "404", 2, "Kenyan shilling", "KSh"),
                    new Currency("KGS", "417", 2, "Kyrgyzstani som", "сом"),
                    new Currency("KHR", "116", 2, "Cambodian riel", "៛"),
                    new Currency("KMF", "174", 0, "Comorian Franc", "CF"), // COMOROS (THE)
                    new Currency("KPW", "408", 0, "North Korean won", "₩"),
                    new Currency("KRW", "410", 0, "South Korean won", "₩"),
                    new Currency("KWD", "414", 3, "Kuwaiti dinar", "د.ك"), // or K.D.
                    new Currency("KYD", "136", 2, "Cayman Islands dollar", "$"),
                    new Currency("KZT", "398", 2, "Kazakhstani tenge", "₸"),
                    new Currency("LAK", "418", 0, "Lao Kip", "₭"), // or ₭N,  LAO PEOPLE’S DEMOCRATIC REPUBLIC(THE), ISO says minor unit=2 but wiki syas Historically, one kip was divided into 100 att (ອັດ).
                    new Currency("LBP", "422", 0, "Lebanese pound", "ل.ل"),
                    new Currency("LKR", "144", 2, "Sri Lankan rupee", "Rs"), // or රු
                    new Currency("LRD", "430", 2, "Liberian dollar", "$"), // or L$, LD$
                    new Currency("LSL", "426", 2, "Lesotho loti", "L"), // L or M (pl.)
                    new Currency("LYD", "434", 3, "Libyan dinar", "ل.د"), // or LD
                    new Currency("MAD", "504", 2, "Moroccan dirham", "د.م."),
                    new Currency("MDL", "498", 2, "Moldovan leu", "L"),
                    new Currency("MGA", "969", Z07, "Malagasy ariary", "Ar"),  // divided into five subunits rather than by a power of ten. 5 is 10 to the power of 0.69897...
                    new Currency("MKD", "807", 0, "Macedonian denar", "ден"),
                    new Currency("MMK", "104", 0, "Myanma kyat", "K"),
                    new Currency("MNT", "496", 2, "Mongolian tugrik", "₮"),
                    new Currency("MOP", "446", 2, "Macanese pataca", "MOP$"),
                    new Currency("MRU", "929", Z07, "Mauritanian ouguiya", "UM", validFrom: new DateTime(2018, 01, 01)), // divided into five subunits rather than by a power of ten. 5 is 10 to the power of 0.69897...
                    new Currency("MUR", "480", 2, "Mauritian rupee", "Rs"),
                    new Currency("MVR", "462", 2, "Maldivian rufiyaa", "Rf"), // or , MRf, MVR, .ރ or /-
                    new Currency("MWK", "454", 2, "Malawi kwacha", "MK"),
                    new Currency("MXN", "484", 2, "Mexican peso", "$"),
                    new Currency("MXV", "979", 2, "Mexican Unidad de Inversion (UDI) (funds code)", Currency.GenericCurrencySign),  // <==== not found
                    new Currency("MYR", "458", 2, "Malaysian ringgit", "RM"),
                    new Currency("MZN", "943", 2, "Mozambican metical", "MTn"), // or MTN
                    new Currency("NAD", "516", 2, "Namibian dollar", "N$"), // or $
                    new Currency("NGN", "566", 2, "Nigerian naira", "₦"),
                    new Currency("NIO", "558", 2, "Nicaraguan córdoba", "C$"),
                    new Currency("NOK", "578", 2, "Norwegian krone", "kr"),
                    new Currency("NPR", "524", 2, "Nepalese rupee", "Rs"), // or ₨ or रू
                    new Currency("NZD", "554", 2, "New Zealand dollar", "$"),
                    new Currency("OMR", "512", 3, "Omani rial", "ر.ع."),
                    new Currency("PAB", "590", 2, "Panamanian balboa", "B/."),
                    new Currency("PEN", "604", 2, "Peruvian sol", "S/."),
                    new Currency("PGK", "598", 2, "Papua New Guinean kina", "K"),
                    new Currency("PHP", "608", 2, "Philippine Peso", "₱"), // or P or PHP or PhP
                    new Currency("PKR", "586", 2, "Pakistani rupee", "Rs"),
                    new Currency("PLN", "985", 2, "Polish złoty", "zł"),
                    new Currency("PYG", "600", 0, "Paraguayan guaraní", "₲"),
                    new Currency("QAR", "634", 2, "Qatari riyal", "ر.ق"), // or QR
                    new Currency("RON", "946", 2, "Romanian new leu", "lei"),
                    new Currency("RSD", "941", 2, "Serbian dinar", "РСД"), // or RSD (or дин or d./д)
                    new Currency("RUB", "643", 2, "Russian rouble", "₽"), // or R or руб (both onofficial)
                    new Currency("RWF", "646", 0, "Rwandan franc", "RFw"), // or RF, R₣
                    new Currency("SAR", "682", 2, "Saudi riyal", "ر.س"), // or SR (Latin) or ﷼‎ (Unicode)
                    new Currency("SBD", "090", 2, "Solomon Islands dollar", "SI$"),
                    new Currency("SCR", "690", 2, "Seychelles rupee", "SR"), // or SRe
                    new Currency("SDG", "938", 2, "Sudanese pound", "ج.س."),
                    new Currency("SEK", "752", 2, "Swedish krona/kronor", "kr"),
                    new Currency("SGD", "702", 2, "Singapore dollar", "S$"), // or $
                    new Currency("SHP", "654", 2, "Saint Helena pound", "£"),
                    new Currency("SLL", "694", 0, "Sierra Leonean leone", "Le"),
                    new Currency("SOS", "706", 2, "Somali shilling", "S"), // or Sh.So.
                    new Currency("SRD", "968", 2, "Surinamese dollar", "$"),
                    new Currency("SSP", "728", 2, "South Sudanese pound", "£"), // not sure about symbol...
                    new Currency("SVC", "222", 2, "El Salvador Colon", "₡"),
                    new Currency("SYP", "760", 2, "Syrian pound", "ܠ.ܣ.‏"), // or LS or £S (or £)
                    new Currency("SZL", "748", 2, "Swazi lilangeni", "L"), // or E (plural)
                    new Currency("THB", "764", 2, "Thai baht", "฿"),
                    new Currency("TJS", "972", 2, "Tajikistani somoni", "смн"),
                    new Currency("TMT", "934", 2, "Turkmenistani manat", "m"), // or T?
                    new Currency("TND", "788", 3, "Tunisian dinar", "د.ت"), // or DT (Latin)
                    new Currency("TOP", "776", 2, "Tongan paʻanga", "T$"), // (sometimes PT)
                    new Currency("TRY", "949", 2, "Turkish lira", "₺"),
                    new Currency("TTD", "780", 2, "Trinidad and Tobago dollar", "$"), // or TT$
                    new Currency("TWD", "901", 2, "New Taiwan dollar", "NT$"), // or $
                    new Currency("TZS", "834", 2, "Tanzanian shilling", "x/y"), // or TSh
                    new Currency("UAH", "980", 2, "Ukrainian hryvnia", "₴"),
                    new Currency("UGX", "800", 2, "Ugandan shilling", "USh"),
                    new Currency("USD", "840", 2, "United States dollar", "$"), // or US$
                    new Currency("USN", "997", 2, "United States dollar (next day) (funds code)", "$"),
                    new Currency("UYI", "940", 0, "Uruguay Peso en Unidades Indexadas (UI) (funds code)", Currency.GenericCurrencySign), // List two
                    new Currency("UYU", "858", 2, "Uruguayan peso", "$"), // or $U
                    new Currency("UZS", "860", 2, "Uzbekistan som", "лв"), // or сўм ?
                    new Currency("VES", "928", 2, "Venezuelan Bolívar Soberano", "Bs.", validFrom: new DateTime(2018, 8, 20)), // or Bs.F. , Amendment 167 talks about delay but from multiple sources on the web the date seems to be 20 aug.
                    new Currency("VND", "704", 0, "Vietnamese dong", "₫"),
                    new Currency("VUV", "548", 0, "Vanuatu vatu", "VT"),
                    new Currency("WST", "882", 2, "Samoan tala", "WS$"), // sometimes SAT, ST or T
                    new Currency("XAF", "950", 0, "CFA franc BEAC", "FCFA"),
                    new Currency("XAG", "961", NotApplicable, "Silver (one troy ounce)", Currency.GenericCurrencySign),
                    new Currency("XAU", "959", NotApplicable, "Gold (one troy ounce)", Currency.GenericCurrencySign),
                    new Currency("XBA", "955", NotApplicable, "European Composite Unit (EURCO) (bond market unit)", Currency.GenericCurrencySign),
                    new Currency("XBB", "956", NotApplicable, "European Monetary Unit (E.M.U.-6) (bond market unit)", Currency.GenericCurrencySign),
                    new Currency("XBC", "957", NotApplicable, "European Unit of Account 9 (E.U.A.-9) (bond market unit)", Currency.GenericCurrencySign),
                    new Currency("XBD", "958", NotApplicable, "European Unit of Account 17 (E.U.A.-17) (bond market unit)", Currency.GenericCurrencySign),
                    new Currency("XCD", "951", 2, "East Caribbean dollar", "$"), // or EC$
                    new Currency("XDR", "960", NotApplicable, "Special drawing rights", Currency.GenericCurrencySign),
                    new Currency("XOF", "952", 0, "CFA franc BCEAO", "CFA"),
                    new Currency("XPD", "964", NotApplicable, "Palladium (one troy ounce)", Currency.GenericCurrencySign),
                    new Currency("XPF", "953", 0, "CFP franc", "F"),
                    new Currency("XPT", "962", NotApplicable, "Platinum (one troy ounce)", Currency.GenericCurrencySign),
                    new Currency("XSU", "994", NotApplicable, "SUCRE", Currency.GenericCurrencySign),
                    new Currency("XTS", "963", NotApplicable, "Code reserved for testing purposes", Currency.GenericCurrencySign),
                    new Currency("XUA", "965", NotApplicable, "ADB Unit of Account", Currency.GenericCurrencySign),
                    new Currency("XXX", "999", NotApplicable, "No currency", Currency.GenericCurrencySign),
                    new Currency("YER", "886", 2, "Yemeni rial", "﷼"), // or ر.ي.‏‏ ?
                    new Currency("ZAR", "710", 2, "South African rand", "R"),
                    new Currency("ZMW", "967", 2, "Zambian kwacha", "ZK"), // or ZMW
                    new Currency("ZWL", "932", 2, "Zimbabwean dollar", "$"),
                    new Currency("STN", "930", 2, "Dobra", "Db", validFrom: new DateTime(2018, 1, 1)), // New Currency of São Tomé and Príncipe from 1 Jan 2018 (Amendment 164)
                    new Currency("STD", "678", 2, "Dobra", "Db", validTo: new DateTime(2018, 1, 1)), // To be replaced Currency of São Tomé and Príncipe from 1 Jan 2018 (Amendment 164),  inflation has rendered the cêntimo obsolete
                    new Currency("UYW", "927", 2, "Unidad Previsional", "Db", validFrom: new DateTime(2018, 8, 29)), // The Central Bank of Uruguay is applying for new Fund currency code (Amendment 169)

                    // Historic ISO-4217 currencies (list three)
                    new Currency("VEF", "937", 2, "Venezuelan bolívar", "Bs.", "ISO-4217-HISTORIC", new DateTime(2018, 8, 20)), // replaced by VEF, The conversion rate is 1000 (old) Bolívar to 1 (new) Bolívar Soberano (1000:1). The expiration date of the current bolívar will be defined later and communicated by the Central Bank of Venezuela in due time.
                    new Currency("MRO", "478", Z07, "Mauritanian ouguiya", "UM", "ISO-4217-HISTORIC", new DateTime(2018, 1, 1)), // replaced by MRU
                    new Currency("ESA", "996", NotApplicable, "Spanish peseta (account A)", "Pta", "ISO-4217-HISTORIC", new DateTime(2002, 3, 1)), // replaced by ESP (EUR)
                    new Currency("ESB", "995", NotApplicable, "Spanish peseta (account B)", "Pta", "ISO-4217-HISTORIC", new DateTime(2002, 3, 1)), // replaced by ESP (EUR)
                    new Currency("LTL", "440", 2, "Lithuanian litas", "Lt", "ISO-4217-HISTORIC", new DateTime(2014, 12, 31), new DateTime(1993, 1, 1)), // replaced by EUR
                    new Currency("USS", "998", 2, "United States dollar (same day) (funds code)", "$", "ISO-4217-HISTORIC", new DateTime(2014, 3, 28)), // replaced by (no successor)
                    new Currency("LVL", "428", 2, "Latvian lats", "Ls", "ISO-4217-HISTORIC", new DateTime(2013, 12, 31), new DateTime(1992, 1, 1)), // replaced by EUR
                    new Currency("XFU", string.Empty, NotApplicable, "UIC franc (special settlement currency) International Union of Railways", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2013, 11, 7)), // replaced by EUR
                    new Currency("ZMK", "894", 2, "Zambian kwacha", "ZK", "ISO-4217-HISTORIC", new DateTime(2013, 1, 1), new DateTime(1968, 1, 16)), // replaced by ZMW
                    new Currency("EEK", "233", 2, "Estonian kroon", "kr", "ISO-4217-HISTORIC", new DateTime(2010, 12, 31), new DateTime(1992, 1, 1)), // replaced by EUR
                    new Currency("ZWR", "935", 2, "Zimbabwean dollar A/09", "$", "ISO-4217-HISTORIC", new DateTime(2009, 2, 2), new DateTime(2008, 8, 1)), // replaced by ZWL
                    new Currency("SKK", "703", 2, "Slovak koruna", "Sk", "ISO-4217-HISTORIC", new DateTime(2008, 12, 31), new DateTime(1993, 2, 8)), // replaced by EUR
                    new Currency("TMM", "795", 0, "Turkmenistani manat", "T", "ISO-4217-HISTORIC", new DateTime(2008, 12, 31), new DateTime(1993, 11, 1)), // replaced by TMT
                    new Currency("ZWN", "942", 2, "Zimbabwean dollar A/08", "$", "ISO-4217-HISTORIC", new DateTime(2008, 7, 31), new DateTime(2006, 8, 1)), // replaced by ZWR
                    new Currency("VEB", "862", 2, "Venezuelan bolívar", "Bs.", "ISO-4217-HISTORIC", new DateTime(2008, 1, 1)), // replaced by VEF
                    new Currency("CYP", "196", 2, "Cypriot pound", "£", "ISO-4217-HISTORIC", new DateTime(2007, 12, 31), new DateTime(1879, 1, 1)), // replaced by EUR
                    new Currency("MTL", "470", 2, "Maltese lira", "₤", "ISO-4217-HISTORIC", new DateTime(2007, 12, 31), new DateTime(1972, 1, 1)), // replaced by EUR
                    new Currency("GHC", "288", 0, "Ghanaian cedi", "GH₵", "ISO-4217-HISTORIC", new DateTime(2007, 7, 1), new DateTime(1967, 1, 1)), // replaced by GHS
                    new Currency("SDD", "736", NotApplicable, "Sudanese dinar", "£Sd", "ISO-4217-HISTORIC", new DateTime(2007, 1, 10), new DateTime(1992, 6, 8)), // replaced by SDG
                    new Currency("SIT", "705", 2, "Slovenian tolar", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2006, 12, 31), new DateTime(1991, 10, 8)), // replaced by EUR
                    new Currency("ZWD", "716", 2, "Zimbabwean dollar A/06", "$", "ISO-4217-HISTORIC", new DateTime(2006, 7, 31), new DateTime(1980, 4, 18)), // replaced by ZWN
                    new Currency("MZM", "508", 0, "Mozambican metical", "MT", "ISO-4217-HISTORIC", new DateTime(2006, 6, 30), new DateTime(1980, 1, 1)), // replaced by MZN
                    new Currency("AZM", "031", 0, "Azerbaijani manat", "₼", "ISO-4217-HISTORIC", new DateTime(2006, 1, 1), new DateTime(1992, 8, 15)), // replaced by AZN
                    new Currency("CSD", "891", 2, "Serbian dinar", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2006, 12, 31), new DateTime(2003, 7, 3)), // replaced by RSD
                    new Currency("MGF", "450", 2, "Malagasy franc", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2005, 1, 1), new DateTime(1963, 7, 1)), // replaced by MGA
                    new Currency("ROL", "642", NotApplicable, "Romanian leu A/05", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2005, 12, 31), new DateTime(1952, 1, 28)), // replaced by RON
                    new Currency("TRL", "792", 0, "Turkish lira A/05", "₺", "ISO-4217-HISTORIC", new DateTime(2005, 12, 31)), // replaced by TRY
                    new Currency("SRG", "740", NotApplicable, "Suriname guilder", "ƒ", "ISO-4217-HISTORIC", new DateTime(2004, 12, 31)), // replaced by SRD
                    new Currency("YUM", "891", 2, "Yugoslav dinar", "дин.", "ISO-4217-HISTORIC", new DateTime(2003, 7, 2), new DateTime(1994, 1, 24)), // replaced by CSD
                    new Currency("AFA", "004", NotApplicable, "Afghan afghani", "؋", "ISO-4217-HISTORIC", new DateTime(2003, 12, 31), new DateTime(1925, 1, 1)), // replaced by AFN
                    new Currency("XFO", string.Empty, NotApplicable, "Gold franc (special settlement currency)", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2003, 12, 31), new DateTime(1803, 1, 1)), // replaced by XDR
                    new Currency("GRD", "300", 2, "Greek drachma", "₯", "ISO-4217-HISTORIC", new DateTime(2000, 12, 31), new DateTime(1954, 1, 1)), // replaced by EUR
                    new Currency("TJR", "762", NotApplicable, "Tajikistani ruble", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2000, 10, 30), new DateTime(1995, 5, 10)), // replaced by TJS
                    new Currency("ECV", "983", NotApplicable, "Ecuador Unidad de Valor Constante (funds code)", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2000, 1, 9), new DateTime(1993, 1, 1)), // replaced by (no successor)
                    new Currency("ECS", "218", 0, "Ecuadorian sucre", "S/.", "ISO-4217-HISTORIC", new DateTime(2000, 12, 31), new DateTime(1884, 1, 1)), // replaced by USD
                    new Currency("BYB", "112", 2, "Belarusian ruble", "Br", "ISO-4217-HISTORIC", new DateTime(1999, 12, 31), new DateTime(1992, 1, 1)), // replaced by BYR
                    new Currency("AOR", "982", 0, "Angolan kwanza readjustado", "Kz", "ISO-4217-HISTORIC", new DateTime(1999, 11, 30), new DateTime(1995, 7, 1)), // replaced by AOA
                    new Currency("BGL", "100", 2, "Bulgarian lev A/99", "лв.", "ISO-4217-HISTORIC", new DateTime(1999, 7, 5), new DateTime(1962, 1, 1)), // replaced by BGN
                    new Currency("ADF", string.Empty, 2, "Andorran franc (1:1 peg to the French franc)", "Fr", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1960, 1, 1)), // replaced by EUR
                    new Currency("ADP", "020", 0, "Andorran peseta (1:1 peg to the Spanish peseta)", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1869, 1, 1)), // replaced by EUR
                    new Currency("ATS", "040", 2, "Austrian schilling", "öS", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1945, 1, 1)), // replaced by EUR
                    new Currency("BEF", "056", 2, "Belgian franc (currency union with LUF)", "fr.", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1832, 1, 1)), // replaced by EUR
                    new Currency("DEM", "276", 2, "German mark", "DM", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1948, 1, 1)), // replaced by EUR
                    new Currency("ESP", "724", 0, "Spanish peseta", "Pta", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1869, 1, 1)), // replaced by EUR
                    new Currency("FIM", "246", 2, "Finnish markka", "mk", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1860, 1, 1)), // replaced by EUR
                    new Currency("FRF", "250", 2, "French franc", "Fr", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1960, 1, 1)), // replaced by EUR
                    new Currency("IEP", "372", 2, "Irish pound (punt in Irish language)", "£", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1938, 1, 1)), // replaced by EUR
                    new Currency("ITL", "380", 0, "Italian lira", "₤", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1861, 1, 1)), // replaced by EUR
                    new Currency("LUF", "442", 2, "Luxembourg franc (currency union with BEF)", "fr.", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1944, 1, 1)), // replaced by EUR
                    new Currency("MCF", string.Empty, 2, "Monegasque franc (currency union with FRF)", "fr.", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1960, 1, 1)), // replaced by EUR
                    new Currency("NLG", "528", 2, "Dutch guilder", "ƒ", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1810, 1, 1)), // replaced by EUR
                    new Currency("PTE", "620", 0, "Portuguese escudo", "$", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(4160, 1, 1)), // replaced by EUR
                    new Currency("SML", string.Empty, 0, "San Marinese lira (currency union with ITL and VAL)", "₤", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1864, 1, 1)), // replaced by EUR
                    new Currency("VAL", string.Empty, 0, "Vatican lira (currency union with ITL and SML)", "₤", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1929, 1, 1)), // replaced by EUR
                    new Currency("XEU", "954", NotApplicable, "European Currency Unit (1 XEU = 1 EUR)", "ECU", "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1979, 3, 13)), // replaced by EUR
                    new Currency("BAD", string.Empty, 2, "Bosnia and Herzegovina dinar", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1998, 12, 31), new DateTime(1992, 7, 1)), // replaced by BAM
                    new Currency("RUR", "810", 2, "Russian ruble A/97", "₽", "ISO-4217-HISTORIC", new DateTime(1997, 12, 31), new DateTime(1992, 1, 1)), // replaced by RUB
                    new Currency("GWP", "624", NotApplicable, "Guinea-Bissau peso", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1997, 12, 31), new DateTime(1975, 1, 1)), // replaced by XOF
                    new Currency("ZRN", "180", 2, "Zaïrean new zaïre", "Ƶ", "ISO-4217-HISTORIC", new DateTime(1997, 12, 31), new DateTime(1993, 1, 1)), // replaced by CDF
                    new Currency("UAK", "804", NotApplicable, "Ukrainian karbovanets", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1996, 9, 1), new DateTime(1992, 10, 1)), // replaced by UAH
                    new Currency("YDD", "720", NotApplicable, "South Yemeni dinar", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1996, 6, 11)), // replaced by YER
                    new Currency("AON", "024", 0, "Angolan new kwanza", "Kz", "ISO-4217-HISTORIC", new DateTime(1995, 6, 30), new DateTime(1990, 9, 25)), // replaced by AOR
                    new Currency("ZAL", "991", NotApplicable, "South African financial rand (funds code)", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1995, 3, 13), new DateTime(1985, 9, 1)), // replaced by (no successor)
                    new Currency("PLZ", "616", NotApplicable, "Polish zloty A/94", "zł", "ISO-4217-HISTORIC", new DateTime(1994, 12, 31), new DateTime(1950, 10, 30)), // replaced by PLN
                    new Currency("BRR", string.Empty, 2, "Brazilian cruzeiro real", "CR$", "ISO-4217-HISTORIC", new DateTime(1994, 6, 30), new DateTime(1993, 8, 1)), // replaced by BRL
                    new Currency("HRD", string.Empty, NotApplicable, "Croatian dinar", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1994, 5, 30), new DateTime(1991, 12, 23)), // replaced by HRK
                    new Currency("YUG", string.Empty, 2, "Yugoslav dinar", "дин.", "ISO-4217-HISTORIC", new DateTime(1994, 1, 23), new DateTime(1994, 1, 1)), // replaced by YUM
                    new Currency("YUO", string.Empty, 2, "Yugoslav dinar", "дин.", "ISO-4217-HISTORIC", new DateTime(1993, 12, 31), new DateTime(1993, 10, 1)), // replaced by YUG
                    new Currency("YUR", string.Empty, 2, "Yugoslav dinar", "дин.", "ISO-4217-HISTORIC", new DateTime(1993, 9, 30), new DateTime(1992, 7, 1)), // replaced by YUO
                    new Currency("BRE", string.Empty, 2, "Brazilian cruzeiro", "₢", "ISO-4217-HISTORIC", new DateTime(1993, 8, 1), new DateTime(1990, 3, 15)), // replaced by BRR
                    new Currency("UYN", "858", NotApplicable, "Uruguay Peso", "$U", "ISO-4217-HISTORIC", new DateTime(1993, 3, 1), new DateTime(1975, 7, 1)), // replaced by UYU
                    new Currency("CSK", "200", NotApplicable, "Czechoslovak koruna", "Kčs", "ISO-4217-HISTORIC", new DateTime(1993, 2, 8), new DateTime(7040, 1, 1)), // replaced by CZK and SKK (CZK and EUR)
                    new Currency("MKN", string.Empty, NotApplicable, "Old Macedonian denar A/93", "ден", "ISO-4217-HISTORIC", new DateTime(1993, 12, 31)), // replaced by MKD
                    new Currency("MXP", "484", NotApplicable, "Mexican peso", "$", "ISO-4217-HISTORIC", new DateTime(1993, 12, 31)), // replaced by MXN
                    new Currency("ZRZ", string.Empty, 3, "Zaïrean zaïre", "Ƶ", "ISO-4217-HISTORIC", new DateTime(1993, 12, 31), new DateTime(1967, 1, 1)), // replaced by ZRN
                    new Currency("YUN", string.Empty, 2, "Yugoslav dinar", "дин.", "ISO-4217-HISTORIC", new DateTime(1992, 6, 30), new DateTime(1990, 1, 1)), // replaced by YUR
                    new Currency("SDP", "736", NotApplicable, "Sudanese old pound", "ج.س.", "ISO-4217-HISTORIC", new DateTime(1992, 6, 8), new DateTime(1956, 1, 1)), // replaced by SDD
                    new Currency("ARA", string.Empty, 2, "Argentine austral", "₳", "ISO-4217-HISTORIC", new DateTime(1991, 12, 31), new DateTime(1985, 6, 15)), // replaced by ARS
                    new Currency("PEI", string.Empty, NotApplicable, "Peruvian inti", "I/.", "ISO-4217-HISTORIC", new DateTime(1991, 10, 1), new DateTime(1985, 2, 1)), // replaced by PEN
                    new Currency("SUR", "810", NotApplicable, "Soviet Union Ruble", "руб", "ISO-4217-HISTORIC", new DateTime(1991, 12, 31), new DateTime(1961, 1, 1)), // replaced by RUR
                    new Currency("AOK", "024", 0, "Angolan kwanza", "Kz", "ISO-4217-HISTORIC", new DateTime(1990, 9, 24), new DateTime(1977, 1, 8)), // replaced by AON
                    new Currency("DDM", "278", NotApplicable, "East German Mark of the GDR (East Germany)", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1990, 7, 1), new DateTime(1948, 6, 21)), // replaced by DEM (EUR)
                    new Currency("BRN", string.Empty, 2, "Brazilian cruzado novo", "NCz$", "ISO-4217-HISTORIC", new DateTime(1990, 3, 15), new DateTime(1989, 1, 16)), // replaced by BRE
                    new Currency("YUD", "891", 2, "New Yugoslavian Dinar", "дин.", "ISO-4217-HISTORIC", new DateTime(1989, 12, 31), new DateTime(1966, 1, 1)), // replaced by YUN
                    new Currency("BRC", string.Empty, 2, "Brazilian cruzado", "Cz$", "ISO-4217-HISTORIC", new DateTime(1989, 1, 15), new DateTime(1986, 2, 28)), // replaced by BRN
                    new Currency("BOP", "068", 2, "Peso boliviano", "b$.", "ISO-4217-HISTORIC", new DateTime(1987, 1, 1), new DateTime(1963, 1, 1)), // replaced by BOB
                    new Currency("UGS", "800", NotApplicable, "Ugandan shilling A/87", "USh", "ISO-4217-HISTORIC", new DateTime(1987, 12, 31)), // replaced by UGX
                    new Currency("BRB", "076", 2, "Brazilian cruzeiro", "₢", "ISO-4217-HISTORIC", new DateTime(1986, 2, 28), new DateTime(1970, 1, 1)), // replaced by BRC
                    new Currency("ILR", "376", 2, "Israeli shekel", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1985, 12, 31), new DateTime(1980, 2, 24)), // replaced by ILS
                    new Currency("ARP", string.Empty, 2, "Argentine peso argentino", "$a", "ISO-4217-HISTORIC", new DateTime(1985, 6, 14), new DateTime(1983, 6, 6)), // replaced by ARA
                    new Currency("PEH", "604", NotApplicable, "Peruvian old sol", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1985, 2, 1), new DateTime(1863, 1, 1)), // replaced by PEI
                    new Currency("GQE", string.Empty, NotApplicable, "Equatorial Guinean ekwele", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1985, 12, 31), new DateTime(1975, 1, 1)), // replaced by XAF
                    new Currency("GNE", "324", NotApplicable, "Guinean syli", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1985, 12, 31), new DateTime(1971, 1, 1)), // replaced by GNF
                    new Currency("MLF", string.Empty, NotApplicable, "Mali franc", "MAF", "ISO-4217-HISTORIC", new DateTime(1984, 12, 31)), // replaced by XOF
                    new Currency("ARL", string.Empty, 2, "Argentine peso ley", "$L", "ISO-4217-HISTORIC", new DateTime(1983, 5, 5), new DateTime(1970, 1, 1)), // replaced by ARP
                    new Currency("ISJ", "352", 2, "Icelandic krona", "kr", "ISO-4217-HISTORIC", new DateTime(1981, 12, 31), new DateTime(1922, 1, 1)), // replaced by ISK
                    new Currency("MVQ", "462", NotApplicable, "Maldivian rupee", "Rf", "ISO-4217-HISTORIC", new DateTime(1981, 12, 31)), // replaced by MVR
                    new Currency("ILP", "376", 3, "Israeli lira", "I£", "ISO-4217-HISTORIC", new DateTime(1980, 12, 31), new DateTime(1948, 1, 1)), // ISRAEL Pound,  replaced by ILR
                    new Currency("ZWC", "716", 2, "Rhodesian dollar", "$", "ISO-4217-HISTORIC", new DateTime(1980, 12, 31), new DateTime(1970, 2, 17)), // replaced by ZWD
                    new Currency("LAJ", "418", NotApplicable, "Pathet Lao Kip", "₭", "ISO-4217-HISTORIC", new DateTime(1979, 12, 31)), // replaced by LAK
                    new Currency("TPE", string.Empty, NotApplicable, "Portuguese Timorese escudo", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1976, 12, 31), new DateTime(1959, 1, 1)), // replaced by IDR
                    new Currency("UYP", "858", NotApplicable, "Uruguay Peso", "$", "ISO-4217-HISTORIC", new DateTime(1975, 7, 1), new DateTime(1896, 1, 1)), // replaced by UYN
                    new Currency("CLE", string.Empty, NotApplicable, "Chilean escudo", "Eº", "ISO-4217-HISTORIC", new DateTime(1975, 12, 31), new DateTime(1960, 1, 1)), // replaced by CLP
                    new Currency("MAF", string.Empty, NotApplicable, "Moroccan franc", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1976, 12, 31), new DateTime(1921, 1, 1)), // replaced by MAD
                    new Currency("PTP", string.Empty, NotApplicable, "Portuguese Timorese pataca", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1958, 12, 31), new DateTime(1894, 1, 1)), // replaced by TPE
                    new Currency("TNF", string.Empty, 2, "Tunisian franc", "F", "ISO-4217-HISTORIC", new DateTime(1958, 12, 31), new DateTime(1991, 7, 1)), // replaced by TND
                    new Currency("NFD", string.Empty, 2, "Newfoundland dollar", "$", "ISO-4217-HISTORIC", new DateTime(1949, 12, 31), new DateTime(1865, 1, 1)), // replaced by CAD

                    // Added historic currencies of amendment 164 (research dates and other info)
                    new Currency("VNC", "704", 2, "Dong", "$", "ISO-4217-HISTORIC"), // VIETNAM
                    new Currency("GNS", "324", NotApplicable, "Guinean Syli", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(1970, 12, 31)), // GUINEA, replaced by GNE?
                    new Currency("UGW", "800", NotApplicable, "Old Shilling", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // UGANDA
                    new Currency("RHD", "716", NotApplicable, "Rhodesian Dollar", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // SOUTHERN RHODESIA
                    new Currency("ROK", "642", NotApplicable, "Leu A/52", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // ROMANIA
                    new Currency("NIC", "558", NotApplicable, "Cordoba", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // NICARAGUA
                    new Currency("MZE", "508", NotApplicable, "Mozambique Escudo", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // MOZAMBIQUE
                    new Currency("MTP", "470", NotApplicable, "Maltese Pound", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // MALTA
                    new Currency("LSM", "426", NotApplicable, "Loti", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // LESOTHO
                    new Currency("GWE", "624", NotApplicable, "Guinea Escudo", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // GUINEA-BISSAU
                    new Currency("CSJ", "203", NotApplicable, "Krona A/53", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // CZECHOSLOVAKIA
                    new Currency("BUK", "104", NotApplicable, "Kyat", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // BURMA
                    new Currency("BGK", "100", NotApplicable, "Lev A / 62", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // BULGARIA
                    new Currency("BGJ", "100", NotApplicable, "Lev A / 52", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // BULGARIA
                    new Currency("ARY", "032", NotApplicable, "Peso", Currency.GenericCurrencySign, "ISO-4217-HISTORIC", new DateTime(2017, 9, 22)), // ARGENTINA
                };
            }
        }
    }
}
