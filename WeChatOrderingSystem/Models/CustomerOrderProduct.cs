namespace WeChatHelloWorld1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerOrderProduct")]
    public partial class CustomerOrderProduct
    {
        public int CustomerOrderProductID { get; set; }

        public int CustomerOrderID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Price { get; set; }

        public int Status { get; set; }

        [StringLength(200)]
        public string ImagePath { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? LatestModify { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }

        public int MerchantID { get; set; }
    }
}
