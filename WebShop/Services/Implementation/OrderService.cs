using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebShop.Entity;
using WebShop.Models;
using WebShop.Services.Interfaces;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Kernel.Colors;
using System.Net.Mail;
using System.Net;

namespace WebShop.Services.Implementation
{

    public class OrderService : IOrderService<Order, CheckOutForm>
    {
        private readonly IDatabase _db;
        //MIKKEL LOCAL PATH
            private readonly string _pdfFolderPath = "C:\\Users\\hassan.hussain\\Desktop\\MH_Webshop\\mh_webshop\\PDF";
        //SERVER PATH
        //private readonly string _pdfFolderPath = "C:\\inetpub\\wwwroot\\MH\\mh_webshop\\PDF\\";
        public OrderService(IDatabase db)
        {
            this._db = db;
        }
        public Order GetOrder(int id)
        {
            Order order = null;
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                conn.Open();

                var selectOrderScript = "SELECT so.Id, so.Createdate, so.PDF, so.Shipping, c.Name, c.Email, c.Address, c.Apartsuite, c.Zip, c.City, c.Phone " +
                                            "FROM ShopOrders so " +
                                            "INNER JOIN Customer c ON so.CustomerId = c.Id WHERE so.Id = @id";

                var selectOrderLines = "SELECT Id, OrderId, Name, ProductSKU, ProductID, Amount, Price FROM OrderLine Where OrderId = @id";

                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction;
                transaction = conn.BeginTransaction("GetOrderTransaction");
                command.Connection = conn;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = selectOrderScript;
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var orderId = int.Parse(oReader["Id"].ToString());
                            var shipping = oReader["Shipping"].ToString();
                            var createdDate = (DateTime)(oReader["Createdate"]);
                            var pdfpath = oReader["PDF"].ToString();
                            Customer cust = new Customer();
                            cust.Name = oReader["Name"].ToString();
                            cust.Email = oReader["Email"].ToString();
                            cust.Address = oReader["Address"].ToString();
                            cust.Apartsuite = oReader["Apartsuite"].ToString();
                            cust.Zip = oReader["Zip"].ToString();
                            cust.City = oReader["City"].ToString();
                            cust.Phone = oReader["Phone"].ToString();
                            order = new Order { OrderNo = orderId, Customer = cust, Shipping = shipping, Createdate = createdDate, PDF = pdfpath };
                        }
                    }
                    
