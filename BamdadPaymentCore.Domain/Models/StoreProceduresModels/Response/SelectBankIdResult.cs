using BamdadPaymentCore.Domain.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response
{
    public class SelectBankIdResult : BaseEntity
    {
        [Key]
        public int Bank_ID { get; set; }
    }
}
