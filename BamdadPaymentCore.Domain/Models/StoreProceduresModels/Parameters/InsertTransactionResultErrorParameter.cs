namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record InsertTransactionResultErrorParameter(
    int? ResultCode,
    string ErrorMessage,
    int? OnlineId,
    string Bank_MerchantID,
    string Bank_User,
    string Bank_Pass,
    string BankCode,
    bool? Online_Status,
    int? Online_Kind,
    int? IsSettle,
    bool? AutoSettle,
    string MobileNomber
    );

}
