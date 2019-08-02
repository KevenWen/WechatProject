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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [StringLength(50)]
        public string WeChatOpenID { get; set; }

        [StringLength(50)]
        public string MerchantName { get; set; }

        [StringLength(100)]
        public string MerchantAddress { get; set; }

        [StringLength(50)]
        public string PhoneNumber { get; set; }
    }
}
