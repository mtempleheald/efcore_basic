using System;
using System.Collections.Generic;

namespace Mth.EfCore.Repository
{
    public class Parent
    {
        public Guid Id { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Pid { get; set; }
        public string Name { get; set; }
        public List<Child> Children {get; set;}
    }
}
