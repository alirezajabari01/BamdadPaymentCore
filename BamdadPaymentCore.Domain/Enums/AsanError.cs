using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Enums
{
    public class AsanError
    {
        public static int VerifyErrored { get; set; } = -2;
        public static int SettleErrored { get; set; } = -1;
    }
}
