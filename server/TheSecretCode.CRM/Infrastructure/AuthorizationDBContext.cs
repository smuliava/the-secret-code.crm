using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Configuration;
using System.Data.Entity.ModelConfiguration;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.Entity.Migrations.History;
using System.ComponentModel.DataAnnotations.Schema;
using TheSecretCode.CRM.Classes;
using System.Data.Entity.Infrastructure;

namespace TheSecretCode.CRM.Infrastructure
{
    public class AuthorizationDbContext : IdentityDbContext<SystemUser>
    {
        public AuthorizationDbContext()
            : base("AuthorizationContext", throwIfV1Schema: false)
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            string dbSchemaName = ConfigurationManager.AppSettings["athentificatinon:DBSchemaName"];
            string dbSystemUsersTableName = ConfigurationManager.AppSettings["athentificatinon:DBSystemUsersTableName"];
            string dbSystemUsersClaimsTableName = ConfigurationManager.AppSettings["athentificatinon:DBSystemUsersClaimsTableName"];
            string dbSystemUsersLoginsTableName = ConfigurationManager.AppSettings["athentificatinon:DBSystemUsersLoginsTableName"];
            string dbSystemUsersRolesTableName = ConfigurationManager.AppSettings["athentificatinon:DBSystemUsersRolesTableName"];
            string dbSystemUserIdFieldName = ConfigurationManager.AppSettings["athentificatinon:DBSystemUserIdFieldName"];

            modelBuilder.HasDefaultSchema(dbSchemaName);

            modelBuilder.Entity<SystemUser>().ToTable(dbSystemUsersTableName);

            modelBuilder.Entity<IdentityUserClaim>()
                .ToTable(dbSystemUsersClaimsTableName)
                .Property(entityProperty => entityProperty.UserId)
                    .HasColumnName(dbSystemUserIdFieldName);

            modelBuilder.Entity<IdentityUserLogin>()
                .ToTable(dbSystemUsersLoginsTableName)
                .Property(entityProperty => entityProperty.UserId)
                    .HasColumnName(dbSystemUserIdFieldName);

            modelBuilder.Entity<IdentityUserRole>()
                .ToTable(dbSystemUsersRolesTableName)
                .Property(entityProperty => entityProperty.UserId)
                    .HasColumnName(dbSystemUserIdFieldName);

            modelBuilder.Entity<IdentityRole>()
                .ToTable("tblRoles");

            modelBuilder.Entity<HistoryRow>()
                .ToTable(tableName: "__MigrationHistory", schemaName: "auth")
                .HasKey(r => new { r.MigrationId, r.ContextKey });
        }

        public static AuthorizationDbContext Create()
        {
            return new AuthorizationDbContext();
        }
    }
}