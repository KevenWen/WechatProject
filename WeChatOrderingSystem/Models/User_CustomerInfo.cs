namespace WeChatHelloWorld1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_CustomerInfo
    {
        [Key]
        [Column(Order = 0)]
        public Guid ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string WeChatOpenID { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string Address { get; set; }
    }
}
