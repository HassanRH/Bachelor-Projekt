using IceCat.Assets.Classes;
using System;
using System.Data.SqlClient;
using System.Collections;
using IceCat.Assets.Interfaces;
using WebShop.Services.Interfaces;
using System.Data;
using System.IO;

namespace IceCat.Assets.DataAcessLayer
{
    class DataAccessLayer : IDataAccessLayer
    {
        private readonly IDatabase _db;

        public DataAccessLayer(IDatabase db)
        {
            _db = db;
        }
        public void insertProduct(Product product)
        {
            Console.WriteLine(product.ToStringShort());

            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;
                    string sql = "INSERT INTO products " +
                        "(ID, Sku, Title, Brand_id, Category_id, Description, Long_summary_description," +
                        "Short_summary_description, Thumb_pic, Manual) " +
                         "VALUES (@id, @sku, @title, @brand, @categoryid, @description, @longsum, @shortsum, @thumbpic, @manual);";
                    command.CommandText = sql;
                    command.Parameters.AddWithValue("@id", product.getID());
                    command.Parameters.AddWithValue("@sku", product.getSKU());
                    command.Parameters.AddWithValue("@title", product.getTitle());
                    command.Parameters.AddWithValue("@brand", product.getBrand());
                    command.Parameters.AddWithValue("@categoryid", product.getCategory());
                    command.Parameters.AddWithValue("@description", product.getDescription());
                    command.Parameters.AddWithValue("@longsum", product.getLongSummary());
                    command.Parameters.AddWithValue("@shortsum", product.getShortSummary());
                    command.Parameters.AddWithValue("@thumbpic", product.getThumbpic());
                    command.Parameters.AddWithValue("@manual", product.getManual());

                    var affectedRows = command.ExecuteNonQuery();

                    if (affectedRows > 0)
                    {
                        foreach (byte[] Picture in product.getPictures())
                        {
                            insertPicture(product.getID(), Picture);
                        }

                    }

                }
                catch (SqlException e)
                {
                    // Cannot insert duplicate key row in object error
                    if (e.Number == 2601 || e.Number == 2627)
                    {
                        // handle duplicate key error
                        Console.WriteLine("Duplicate error");
                        return;
                    }
                    else
                    {
                        Console.WriteLine(e + "\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                conn.Close();
            }
        }

        public void insertCategory(Category category)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = conn;
                    string sqlSelect = "SELECT Category_name FROM Category WHERE Category_id = @id";
                    command.CommandText = sqlSelect;
                    command.Parameters.AddWithValue("@id", category.getID());
                    var res = command.ExecuteScalar();
                    if (res == null)
                    {
                        string sqlInsert = "INSERT INTO Category (Category_id, Category_name) VALUES (@id,@name);";
                        command.CommandText = sqlInsert;
                        command.Parameters.AddWithValue("@name", category.getName());
                        var affectedRows = command.ExecuteNonQuery();
                    }
                }
                catch (SqlException e)
                {
                    if (e.Number == 2601 || e.Number == 2627)
                    { // Cannot insert duplicate key row in object error
                        // handle duplicate key error
                        Console.WriteLine(e);
                        return;
                    }
                    else
                    {
                        Console.WriteLine(e);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                conn.Close();
            }
        }

        public void insertBrand(Brand brand)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand();
                    string sqlSelect = "SELECT Brand_name FROM Brand WHERE Brand_id = @id";

                    command.CommandText = sqlSelect;
                    command.Connection = conn;
                    command.Parameters.AddWithValue("@id", brand.getID());
                    var res = command.ExecuteScalar();
                    if (res == null)
                    {
                        string sqlInsert = "INSERT INTO Brand (Brand_id, Brand_name, Brand_logo) VALUES ( @id, @name, @logo );";
                        command.CommandText = sqlInsert;
                        command.Parameters.AddWithValue("@name", brand.getName());
                        command.Parameters.AddWithValue("@logo", brand.getLogo());
                        var affectedRows = command.ExecuteNonQuery();
                    }
                }
                catch (SqlException e)
                {
                    // Cannot insert duplicate key row in object error
                    if (e.Number == 2601 || e.Number == 2627)
                    {
                        // handle duplicate key error
                        Console.WriteLine("duplicate error");
                        return;
                    }
                    else
                    {
                        Console.WriteLine(e);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                conn.Close();
            }
        }

