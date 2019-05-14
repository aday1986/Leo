using System;
using System.Collections.Generic;
using System.Linq;

namespace Leo.Data.EF
{
    public class EntityTypeCollection :List<Type>, IEntityTypeCollection
    {
        public EntityTypeCollection()
        {
        }

        public EntityTypeCollection(params Type[] types)
        {
            this.AddRange(types);
        }

       
       
    }
}
