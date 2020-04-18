using System;

namespace AzureAI.CognitiveSearch.CustomSkills.Infrastructure.Model
{
    public class InvoiceData : IEntity
    {
        public Guid Id { get; set; }

        public string Charges { get; set; }

        public string ForCompany { get; set; }

        public string FromCompany { get; set; }

        public string InvoiceDate { get; set; }

        public string InvoiceDueDate { get; set; }

        public string InvoiceNumber { get; set; }

        public string VatID { get; set; }
    }
}
