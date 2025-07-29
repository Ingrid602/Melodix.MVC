using Melodix.Modelos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Melodix.MVC.Controllers
{
    public class PanelAdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public PanelAdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.Rol != RolUsuario.Admin)
            {
                return Unauthorized();
            }

            return View();
        }
    }
}
