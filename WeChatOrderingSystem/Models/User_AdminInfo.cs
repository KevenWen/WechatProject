namespace WeChatHelloWorld1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User_AdminInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

  
        [StringLength(50)]
        public string WeChatOpenID { get; set; }

 
        [StringLength(50)]
        public string AdminName { get; set; }
     
        [StringLength(50)]
        public string PhoneNumber { get; set; }
    }
}
