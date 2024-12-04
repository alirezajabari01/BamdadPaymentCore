﻿using RestService.models;
using RestService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestService.models.verify;
using RestService.models.settle;
using RestService.models.reverse;
using RestService.models.bill;
using BamdadPaymentCore.Domain.StoreProceduresModels.Response;

namespace BamdadPaymentCore.Domain.IServices
{
    public interface IAsanResetService
    {
        public Task<TResult> GetToken<TRequest, TResult>(TRequest request, SelectPaymentDetailResult paymentDetail)
            where TRequest : ITokenCommand
            where TResult : class, ITokenVm, new();

        public Task<VerifyVm> VerifyTransaction(AsanRestRequest requst);

        public Task<SettleVm> SettleTransaction(AsanRestRequest requst);

        public Task<ReverseVm> ReverseTransaction(AsanRestRequest requst);

        public Task<PaymentResultVm> TransactionResult(TransactionResultRequest request);

        public Task<CancelResultVm> CancelTransaction(AsanRestRequest requst);
    }
}
