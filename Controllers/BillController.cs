using Back.Interfaces;
using Back.Models;
using Back.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using static Back.Models.Bill;
using static Back.Models.Reply;

namespace Back.Controllers
{
    [ApiController]
    public class BillController : ControllerBase
    {
        
        private readonly ILogger<BillController> _logger;
        private readonly IBill _bill;
        public ReplySucess oReply = new ReplySucess();
        public BillController(ILogger<BillController> logger, IBill bill)
        {
            _logger = logger;
            _bill = bill;
        }

        /// <summary>
        /// guardar fatcuras nuevas
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/SaveBill")]
        public async Task<ReplyData> SaveBill(BillModel data)
        {
            var rta = await _bill.SaveBill(data);
            oReply.Data = rta.Data;
            oReply.Ok = rta.Flag;
            oReply.Message = rta.Message;

            if (rta.Status == 200)
            {
                return rta;
            }
            else
            {
                return rta;
            }
        }

        /// <summary>
        /// obtener facturas segun filtros
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/GetBills")]
        public async Task<ReplyData> GetBills(BillFilter data)
        {
            var rta = await _bill.GetBills(data);
            oReply.Data = rta.Data;
            oReply.Ok = rta.Flag;
            oReply.Message = rta.Message;

            if (rta.Status == 200)
            {
                return rta;
            }
            else
            {
                return rta;
            }
        }
    }
}