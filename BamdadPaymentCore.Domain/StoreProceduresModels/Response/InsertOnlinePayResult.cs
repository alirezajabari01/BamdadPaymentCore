using BamdadPaymentCore.Domain.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.StoreProceduresModels.Response
{
    public class InsertOnlinePayResult : BaseEntity
    {
        [Key]
        public decimal OnlineID { get; set; }
    }
}
