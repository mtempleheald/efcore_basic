using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Mth.EfCore.Repository
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options)
            : base(options) { }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Child> Children { get; set; }
        public DbQuery<CountChildrenByParent> CountChildrenByParent { get; set; } // relies on a view existing called CountChildrenByParent, can't create using EnsureCreated()
    }
}