using BamdadPaymentCore.Domain.StoreProceduresModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface ISendToBankService
    {
        string SendToBank(string onlineId);

        string SendToMellatPaymentGateway(SelectPaymentDetailResult paymentDetail,string onlineId);

        string SendToAsanPardakhtPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId);

        string SendToPasianPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId);
    }
}
