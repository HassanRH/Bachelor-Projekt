using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebShop.Entity;
using System.Collections.Generic;

namespace CheckOutFormTest
{
    [TestClass]
    public class unitTest
    {
        [TestMethod]
        public void TestCustomer()
        {
            // Test customer
            string name = "InkNu";
            string email = "info@inknu.dk";
            string address = "Aksel Mølllers Have 26";
            string apartsuite = "N/A";
            string company = "InkNu IVS";
            string zip = "2000";
            string city = "Frederiksberg";
            string phone = "38873842";

            //Act
            var testcustomer = new Customer(name, email, address, apartsuite, company, zip, city, phone);

            //Assert
            // Test customer is created correctly
            StringAssert.Equals(testcustomer.Name, name);
            StringAssert.Equals(testcustomer.Email, email);
            StringAssert.Equals(testcustomer.Address, address);
            StringAssert.Equals(testcustomer.Apartsuite, apartsuite);
            StringAssert.Equals(testcustomer.Company, company);
            StringAssert.Equals(testcustomer.Zip, zip);
            StringAssert.Equals(testcustomer.City, city);
            StringAssert.Equals(testcustomer.Phone, phone);
        }
            [TestMethod]
            public void TestCustomerFailed()
            {
                // Test customer
                string name = "InkNu";
                string email = "info@inknu.dk";
                string address = "Aksel Mølllers Have 26";
                string apartsuite = "N/A";
                string company = "InkNu IVS";
                string zip = "2000";
                string city = "Frederiksberg";
                string phone = "38873842";

                //Act
                var testcustomer = new Customer(name, email, address, apartsuite, company, zip, city, phone);

                //Assert
                // Test customer is created correctly
                StringAssert.Equals(testcustomer.Name, null);
                StringAssert.Equals(testcustomer.Email, email);
                StringAssert.Equals(testcustomer.Address, address);
                StringAssert.Equals(testcustomer.Apartsuite, apartsuite);
                StringAssert.Equals(testcustomer.Company, company);
                StringAssert.Equals(testcustomer.Zip, zip);
                StringAssert.Equals(testcustomer.City, city);
                StringAssert.Equals(testcustomer.Phone, phone);
            }


            [TestMethod]
        public void TestProduct()
        {
            // Test product
            //Arrange
            int id = 1;
            string title = "MyTestProduct";
            string sku = "p00001";
            string description = "Sample product for testing";
            string longsummary = "Long test product description";
            string shortsummary = "Short test product description";
            string brand = "TestBrand";
            string category = "Test";
            List<int> pictures = new List<int>();
            List<string> bulletpoints = new List<string>();
            List<string> tags = new List<string>();
            int price = 100;

            // act
            var testproduct = new product(id, title, sku, description, longsummary, shortsummary, brand, category, pictures, bulletpoints, tags, price);

            // assert
            // Test product is created correctly
            StringAssert.Equals(testproduct.Id, id);
            StringAssert.Equals(testproduct.Title, title);
            StringAssert.Equals(testproduct.Sku, sku);
            StringAssert.Equals(testproduct.Description, description);
            StringAssert.Equals(testproduct.Longsummary, longsummary);
            StringAssert.Equals(testproduct.Shortsummary, shortsummary);
            StringAssert.Equals(testproduct.Brand, brand);
            StringAssert.Equals(testproduct.Category, category);
        }


        [TestMethod]
        public void TestProductFailed()
        {
            // Test product
            //Arrange
            int id = 1;
            string title = "MyTestProduct";
            string sku = "p00001";
            string description = "Sample product for testing";
            string longsummary = "Long test product description";
            string shortsummary = "Short test product description";
            string brand = "TestBrand";
            string category = "Test";
            List<int> pictures = new List<int>();
            List<string> bulletpoints = new List<string>();
            List<string> tags = new List<string>();
            int price = 100;

            // act
            var testproduct = new product(id, title, sku, description, longsummary, shortsummary, brand, category, pictures, bulletpoints, tags, price);

            // assert
            // Test product is created correctly
            StringAssert.Equals(testproduct.Id, id);
            StringAssert.Equals(testproduct.Title, title);
            StringAssert.Equals(testproduct.Sku, sku);
            StringAssert.Equals(testproduct.Description, description);
            StringAssert.Equals(testproduct.Longsummary, longsummary);
            StringAssert.Equals(testproduct.Shortsummary, shortsummary);
            StringAssert.Equals(testproduct.Brand, brand);
            StringAssert.Equals(testproduct.Category, category);
        }


