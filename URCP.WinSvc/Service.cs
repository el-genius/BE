using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core.Entities;
using URCP.Core.Enum;
using URCP.Core.SearchEntities;
using URCP.Domain.Interface;

namespace URCP.WinSvc
{
    public class Service
    {
        private readonly IMortgageService _mortgageService;
        private readonly IInvoiceService _invoiceService;
        private readonly IMortgageInquiryService _mortgageInquiryService;

        public Service(
            IMortgageService mortgageService,
            IInvoiceService invoiceService,
            IMortgageInquiryService mortgageInquiryService)
        {
            this._mortgageService = mortgageService;
            this._invoiceService = invoiceService;
            this._mortgageInquiryService = mortgageInquiryService;
        }

        public void Run()
        {
            try
            {
                this.ManageMortgageAwaitGenerateInvoiceStatus();
            }
            catch (Exception)
            {

            }

            try
            {
                this.ManageMortgagePendingStatus();
            }
            catch (Exception)
            {

            }

            try
            {
                var result = this.ManageMortgageAwaitPaymentStatusAsync();
                result.Wait();
            }
            catch (Exception)
            {

            }

            try
            {
                var result = this.ManageInquiryAwaitPaymentStatusAsync();
                result.Wait();
                
            }
            catch (Exception)
            {

            }

            try
            {
                var result = this.ManageMortgageContractDateAsync();
                result.Wait();
            }
            catch (Exception)
            {

            }
        }

        private async Task ManageMortgageContractDateAsync()
        {
            await _mortgageService.ManageMortgageContractDateAsync();
        }


        private void ManageMortgageAwaitGenerateInvoiceStatus()
        {
            _mortgageService.ManageMortgageAwaitGenerateInvoiceStatus();
        }

        private void ManageMortgagePendingStatus()
        {
            _mortgageService.ManageMortgagePendingStatus();
        }

        private async Task ManageMortgageAwaitPaymentStatusAsync()
        {
            await _mortgageService.ManageMortgageAwaitPaymentStatusAsync();
        }

        private async Task ManageInquiryAwaitPaymentStatusAsync()
        {
            await _mortgageInquiryService.ManageInquiryAwaitPaymentStatusAsync();
        }
    }
}
