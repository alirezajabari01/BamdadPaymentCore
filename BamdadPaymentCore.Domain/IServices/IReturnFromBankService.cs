﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface IReturnFromBankService
    {
        string ReturnUrlRedirectionFromBank(string bankType);
    }
}
