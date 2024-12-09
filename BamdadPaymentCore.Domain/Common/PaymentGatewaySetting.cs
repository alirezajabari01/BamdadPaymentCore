using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Common
{
    public class PaymentGatewaySetting
    {
        public string AsanMerchantId { get; set; }

        public string MelatMerchantId { get; set; }

        public string ParsianMerchantId { get; set; }

        public string AsanpardakhtGateWay { get; set; }

        public string MelatReturnBank { get; set; }

        public string MelatReturnBankWithAccept { get; set; }

        public string MellatGateWay { get; set; }
    }
}
