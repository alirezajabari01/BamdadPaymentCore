using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface IReturnFromBankService
    {
        string ReturnFromAsan(HttpRequest Request);
        
        string ReturnFromMellat(HttpRequest Request);
    }
}
