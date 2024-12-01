using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Enums
{
    public enum BankCode
    {
        [Display(Name = "Mellat")]
        Mellat = 1,
        [Display(Name = "Parsian")]
        Parsian = 2,
        [Display(Name = "Zarin")]
        Zarin = 3,
        [Display(Name = "Asan")]
        Asan = 4
    }
}
