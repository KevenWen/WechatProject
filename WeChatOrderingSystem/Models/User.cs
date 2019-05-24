using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WeChatHelloWorld1.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [StringLength(28)]
        public string OpenID { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(100)]
        public string Address { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }
    }
}