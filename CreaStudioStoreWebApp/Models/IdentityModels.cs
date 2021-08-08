using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks; 
using CreaStudioStoreWebApp.Entities;
using CreaStudioStoreWebApp.Entities.Base;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CreaStudioStoreWebApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        #region plus 
        public Guid UserId { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public ObjectContext ObjectContext
        {
            get
            {
                var objectContext = (this as IObjectContextAdapter);
                if (objectContext != null)
                    return (this as IObjectContextAdapter).ObjectContext;
                else
                    return null;
            }
        }



        public static List<ApplicationDbContext> listDBContext = new List<ApplicationDbContext>();
        private static readonly object lockRoot = new object();
        public static int elapseTime = 5000;

        public bool isBusy = false;
        public Stopwatch stopWatch = new Stopwatch();

       

        public void doUpdateAccessDbContext(bool isBusy)
        {
            this.isBusy = isBusy;
            this.stopWatch = new Stopwatch();
            this.stopWatch.Start();
        }

        public static ApplicationDbContext doGetDbContext()
        {
            lock (lockRoot)
            {
                ApplicationDbContext temp = null;
                ApplicationDbContext db;
                try
                {

                    for (int i = 0; i < listDBContext.Count; i++)
                    {
                        db = listDBContext.ElementAt(i);
                        if (db.Database.Connection != null && db.Database.Connection.State == ConnectionState.Open && db.isBusy == false)
                        {
                            temp = db;
                            break;
                        }
                    }
                    if (temp == null)
                    {
                        temp = new ApplicationDbContext();
                        listDBContext.Add(temp);
                    }
                    while (temp.Database.Connection.State == ConnectionState.Closed)
                    {
                        temp.Database.Connection.Open();
                        Thread.Sleep(10);
                    }
                    temp.doUpdateAccessDbContext(true);
                    return temp;
                }
                catch (System.Exception ex)
                {
                    return null;

                }
            }
        }

        public static void doCheckListDBContext()
        {
            lock (lockRoot)
            {
                try
                {
                    ApplicationDbContext temp = null;
                    for (int i = 0; i < listDBContext.Count; i++)
                    {
                        temp = listDBContext.ElementAt(i);
                        if (temp.stopWatch.ElapsedMilliseconds > elapseTime)
                        {
                            temp.Dispose();
                            listDBContext.Remove(temp);
                            i--;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                }
            }
        }


        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is EntityBase && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {


                if (entityEntry.State == EntityState.Added)
                {
                    ((EntityBase)entityEntry.Entity).CreatedOn = DateTime.Now;
                    ((EntityBase)entityEntry.Entity).Id = Guid.NewGuid();
                    ((EntityBase)entityEntry.Entity).CreatedBy = UserId;
                }
                if (entityEntry.State == EntityState.Modified)
                {
                    ((EntityBase)entityEntry.Entity).ModifiedBy = UserId;
                    ((EntityBase)entityEntry.Entity).LatestUpdatedOn = DateTime.Now;
                    // ((Infrastructure.Entity.EntityBase)entityEntry.Entity).IsDeleted = false;
                }

            }

            //UpdateDates();
            return base.SaveChanges();
        }
        #endregion

        public DbSet<Blog> BlogContext { get; set; }
        public DbSet<Cart> CartContext { get; set; }
        public DbSet<CartItem> CartItemContext { get; set; }
        public DbSet<Category> CategoryContext { get; set; }
        public DbSet<Compare> CompareContext { get; set; }
        public DbSet<Contact> ContactContext { get; set; }
        public DbSet<List> ListContext { get; set; }
        public DbSet<Order> OrderContext { get; set; }
        public DbSet<OrderItem> OrderItemContext { get; set; }
        public DbSet<Product> ProductContext { get; set; }
        public DbSet<ProductCompare> ProductCompareContext { get; set; }
        public DbSet<ProductList> ProductListContext { get; set; }
        public DbSet<ProductRelatedProduct> ProductRelatedProductContext { get; set; }
        public DbSet<ProductWishlist> ProductWishlistContext { get; set; }
        public DbSet<Review> ReviewContext { get; set; }
        public DbSet<Specification> SpecificationContext { get; set; }
        public DbSet<Tag> TagContext { get; set; }
        public DbSet<Wishlist> WishlistContext { get; set; }
    }
}