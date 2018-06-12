using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace WorflowDemo1.Data.Migrations
{
    [Migration(201812062250)]
    public class _002_ForeignKey : AutoReversingMigration
    {
        public override void Up()
        {
            Create.ForeignKey("FK_ProductCategory")
                .FromTable("Products").ForeignColumn("CategoryId").ToTable("Categories").PrimaryColumn("Id")
                    .OnDelete(Rule.Cascade);
        }
    }
}