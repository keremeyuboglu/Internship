using Altamira.Data.Entities;
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
                if (!context.Users.Any())
                {
                    context.AddRange(users);
                    context.SaveChanges();
                }
            }
        }
    }
}