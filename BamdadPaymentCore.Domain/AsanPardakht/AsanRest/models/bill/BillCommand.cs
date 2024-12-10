using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.bill
{
    public class BillCommand : SaleCommand
    {
        public BillCommand(int merchantConfigurationId, int serviceTypeId, long orderId, ulong amountInRials, string callbackURL
            , string billId, string payId)
            : base(merchantConfigurationId, serviceTypeId, orderId, amountInRials, callbackURL, JsonConvert.SerializeObject(new
            {
                billId,
                payId
            }))
        {
        }
    }
}