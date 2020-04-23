namespace CafeApps.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        MenusId = c.Int(nullable: false),
                        UsersId = c.Int(nullable: false),
                        FoodName = c.String(),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalDue = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CartId)
                .ForeignKey("dbo.Menus", t => t.MenusId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UsersId, cascadeDelete: true)
                .Index(t => t.MenusId)
                .Index(t => t.UsersId);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        MenuId = c.Int(nullable: false, identity: true),
                        Category = c.Int(nullable: false),
                        FoodName = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remarks = c.String(),
                        Image = c.Binary(),
                    })
                .PrimaryKey(t => t.MenuId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Role = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Tables",
                c => new
                    {
                        TableId = c.Int(nullable: false, identity: true),
                        TableName = c.String(),
                        Status = c.Int(nullable: false),
                        UserId = c.Int(),
                    })
                .PrimaryKey(t => t.TableId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tables", "UserId", "dbo.Users");
            DropForeignKey("dbo.Carts", "UsersId", "dbo.Users");
            DropForeignKey("dbo.Carts", "MenusId", "dbo.Menus");
            DropIndex("dbo.Tables", new[] { "UserId" });
            DropIndex("dbo.Carts", new[] { "UsersId" });
            DropIndex("dbo.Carts", new[] { "MenusId" });
            DropTable("dbo.Tables");
            DropTable("dbo.Users");
            DropTable("dbo.Menus");
            DropTable("dbo.Carts");
        }
    }
}
