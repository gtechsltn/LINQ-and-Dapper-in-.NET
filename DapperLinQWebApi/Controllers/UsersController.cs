using DapperLinQWebApi.Models;
using DapperLinQWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperLinQWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> PostUser(User user)
        {
            await _userRepository.AddUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutUser(int id, User user)
        {
            if (id != user.Id) return BadRequest();
            await _userRepository.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userRepository.DeleteUserAsync(id);
            return NoContent();
        }

        [HttpGet("order-counts")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetUserOrderCounts()
        {
            var orderCounts = await _userRepository.GetUserOrderCountsAsync();
            return Ok(orderCounts);
        }

        [HttpGet("sorted")]
        public async Task<ActionResult<IEnumerable<User>>> GetSortedUsers(string sortField = "Name", bool ascending = true)
        {
            var users = await _userRepository.GetAllUsersSortedAsync(sortField, ascending);
            return Ok(users);
        }


    }
}
