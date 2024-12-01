namespace BamdadPaymentCore.Domain.Entites
{
    public class Site
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string SiteReturnUrl { get; set; }
        public string SiteIp { get; set; }
        public string SiteUserName { get; set; }
        public string SitePass { get; set; }
        public DateTime SiteCreateDate { get; set; }
        public bool SiteIsActive { get; set; }
    }
}
