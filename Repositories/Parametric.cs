using Back.Context.Connection;
using Back.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using static Back.Models.Parametric;
using static Back.Models.Reply;

namespace Back.Repositories
{
    public class Parametric : IParametric
    {
        public Connection conn;
        private readonly IConfiguration _configuration;
        ReplyData r = new ReplyData();

        public Parametric(IConfiguration configuration)
        {
            _configuration = configuration;
            conn = new Connection();
        }

        public async Task<ReplyData> GetClients()
        {
            try
            {
                using (SqlConnection connection = conn.ConnectBD(_configuration))
                {
                    connection.Open();

                    List<DefaultClass> Clients = new List<DefaultClass>();

                    SqlCommand cmd = new SqlCommand("sp_Consult_Clients", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader sqldr = await cmd.ExecuteReaderAsync();

                    while (await sqldr.ReadAsync())
                    {
                        DefaultClass db = new DefaultClass();
                        db.Id = Convert.ToInt32(sqldr["IdClient"]);
                        db.Name = sqldr["SocialReasoning"].ToString();
                        Clients.Add(db);
                    }
                    await sqldr.CloseAsync();
                    r.Data = Clients;
                    r.Status = 200;
                    r.Message = "OK";
                    return r;
                }
            }catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<ReplyData> GetProducts()
        {
            try
            {
                using (SqlConnection connection = conn.ConnectBD(_configuration))
                {
                    connection.Open();

                    List<Products> Products = new List<Products>();

                    SqlCommand cmd = new SqlCommand("sp_Consult_Products", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader sqldr = await cmd.ExecuteReaderAsync();

                    while (await sqldr.ReadAsync())
                    {
                        Products db = new Products();
                        db.IdProduct = Convert.ToInt32(sqldr["IdProduct"]);
                        db.ProductName = sqldr["ProductName"].ToString();
                        db.ProductImage = sqldr["ProductImage"].ToString();
                        db.UnitPriceProduct = Convert.ToDecimal(sqldr["UnitPriceProduct"]);
                        Products.Add(db);
                    }
                    await sqldr.CloseAsync();
                    r.Data = Products;
                    r.Status = 200;
                    r.Message = "OK";
                    return r;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}
