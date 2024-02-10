using MCIBilling.ClientCallback.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using URCP.Core;
using URCP.Core.Entities;
using URCP.Core.SearchEntities;
using URCP.Domain.Interface;

namespace URCP.WebSvc
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BillingCallback" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BillingCallback.svc or BillingCallback.svc.cs at the Solution Explorer and start debugging.
    public class BillingCallback : ISadadBillCallback
    {
        private readonly IInvoiceService _invoicetService;
        private readonly IMortgageService _mortgageService;
        private readonly IMortgageInquiryService _mortgageInquiryService;

        public BillingCallback(IInvoiceService invoicetService, IMortgageService mortgageService, IMortgageInquiryService mortgageInquiryService)
        {
            this._invoicetService = invoicetService;
            this._mortgageService = mortgageService;
            this._mortgageInquiryService = mortgageInquiryService;
        }
        public bool SadadBillCallbackTest()
        {
            return true;
        }

        public SadadBillCreatedResponse SadadBillCreated(SadadBillCreatedRequest request)
        {
            throw new NotImplementedException();
        }

        public SadadBillStatusChangedResponse SadadBillStatusChanged(SadadBillStatusChangedRequest request)
        {
            try
            {
                InvoiceStatus status = InvoiceStatus.Canceled;
                switch (request.BillStatus)
                {
                    case BillStatusEnum.Paid:
                        status = InvoiceStatus.Paid;
                        break;
                    case BillStatusEnum.Expired:
                        status = InvoiceStatus.Expired;
                        break;
                    case BillStatusEnum.Canceled:
                        status = InvoiceStatus.Canceled;
                        break;
                }

                var invoice = _invoicetService.FindBy(new InvoiceSearchCriteria { Id = int.Parse(request.RefId), IncludeRequest = true }).Items.FirstOrDefault();
                switch (invoice.Request.Type)
                {
                    case Core.Enum.RequestType.RegisterMortgage:
                        var result1 = _mortgageService.PayInvoice(invoice.RequestId, status);
                        result1.Wait();
                        break;
                    case Core.Enum.RequestType.MortgageInquiry:
                        var result = _mortgageInquiryService.PayInvoice(invoice.RequestId, status);
                        result.Wait();
                        break;
                }

                return new SadadBillStatusChangedResponse { ResponseCode = ResponseCodeEnum.Success, Message = "Invoice Updated Successfully" };
            }
            catch (EntityNotFoundException ex)
            {
                LogException(ex);
                return new SadadBillStatusChangedResponse { ResponseCode = ResponseCodeEnum.ErrorNoRetry, Message = ex.Message };
            }
            catch(BusinessRuleException ex)
            {
                LogException(ex);
                return new SadadBillStatusChangedResponse { ResponseCode = ResponseCodeEnum.ErrorNoRetry, Message = ex.Message };
            }
            catch (Exception ex)
            {
                LogException(ex);
                return new SadadBillStatusChangedResponse { ResponseCode = ResponseCodeEnum.ErrorRetry, Message = ex.Message };
            }
        }

        void LogException(Exception ex)
        {

        }
    }
}
