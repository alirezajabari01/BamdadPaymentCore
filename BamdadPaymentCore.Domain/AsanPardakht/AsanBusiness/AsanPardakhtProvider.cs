using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.AspNetCore.Http.Extensions;
using ipgsoap.asanpardakht.ir;

/// <summary>
/// Summary description for AsanPardakhtProvider
/// </summary>
public class AsanPardakhtProvider
{
    private int merchantID;
    private int merchantConfigID;
    private string userName;
    private string password;
    private string encryptionKey;
    private string encryptionVector;

    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly merchantservicesSoap _merchantservicesSoap;
    public AsanPardakhtProvider(int merchantID, int merchantConfigID, string userName, string password, string encryptionKey, string encryptionVector, IHttpContextAccessor httpContextAccessor, merchantservicesSoap merchantservicesSoap)

    {
        this.merchantID = merchantID;
        this.merchantConfigID = merchantConfigID;
        this.userName = userName;
        this.password = password;
        this.encryptionKey = encryptionKey;
        this.encryptionVector = encryptionVector;
        this.httpContextAccessor = httpContextAccessor;
        _merchantservicesSoap = merchantservicesSoap;
    }

    public bool PrepareForPayment(int localInvoiceID, ulong amount, out string result, out string token)
    {
        result = string.Empty;
        token = string.Empty;

        string p1 = "1";
        string p2 = this.userName;
        string p3 = this.password;
        string p4 = localInvoiceID.ToString();
        string p5 = amount.ToString();
        string p6 = GetDateTimeProperFormat();
        string p7 = " ";
        string p8 = httpContextAccessor.HttpContext.Request.GetDisplayUrl().Replace("startpayment", "receivepayment");
        string p9 = " ";
        string toBeEncrypted = p1 + "," + p2 + "," + p3 + "," + p4 + "," + p5 + "," + p6 + "," + p7 + "," + p8 + "," + p9;


        string encryptedString = string.Empty;
        AES2 aesProvider = new AES2(this.encryptionKey, this.encryptionVector);
        bool encryptionIsSuccessful = aesProvider.Encrypt(toBeEncrypted, out encryptedString);

        if (!encryptionIsSuccessful)
            return false;

        try
        {
            //var merchantServices = new BamdadPayment.ir.asanpardakht.ipgsoap.merchantservices();
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chains, sslPolicyErrors) => { return true; };
            var res = _merchantservicesSoap.RequestOperation(new RequestOperationRequest(new RequestOperationRequestBody( this.merchantConfigID, encryptedString)));


            string[] splittedArray = res.ToString().Split(',');
            //merchantServices.Abort();
            //merchantServices = null;
            if (splittedArray.Length == 2)
            {
                result = splittedArray[0];
                token = splittedArray[1];
                return true;
            }
            else if (splittedArray.Length == 1)
            {
                result = splittedArray[0];
                token = string.Empty;
                return false;
            }
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public bool VerifyTrx(ulong tranID, out string res)
    {
        try
        {
            res = string.Empty;
            string toBeEncrypted = userName + "," + password;
            string encryptedString = string.Empty;
            AES2 aesProvider = new AES2(this.encryptionKey, this.encryptionVector);
            bool encryptionIsSuccessful = aesProvider.Encrypt(toBeEncrypted, out encryptedString);
            if (encryptionIsSuccessful)
            {
                //var merchantServices = new BamdadPayment.ir.asanpardakht.ipgsoap.merchantservices();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
                //res = merchantServices.RequestVerification(this.merchantConfigID, encryptedString, tranID);

                return (res == "500");
            }
            else
            {
                res = string.Empty;
                return false;
            }
        }
        catch (Exception ex)
        {
            res = string.Empty;
            return false;
        }
    }

    public bool SettleTrx(ulong tranID, out string res)
    {
        try
        {
            res = string.Empty;
            string toBeEncrypted = userName + "," + password;
            string encryptedString = string.Empty;
            AES2 aesProvider = new AES2(this.encryptionKey, this.encryptionVector);
            bool encryptionIsSuccessful = aesProvider.Encrypt(toBeEncrypted, out encryptedString);
            if (encryptionIsSuccessful)
            {
                //var merchantServices = new BamdadPayment.ir.asanpardakht.ipgsoap.merchantservices();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
                //res = merchantServices.RequestReconciliation(this.merchantConfigID, encryptedString, tranID);
                return (res == "600");
            }
            else
            {
                res = string.Empty;
                return false;
            }
        }
        catch (Exception ex)
        {
            res = string.Empty;
            return false;
        }
    }

    public bool ReverseTrx(ulong tranID, out string res)
    {
        try
        {
            res = string.Empty;
            string toBeEncrypted = userName + "," + password;
            string encryptedString = string.Empty;
            AES2 aesProvider = new AES2(this.encryptionKey, this.encryptionVector);
            bool encryptionIsSuccessful = aesProvider.Encrypt(toBeEncrypted, out encryptedString);
            if (encryptionIsSuccessful)
            {
                //var  merchantServices = new BamdadPayment.ir.asanpardakht.ipgsoap.merchantservices();
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
                //res = merchantServices.RequestReversal(this.merchantConfigID, encryptedString, tranID);
                return (res == "700");
            }
            else
            {
                res = string.Empty;
                return false;
            }
        }
        catch (Exception ex)
        {
            res = string.Empty;
            return false;
        }
    }

    private string GetDateTimeProperFormat()
    {
        DateTime now = DateTime.Now;
        return now.Year.ToString() + now.Month.ToString().PadLeft(2, '0') +
            now.Day.ToString().PadLeft(2, '0') + " " + now.Hour.ToString().PadLeft(2, '0') +
            now.Minute.ToString().PadLeft(2, '0') + now.Second.ToString().PadLeft(2, '0');
    }
}
