using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskingSystem.Models;

namespace TaskingSystem.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public Users(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId)
        {

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                throw new Exception();

            return Ok();


        }
    }
}
