using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeSystem.Application.common
{
    public static class DiscountCodeConstants
    {
        public const byte SUCCESS = 0;
        public const byte ALREADY_USED_CODE = 1;
        public const byte INVALID_CODE = 2;
       public const string DiscountCodeCharacterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
       public const byte MAX_RETRIES = 5;



        public const string UseCodeError = "An error occurred while using the code.";
       public const string GenerateCodeError = "An error occurred while generating codes.";
    }
}
