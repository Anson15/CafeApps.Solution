namespace CafeApps.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProperty : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Menus", "Image", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Menus", "Image", c => c.Binary());
        }
    }
}
