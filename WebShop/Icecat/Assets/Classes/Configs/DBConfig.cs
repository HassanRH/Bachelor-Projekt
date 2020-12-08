using System;
using System.Collections.Generic;
using System.Text;

namespace IceCat.Assets.Classes.DBConfig
{
    class DBConfig
    {
        public string getConnectionString()
        {
            return "Server=praktikantsql01\\mssqlserver2017;Database=HM_Products;Trusted_Connection=true";
        }


    }
}
