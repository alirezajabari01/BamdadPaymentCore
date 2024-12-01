using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Common
{
    public class SendToBankResponseMessage
    {
        public const string NullOrEmptyOnlineId = "اطلاعات کامل نمی باشد با مدیر سیستم تماس حاصل نمایید.";

        public const string PaymentNotValidOrAlreadyPaid = "ارتباط معتبر نمی باشد با مدیر سیستم تماس حاصل نمایید.";

        public const string BankConnectionFailed = "خطا در اتصال به بانک:";
    }
}
