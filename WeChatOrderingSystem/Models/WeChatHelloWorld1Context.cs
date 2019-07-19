using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WeChatHelloWorld1.Models
{
    public class WeChatHelloWorld1Context : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public WeChatHelloWorld1Context() : base("name=WeChatContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<WeChatHelloWorld1Context>(null);
            base.OnModelCreating(modelBuilder);
        }

        public System.Data.Entity.DbSet<WeChatHelloWorld1.Models.WeChatUser> WeChatUsers { get; set; }

        public System.Data.Entity.DbSet<WeChatHelloWorld1.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<WeChatHelloWorld1.Models.CustomerOrder> CustomerOrders { get; set; }

        public System.Data.Entity.DbSet<WeChatHelloWorld1.Models.CustomerOrderProduct> CustomerOrderProducts { get; set; }

        public System.Data.Entity.DbSet<WeChatHelloWorld1.Models.CustomerOrderStatue> CustomerOrderStatues { get; set; }

        public System.Data.Entity.DbSet<WeChatHelloWorld1.Models.User_AdminInfo> User_AdminInfo { get; set; }

        public System.Data.Entity.DbSet<WeChatHelloWorld1.Models.User_CustomerInfo> User_CustomerInfo { get; set; }

        public System.Data.Entity.DbSet<WeChatHelloWorld1.Models.User_MerchantInfo> User_MerchantInfo { get; set; }
    }
}
