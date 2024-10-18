using DapperLinQWebApi.Models;
using DapperLinQWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperLinQWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepository _repository;
  
        public OrdersController(OrderRepository repository)
        {
            _repository = repository;
        }

        
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders(int id)
        {
            var orders = await _repository.GetOrdersByUserIdAsync(id);
            return Ok(orders);
        }
    }
}
