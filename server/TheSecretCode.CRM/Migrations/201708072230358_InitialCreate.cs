namespace TheSecretCode.CRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "auth.tblRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "auth.tblSystemUsersRoles",
                c => new
                    {
                        SystemUserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.SystemUserId, t.RoleId })
                .ForeignKey("auth.tblRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("auth.tblSystemUsers", t => t.SystemUserId, cascadeDelete: true)
                .Index(t => t.SystemUserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "auth.tblSystemUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        RegistrationDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "auth.tblSystemUsersClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SystemUserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("auth.tblSystemUsers", t => t.SystemUserId, cascadeDelete: true)
                .Index(t => t.SystemUserId);
            
            CreateTable(
                "auth.tblSystemUsersLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        SystemUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.SystemUserId })
                .ForeignKey("auth.tblSystemUsers", t => t.SystemUserId, cascadeDelete: true)
                .Index(t => t.SystemUserId);
            
            CreateTable(
                "auth.__MigrationHistory",
                c => new
                    {
                        MigrationId = c.String(nullable: false, maxLength: 128),
                        ContextKey = c.String(nullable: false, maxLength: 128),
                        Model = c.Binary(),
                        ProductVersion = c.String(),
                    })
                .PrimaryKey(t => new { t.MigrationId, t.ContextKey });
            
        }
        
        public override void Down()
        {
            DropForeignKey("auth.tblSystemUsersRoles", "SystemUserId", "auth.tblSystemUsers");
            DropForeignKey("auth.tblSystemUsersLogins", "SystemUserId", "auth.tblSystemUsers");
            DropForeignKey("auth.tblSystemUsersClaims", "SystemUserId", "auth.tblSystemUsers");
            DropForeignKey("auth.tblSystemUsersRoles", "RoleId", "auth.tblRoles");
            DropIndex("auth.tblSystemUsersLogins", new[] { "SystemUserId" });
            DropIndex("auth.tblSystemUsersClaims", new[] { "SystemUserId" });
            DropIndex("auth.tblSystemUsers", "UserNameIndex");
            DropIndex("auth.tblSystemUsersRoles", new[] { "RoleId" });
            DropIndex("auth.tblSystemUsersRoles", new[] { "SystemUserId" });
            DropIndex("auth.tblRoles", "RoleNameIndex");
            DropTable("auth.__MigrationHistory");
            DropTable("auth.tblSystemUsersLogins");
            DropTable("auth.tblSystemUsersClaims");
            DropTable("auth.tblSystemUsers");
            DropTable("auth.tblSystemUsersRoles");
            DropTable("auth.tblRoles");
        }
    }
}
