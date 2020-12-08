using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebShop.Entity;
using WebShop.Services.Interfaces;

namespace WebShop.Services.Implementation
{
    public class ProductService : IProductService<product, BinaryClass>
    {
        private readonly IDatabase _db;
        public ProductService(IDatabase db) 
        {
            this._db = db;
        }
        public List<product> GetAllProducts()
        {
                List<product> list = new List<product>();
                using (SqlConnection conn = new SqlConnection())
                {
                    var script = "SELECT Products.ID, Products.Title, Products.Sku, Brand.Brand_name, Products.Price, Products.Thumb_pic FROM Products INNER JOIN Brand ON Products.Brand_id = Brand.Brand_id";
                    SqlCommand command = new SqlCommand(script, conn);
                    conn.ConnectionString = _db.getConnectionString();
                    conn.Open();
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                        product prod = new product
                        {
                                Id = int.Parse(oReader["ID"].ToString()),
                                Title = oReader["Title"].ToString(),
                                Sku = oReader["Sku"].ToString(),
                                Brand = oReader["Brand_name"].ToString(),
                                Price = double.Parse(oReader["Price"].ToString())
                            };

                            list.Add(prod);
                        }
                    }
                    return list;
                }
        }

        public byte[] GetThumbnailFromProductId(Int64 id)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                conn.Open();
                var imageScript = "SELECT Thumb_pic From Products WHERE ID = @id";
                SqlCommand command = new SqlCommand(imageScript, conn);
                command.Parameters.AddWithValue("@id", id);
                try
                {
                    var image = (byte[])command.ExecuteScalar();
                    
                    return image;
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
        }

        public byte[] GetThumbnailFromProductSku(string sku)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                conn.Open();
                var imageScript = "SELECT Thumb_pic From Products WHERE Sku = @sku";
                SqlCommand command = new SqlCommand(imageScript, conn);
                command.Parameters.AddWithValue("@sku", sku);
                try
                {
                    var image = (byte[])command.ExecuteScalar();

                    return image;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public BinaryClass GetThumbnailFromBrandId(Int64 id)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                conn.Open();
                var imageScript = "SELECT Brand_logo From Brand WHERE Brand_id = @id";
                SqlCommand command = new SqlCommand(imageScript, conn);
                command.Parameters.AddWithValue("@id", id);
                try
                {
                    BinaryClass binaryFile = new BinaryClass();
                    binaryFile.Binary = (byte[])command.ExecuteScalar();
                    return binaryFile;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public object GetBinaryFileById(Int64 id)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                conn.Open();
                var imageScript = "SELECT Picture, content_type From Products_Gallery WHERE ID = @id";
                SqlCommand command = new SqlCommand(imageScript, conn);
                command.Parameters.AddWithValue("@id", id);
                try
                {
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        BinaryClass binaryFile = new BinaryClass();
                        while (oReader.Read())
                        {
                            binaryFile.Binary = (byte[])oReader["Picture"];
                            binaryFile.Contenttype = oReader["content_type"].ToString();
                        }
                        return binaryFile;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
        }

        public product GetSingleProduct(Int64 id)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                conn.Open();
                var selectProductScript = "SELECT ID, Title, Sku, EANCode, Price, Description, Short_summary_description, Long_summary_description, Thumb_pic FROM Products WHERE Products.ID = @id";
                var selectProductPicturesScript = "SELECT ID FROM Products_Gallery WHERE Product_id = @id";
                var selectProductBulletpointsScript = "SELECT bulletpoint FROM BulletPoints_Products INNER JOIN BulletPoint ON BulletPoints_Products.BulletPointID = BulletPoint.ID WHERE BulletPoints_Products.ProductID = @id";
                var selectProductTagsScript = "SELECT tag FROM Products_Tags INNER JOIN Tags ON Products_Tags.TagID = Tags.ID WHERE Products_Tags.ProductID = @id";
                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                command.Connection = conn;
                command.Transaction = transaction;
                command.Parameters.AddWithValue("@id",id);
                
                product prod = new product();
                try
                {
                    command.CommandText = selectProductScript;
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            prod.Id = int.Parse(oReader["ID"].ToString());
                            prod.Title = oReader["Title"].ToString();
                            prod.Price = Math.Round(double.Parse(oReader["Price"].ToString()), 2, MidpointRounding.AwayFromZero);
                            prod.Sku = oReader["Sku"].ToString();
                            prod.Ean = oReader["EANCode"].ToString();
                            prod.Description = oReader["Description"].ToString();
                            prod.Shortsummary = oReader["Short_summary_description"].ToString();
                            prod.Longsummary = oReader["Long_summary_description"].ToString();
                        }
                    }
                    command.Connection = conn;
                    command.Transaction = transaction;
                    command.CommandText = selectProductPicturesScript;
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            if(prod.pictures.Count < 5)
                            {
                                prod.addPicture(int.Parse(oReader["ID"].ToString()));
                            }                         
                        }
                    }
                    command.Connection = conn;
                    command.Transaction = transaction;
                    command.CommandText = selectProductBulletpointsScript;
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            prod.addBulletPoint(oReader["bulletpoint"].ToString());
                        }
                    }

                    command.Connection = conn;
                    command.Transaction = transaction;
                    command.CommandText = selectProductTagsScript;
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            prod.addTag(oReader["tag"].ToString());
                        }
                    }


                    transaction.Commit();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (SqlException ex)
                    {
                        if (transaction.Connection != null)
                        {
                            Console.WriteLine("An exception of type " + ex.GetType() +
                                " was encountered while attempting to roll back the transaction.");
                        }
                    }
                }
                return prod;
            }
        }

        public product GetSingleProductInfo(Int64 id)
        {
             using (SqlConnection conn = new SqlConnection())
            {
                var script = "SELECT Products.ID, Products.Title, Brand.Brand_name, Products.Price, Products.Thumb_pic FROM Products INNER JOIN Brand ON Products.Brand_id = Brand.Brand_id WHERE Products.Id = @id";
                conn.ConnectionString = _db.getConnectionString();

                SqlCommand command = new SqlCommand(script, conn);
                command.Parameters.AddWithValue("@id", id);
               
                conn.Open();
                product prod = new product();
                using (SqlDataReader oReader = command.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        prod.Id = int.Parse(oReader["ID"].ToString());
                        prod.Title = oReader["Title"].ToString();
                        prod.Brand = oReader["Brand_name"].ToString();
                        prod.Price = double.Parse(oReader["Price"].ToString());
                    }
                }
                return prod;
            }
        }

    }
}