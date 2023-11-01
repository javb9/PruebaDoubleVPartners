using static Back.Models.Bill;
using static Back.Models.Reply;

namespace Back.Interfaces
{
    public interface IBill
    {
        Task<ReplyData> SaveBill(BillModel data);
        Task<ReplyData> GetBills(BillFilter data);
    }
}
