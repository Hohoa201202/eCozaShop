using System;
using eCozaStore.Models;
using Microsoft.EntityFrameworkCore;


namespace eCozaStore.Models
{
    public partial class dbCozaStoreContext : DbContext
    {
      

        public dbCozaStoreContext(DbContextOptions<dbCozaStoreContext> options)
            : base(options)
        {
        }

        public  DbSet<TblAccount> TblAccounts { get; set; }
        public  DbSet<TblAdminMenu> TblAdminMenus { get; set; }
 
        public  DbSet<TblCategory> TblCategories { get; set; } = null!;
        public  DbSet<TblComment> TblComments { get; set; } = null!;
        public  DbSet<TblCustomer> TblCustomers { get; set; } = null!;
        public  DbSet<TblFeedback> TblFeedbacks { get; set; } = null!;
        public  DbSet<TblImageProduct> TblImageProducts { get; set; } = null!;
        public  DbSet<TblMenu> TblMenus { get; set; } = null!;
        public  DbSet<TblOrder> TblOrders { get; set; } = null!;
        public  DbSet<TblOrderDetail> TblOrderDetails { get; set; } = null!;
        public  DbSet<TblPost> TblPosts { get; set; } = null!;
        public  DbSet<TblProduct> TblProducts { get; set; } = null!;
        public  DbSet<TblRole> TblRoles { get; set; } = null!;
        public  DbSet<TblSlider> TblSliders { get; set; } = null!;
        public  DbSet<TblLocation> TblLocations { get; set; } = null!;
        public  DbSet<TblFavourite> TblFavourites { get; set; } = null!;
        public  DbSet<TblPostStatus> TblPostStatus { get; set; } = null!;
        public  DbSet<TblTransactStatus> TblTransactStatuses { get; set; } = null!;
        public  DbSet<ViewCategoryProduct> ViewCategoryProducts { get; set; } = null!;
        public  DbSet<ViewImageProduct> ViewImageProducts { get; set; } = null!;
        public  DbSet<ViewMenuProduct> ViewMenuProducts { get; set; } = null!;
        public  DbSet<ViewPostCat> ViewPostCats { get; set; } = null!;
        public  DbSet<ViewOrderDetail> ViewOrderDetails { get; set; } = null!;
        public  DbSet<ViewFeedback> ViewFeedbacks { get; set; } = null!;
        public  DbSet<ViewComment> ViewComments { get; set; } = null!;
        public  DbSet<ViewOrder> ViewOrders { get; set; } = null!;
        public  DbSet<ViewFavourite> ViewFavourites { get; set; } = null!;

        // View ADMIN
        public DbSet<ViewBestSellers> ViewBestSeller { get; set; } // bán chạy nhất
        public DbSet<ViewRecentSales> ViewRecentSale { get; set; } // bán gần đây


    }
}
