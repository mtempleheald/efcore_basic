using System;

namespace Mth.EfCore.Repository
{
    public class Child
    {
        public Guid Id { get; set; }
        public DateTime LastUpdated { get; set; }
        public int Pid { get; set; }
        public string Name { get; set; }
    }
}
