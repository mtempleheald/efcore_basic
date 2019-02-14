using System;
using System.Collections.Generic;

namespace Mth.EfCore.Repository
{
    public class CountChildrenByParent
    {
        public Guid ParentId { get; set; }
        public string ParentName { get; set; }
        public int NumberOfChildren { get; set; }
    }
}
