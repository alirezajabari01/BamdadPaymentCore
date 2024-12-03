namespace BamdadPaymentCore.Domain.StoreProceduresModels.Parameters
{
    public record UpdateOnlinePayFailedParameter
   (
        string ReferenceNumber,
        int Online_ID,
        string Online_TransactionNo = "Failed",
        string Online_OrderNo = "Failed",
        int Online_ErrorCode = 0,
        string CardHolderInfo = ""
   );
}
