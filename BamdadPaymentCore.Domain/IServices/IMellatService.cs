using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.bill;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.reverse;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.settlement;
using BamdadPaymentCore.Domain.AsanPardakht.AsanRest.models.verify;
using BamdadPaymentCore.Domain.Models.ServicesModels;
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
        public Task<VerifyVm> VerifyTransaction(MellatRequest requst);

        public Task<SettleVm> SettleTransaction(MellatRequest requst);

        public Task<ReverseVm> ReverseTransaction(MellatRequest requst);

        public Task<PaymentResultVm> InquiryTransaction(MellatRequest request);

        public Task<CancelResultVm> CancelTransaction(MellatRequest requst);

        string ProcessCallBackFromBank(HttpRequest Request);
    }
}
