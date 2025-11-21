namespace my_pospointe.Models.DynamicColumn
{
    public class ItemDto
    {
        public string ItemId { get; set; } = null!;

        public string ItemName { get; set; } = null!;

        public string ItemDeptId { get; set; } = null!;

        public decimal? ItemPrice { get; set; }

        public string Tax1Status { get; set; } = null!;

        public string? Picturepath { get; set; }

        public string PricePrompt { get; set; } = null!;

        public string? BtnColor { get; set; }

        public string Visible { get; set; } = null!;

        public string? EnableName { get; set; }

        public string? EnablePicture { get; set; }

        public int? ListOrder { get; set; }

        public int? IsKot { get; set; }

        public bool? IsKot2 { get; set; }

        public bool? IsModifer { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? PromptDescription { get; set; }

        public bool? ShowInKitchenDisplay { get; set; }

        public decimal? LoyalityCredit { get; set; }

        public decimal? Qty { get; set; }

        public DateTime? LastSold { get; set; }

        public decimal? OnlinePrice { get; set; }

        public bool? Idcheck { get; set; }

        public bool? FoodStampable { get; set; }

        public bool? ManagersOnly { get; set; }

        public bool? SellOnline { get; set; }

        public bool? Countthis { get; set; }

        public DateTime? Lastimported { get; set; }

        public DateTime? CreatedDate { get; set; }

        public decimal? Cost { get; set; }

        public string? Brand { get; set; }

        public decimal? IteminCase { get; set; }

        public decimal? CasePrice { get; set; }

        public string? Soldby { get; set; }

        public string? OnlineImagelink { get; set; }

        public string? ItemDescription { get; set; }
    }
}
