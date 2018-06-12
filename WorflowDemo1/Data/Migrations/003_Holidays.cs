using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace WorflowDemo1.Data.Migrations
{
    [Migration(201806122350)]
    public class _003_Holidays : AutoReversingMigration
    {
        public override void Up()
        {
            Create.Table("Holidays")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey("PK__Holiday")
                .WithColumn("Name").AsString(250).NotNullable()
                .WithColumn("Date").AsDateTime().NotNullable();
        }
    }
}