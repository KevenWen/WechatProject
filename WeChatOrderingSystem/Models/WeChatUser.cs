using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeChatHelloWorld1.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WeChatUser")]
    public partial class WeChatUser
    {
        [Key]
        [StringLength(28)]
        public string OpenID { get; set; }

        [StringLength(100)]
        public string Nickname { get; set; }

        public int? Sex { get; set; }

        [StringLength(30)]
        public string Province { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(200)]
        public string HeadImgUrl { get; set; }

        public DateTime? SubscribeTime { get; set; }

        [StringLength(20)]
        public string Language { get; set; }

        public short? UserType { get; set; }
  
    }
}
