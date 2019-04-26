using System;
using System.Collections.Generic;

namespace Leo.Data.EF
{
    public class EntityTypeProvider : IEntityTypeProvider
    {
        private readonly IEnumerable<Type> types;

        public EntityTypeProvider(IEnumerable<Type> types)
        {
            this.types = types;
        }

        public IEnumerable<Type> GetTypes()
        {
            return types;
        }
    }
}
