namespace ChienVHShopOnline.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Category
    {
    
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
