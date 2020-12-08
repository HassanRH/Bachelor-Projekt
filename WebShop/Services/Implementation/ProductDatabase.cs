using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebShop.Services.Interfaces;

namespace WebShop.Services.Implementation
{
    public class ProductDatabase : IDatabase
    { 

        public ProductDatabase() { }

        public string getConnectionString()
        {
            return "Persist Security Info=False;Server=(local);Initial Catalog=HM_Products;User ID=Webshop_User;Password=A123456789";
        }

        public void initDatabase()
        {
            using (var conn = new SqlConnection(getConnectionString()))
            using (var command = new SqlCommand("initDatabaseTables", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
        }

        public void teardownDatabase()
        {
            using (var conn = new SqlConnection(getConnectionString()))
            using (var command = new SqlCommand("DropAllTables", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}