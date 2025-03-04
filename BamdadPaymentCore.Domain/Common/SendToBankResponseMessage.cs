﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Common
{
    public class SiteErrorResponse
    {
        public const string NullOrEmptyOnlineId = "اطلاعات کامل نمی باشد با مدیر سیستم تماس حاصل نمایید.";

        public const string PaymentNotValid = "ارتباط معتبر نمی باشد با مدیر سیستم تماس حاصل نمایید.";

        public const string BankConnectionFailed = "خطا در اتصال به بانک:";

        public const string BankSettleException = "Settle In Bank Caused Exception";

        public const string BankVerifyException = "Verify In Bank Caused Exception";

        public const string BankTransationResultException = "Verify In Bank Caused Exception";
    }
}
