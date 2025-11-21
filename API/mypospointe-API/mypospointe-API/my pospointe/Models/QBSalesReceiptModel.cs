namespace my_pospointe.Models
{
    public class QBSalesReceiptModel
    {
        public string? Description { get; set; }
        public string? DetailType { get; set; }
        public SalesItemLineDetail? SalesItemLineDetail { get; set; } = new SalesItemLineDetail();
        public int? LineNum { get; set; }
        public float? Amount { get; set; }
        public string? Id { get; set; }
    }

    public class SalesItemLineDetail
    {
        public TaxCodeRef? TaxCodeRef { get; set; } = new TaxCodeRef();
        public float? Qty { get; set; }
        public float? UnitPrice { get; set; }
        public ItemRef? ItemRef { get; set; } = new ItemRef();
    }

    public class ItemRef
    {
        public string? name { get; set; }
        public string? value { get; set; }
    }

    public class TaxCodeRef
    {
        public string? value { get; set; }
    }
}
