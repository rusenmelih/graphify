using System;
using System.Collections.Generic;
using System.Text;

namespace MelofyAPI
{
    public static class RegexPattern
    {
        public static readonly string Phone = @"^(0)([23458]{1})([0-9]{2})\s?([0-9]{3})\s?([0-9]{2})\s?([0-9]{2})$";
        public static readonly string Email = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
        public static readonly string Password = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$";
        public static readonly string TaxNumber = @"^[0-9]{10}$";
        public static readonly string Mersis = @"^[0-9]{16}$";
        public static readonly string KepAdress = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        public static readonly string CraditCard = @"^([0-9]{4})\s?([0-9]{4})\s?([0-9]{4})\s?([0-9]{4})$";
        public static readonly string FullName = @"^[a-zA-Z ĞÜŞİÖÇğüşıöç]+$";
        public static readonly string StoreName = @"^[a-zA-Z0-9 ğüşıöçĞÜŞİÖÇ\-&']{2,32}$";
        public static readonly string Identity = @"^[1-9]{1}[0-9]{9}[02468]{1}$";

        public static readonly string ModelName = @"";
        public static readonly string ModelCode = "";
        public static readonly string SKU = "";
        public static readonly string Barcode = "";
        public static readonly string ProductName = "";
        public static readonly string ProductNameFull = "";
        public static readonly string SKUFull = "";
    }
}
