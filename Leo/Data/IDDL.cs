using Leo.Data.Expressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;

namespace Leo.Data
{

    public interface IDDL
    {
        void CreateDatabase(string source);
        void DropDatabase(string source);
        void CreateTable(Type type);
        void DropTable(Type type);
        void AlterTable(Type type);
        int SaveChanges();
    }

}
