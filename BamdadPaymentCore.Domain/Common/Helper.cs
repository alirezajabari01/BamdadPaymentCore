using BamdadPaymentCore.Domain.StoreProceduresModels.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;

namespace BamdadPaymentCore.Domain.Common
{
    public class Helper
    {
        public static string HashMd5(string data) => ((IEnumerable<byte>)new MD5CryptoServiceProvider()
            .ComputeHash(Encoding.UTF8.GetBytes(data)))
            .Aggregate<byte, string>("", (Func<string, byte, string>)
            ((current, t) => current + string.Format("{0:x02}", (object)t)));

        public static long RefundAmount(long refundAmount, SelectOnlinePayDetailResult SelectOnlinePayDetailResult)
        => (refundAmount == 0 || refundAmount > SelectOnlinePayDetailResult.Online_Price) ? Convert.ToInt64(SelectOnlinePayDetailResult.Online_Price) : refundAmount;

    }
}
