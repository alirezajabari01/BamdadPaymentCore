using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Entites
{
    public class Bank
    {
        public int Bank_ID { get; set; }
        public string? Bank_Name { get; set; }
        public string? Bank_MerchantID { get; set; }
        public string? Bank_User { get; set; }
        public string? Bank_Pass { get; set; }
        public DateTime? Bank_CreatedDate { get; set; }
        public bool? Bank_IsActive { get; set; }
        public string? BankCode { get; set; }
    }
}
