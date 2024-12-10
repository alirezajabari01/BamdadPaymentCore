using BamdadPaymentCore.Domain.Models.ControllerDto;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
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

        string SendToPasianPaymentGateway(SelectPaymentDetailResult paymentDetail, string onlineId);
    }
}
