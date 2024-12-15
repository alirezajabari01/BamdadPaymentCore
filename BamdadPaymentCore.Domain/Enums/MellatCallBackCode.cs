using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Enums
{
    public class MellatCallBackCode
    {
        public const string Success = "0";
        public const string Canceled = "17";
        public const string RepeatitiveVerifyRequest = "43";
        public const string NotSettled = "46";
        public const string Reversed = "48";
        public const string RepetitiveTransaction = "51";
        public const string InValidTransaction = "55";
    }
}
