using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.SoapDto.Requests;
using BamdadPaymentCore.Domain.StoreProceduresModels.Parameters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.SOAP
{
    public class PaymentSoapService(IPaymentService paymentService) : IPaymentSoapService
    {
       
        public string GetOnlineId(string username, string pass, string price, string desc, string reqId)
        {
            string result = string.Empty;
            try
            {
                result = paymentService.GetOnlineId(GetOnlineIdRequestMapper.ToGetOnlineIdRequest(username, pass, price, desc, reqId));
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source.ToString()));
            }

            return result;
        }

        public string GetOnlineIdkind(string username, string pass, string price, string desc, string reqId, string kind)
        {
            string result = string.Empty;
            try
            {
                result = paymentService.GetOnlineIdkind(GetOnlineIdkindRequestMapper.ToGetOnlineIdkindRequest(username, pass, price, desc, reqId, kind));
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source.ToString()));
            }

            return result;
        }

        public string GetOnlineIdWithSettle(string username, string pass, string price, string desc, string reqId, string kind)
        {
            string result = string.Empty;
            try
            {
                result = paymentService.GetOnlineIdWithSettle(GetOnlineIdWithSettleRequestMapper.ToGetOnlineIdWithSettleRequest(username, pass, price, desc, reqId, kind));
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

        // Must Be Changed To Cancel 
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

        // This One Was On Mellat But Now On Asan
        public bool ReqReversal(string username, string pass, string onlineId)
        {
            bool result = false;
            try
            {
                //result = paymentService.ReqReversal(ReqReversalRequestMappe.ToReqReversalRequest(username, pass, onlineId));
                result = paymentService.RequestReversal(username, pass, onlineId);
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source.ToString()));
            }
            return result;
        }

        //UseLess

        public bool ReqReversal(int merchantConfigurationID, string encryptedCredentials, ulong payGateTranID)
        {
            throw new NotImplementedException();
        }


        //On Mellat
        public DataTable ReqSettleOnline(string username, string pass, string onlineId)
        {
            DataTable onlineStatus = new DataTable()
            {
                TableName = "OnlinePayStatus"
            };
            try
            {
                onlineStatus = paymentService.ReqSettleOnline(ReqSettleOnlineRequestMapper.ToReqSettleOnlineRequest(username, pass, onlineId));
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source.ToString()));
            }
            return onlineStatus;
        }

        //Asan
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

        //On Asan
        public DataTable RequestSettleOnline(string username, string pass, string onlineId)
        {
            DataTable dataTable = new DataTable()
            {
                TableName = "ResSettleOnlinePayStatus"
            };
            try
            {
                dataTable = paymentService.RequestSettleOnline(new SettleOnlineRequest(username, pass, onlineId));
            }
            catch (Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source));
            }
            return dataTable;
        }

        // Asan 
        public bool ReqVerify(string username, string pass, string onlineId)
        => paymentService.ReqVerify(new VerifyRequest(username, pass, onlineId));

        //TODO Not Yet Completed
        public bool Cancel(string username, string pass, string onlineId)
        {
            bool result = false;
            try
            {
               // result = paymentService.CancelPayment(onlineId);
            }
            catch(Exception ex)
            {
                paymentService.InsertSiteError(new InsertSiteErrorParameter(ex.Message, ex.Source));
            }

            return result;
        }
    }
}
