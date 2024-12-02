using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Services
{
    internal class ReturnBankWithAcceptService(IPaymentService paymentService) : IReturnBankWithAcceptService
    {
        public string RedirectToUrl(string onlineId, string refId, string saleReferenceId, string resCode, string cardInfo = "")
        {
            cardInfo = cardInfo + "?OnlineID=" + onlineId;

            if (string.IsNullOrEmpty(onlineId)) return string.Empty;

            if (resCode != "0") return paymentService.UpdateOnlinePayFailed(onlineId, refId, saleReferenceId, resCode, cardInfo);

            return paymentService.UpdateOnlinePayWithSettle(new UpdateOnlinePayWithSettleParameter(Convert.ToInt32(onlineId), refId, saleReferenceId, 0, cardInfo));
        }
    }
}
