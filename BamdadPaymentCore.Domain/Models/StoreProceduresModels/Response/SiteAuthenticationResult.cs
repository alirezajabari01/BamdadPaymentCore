using BamdadPaymentCore.Domain.Entites;
using System.ComponentModel.DataAnnotations;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response
{
    public class SiteAuthenticationResult : BaseEntity
    {
        [Key]
        public int? Site_ID { get; set; }
        public string Site_Name { get; set; }
        public string Site_ReturnUrl { get; set; }
        public string Site_IP { get; set; }
        public string Site_UserName { get; set; }
        public string Site_Pass { get; set; }
        public DateTime Site_CreateDate { get; set; }
        public bool Site_IsActive { get; set; }
    }

}
