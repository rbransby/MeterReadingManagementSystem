using CsvHelper;
using CsvHelper.Configuration;
using MeterReadingsManagementSystem.Server.Data;
using MeterReadingsManagementSystem.Shared.Model;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace MeterReadingsManagementSystem.Server.Migrations
{
    public partial class AddSeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "MeterReadingsManagementSystem.Data.SeedData";
            using (var dataContext =  new DataContext())
            {
                Console.WriteLine(assembly.GetName().Name);
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                        csvReader.Context.RegisterClassMap<AccountMap>();
                        var accounts = csvReader.GetRecords<Account>();
                        dataContext.Accounts.AddRange(accounts);
                        dataContext.SaveChanges();
                    }
                }
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            using (var dataContext = new DataContext())
            {
                dataContext.Accounts.RemoveRange(dataContext.Accounts);
                dataContext.SaveChanges();
            }
        }

        public class AccountMap : ClassMap<Account>
        {
            public AccountMap()
            {
                Map(m => m.Id).Name("AccountId");                
            }
        }
    }
}
