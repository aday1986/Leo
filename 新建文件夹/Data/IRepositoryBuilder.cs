using System;
using System.Collections.Generic;
using System.Text;

namespace Leo.Data
{
   public interface IRepositoryBuilder
    {
        IRepository<T> Create<T>() where T : class;
    }
}
