using SampleOrders.Models;
using System.Collections.Generic;
using System.Linq;

namespace SampleOrders.Services
{
    public static class OrderService
    {
        static List<Order> Orders { get; }
        static int nextId = 3;
        static OrderService()
        {
            Orders = new List<Order>
            {
                new Order { OrderId = 1, OrderName = "Pen", OrderPrice = 5},
                new Order { OrderId = 2, OrderName = "Book", OrderPrice = 15}
            };
        }

        public static List<Order> GetAll() => Orders;

        public static Order Get(int OrderId) => Orders.FirstOrDefault(o => o.OrderId == OrderId);

        public static void Add(Order order)
        {
            order.OrderId = nextId++;
            Orders.Add(order);
        }

        public static void Delete(int OrderId)
        {
            var Order= Get(OrderId);
            if(Order is null)
                return;

            Orders.Remove(Order);
        }

        public static void Update(Order order)
        {
            var index = Orders.FindIndex(o => o.OrderId == order.OrderId);
            if(index == -1)
                return;

            Orders[index] = order;
        }
    }
}