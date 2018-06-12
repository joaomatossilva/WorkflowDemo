using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace WorflowDemo1.Data.Migrations
{
    [Migration(201706122145)]
    public class _001_InitialMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Categories")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey("PK__Categories")
                .WithColumn("Name").AsString(200).NotNullable();

            Create.Table("Products")
                .WithColumn("Id").AsInt32().NotNullable().Identity().PrimaryKey("PK__Produtcs")
                .WithColumn("Name").AsString(200).NotNullable()
                .WithColumn("CategoryId").AsInt32().Nullable();

        }
    }
}