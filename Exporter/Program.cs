using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using FlatMapper;

namespace Exporter
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var connection =
                new SqlConnection(ConfigurationManager.ConnectionStrings["appConString"].ConnectionString))
            {
                connection.Open();

                var groups = connection.Query<HolidayGrouped>(@"
SELECT 
    DATEPART(Year, Date) as Year,
    DATEPART(Month, Date) as Month,
    count(1) as Count
FROM Holidays
GROUP BY DATEPART(Year, Date), DATEPART(Month, Date)
ORDER BY DATEPART(Year, Date), DATEPART(Month, Date)
");

                var settings = new Formo.Configuration().Bind<ExportSettings>();

                var layout = new Layout<HolidayGrouped>.DelimitedLayout()
                    .WithMember(x => x.Year)
                    .WithMember(x => x.Month)
                    .WithMember(x => x.Count)
                    .WithQuote("\"")
                    .WithDelimiter(";");
                using (var file = File.OpenWrite(settings.FileName))
                {
                    var flatfile = new FlatFile<HolidayGrouped>(layout, file);
                    flatfile.Write(groups);
                }


        }

        }
    }

    public class HolidayGrouped
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Count { get; set; }
    }

    public class ExportSettings
    {
        public string FileName { get; set; }
    }
}
