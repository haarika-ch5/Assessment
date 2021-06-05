using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SampleOrders.Models;
using SampleOrders.Services;

namespace SampleOrders.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        public OrderController()
        {
        }

        // GET all action

        [HttpGet]
public ActionResult<List<Order>> GetAll() =>
    OrderService.GetAll();

        // GET by Id action

        [HttpGet("{OrderId}")]
public ActionResult<Order> Get(int OrderId)
{
    var order = OrderService.Get(OrderId);

    if(order == null)
        return NotFound();

    return order;
}

        // POST action
[HttpPost]
public IActionResult Create(Order order)
{            
    OrderService.Add(order);
    return CreatedAtAction(nameof(Create), new { id = order.OrderId }, order);
}
        // PUT action
[HttpPut("{OrderId}")]
public IActionResult Update(int OrderId, Order order)
{
    if (OrderId!= order.OrderId)
        return BadRequest();

    var existingOrder = OrderService.Get(OrderId);
    if(existingOrder is null)
        return NotFound();

    OrderService.Update(order);           

    return NoContent();
}
        // DELETE action
        [HttpDelete("{OrderId}")]
public IActionResult Delete(int OrderId)
{
    var order = OrderService.Get(OrderId);

    if (order is null)
        return NotFound();

    OrderService.Delete(OrderId);

    return NoContent();
}
    }
}