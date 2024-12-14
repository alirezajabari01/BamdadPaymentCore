using BamdadPaymentCore.Domain.Models.StoreProceduresModels;

namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public class InsertIntoOnlinePayParameter
    {
        public int Bank_ID { get; set; }

        public int? Site_ID { get; set; }

        public int Online_Price { get; set; }

        public string Online_Desc { get; set; }

        public int Online_ReqID { get; set; }

        public int Online_Kind { get; set; }

        public string Online_Type { get; set; }

        public string? MobileNomber { get; set; }

        public bool AutoSettle { get; set; }

        public InsertIntoOnlinePayParameter(int bank_ID, int? site_ID, string online_Price,
        string online_Desc, string online_ReqID, string online_Kind, bool autoSettle, string online_Type, string? mobileNomber)
        {
            Bank_ID = bank_ID;
            Site_ID = site_ID;
            Online_Price = Convert.ToInt32(online_Price);
            Online_ReqID = Convert.ToInt32(online_ReqID);
            Online_Kind = Convert.ToInt32(online_Kind);
            Online_Desc = online_Desc;
            Online_Type = online_Type;
            MobileNomber= mobileNomber;
            AutoSettle = autoSettle;
        }
    }
}
