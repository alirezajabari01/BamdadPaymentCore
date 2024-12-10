using BamdadPaymentCore.Domain.Models.StoreProceduresModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record SelectOnlinePayParameter(string Online_ID) : StoreProcedureRequestModel;
}
