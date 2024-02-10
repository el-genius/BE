using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using URCP.Resources;

namespace URCP.Web.Models.Sadad
{
    public class SadadModel
    {
        DateTime? _invoiceIssueDate, _invoiceDueDate;

        [Display(Name = "Cost", ResourceType = typeof(SharedStrings_Ar))]
        public double Cost { get; set; }

        [Display(Name = "VatCost", ResourceType = typeof(SharedStrings_Ar))]
        public double VatCost { get; set; }

        [Display(Name = "Total", ResourceType = typeof(SharedStrings_Ar))]
        public double Total { get; set; }

        [Display(Name = "InvoiceNumber", ResourceType = typeof(SharedStrings_Ar))]
        public string InvoiceNumber { get; set; }

        [Display(Name = "InvoiceIssueDate", ResourceType = typeof(SharedStrings_Ar))]
        public DateTime? InvoiceIssueDate
        {
            get
            {
                return _invoiceIssueDate;
            }
            set
            {
                if (value.HasValue && value != DateTime.MinValue)
                {
                    var geCul = new CultureInfo("en-US");
                    geCul.DateTimeFormat.Calendar = new GregorianCalendar();
                    InvoiceIssueDateString = Util.HijriToGreg(value.Value.ToString("dd/MM/yyyy hh:mm"), "dd/MM/yyyy hh:mm");

                    _invoiceIssueDate = value;
                }
            }
        }

        [Display(Name = "InvoiceDueDate", ResourceType = typeof(SharedStrings_Ar))]
        public DateTime? InvoiceDueDate
        {
            get
            {
                return _invoiceDueDate;
            }
            set
            {
                if (value.HasValue && value != DateTime.MinValue)
                {
                    var geCul = new CultureInfo("en-US");
                    geCul.DateTimeFormat.Calendar = new GregorianCalendar();
                    InvoiceDueDateString = Util.HijriToGreg(value.Value.ToString("dd/MM/yyyy hh:mm"), "dd/MM/yyyy hh:mm");

                    _invoiceDueDate = value;
                }
            }
        }

        [Display(Name = "InvoiceIssueDate", ResourceType = typeof(SharedStrings_Ar))]
        public string InvoiceIssueDateString { get; private set; }

        [Display(Name = "InvoiceDueDate", ResourceType = typeof(SharedStrings_Ar))]
        public string InvoiceDueDateString { get; private set; }

    }
}