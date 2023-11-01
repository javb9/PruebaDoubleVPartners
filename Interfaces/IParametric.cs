using static Back.Models.Reply;

namespace Back.Interfaces
{
    public interface IParametric
    {
        Task<ReplyData> GetClients();
        Task<ReplyData> GetProducts();
    }
}
