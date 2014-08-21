using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebApiOAuthBase.Identity.DataModels;
using WebApiOAuthBase.Infrastructure;
using WebApiOAuthBase.Infrastructure.Interface;



namespace WebApiOAuthBase.Identity
{
    public class SecurityDataContext : IdentityDbContext<ApplicationUser>
    {
        #region Constructors
        public SecurityDataContext()
            : base(AppConfig.SecuritDatabaseConnectionString, throwIfV1Schema: false)
        {

        }


        public static SecurityDataContext Create()
        {
            return new SecurityDataContext();
        }
        #endregion

        #region Auditing
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            ApplyAudit();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            ApplyAudit();
            return base.SaveChanges();
        }

        private void ApplyAudit()
        {

            try
            {
                foreach (var auditableEntity in ChangeTracker.Entries<IAuditable>())
                {
                    if (auditableEntity.State == EntityState.Added ||
                        auditableEntity.State == EntityState.Modified)
                    {
                        // implementation may change based on the useage scenario, this
                        // sample is for forma authentication.
                        string currentUser = "InstalUser";
                        try
                        {
                            currentUser = HttpContext.Current.User.Identity.Name;
                        }
                        catch (Exception)
                        {
                            currentUser = "InstalUser";
                        }

                        // modify updated date and updated by column for 
                        // adds of updates.
                        auditableEntity.Entity.UpdatedDate = DateTime.Now;
                        auditableEntity.Entity.UpdatedBy = currentUser;

                        // pupulate created date and created by columns for
                        // newly added record.
                        if (auditableEntity.State == EntityState.Added)
                        {
                            auditableEntity.Entity.CreatedDate = DateTime.Now;
                            auditableEntity.Entity.CreatedBy = currentUser;
                        }
                        else
                        {
                            // we also want to make sure that code is not inadvertly
                            // modifying created date and created by columns 
                            auditableEntity.Property(p => p.CreatedDate).IsModified = false;
                            auditableEntity.Property(p => p.CreatedBy).IsModified = false;
                        }


                        //TODO implementa calendar
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region DbSets       
        public DbSet<ClientApplication> ClientApplications { get; set; }
        public DbSet<UserClientAppAssignation> UserClientAppAssignations { get; set; }
        public DbSet<ApplicationAuthorization> ApplicationAuthorizations { get; set; }
        public DbSet<UserAuthorizationAssignation> UserAuthorizationAssignations { get; set; }
     

        
        #endregion

        #region Overriding OnModelCreate
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Types().Configure(t => t.MapToStoredProcedures());
        }
        #endregion

    }
}