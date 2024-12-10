using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.settlement
{
    public class SettleCommand
    {
        public int merchantConfigurationId { get; set; }
        public ulong payGateTranId { get; set; }
    }
}