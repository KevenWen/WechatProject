namespace WeChatHelloWorld1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_MerchantInfo
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
        [StringLength(50)]
        public string MerchantName { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string MerchantAddress { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(10)]
        public string PhoneNumber { get; set; }
    }
}
