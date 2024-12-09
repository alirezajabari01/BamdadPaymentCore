using BamdadPaymentCore.Domain.ControllerDto;
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
        SendToBankResultVm SendToBank(string onlineId);

        string SendToMellatPaymentGateway(SelectPaymentDetailResult paymentDetail,string onlineId);

        string SendToPasianPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId);
    }
}
