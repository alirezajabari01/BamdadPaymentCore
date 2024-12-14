using BamdadPaymentCore.Domain.IServices;
using System.Data;
using BamdadPaymentCore.Domain.Models.SoapDto.Requests;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.Common;


namespace BamdadPaymentCore.SOAP
{

    public class PaymentSoapService(IPaymentService paymentService) : IPaymentSoapService
    {

        public string GetOnlineId(string username, string pass, string price, string desc, string reqId, string? mobleNomber = null)
        {
            string result = string.Empty;
            try
            {
                result = paymentService.GetOnlineIdDifferentTypes(username, pass, price, desc, reqId, Kind.NormalKind, false, OnlineType.Payment, mobleNomber);
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source.ToString()));
            }

            return result;
        }

        public string normal(string username, string pass, string price, string desc, string reqId, string? mobleNomber = null)
        {
            string result = string.Empty;
            try
            {
                result = paymentService.GetOnlineIdDifferentTypes(username, pass, price, desc, reqId, Kind.NormalKind, true, OnlineType.Payment, mobleNomber);
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source));
            }

            return result;
        }

        public string GetOnlineIdkind(string username, string pass, string price, string desc, string reqId, string kind, string? mobleNomber = null)
        {
            string result = string.Empty;
            try
            {
                result = paymentService.GetOnlineIdDifferentTypes(username, pass, price, desc, reqId, kind, false, OnlineType.Payment, mobleNomber);
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source.ToString()));
            }

            return result;
        }

        public string GetOnlineIdWithSettle(string username, string pass, string price, string desc, string reqId, string kind, string? mobleNomber = null)
        {
            var result = string.Empty;
            try
            {
                result = paymentService.GetOnlineIdDifferentTypes(username, pass, price, desc, reqId, kind, true,  mobleNomber);
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source.ToString()));
            }
            return result;
        }

        public DataTable GetOnlineStatus(string username, string pass, string onlineId)
        {
            DataTable onlineStatus = new DataTable()
            {
                TableName = "OnlinePayStatus"
            };
            try
            {
                onlineStatus = paymentService.GetOnlineStatus(GetOnlineStatusRequestMapper.ToGetOnlineStatusRequest(username, pass, onlineId));
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source.ToString()));
            }
            return onlineStatus;
        }

        public bool ReqRefund(string username, string pass, string onlineId, string refundAmount)
        {
            bool result = false;
            try
            {
                result = paymentService.ReqRefund(ReqRefundRequestMapper.ToReqRefundRequest(username, pass, onlineId, refundAmount));
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source.ToString()));
            }
            return result;
        }

        public bool ReqReversal(string username, string pass, string onlineId)
        {
            bool result = false;
            try
            {
                result = paymentService.RequestReversal(username, pass, onlineId);
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source.ToString()));
            }
            return result;
        }

        public DataTable ReqSettleOnline(string username, string pass, string onlineId)
        {
            DataTable onlineStatus = new DataTable()
            {
                TableName = "OnlinePayStatus"
            };
            try
            {
                onlineStatus = paymentService.ReqSettleOnline(new ReqSettleOnlineRequest(username, pass, onlineId));
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source.ToString()));
            }
            return onlineStatus;
        }

        public bool RequestReversal(string username, string pass, string onlineId)
        {
            bool result = false;
            try
            {
                result = paymentService.RequestReversal(username, pass, onlineId);
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source));
            }
            return false;
        }

        public DataTable RequestSettleOnline(string username, string pass, string onlineId)
        {
            DataTable dataTable = new DataTable()
            {
                TableName = "ResSettleOnlinePayStatus"
            };
            try
            {
                dataTable = paymentService.ReqSettleOnline(ReqSettleOnlineRequestMapper.ToReqSettleOnlineRequest(username, pass, onlineId));
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source));
            }
            return dataTable;
        }

        public bool ReqVerify(string username, string pass, string onlineId)
        => paymentService.ReqVerify(new VerifyRequest(username, pass, onlineId));

        public bool Cancel(string username, string pass, string onlineId)
        {
            bool result = false;
            try
            {
                result = paymentService.ReqRefund(new ReqRefundRequest(username, pass, onlineId, "654"));
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source));
            }

            return result;
        }
    }
}
