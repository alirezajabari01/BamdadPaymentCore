namespace BamdadPaymentCore.Domain.Models.ControllerDto
{
    public record GetPaymentReportResponse
    (
        int? Online_Price,
        string Online_TransactionNo,
        string Online_OrderNo,
        bool? Online_Status,
        int? Online_ID,
        int? Online_ReqID
    );
}
