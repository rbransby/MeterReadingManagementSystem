using MeterReadingsManagementSystem.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadingsManagementSystem.Server.Tests
{
    public static class TestData
    {
        public static IEnumerable<Account> GetTestAccounts()
        {
            return new[]
            {
                new Account
                {
                    Id = 2344,
                    FirstName = "Tommy",
                    LastName = "Test"
                },
                new Account{
                    Id = 4534,
                    FirstName = "JOSH",
                    LastName = "TEST"
                },                
                new Account{
                    Id = 2351,
                    FirstName = "Gladys",
                    LastName = "Test"
                },                
                new Account{
                    Id = 1244,
                    FirstName = "Tony",
                    LastName = "Test"
                },                
                new Account{
                    Id = 1248,
                    FirstName = "Pam",
                    LastName = "Test"
                },
            };
        }
    }
}
