using BamdadPaymentCore.Domain.IServices;
using bpm.shaparak.ir;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.Enums;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;

namespace BamdadPaymentCore.Domain.Services
{
    public class ReturnFromBankService(IAsanRestService asanRestService, IMellatService mellatService) : IReturnFromBankService
    {
        public string ReturnFromAsan(HttpRequest Request) => asanRestService.ProcessCallBackFromBank(Request);

        public string ReturnFromMellat(HttpRequest Request) => mellatService.ProcessCallBackFromBank(Request);
    }
}