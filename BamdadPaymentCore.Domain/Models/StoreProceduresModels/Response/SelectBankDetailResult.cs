using BamdadPaymentCore.Domain.Models.StoreProceduresModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response
{

    public class SelectBankDetailResult : StoreProcedureResponseModel
    {
        public int Bank_ID { get; set; }
        public string Bank_MerchantID { get; set; }
        public string Bank_User { get; set; }
        public string Bank_Pass { get; set; }
        public int Site_ID { get; set; }
    }
}
