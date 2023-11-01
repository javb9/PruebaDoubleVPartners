using Back.Interfaces;
using Back.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using static Back.Models.Reply;

namespace Back.Controllers
{
    [ApiController]
    public class ParametricController : ControllerBase
    {
        
        private readonly ILogger<ParametricController> _logger;
        private readonly IParametric _parametric;
        public ReplySucess oReply = new ReplySucess();
        public ParametricController(ILogger<ParametricController> logger, IParametric parametric)
        {
            _logger = logger;
            _parametric = parametric;
        }

        /// <summary>
        /// obtener clientes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/GetClients")]
        public async Task<ReplyData> GetClients()
        {
            var rta = await _parametric.GetClients();
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
        /// obtener productos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/GetProducts")]
        public async Task<ReplyData> GetProducts()
        {
            var rta = await _parametric.GetProducts();
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