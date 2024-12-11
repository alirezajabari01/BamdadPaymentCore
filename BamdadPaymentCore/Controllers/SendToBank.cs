using BamdadPaymentCore.Domain.AsanPardakht.AsanRest;
using BamdadPaymentCore.Domain.Common;
using BamdadPaymentCore.Domain.Enums;
using BamdadPaymentCore.Domain.Exceptions;
using BamdadPaymentCore.Domain.IRepositories;
using BamdadPaymentCore.Domain.IServices;
using BamdadPaymentCore.Domain.Models.ControllerDto;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Parameters;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
using BamdadPaymentCore.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Net;

namespace BamdadPaymentCore.Controllers
{
    public class SendToBank(IPaymentService paymentService, IOptions<PaymentGatewaySetting> paymentGatewaySetting, IAsanRestService asanRestService
        , IBamdadPaymentRepository paymentRepository, IMellatService mellatService) : Controller
    {
        public IActionResult Index()
        {
            string onlineId = Request.Query["OnlineID"];

            ViewBag.GatewayUrl = "";
            ViewBag.RefId = "";
            ViewBag.Message = "خطایی رخ داده";

            if (string.IsNullOrWhiteSpace(onlineId)) throw new OnlineIdNotFoundException();

            SelectPaymentDetailResult paymentDetail = paymentRepository.SelectPaymentDetail(new SelectPaymentDetailParameter(onlineId));

            ViewBag.MobileNo = paymentDetail.MobileNomber ?? "";

            if (paymentDetail is null || paymentDetail.Online_Status == true)
            {
                throw new PaymentDetailException();
            }

            if (paymentDetail.Online_Price == 0)
            {
                string url = paymentService.FreePayment(onlineId);
                return Redirect(url + "?OnlineID=" + onlineId);
            }

            if (paymentDetail.BankCode == nameof(BankCode.Mellat))
            {
                ViewBag.GatewayUrl = paymentGatewaySetting.Value.MellatGateWay;
                ViewBag.RefId = mellatService.SendToMellatPaymentGateway(paymentDetail, onlineId);
            }

            if (paymentDetail.BankCode == nameof(BankCode.Parsian))
            {
                // Send To Parsian Payment Gateway Logic 
            }

            if (paymentDetail.BankCode == nameof(BankCode.Asan))
            {
                ViewBag.GatewayUrl = paymentGatewaySetting.Value.AsanpardakhtGateWay;
                ViewBag.RefId = asanRestService.SendToAsanPardakhtPaymentGateway(paymentDetail, onlineId);
            }

            return View();
        }
    }
}

