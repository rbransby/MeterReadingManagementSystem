using MeterReadingsManagementSystem.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeterReadingsManagementSystem.Server.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<MeterReading> MeterReadings { get; set; }

        public string DbPath { get; private set; }

        public DataContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }
    }
}
