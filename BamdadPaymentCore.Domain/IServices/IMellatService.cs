using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.bill;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.reverse;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.settlement;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.verify;
using BamdadPaymentCore.Domain.Models.ServicesModels;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface IMellatService
    {
        public string VerifyTransaction(MellatRequest requst);

        public string SettleTransaction(MellatRequest requst);

        public string ReverseTransaction(MellatRequest requst);

        public string InquiryTransaction(MellatRequest request);

        public string RefundTransaction(MellatRequest request);

        string ProcessCallBackFromBank(HttpRequest Request);

        string SendToMellatPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId);
    }
}