        [TestMethod]
        public void TestOrderLine()
        {
            //Arrange
            int id = 1;
            string title = "MyTestProduct";
            string sku = "p00001";
            string description = "Sample product for testing";
            string longsummary = "Long test product description";
            string shortsummary = "Short test product description";
            string brand = "TestBrand";
            string category = "Test";
            List<int> pictures = new List<int>();
            List<string> bulletpoints = new List<string>();
            List<string> tags = new List<string>();
            int price = 100;
            int amount = 100;

            //Act
            var testproduct = new product(id, title, sku, description, longsummary, shortsummary, brand, category, pictures, bulletpoints, tags, price);
            var orderline = new OrderLine(testproduct.Sku, testproduct.Id, testproduct.Title, amount, (int)testproduct.Price);

            //Assert
            StringAssert.Equals(testproduct.Id, id);
            StringAssert.Equals(testproduct.Title, title);
            StringAssert.Equals(testproduct.Sku, sku);
            StringAssert.Equals(testproduct.Description, description);
            StringAssert.Equals(testproduct.Longsummary, longsummary);
            StringAssert.Equals(testproduct.Shortsummary, shortsummary);
            StringAssert.Equals(testproduct.Brand, brand);
            StringAssert.Equals(testproduct.Category, category);

        }

        [TestMethod]
        public void TestOrderLineFailed()
        {
            //Arrange
            int id = 1;
            string title = "MyTestProduct";
            string sku = "p00001";
            string description = "Sample product for testing";
            string longsummary = "Long test product description";
            string shortsummary = "Short test product description";
            string brand = "TestBrand";
            string category = "Test";
            List<int> pictures = new List<int>();
            List<string> bulletpoints = new List<string>();
            List<string> tags = new List<string>();
            int price = 100;
            int amount = 100;

            //Act
            var testproduct = new product(id, title, sku, description, longsummary, shortsummary, brand, category, pictures, bulletpoints, tags, price);
            var orderline = new OrderLine(testproduct.Sku, testproduct.Id, testproduct.Title, amount, (int)testproduct.Price);

            //Assert
            StringAssert.Equals(testproduct.Id, id);
            StringAssert.Equals(testproduct.Title, title);
            StringAssert.Equals(testproduct.Sku, sku);
            StringAssert.Equals(testproduct.Description, description);
            StringAssert.Equals(testproduct.Longsummary, longsummary);
            StringAssert.Equals(testproduct.Shortsummary, shortsummary);
            StringAssert.Equals(testproduct.Brand, brand);
            StringAssert.Equals(testproduct.Category, category);

        }

        [TestMethod]
        public void TestOrder()
        {
            // Test customer
            string name = "InkNu";
            string email = "info@inknu.dk";
            string address = "Aksel Mølllers Have 26";
            string apartsuite = "N/A";
            string company = "InkNu IVS";
            string zip = "2000";
            string city = "Frederiksberg";
            string phone = "38873842";

            // Test product
            int id = 1;
            string title = "MyTestProduct";
            string sku = "p00001";
            string description = "Sample product for testing";
            string longsummary = "Long test product description";
            string shortsummary = "Short test product description";
            string brand = "TestBrand";
            string category = "Test";
            List<int> pictures = new List<int>();
            List<string> bulletpoints = new List<string>();
            List<string> tags = new List<string>();
            int price = 100;
            int amount = 10;

            //Act
            var testcustomer = new Customer(name, email, address, apartsuite, company, zip, city, phone);
            var testproduct = new product(id, title, sku, description, longsummary, shortsummary, brand, category, pictures, bulletpoints, tags, price);
            var orderline = new OrderLine(testproduct.Sku, testproduct.Id, testproduct.Title, amount, (int)testproduct.Price);
            var order = new Order(testcustomer, "local", new List<OrderLine> { orderline });

            var expectedtotal = amount * price;

            //Assert
            // Test order is created to customer with correct price
            Assert.AreEqual(order.Customer, testcustomer);
            Assert.AreEqual(order.getTotalPrice(), expectedtotal);
        }



        [TestMethod]
        public void TestOrderFailed()
        {
            // Test customer
            string name = "InkNu";
            string email = "info@inknu.dk";
            string address = "Aksel Mølllers Have 26";
            string apartsuite = "N/A";
            string company = "InkNu IVS";
            string zip = "2000";
            string city = "Frederiksberg";
            string phone = "38873842";

            // Test product
            int id = 1;
            string title = "MyTestProduct";
            string sku = "p00001";
            string description = "Sample product for testing";
            string longsummary = "Long test product description";
            string shortsummary = "Short test product description";
            string brand = "TestBrand";
            string category = "Test";
            List<int> pictures = new List<int>();
            List<string> bulletpoints = new List<string>();
            List<string> tags = new List<string>();
            int price = 100;
            int amount = 10;

            //Act
            var testcustomer = new Customer(name, email, address, apartsuite, company, zip, city, phone);
            var testproduct = new product(id, title, sku, description, longsummary, shortsummary, brand, category, pictures, bulletpoints, tags, price);
            var orderline = new OrderLine(testproduct.Sku, testproduct.Id, testproduct.Title, amount, 0);
            var order = new Order(testcustomer, "local", new List<OrderLine> { orderline });

            var expectedtotal = amount * price;

            //Assert
            // Test order is created to customer with correct price
            Assert.AreEqual(order.Customer, testcustomer);
            Assert.AreEqual(order.getTotalPrice(), expectedtotal);

        }
    }
}
