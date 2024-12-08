using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Domain.Entites
{
    public class SiteError
    {
        [Key]
        public int SiteErrorId { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorSource { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}
