
namespace ChienVHShopOnline.Models.Dto
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetailCreateDto
    {
        public int ProductID { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<int> Quantity { get; set; }    
    }
}
