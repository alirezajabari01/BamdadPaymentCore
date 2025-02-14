﻿namespace BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters
{
    public record InsertTransactionResultParameter(
         string ReferenceNumber,
         int Online_ID,
         string Online_TransactionNo = "Failed",
         string Online_OrderNo = "Failed",
         int Online_ErrorCode = 0,
         string CardHolderInfo = ""
    );
}