        public void insertBrandsFromBulk(ArrayList brands)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    conn.Open();
                    command.Connection = conn;

                    command.Parameters.Add("@id", SqlDbType.Int);
                    command.Parameters.Add("@name", SqlDbType.VarChar);
                    command.Parameters.Add("@logo", SqlDbType.VarBinary);
                    string sqlSelect = "SELECT Brand_name FROM Brand WHERE Brand_id = @id";

                    string sqlInsert = "INSERT INTO Brand (Brand_id, Brand_name, Brand_logo) VALUES ( @id, @name, @logo );";

                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (Brand brand in brands)
                            {
                                command.CommandText = sqlSelect;
                                command.Parameters.AddWithValue("@id", brand.getID());
                                var res = command.ExecuteScalar();
                                if (res == null)
                                {
                                    command.CommandText = sqlInsert;
                                    command.Parameters.AddWithValue("@name", brand.getName());
                                    command.Parameters.AddWithValue("@logo", brand.getLogo());
                                    var affectedRows = command.ExecuteNonQuery();
                                    trans.Commit();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            throw e;
                        }
                    }
                }
                catch (SqlException e)
                {
                    // Cannot insert duplicate key row in object error
                    if (e.Number == 2601 || e.Number == 2627)
                    {
                        // handle duplicate key error
                        Console.WriteLine("duplicate error");
                        return;
                    }
                    else
                    {
                        Console.WriteLine(e);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                conn.Close();
            }
        }

        public ArrayList getProductIds()
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                ArrayList myList = new ArrayList();
                SqlCommand command = new SqlCommand();
                conn.Open();

                string sql = "SELECT ID FROM Products;";

                command.CommandText = sql;
                command.Connection = conn;

                //var producIds = command.ExecuteReader();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    myList.Add(reader[0]);
                }
                reader.Close();
                conn.Close();
                return myList;
            }
        }

        public ArrayList getBrandIds()
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                ArrayList myList = new ArrayList();
                conn.Open();
                SqlCommand command = new SqlCommand();
                string sql = "SELECT Brand_id FROM Brand;";

                command.CommandText = sql;
                command.Connection = conn;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    myList.Add(reader[0]);
                }
                conn.Close();
                return myList;
            }
        }

        public SqlCommand updateProduct(SqlCommand command, Product product, Product oldproduct)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    string sqlUpdateProduct = "UPDATE products SET Sku=@sku, Title=@title, Description= @description,Long_summary_description=@longsum, Short_summary_description=@shortsum, Thumb_pic=@thumbpic, Manual=@manual WHERE ID=@id;";
                    string sqlUpdateProductCategory = "Update products Set Category_id = @catid WHERE ID = @prodid";
                    string sqlInsertCategory = "INSERT INTO Category (Category_id, Category_name) VALUES (@catid, @catname); SELECT SCOPE_IDENTITY()";
                    string sqlCategorySelect = "SELECT Category_name FROM Category where Category_id = @catid;";

                    if (product.getCategory() == oldproduct.getCategory())
                    {
                        command.CommandText = sqlUpdateProduct;
                        command.Parameters.AddWithValue("@id", product.getID());
                        command.Parameters.AddWithValue("@sku", product.getSKU());
                        command.Parameters.AddWithValue("@title", product.getTitle());
                        command.Parameters.AddWithValue("@description", product.getDescription());
                        command.Parameters.AddWithValue("@longsum", product.getLongSummary());
                        command.Parameters.AddWithValue("@shortsum", product.getShortSummary());
                        command.Parameters.AddWithValue("@thumbpic", product.getThumbpic());
                        command.Parameters.AddWithValue("@manual", product.getManual());
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = sqlCategorySelect;
                        var catName = command.ExecuteScalar();
                        //Check if category exists at all
                        if (catName == null)
                        {
                            command.CommandText = sqlInsertCategory;
                            command.Parameters.AddWithValue("@catid", product.getCategory().getID());
                            command.Parameters.AddWithValue("@catname", product.getCategory().getName());
                            command.ExecuteNonQuery();
                        }
                        // Update product
                        command.CommandText = sqlUpdateProduct;
                        command.Parameters.AddWithValue("@id", product.getID());
                        command.Parameters.AddWithValue("@sku", product.getSKU());
                        command.Parameters.AddWithValue("@title", product.getTitle());
                        command.Parameters.AddWithValue("@description", product.getDescription());
                        command.Parameters.AddWithValue("@longsum", product.getLongSummary());
                        command.Parameters.AddWithValue("@shortsum", product.getShortSummary());
                        command.Parameters.AddWithValue("@thumbpic", product.getThumbpic());
                        command.Parameters.AddWithValue("@manual", product.getManual());
                        command.ExecuteNonQuery();
                        // Update Category on Product
                        command.CommandText = sqlUpdateProductCategory;
                        command.Parameters.AddWithValue("@id", product.getID());
                        command.Parameters.AddWithValue("@catid", product.getCategory().getID());
                        command.ExecuteNonQuery();
                    }

                    return command;

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void updateBrandLogo(int brand_id, byte[] supp_logo)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    conn.Open();
                    string sqlUpdate = "UPDATE Brand SET Brand_logo = @logo where Brand_id = @id;";

                    command.Connection = conn;
                    command.CommandText = sqlUpdate;
                    command.Parameters.AddWithValue("@id", brand_id);
                    command.Parameters.AddWithValue("@logo", supp_logo);
                    var res = command.ExecuteNonQuery();

                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                conn.Close();
            }
        }

        public void deleteProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    conn.Open();

                    string sqlDelete = "DELETE FROM Products where ID = @id;";

                    command.CommandText = sqlDelete;
                    command.Connection = conn;
                    command.Parameters.AddWithValue("@id", id);
                    var affectedRows = command.ExecuteNonQuery();
                    Console.WriteLine("DELETE \n", affectedRows);
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                conn.Close();
            }
        }

        public void insertPicture(int id, byte[] picture)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    conn.Open();

                    string sqlInsert = "INSERT INTO Products_Gallery (Picture,Product_id) VALUES (@Picture, @product_id)";

                    command.CommandText = sqlInsert;
                    command.Connection = conn;

                    command.Parameters.AddWithValue("@product_id", id);
                    command.Parameters.AddWithValue("@Picture", picture);
                    var affectedRows = command.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                conn.Close();
            }
        }

        public void insertBrandBulk(ArrayList brands)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    conn.Open();
                    int count = 1;
                    string sqlSelect = "SELECT Brand_name FROM Brand WHERE Brand_id = @id";
                    string sqlInsert = "INSERT INTO Brand (Brand_id, Brand_name, Brand_logo) VALUES ( @id, @name, @logo );";



                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (Brand brand in brands)
                            {
                                Console.Write("\r{0}   ", count + "/" + brands.Count + " Brands");
                                count++;
                                command.CommandText = sqlSelect;
                                command.Parameters.AddWithValue("@id", brand.getID());

                                var res = command.ExecuteScalar();

                                if (res == null)
                                {
                                    command.Parameters.AddWithValue("@name", brand.getName());
                                    command.Parameters.AddWithValue("@logo", brand.getLogo());

                                    command.ExecuteNonQuery();
                                }

                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("myerror: " + e.StackTrace);
                            trans.Rollback();
                            throw e;
                        }
                        finally
                        {
                            trans.Commit();
                            Console.WriteLine("");
                        }
                    }
                }
                catch (SqlException e)
                {
                    // Cannot insert duplicate key row in object error
                    if (e.Number == 2601 || e.Number == 2627)
                    {
                        // handle duplicate key error
                        Console.WriteLine("duplicate error");
                    }
                    else
                    {
                        Console.WriteLine(e);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                conn.Close();
            }
        }

        public void insertCategoryBulk(ArrayList categories)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    conn.Open();
                    int count = 1;

                    string sqlSelect = "SELECT Category_name FROM Category WHERE Category_id = @id";
                    string sqlInsert = "INSERT INTO Category (Category_id, Category_name) VALUES (@id,@name);";

                    command.Connection = conn;
                    command.CommandText = sqlSelect;

                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (Category category in categories)
                            {
                                command.CommandText = sqlSelect;
                                command.Parameters.AddWithValue("@id", category.getID());
                                var res = command.ExecuteScalar();
                                if (res == null)
                                {
                                    command.CommandText = sqlInsert;
                                    command.Parameters.AddWithValue("@name", category.getName());
                                    command.ExecuteNonQuery();
                                }
                                Console.Write("\r{0}   ", count + "/" + categories.Count + " Categories");
                                count++;
                            }
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            throw e;
                        }
                        finally
                        {
                            trans.Commit();
                            Console.WriteLine("");
                        }
                    }
                }
                catch (SqlException e)
                {
                    // Cannot insert duplicate key row in object error
                    if (e.Number == 2601 || e.Number == 2627)
                    {
                        // handle duplicate key error
                        Console.WriteLine("duplicate error");
                    }
                    else
                    {
                        Console.WriteLine(e);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                conn.Close();
            }
        }

        public void insertTagsBulk(ArrayList tags)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    conn.Open();
                    int count = 1;
                    string sqlSelect = "Select tag From Tags WHERE tag = @tag;";
                    string sqlInsert = "INSERT INTO Tags (tag) VALUES (@tag);";
                    command.CommandText = sqlInsert;

                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (string tagStr in tags)
                            {
                                Console.Write("\r{0}   ", count + "/" + tags.Count + " Tags");
                                command.CommandText = sqlSelect;
                                command.Parameters.AddWithValue("@tag", tagStr);
                                var res = command.ExecuteScalar();
                                if (res == null)
                                {
                                    command.CommandText = sqlInsert;
                                    command.ExecuteNonQuery();
                                }
                                count++;
                            }
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            Console.WriteLine(e);
                            throw e;
                        }
                        finally
                        {
                            trans.Commit();
                            Console.WriteLine("");
                        }
                    }
                }
                catch (SqlException e)
                {
                    // Cannot insert duplicate key row in object error
                    if (e.Number == 2601 || e.Number == 2627)
                    {
                        // handle duplicate key error
                        Console.WriteLine("duplicate error");
                    }
                    else
                    {
                        Console.WriteLine(e);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                conn.Close();
            }
        }

        public void insertProductBulk(ArrayList products)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    conn.Open();
                    int count = 1;

                    string sqlInsert = "INSERT INTO Products (ID, Sku, EANCode, Title, Price, Brand_id, Category_id, Description, Long_summary_description, Short_summary_description, Thumb_pic, Manual) " +
                                    "VALUES (@id, @sku, @ean, @title, @price, @brand, @categoryid, @description, @longsum, @shortsum, @thumbpic, @manual);";

                    command.Connection = conn;
                    command.CommandText = sqlInsert;


                    using (var trans = conn.BeginTransaction())
                    {
                        try
                        {
                            foreach (Product product in products)
                            {
                                Console.Write("\r{0}   ", count + "/" + products.Count + " Products");
                                count++;

                                command.Parameters.AddWithValue("@id", product.getID());
                                command.Parameters.AddWithValue("@sku", product.getSKU());
                                command.Parameters.AddWithValue("@ean", product.getEANCode());
                                command.Parameters.AddWithValue("@title", product.getTitle());
                                command.Parameters.AddWithValue("@price", product.getPrice());
                                command.Parameters.AddWithValue("@brand", product.getBrand());
                                command.Parameters.AddWithValue("@categoryid", product.getCategory());
                                command.Parameters.AddWithValue("@description", product.getDescription());
                                command.Parameters.AddWithValue("@longsum", product.getLongSummary());
                                command.Parameters.AddWithValue("@shortsum", product.getShortSummary());
                                command.Parameters.AddWithValue("@thumbpic", product.getThumbpic());
                                command.Parameters.AddWithValue("@manual", product.getManual());

                                var insertedProductId = command.ExecuteNonQuery();

                                foreach (byte[] picture in product.getPictures())
                                {
                                    string sqlInsertPicture = "INSERT INTO Products_Gallery (Picture,Product_id) VALUES (@Picture, @product_id)";

                                    command.CommandText = sqlInsertPicture;
                                    command.Parameters.AddWithValue("@Picture", picture);
                                    command.Parameters.AddWithValue("@product_id", product.getID());
                                    command.ExecuteNonQuery();
                                }

                                foreach (string tagStr in product.getFeatures())
                                {
                                    string selectTag = "SELECT ID FROM Tags Where tag = @tag;";
                                    string insertTag = "INSERT INTO Tags (Tag) VALUES (@tag); SELECT SCOPE_IDENTITY()";
                                    string insertProductTag = "INSERT INTO Products_Tags (TagID,ProductID) VALUES (@tagid,@productid);";
                                    //Get Tag from Db if Exists
                                    command.CommandText = selectTag;
                                    command.Parameters.AddWithValue("@tag", tagStr);

                                    var res = command.ExecuteScalar();

                                    //check if Tag exists. If it does insert into products_tags
                                    if (res != null)
                                    {
                                        command.CommandText = insertProductTag;
                                        command.Parameters.AddWithValue("@tagid", int.Parse(res.ToString()));
                                        command.Parameters.AddWithValue("@productid", product.getID());
                                        command.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        command.CommandText = insertTag;
                                        var tagId = command.ExecuteScalar();
                                        command.CommandText = insertProductTag;
                                        command.Parameters.AddWithValue("@tagid", int.Parse(tagId.ToString()));
                                        command.Parameters.AddWithValue("@productid", product.getID());
                                        command.ExecuteNonQuery();

                                    }
                                }

                                foreach (string bulletpoint in product.getBulletPoints())
                                {
                                    string selectBulletPoint = "SELECT ID FROM BulletPoint Where bulletpoint = @bullet;";
                                    string insertBulletPoint = "INSERT INTO BulletPoint (bulletpoint) VALUES (@bullet); SELECT SCOPE_IDENTITY()";
                                    string insertBulletPointsAndProducts = "INSERT INTO BulletPoints_Products (ProductID, BulletPointID) VALUES (@productid, @bulletid);";

                                    command.CommandText = selectBulletPoint;
                                    command.Parameters.AddWithValue("@bullet", bulletpoint);

                                    var result = command.ExecuteScalar();

                                    if (result != null)
                                    {
                                        command.CommandText = insertBulletPointsAndProducts;
                                        command.Parameters.AddWithValue("@bulletid", int.Parse(result.ToString()));
                                        command.Parameters.AddWithValue("@productid", product.getID());
                                        command.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        command.CommandText = insertBulletPoint;
                                        var res = command.ExecuteScalar();
                                        command.CommandText = insertBulletPointsAndProducts;
                                        command.Parameters.AddWithValue("@bulletid", int.Parse(res.ToString()));
                                        command.Parameters.AddWithValue("@productid", product.getID());
                                        command.ExecuteNonQuery();
                                    }
                                }

                            }
                            trans.Commit();
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            Console.WriteLine(e.StackTrace);
                            throw e;
                        }
                        finally
                        {

                        }
                    }
                }
                catch (SqlException e)
                {
                    // Cannot insert duplicate key row in object error
                    if (e.Number == 2601 || e.Number == 2627)
                    {
                        // handle duplicate key error
                        Console.WriteLine("duplicate error");
                    }
                    else
                    {
                        Console.WriteLine(e);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                conn.Close();
            }
        }

        public void updateProductBulk(ArrayList products)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    SqlTransaction transaction;
                    conn.Open();
                    int count = 1;
                    string sqlSelect = "SELECT * FROM products where ID=@id;";

                    transaction = conn.BeginTransaction("UpdateProductBulk");
                    command.Connection = conn;
                    command.Transaction = transaction;

                    try
                    {
                        foreach (Product product in products)
                        {
                            command.CommandText = sqlSelect;
                            SqlDataReader reader = command.ExecuteReader();

                            Product oldprod = GetFullProduct(product.getID());

                            if (oldprod.Equals(product))
                            {
                                continue;
                            }
                            else
                            {
                                updateFullProduct(product, oldprod);
                                Console.Write("\r{0}   ", count + "/" + products.Count + " Checked/Updated Products");
                                Console.WriteLine("ID: " + product.getID());
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("");
                        transaction.Rollback();
                        throw e;
                    }
                    finally
                    {
                        transaction.Commit();
                        Console.WriteLine("");
                    }

                }
                catch (SqlException e)
                {
                    // Cannot insert duplicate key row in object error
                    if (e.Number == 2601 || e.Number == 2627)
                    {
                        // handle duplicate key error
                        Console.WriteLine("duplicate error");
                    }
                    else
                    {
                        Console.WriteLine("\n" + e);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\n" + ex);
                }
                conn.Close();
            }
        }

        public Product GetFullProduct(int id)
        {
            Product product = new Product();
            string sqlSelectProduct = "SELECT * FROM products INNER JOIN Brand ON products.Brand_id = brand.Brand_id INNER JOIN Category ON Products.Category_id = Category.Category_id WHERE ID =@id;";
            string sqlSelectPictures = "SELECT picture FROM Products_Gallery where Product_id=@id;";
            string sqlSelectTags = "SELECT tag FROM Tags INNER JOIN Products_Tags ON Tags.ID = Products_Tags.TagID WHERE Products_Tags.ProductID = @id;";
            string sqlSelectBulletPoints = "SELECT bulletpoint FROM BulletPoint INNER JOIN BulletPoints_Products ON BulletPoint.ID = BulletPoints_Products.BulletPointID WHERE BulletPoints_Products.ProductID = @id;";

            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    //Setup
                    SqlCommand command = new SqlCommand();
                    SqlTransaction transaction;
                    conn.Open();
                    transaction = conn.BeginTransaction("FetchFullProduct");
                    //Read product table
                    command.CommandText = sqlSelectProduct;
                    command.Transaction = transaction;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        product.setID(int.Parse(reader["ID"].ToString()));
                        product.setSKU(reader["Sku"].ToString());
                        product.setEANCode(reader["EANCode"].ToString());
                        product.setTitle(reader["Title"].ToString());
                        product.setPrice(double.Parse(reader["Price"].ToString()));
                        product.setBrand(new Brand(int.Parse(reader["Brand_id"].ToString()), reader["Brand_name"].ToString(), (byte[])reader["Brand_logo"]));
                        product.setCategory(new Category(int.Parse(reader["Category_id"].ToString()), reader["Category_name"].ToString()));
                        product.setDescription(reader["Description"].ToString());
                        product.setShortSummary(reader["Short_summary_description"].ToString());
                        product.setLongSummary(reader["Long_summary_description"].ToString());
                        product.setThumbpic((byte[])reader["Thumb_pic"]);
                        product.setManual((byte[])reader["Manual"]);

                    }
                    //Read bulletpoints
                    command.CommandText = sqlSelectBulletPoints;
                    SqlDataReader readerBullet = command.ExecuteReader();
                    while (readerBullet.Read())
                    {
                        product.addBulletPoint(readerBullet["bulletpoint"].ToString());
                    }
                    //Read Tags
                    command.CommandText = sqlSelectTags;
                    SqlDataReader readerTags = command.ExecuteReader();
                    while (readerTags.Read())
                    {
                        product.addFeautre(readerTags["tag"].ToString());
                    }

                    //Read Pictures
                    command.CommandText = sqlSelectPictures;
                    SqlDataReader readerPictures = command.ExecuteReader();
                    while (readerPictures.Read())
                    {
                        product.addPicture((byte[])readerPictures["Picture"]);
                    }
                    transaction.Commit();
                    return product;

                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void updateFullProduct(Product product, Product oldproduct)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    SqlCommand command = new SqlCommand();
                    SqlTransaction transaction;
                    transaction = conn.BeginTransaction("UpdateProduct");
                    command.Transaction = transaction;

                    //Tags
                    command = CheckAndUpdateTags(command, product, oldproduct);
                    //Bulletpoint
                    command = CheckAndUpdateBulletpoints(command, product, oldproduct);
                    //Pictures
                    //command = CheckAndUpdatePictures(command, product, oldproduct);
                    //Product
                    command = updateProduct(command, product, oldproduct);

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public SqlCommand CheckAndUpdateTags(SqlCommand command, Product newproduct, Product oldproduct)
        {
            string sqlInsertTag = "INSERT INTO Tags (tag) VALUES (@tag); SELECT SCOPE_IDENTITY()";
            string sqlInsertProductTag = "INSERT INTO Products_Tags (TagID, ProductID) VALUES (@tagid, @prodid);";
            string sqlSelectTagID = "SELECT ID FROM Tags WHERE tag = @tag;";
            //Tag
            foreach (var tag in newproduct.getFeatures())
            {
                if (oldproduct.getFeatures().Contains(tag))
                {
                    continue;
                }
                else
                {
                    command.CommandText = sqlSelectTagID;
                    command.Parameters.AddWithValue("@tag", tag);
                    var tagID = command.ExecuteScalar();

                    if (tagID == null)
                    {
                        command.CommandText = sqlInsertTag;
                        command.Parameters.AddWithValue("@tag", tag);
                        tagID = command.ExecuteScalar();

                        command.CommandText = sqlInsertProductTag;
                        command.Parameters.AddWithValue("@tagid", tagID);
                        command.Parameters.AddWithValue("@prodid", newproduct.getID());
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = sqlInsertProductTag;
                        command.Parameters.AddWithValue("@prodid", newproduct.getID());
                        command.ExecuteNonQuery();
                    }


                    command.CommandText = sqlInsertTag;
                    command.Parameters.AddWithValue("@tag", tag);
                    var tagId = command.ExecuteScalar();
                }
            }

            return command;
        }

        public SqlCommand CheckAndUpdateBulletpoints(SqlCommand command, Product newproduct, Product oldproduct)
        {
            string sqlInsertBulletpoint = "INSERT INTO BulletPoint (bulletpoint) VALUES (@bullet); SELECT SCOPE_IDENTITY()";
            string sqlInsertProductBulletpoint = "INSERT INTO BulletPoints_Products (BulletPointID, ProductID) VALUES (@bullletid, @prodid);";
            string sqlSelectBulletID = "SELECT ID FROM BulletPoint WHERE bulletpoint = @bullet;";
            //Tag
            foreach (var bullet in newproduct.getBulletPoints())
            {
                if (oldproduct.getFeatures().Contains(bullet))
                {
                    continue;
                }
                else
                {
                    command.CommandText = sqlSelectBulletID;
                    command.Parameters.AddWithValue("@bullet", bullet);
                    var tagID = command.ExecuteScalar();

                    if (tagID == null)
                    {
                        command.CommandText = sqlInsertBulletpoint;
                        tagID = command.ExecuteScalar();

                        command.CommandText = sqlInsertProductBulletpoint;
                        command.Parameters.AddWithValue("@tagid", tagID);
                        command.Parameters.AddWithValue("@prodid", newproduct.getID());
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = sqlInsertProductBulletpoint;
                        command.Parameters.AddWithValue("@tagid", tagID);
                        command.Parameters.AddWithValue("@prodid", newproduct.getID());
                        command.ExecuteNonQuery();
                    }
                }
            }

            return command;
        }

        public SqlCommand CheckAndUpdatePictures(SqlCommand command, Product newproduct, Product oldproduct)
        {
            string sqlInsertBulletpoint = "INSERT INTO BulletPoint (bulletpoint) VALUES (@bullet); SELECT SCOPE_IDENTITY()";
            string sqlInsertProductBulletpoint = "INSERT INTO BulletPoints_Products (BulletPointID, ProductID) VALUES (@bullletid, @prodid);";
            string sqlSelectBulletID = "SELECT Picture FROM BulletPoint WHERE bulletpoint = @bullet;";
            //Tag
            foreach (var bullet in newproduct.getBulletPoints())
            {
                if (oldproduct.getFeatures().Contains(bullet))
                {
                    continue;
                }
                else
                {
                    command.CommandText = sqlSelectBulletID;
                    command.Parameters.AddWithValue("@bullet", bullet);
                    var tagID = command.ExecuteScalar();

                    if (tagID == null)
                    {
                        command.CommandText = sqlInsertBulletpoint;
                        tagID = command.ExecuteScalar();

                        command.CommandText = sqlInsertProductBulletpoint;
                        command.Parameters.AddWithValue("@tagid", tagID);
                        command.Parameters.AddWithValue("@prodid", newproduct.getID());
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        command.CommandText = sqlInsertProductBulletpoint;
                        command.Parameters.AddWithValue("@tagid", tagID);
                        command.Parameters.AddWithValue("@prodid", newproduct.getID());
                        command.ExecuteNonQuery();
                    }
                }
            }

            return command;
        }

        public ArrayList getProducts()
        {
            throw new NotImplementedException();
        }

        public ArrayList getBrands()
        {
            throw new NotImplementedException();
        }

        public ArrayList getCategories()
        {
            throw new NotImplementedException();
        }

        public ArrayList getProduct(int id)
        {
            throw new NotImplementedException();
        }

        public ArrayList getBrand(int id)
        {
            throw new NotImplementedException();
        }

        public ArrayList getCategory(int id)
        {
            throw new NotImplementedException();
        }

        public void insertProduct(IProduct product)
        {
            throw new NotImplementedException();
        }

        public void initDatabase()
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {

                    SqlCommand command = conn.CreateCommand();
                    conn.Open();
                    //string prefix = "..\\..\\..\\Assets\\init\\";
                    string prefix = "WebShop\\Icecat\\Assets\\init\\";
                    string script = File.ReadAllText(prefix + "drop_tables.sql");
                    string script1 = File.ReadAllText(prefix + "create_brand_table.sql");
                    string script2 = File.ReadAllText(prefix + "create_category_table.sql");
                    string script3 = File.ReadAllText(prefix + "create_tags_table.sql");
                    string script4 = File.ReadAllText(prefix + "create_products_table.sql");
                    string script5 = File.ReadAllText(prefix + "create_products_tags_table.sql");
                    string script6 = File.ReadAllText(prefix + "create_products_gallery_table.sql");
                    string script7 = File.ReadAllText(prefix + "create_bulletpoint_table.sql");
                    string script8 = File.ReadAllText(prefix + "create_bulletpoints_products_table.sql");
                    string script9 = File.ReadAllText(prefix + "create_Customer_table.sql");
                    string script10 = File.ReadAllText(prefix + "create_Orderline_table.sql");
                    string script11 = File.ReadAllText(prefix + "create_Orders.sql");
                    string script12 = File.ReadAllText(prefix + "create_subscriptions_table.sql");

                    var affectedRows = command.ExecuteNonQuery();

                    command.CommandText = script;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Dropped all tables");
                    command.CommandText = script1;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Created brand table");
                    command.CommandText = script2;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Created category table");
                    command.CommandText = script3;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Created tags table");
                    command.CommandText = script4;
                    command.ExecuteNonQuery();
                    Console.WriteLine("created products table");
                    command.CommandText = script5;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Created products_tags table");
                    command.CommandText = script6;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Created products_gallery tables");
                    command.CommandText = script7;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Created bulletpoint table");
                    command.CommandText = script8;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Created bulletpoints_products table");
                    command.CommandText = script9;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Created Customer table");
                    command.CommandText = script10;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Created Orderline table");
                    command.CommandText = script11;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Created Orders table");
                    command.CommandText = script12;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Created Subscriptions table");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //throw (e);
                }
            }
        }

        public void teardownDatabase()
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                try
                {
                    Directory.GetCurrentDirectory();
                    string prefix = "WebShop\\Icecat\\Assets\\init\\";
                    conn.Open();
                    SqlCommand command = conn.CreateCommand();
                    string script = File.ReadAllText(prefix + "drop_tables.sql");
                    command.CommandText = script;

                    var affectedRows = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //throw (e);
                }
            }
        }

    }

}
