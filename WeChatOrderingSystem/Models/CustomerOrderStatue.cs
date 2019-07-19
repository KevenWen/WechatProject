namespace WeChatHelloWorld1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CustomerOrderStatue")]
    public partial class CustomerOrderStatue
    {
        [Key]
        public int StatueID { get; set; }

        public int CustomerOrderID { get; set; }

        public short Statue { get; set; }

        public DateTime UpdateDate { get; set; }

        [StringLength(100)]
        public string Comment { get; set; }
    }
}
