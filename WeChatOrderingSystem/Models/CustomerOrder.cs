namespace WeChatHelloWorld1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerOrder")]
    public partial class CustomerOrder
    {
        public int CustomerOrderID { get; set; }

        public int MerchantID { get; set; }

        [StringLength(20)]
        public string CourierName { get; set; }

        [StringLength(50)]
        public string CourierPhone { get; set; }

        public int CustomerID { get; set; }

        [Required]
        [StringLength(20)]
        public string CustomerName { get; set; }

        [Required]
        [StringLength(50)]
        public string CustomerPhone { get; set; }

        [Required]
        [StringLength(100)]
        public string DeliveryAdress { get; set; }

        public decimal DeliveryX { get; set; }

        public decimal DeliveryY { get; set; }
    }
}
