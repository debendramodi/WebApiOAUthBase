namespace WebApiOAuthBase.Identity.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using WebApiOAuthBase.Identity.DataModels;


    internal sealed class Configuration : DbMigrationsConfiguration<SecurityDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SecurityDataContext context)
        {
            Seeder.SeedAdminUser(context);
            base.Seed(context);
        }
    }
}
