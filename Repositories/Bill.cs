using Back.Context.Connection;
using Back.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using static Back.Models.Bill;
using static Back.Models.Parametric;
using static Back.Models.Reply;

namespace Back.Repositories
{
    public class Bill : IBill
    {
        public Connection conn;
        private readonly IConfiguration _configuration;
        ReplyData r = new ReplyData();

        public Bill(IConfiguration configuration)
        {
            _configuration = configuration;
            conn = new Connection();
        }

        public async Task<ReplyData> SaveBill(BillModel data)
        {
            try
            {
                using (SqlConnection connection = conn.ConnectBD(_configuration))
                {
                    connection.Open();

                    List<DefaultClass> Clients = new List<DefaultClass>();

                    SqlCommand cmd = new SqlCommand("sp_Create_Bill", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ID_CLIENT", data.IdClient));
                    cmd.Parameters.Add(new SqlParameter("@BILL_NUMBER", data.BillNumber));
                    cmd.Parameters.Add(new SqlParameter("@TOTAL_ARTICLES", data.TotalArticles));
                    cmd.Parameters.Add(new SqlParameter("@SUBTOTAL_INVOICED", data.SubTotalInvoiced));
                    cmd.Parameters.Add(new SqlParameter("@TOTAL_TAX", data.TtotalTax));
                    cmd.Parameters.Add(new SqlParameter("@TOTAL_INVOICED", data.TotalInvoiced));
                    cmd.Parameters.Add("@FLAG", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@ID_BILL", SqlDbType.Int).Direction = ParameterDirection.Output;
                    SqlDataReader sqldr = await cmd.ExecuteReaderAsync();
                    r.Flag = cmd.Parameters["@FLAG"].Value != null ? (bool)cmd.Parameters["@FLAG"].Value : false;
                    if (r.Flag)
                    {
                        data.IdBill = Convert.ToInt32(cmd.Parameters["@ID_BILL"].Value);
                        await sqldr.CloseAsync();

                        foreach (var product in data.BillDetail)
                        {
                            SqlCommand cmd2 = new SqlCommand("sp_Create_DetailBill", connection);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.Add(new SqlParameter("@ID_BILL", data.IdBill));
                            cmd2.Parameters.Add(new SqlParameter("@ID_PRODUCT", product.IdProduct));
                            cmd2.Parameters.Add(new SqlParameter("@AMOUNT", product.Amount));
                            cmd2.Parameters.Add(new SqlParameter("@UNIT_PRICE", product.UnitPrice));
                            cmd2.Parameters.Add(new SqlParameter("@SUBTOTAL", product.SubTotal));
                            SqlDataReader sqldr2 = await cmd2.ExecuteReaderAsync();
                            await sqldr2.CloseAsync();
                        }

                        r.Data = data.IdBill;
                        r.Status = 200;
                        r.Message = "OK";
                    }
                    else
                    {
                        r.Data = null;
                        r.Status = 200;
                        r.Message = "Numero de factura ya existe";
                    }

                    return r;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<ReplyData> GetBills(BillFilter data)
        {
            try
            {
                using (SqlConnection connection = conn.ConnectBD(_configuration))
                {
                    connection.Open();

                    List<BillFound> Bills = new List<BillFound>();

                    SqlCommand cmd = new SqlCommand("sp_Consult_Bill", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@BILL_NUMBER", data.BillNumber));
                    cmd.Parameters.Add(new SqlParameter("@ID_CLIENT", data.IdClient));
                    SqlDataReader sqldr = await cmd.ExecuteReaderAsync();

                    while (await sqldr.ReadAsync())
                    {
                        BillFound db = new BillFound();
                        db.BillNumber = Convert.ToInt32(sqldr["BillNumber"]);
                        db.EmisionDateBill = Convert.ToDateTime(sqldr["EmisionDateBill"]).ToString("dd-MM-yyyy hh:mm:ss tt");
                        db.TotalInvoiced = Convert.ToDecimal(sqldr["TotalInvoiced"]);
                        Bills.Add(db);
                    }
                    await sqldr.CloseAsync();
                    r.Data = Bills;
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