                    command.CommandText = selectOrderLines;
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            var lineId = int.Parse(oReader["Id"].ToString());
                            var lineName = oReader["Name"].ToString();
                            var lineProductSKU = oReader["ProductSKU"].ToString();
                            var lineProductID = int.Parse(oReader["ProductID"].ToString());
                            var lineAmount = int.Parse(oReader["Amount"].ToString());
                            var linePrice = Math.Round(double.Parse(oReader["Price"].ToString()), 2, MidpointRounding.AwayFromZero);
                            OrderLine line = new OrderLine {Id = lineId, Name = lineName, ProductSKU = lineProductSKU , ProductID = lineProductID, Amount = lineAmount, Price = linePrice};
                            order.addOrderLine(line);
                        }
                    }
                    transaction.Commit();
                    return order;
                }
                catch(Exception e)
                {
                    throw e;
                }
            }
                
        }

        public void AddSubscriber(string email)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                conn.Open();

                var insertSubscriber = "INSERT INTO Subscriptions (mail) VALUES (@email)";
                SqlCommand command = conn.CreateCommand();
                command.Connection = conn;
                try
                {
                    command.CommandText = insertSubscriber;
                    command.Parameters.AddWithValue("@email", email);
                    command.ExecuteScalar();
                }catch(Exception e)
                {
                    
                }
            }
        }

        public int insertOrder(Order order)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                conn.Open();
                    //Scripts
                var insertCustomerScript = "INSERT INTO Customer (Name,Email,Address,Apartsuite,Company,Country_ISO,Zip,City,Phone) VALUES(@name,@email,@address,@apartsuite,@company,@country,@zip,@city,@phone);" +
                                            "SELECT SCOPE_IDENTITY()";
                var insertOrderScript = "INSERT INTO ShopOrders (CustomerID,Shipping) VALUES (@custid,@shipping); SELECT SCOPE_IDENTITY()";
                var insertOrderLineScript = "INSERT INTO OrderLine (OrderID,Name,ProductSKU,ProductID,Amount,Price) VALUES (@oid, @productname, @prodSKU, @prodID, @amount, @price)";

                SqlCommand command = conn.CreateCommand();
                SqlTransaction transaction;
                 transaction = conn.BeginTransaction("AddOrderTransaction");

                command.Connection = conn;
                command.Transaction = transaction;


                try
                {
                    //Customer
                    command.CommandText = insertCustomerScript;
                    command.Parameters.AddWithValue("@name", order.Customer.Name);
                    command.Parameters.AddWithValue("@email", order.Customer.Email);
                    command.Parameters.AddWithValue("@address", order.Customer.Address);
                    command.Parameters.AddWithValue("@country", order.Customer.Country);
                    command.Parameters.AddWithValue("@zip", order.Customer.Zip);
                    command.Parameters.AddWithValue("@city", order.Customer.City);
                    command.Parameters.AddWithValue("@phone", order.Customer.Phone);
                    command.Parameters.AddWithValue("@apartsuite", order.Customer.Apartsuite);
                    command.Parameters.AddWithValue("@company", order.Customer.Company);

                    var customerId = command.ExecuteScalar();
                    order.Customer.Id = int.Parse(customerId.ToString());
                    //Order
                    command.CommandText = insertOrderScript;
                    command.Parameters.AddWithValue("@custid", order.Customer.Id);
                    command.Parameters.AddWithValue("@shipping", order.Shipping);
                    var orderNo = command.ExecuteScalar();
                    order.OrderNo = int.Parse(orderNo.ToString());
                    //OrderLine
                    command.CommandText = insertOrderLineScript;
                    command.Parameters.Add("@oid", SqlDbType.Int);
                    command.Parameters.Add("@productname", SqlDbType.VarChar);
                    command.Parameters.Add("@prodSKU", SqlDbType.VarChar);
                    command.Parameters.Add("@prodID", SqlDbType.Int);
                    command.Parameters.Add("@amount", SqlDbType.Int);
                    command.Parameters.Add("@price", SqlDbType.Money);
                    foreach (var line in order.OrderLines)
                    {
                        command.CommandText = insertOrderLineScript;
                        command.Parameters["@oid"].Value = order.OrderNo;
                        command.Parameters["@productname"].Value = line.Name;
                        command.Parameters["@prodID"].Value = line.ProductID;
                        command.Parameters["@prodSKU"].Value = line.ProductSKU;
                        command.Parameters["@amount"].Value = line.Amount;
                        command.Parameters["@price"].Value = line.Price;
                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return order.OrderNo;
                }
                catch (Exception e)
                {
                    try
                    {
                        transaction.Rollback();
                        throw e;
                    }
                    catch (Exception ex2)
                    {
                        throw ex2;
                    }
                }

            }
        }

        public Order ConvertModelToOrderTypeT(CheckOutForm model)
        {
            var company = model.Company == null ? "" : model.Company;
            var apartsuite = model.Apartsuite == null ? "" : model.Apartsuite;
            Customer customer = new Customer{ Name = model.First_Name + " " + model.Last_Name, Email = model.Email, Address = model.Address,
                                              Apartsuite = apartsuite, Company = company, Zip = model.ZipCode,City = model.City, Phone = model.Phone, Country = model.Country };
            List<OrderLine> orderLines = new List<OrderLine>();
            foreach(var product in model.products)
            {
                OrderLine line = new OrderLine { ProductSKU = product.Sku, ProductID = product.Id, Name = product.Title, Price = product.Price, Amount = product.Amount };
                orderLines.Add(line);
            }
            return new Order(customer, model.Shipping, orderLines);
        }

        public Order createOrder(Order param)
        {
            throw new NotImplementedException();
        }

        public string GenerateOrderPDF(Order order)
        {
            var pdfPath = _pdfFolderPath + "O" + order.OrderNo.ToString() + ".pdf";
            PdfWriter writer = new PdfWriter(pdfPath);
            PdfDocument pdf = new PdfDocument(writer);

            Document document = new Document(pdf);
            Paragraph header = new Paragraph("Order: " + order.OrderNo.ToString())
               .SetTextAlignment(TextAlignment.CENTER)
               .SetFontSize(20);

            document.Add(header);



            Paragraph shippingParagraph = new Paragraph("Shipping:").SetTextAlignment(TextAlignment.RIGHT).SetFontSize(15);
            document.Add(shippingParagraph);
            Paragraph shippingParagraph1 = new Paragraph(order.Shipping).SetTextAlignment(TextAlignment.RIGHT).SetFontSize(15);
            document.Add(shippingParagraph1);

            Paragraph subheader = new Paragraph("Customer Information")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(15);
            document.Add(subheader);

            LineSeparator ls = new LineSeparator(new SolidLine());
            document.Add(ls);

            Paragraph customer1 = new Paragraph(order.Customer.Name).SetTextAlignment(TextAlignment.LEFT).SetFontSize(12);
            document.Add(customer1);

            Paragraph customer2 = new Paragraph(order.Customer.Email).SetTextAlignment(TextAlignment.LEFT).SetFontSize(12);
            document.Add(customer2);

            Paragraph customer3 = new Paragraph(order.Customer.Zip + " " + order.Customer.City).SetTextAlignment(TextAlignment.LEFT).SetFontSize(12);
            document.Add(customer3);

            Paragraph customer4 = new Paragraph(order.Customer.Address).SetTextAlignment(TextAlignment.LEFT).SetFontSize(12);
            document.Add(customer4);

            document.Add(ls);

            Paragraph subheader1 = new Paragraph("Order Information")
            .SetTextAlignment(TextAlignment.CENTER)
             .SetFontSize(15);
            document.Add(subheader1);


            Table table = new Table(5, false);

            Cell cell11 = new Cell(1, 1).SetBackgroundColor(ColorConstants.GRAY).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Product Name"));
            Cell cell12 = new Cell(1, 1).SetBackgroundColor(ColorConstants.GRAY).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Product SKU"));
            Cell cell13 = new Cell(1, 1).SetBackgroundColor(ColorConstants.GRAY).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Amount"));
            Cell cell14 = new Cell(1, 1).SetBackgroundColor(ColorConstants.GRAY).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Price"));
            Cell cell15 = new Cell(1, 1).SetBackgroundColor(ColorConstants.GRAY).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph("Total"));

            table.AddCell(cell11);
            table.AddCell(cell12);
            table.AddCell(cell13);
            table.AddCell(cell14);
            table.AddCell(cell15);

            foreach (var orderline in order.OrderLines) 
            {
                table.AddCell(new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(orderline.Name)));
                table.AddCell(new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(orderline.ProductSKU)));
                table.AddCell(new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(orderline.Amount.ToString())));
                table.AddCell(new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(orderline.Price.ToString())));
                var totalprice = (orderline.Amount * orderline.Price).ToString();
                table.AddCell(new Cell(1, 1).SetTextAlignment(TextAlignment.CENTER).Add(new Paragraph(totalprice)));
            }

            document.Add(table);
            document.Close();
            return pdfPath;
        }

        public void UpdateOrderWithPDF(Order order, string path)
        {
            using (SqlConnection conn = new SqlConnection(_db.getConnectionString()))
            {
                conn.Open();
                //Scripts
                var insertCustomerScript = "UPDATE ShopOrders SET PDF = @pdf WHERE Id = @id";

                SqlCommand command = conn.CreateCommand();

                command.Connection = conn;
                command.CommandText = insertCustomerScript;
                command.Parameters.AddWithValue("@pdf", path);
                command.Parameters.AddWithValue("@id", order.OrderNo);
                command.ExecuteNonQuery();
            }
        }

        public void SendOrderConfirmationMailByGmail(int orderno)
        {
            var order = GetOrder(orderno);

            var fromAddress = new MailAddress("hassanrh1996@gmail.com", "MHShop - Order Confirmation - O" + order.OrderNo.ToString());
            var toAddress = new MailAddress(order.Customer.Email, "O" + order.OrderNo.ToString());
            string fromPassword = "Stilldre1";
            string subject = "MHShop - Order Confirmation";
            string orderlinesStr = "";
            foreach(var line in order.OrderLines)
            {
                orderlinesStr += line.Amount + " x " + line.Name + "\n";
            }

            string body =
                "Order: O" + order.OrderNo.ToString() + "\n" +
                "Shipping: \n" + order.Shipping + "\n" +
                "_____________________" + "\nCustomer Information: " + "\n" +
                order.Customer.Company + "\n" + 
                order.Customer.Name + "\n" +
                order.Customer.Zip + " " + order.Customer.City + "\n" +
                order.Customer.Address + " " + order.Customer.Apartsuite + "\n" +
                order.Customer.Phone + "\n\nOrderlines:\n" +
                orderlinesStr;

            System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
            contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Pdf;
            contentType.Name = "O" + order.OrderNo + ".pdf";
            

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress){   Subject = subject + " - " + order.OrderNo.ToString(), Body = body   })
            {
                message.Attachments.Add(new Attachment(order.PDF, contentType));
                smtp.Send(message);
            }
        }
    }
}