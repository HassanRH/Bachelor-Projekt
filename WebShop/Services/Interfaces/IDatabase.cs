using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Services.Interfaces
{
    public interface IDatabase
    {
        string getConnectionString();

        void teardownDatabase();

        void initDatabase();
    }
}
