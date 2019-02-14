using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Mth.EfCore.Repository;

namespace Mth.EfCore.Cli
{
    class Program
    {
        private static string _connStr = @"
            Server=localhost,1433;
            Database=MthEfDb;
            User ID=SA;
            Password=Nothing2SeeHere;
        ";
        //  async main requires C# 7.1 - add to the project file <LangVersion>7.1</LangVersion>
        public static async Task Main(string[] args)
        {

            // ASP.NET Core web app would use
            // services.AddDbContext<Context>(options =>
            //     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            var optionsBuilder = new DbContextOptionsBuilder<SampleContext>();
            optionsBuilder.UseSqlServer(_connStr);
            using (var context = new SampleContext(optionsBuilder.Options))
            {
                Console.WriteLine("Creating database (if necessary)...");
                var success = await context.Database.EnsureCreatedAsync(); // Microsoft.EntityFrameworkCore.Storage.IDatabaseCreator
                if (success) {
                    Console.WriteLine("Database ready.");
                }

                Console.WriteLine("Creating a new parent record...");
                var newparent = new Parent { Name = "MTH" };
                context.Parents.Add(newparent);
                var recordCount = await context.SaveChangesAsync(); // Microsoft.EntityFrameworkCore.Storage.IDatabase
                Console.WriteLine ("Created {0} records", recordCount);

                Console.WriteLine("Query all directly, readonly, using AsNoTracking to improve performance...");
                var parents = await context.Parents.AsNoTracking().ToListAsync();// Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                parents.ForEach(p => Console.WriteLine("{0}", p.Name));

                // Query a record and update it
                Console.WriteLine("Query and then update a parent record...");
                var parent = await context.Parents.FirstOrDefaultAsync(p => p.Name == "MTH"); // Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions
                parent.Name = "MTH Updated";
                recordCount = await context.SaveChangesAsync();// Microsoft.EntityFrameworkCore.Storage.IDatabase
                Console.WriteLine ("Affected {0} records", recordCount);

                Console.WriteLine ("Query all using direct SQL");
                // To use a DB view can't use EnsureCreated(), have to go DB-first which would be preferable for production anyway
                // Also much have a DbQuery registered in the context for this to work
                // Microsoft.EntityFrameworkCore.Relational.RelationalQueryableExtensions
                var families = await context.CountChildrenByParent.FromSql(
                    @"SELECT p.Id AS ParentID
                      ,      p.Name AS ParentName
                      ,      COUNT(1) AS NumberOfChildren
                      FROM      MthEfDb.dbo.Parents p
                      LEFT JOIN MthEfDb.dbo.Children c ON  p.Id = c.ParentId
                      GROUP BY p.Id, p.Name;").ToListAsync(); 
                families.ForEach(p => Console.WriteLine("{0}", p.ParentName));
                
                // DML action                
                await context.Database.ExecuteSqlCommandAsync("delete from parents where name='MTH'");// Microsoft.EntityFrameworkCore.RelationalDatabaseFacadeExtensions

                Console.WriteLine ("Deleting database ready for the next run...");
                success = await context.Database.EnsureDeletedAsync();
                if (success) {
                    Console.WriteLine("Database deleted.");
                }
            }
        }
    }
}