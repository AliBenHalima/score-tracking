using AutoFixture;
using Bogus;
using Bogus.DataSets;
using Elastic.Clients.Elasticsearch.IndexManagement;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Database.Seeders
{
    public class DatabaseSeeder
    {
        private readonly DatabaseContext _context;

        public DatabaseSeeder (DatabaseContext context)
        {
            _context = context;
        }

        public async Task Seed ()
        {
            var fixture = new Fixture();
            IEnumerable<User> users = new Faker<User>()
                         .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                         .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                         .RuleFor(u => u.Email, (f, u) => f.Internet.Email() + f.Random.Number(10))
                         .RuleFor(u => u.Phone, (f, u) => f.Phone.PhoneNumber())
                         .RuleFor(u => u.Password, (f, u) => f.Internet.Password())
                         .RuleFor(u => u.Longitude, (f, u) => f.Address.Longitude())
                         .RuleFor(u => u.Latitude, (f, u) => f.Address.Latitude())
                         .Generate(10000);
            var testResult = Enumerable.Range(0, users.Count());
            if (_context.Users.Any())
            {
                int batchSize = 1000;
                foreach (int batchedUsers in Enumerable.Range(0, users.Count()).Where(i => i % batchSize == 0))
                {
                    _context.Users.AddRange(users.Skip(batchedUsers).Take(batchSize));
                    await _context.SaveChangesAsync();

                }
                // Alernative Solution
                //for (int i = 0; i < users.Count(); i += batchSize)
                //{
                //    _context.Users.AddRange(users.Skip(i).Take(batchSize));
                //    await _context.SaveChangesAsync();
                //}
            }
        }
    }
}
