namespace ChienVHShopOnline.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Color
    {
        public int ColorId { get; set; }
        public string Color1 { get; set; }
    
        public virtual ICollection<Product> Products { get; set; }
    }
}
