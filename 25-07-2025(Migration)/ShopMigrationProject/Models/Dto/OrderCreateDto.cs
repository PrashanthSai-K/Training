
namespace ChienVHShopOnline.Models.Dto
{
    using System;
    using System.Collections.Generic;

    public partial class OrderCreateDto
    {
        public string OrderName { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public string PaymentType { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }

        public virtual ICollection<OrderDetailCreateDto> OrderDetailsDto { get; set; }
    }
}
