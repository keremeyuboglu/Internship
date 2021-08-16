using Altamira.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Altamira
{
    public static class Seeder
    {
        public static void Seed(string jsonData,
                                  IServiceProvider serviceProvider)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            List<User> users =
             JsonConvert.DeserializeObject<List<User>>(
               jsonData, settings);
            using (
             var serviceScope = serviceProvider
               .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope
                              .ServiceProvider.GetService<AltamiraContext>();
                ;
            //    serviceScope.ServiceProvider.GetService<IWebHostEnvironment>();
            //    context.Database.Migrate();
                context.Database.EnsureCreated();
              if (!context.Users.Any())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        users.ForEach(u => u.Id = 0);
                        context.AddRange(users);
                        context.SaveChanges();
                        transaction.Commit();
                    }

                }
            }
        }
    }
}