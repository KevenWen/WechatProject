namespace WeChatHelloWorld1.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;

    public partial class CustomerOrderMaster
    {

        public CustomerOrder CustomerOrder { get; set; }

        public List<CustomerOrderProduct> ProductList { get; set; }
        public List<CustomerOrderStatue> StatueList { get; set; }
    }
}
