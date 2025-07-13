

//using Dapper;



//using System.Data;
//using System.Drawing.Printing;
//using TradeApp.DbContex;
//using TradeApp.Services;

//namespace Pms.Data.Repository
//{
//    public class OrderServiceWithSp
//    {
//        private readonly IDbConnection _db;
//        private readonly BaseHandaler _handaler;

//        public OrderServiceWithSp(DbConnection db)
//        {
//            _db = db.GetDbConnection();
//            _handaler = new BaseHandaler(db);
//        }
//        public async Task<IEnumerable<Order>> GetOrders(long? order_id,string? product_name,int? pagenumber, int? pageSize)
//        {
//            return await _handaler.GetEntities<Order>("sp_order_getAll", new { @OrderId = order_id, @ProductName = product_name, @page= pagenumber, @page_size= pageSize});
//        }
//        public async Task<Order> GetOrderById(long order_id)

//        {
//          var order= await (GetOrders(order_id, null, 1, 10));
//          return order.FirstOrDefault();
//        }
//        public async Task<long> AddOrder(Order order)
//        {
//            long orderId = await _handaler.AddEntity<Order>(order, "@OrderId", "sp_insert_order");
//            return orderId;
//        }
//        public async Task<bool> UpdateOrder(Order order)
//        {
//            bool isUpdated = await _handaler.UpdateEntity<Order>(order, "sp_order_update");
//            return isUpdated;
//        }
//        public async Task<bool> DeleteOrder(long OrderId)
//        {
//            var order = await (GetOrders(OrderId, null, 1, 10));
//            var deleteObj =   order.FirstOrDefault();
//            bool isDeleted = false;
//            if (deleteObj != null)
//            {
//                deleteObj.Status = "Deleted";
//                isDeleted = await _handaler.UpdateEntity<Order>(deleteObj, "sp_order_update");
//            }
          
//            return isDeleted;
//        }
        

//    }






//}
