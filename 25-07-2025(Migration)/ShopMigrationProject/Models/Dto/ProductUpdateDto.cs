namespace ChienVHShopOnline.Models.Dto
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductUpdateDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> ColorId { get; set; }
        public Nullable<int> ModelId { get; set; }
        public Nullable<int> StorageId { get; set; }
        public Nullable<System.DateTime> SellStartDate { get; set; }
        public Nullable<System.DateTime> SellEndDate { get; set; }
        public Nullable<int> IsNew { get; set; }
    }
}
