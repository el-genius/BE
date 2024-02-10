using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URCP.Core;
using URCP.Core.Enum;
using URCP.Core.Model;
using URCP.Core.Util;
using URCP.ServicesRepository.AttachmentService;
using URCP.ServicesRepository.NotificationGateway;
using URCP.ServicesRepository.PaymentService;

namespace URCP.ServicesRepository.Helper
{
    public static class Mapper
    {
        public static SendEmailRequest ToEntity(this EmailNotification emailNotification)
        {
            return new SendEmailRequest()
            {
                RefrenceId = emailNotification.Id.ToString(),
                EmailTo = emailNotification.To.ToList(),
                TemplateCode = emailNotification.TemplateCode,
                TemplateValues = emailNotification.TemplateValuesList.ToList(),
                IsArabic = emailNotification.IsArabic
                
            };

        }
        public static SendSMSNotificationRequest ToEntity(this SmsNotification smsNotification)
        {
            return new SendSMSNotificationRequest()
            {
                RefrenceId = smsNotification.Id.ToString(),
                IsArabic = smsNotification.IsArabic,
                MobileNo = smsNotification.MobileNo,
                TemplateCode = smsNotification.TemplateCode,
                TemplateValues = smsNotification.TemplateValuesList.ToList()
            };
        }

        public static GenerateBillSadadRequest ToEntity(this InvoiceModel invoiceModel)
        {
            GenerateBillSadadRequest generateBillSadadRequest = new GenerateBillSadadRequest()
            {
                ExpireDate = DateTime.Now.AddDays(invoiceModel.DueDate),
                BillDescription = invoiceModel.Description,
                RefDesc = invoiceModel.RefDecription,
                RefId = invoiceModel.RefId,
                MobileNo = invoiceModel.ClientMobileNo
            };

            #region Init invoice metadata
            var ListInvoiceDetails = new List<RequestedBillDetail>();
            invoiceModel.invoices.ForEach((Action<InvoiceMetadataModel>)(m =>
            {
                var requestedBillDetail = new RequestedBillDetail()
                {
                    Description = m.Description,
                    RefId = m.RefId,
                    RefDesc = m.RefDecription
                };


                requestedBillDetail.BillDetailMetadata = new Dictionary<string, string>();

                switch (m.InvoiceType)
                {
                    case InvoiceType.RegisterMortgageContract:
                        {
                            requestedBillDetail.ServiceCode = KeyConfig.PaymentService.ServiceCode.RegisterMortgageContract;
                            requestedBillDetail.BillDetailMetadata.Add("RegisterationsCount", "1");
                            break;
                        }
                    case InvoiceType.MortgageInquiryByContractNumber:
                        {
                            requestedBillDetail.ServiceCode = KeyConfig.PaymentService.ServiceCode.MortgageInquiryByContractNumber;
                            requestedBillDetail.BillDetailMetadata.Add("InquiriesCount", "1");
                            break;
                        }
                    default:
                        break;
                }

                requestedBillDetail.BillDetailMetadata.Add("CityName", m.CityName);
                requestedBillDetail.BillDetailMetadata.Add("ContractType", m.ContractType);
                requestedBillDetail.BillDetailMetadata.Add("NationalID", m.NationalId.ToString());
                requestedBillDetail.BillDetailMetadata.Add("RequestNo", m.RequestNo.ToString());

                ListInvoiceDetails.Add(requestedBillDetail);
            }));
            #endregion

            generateBillSadadRequest.BillDetails = ListInvoiceDetails;
            return generateBillSadadRequest;
        }

        public static UploadRequest ToEntity(this AttachmentModel invoiceModel)
        {
            return new UploadRequest()
            {
                ApplicationId = KeyConfig.AttachmentService.ApplicationId,
                Content = invoiceModel.Content,
                FileName = invoiceModel.FileName,
                FileSize = invoiceModel.FileSize,
                FileType = invoiceModel.FileType
            };
        }

        public static AttachmentModel ToModel(this DownloadResponse downloadResponse)
        {
            return new AttachmentModel()
            {
                Content = downloadResponse.Content,
                FileName = downloadResponse.FileName,
                FileSize = downloadResponse.FileSize ?? downloadResponse.FileSize.Value,
                FileType = downloadResponse.FileType
            };
        }
    }
}
