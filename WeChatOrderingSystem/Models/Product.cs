namespace WeChatHelloWorld1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public int ProductID { get; set; }

        public int MerchantID { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(200)]
        public string ImagePath { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? LatestModify { get; set; }

        [StringLength(500)]
        public string Comment { get; set; }
    }
}
