using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskingSystem.Data;
using TaskingSystem.Global;
using TaskingSystem.Models;

namespace TaskingSystem.Controllers
{
    [Authorize(Roles = Roles.SuperAdmin)]
    public class ThemesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThemesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Themes
        public async Task<IActionResult> Index()
        {
            var selectedTheme = await _context.Themes.FirstOrDefaultAsync(a => a.ThemeSelected);
            ViewData["Themes"] = new SelectList(_context.Themes, "Id", "ThemeName", selectedTheme?.Id);
            return View(await _context.Themes.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Selected(int themeId)
        {
            var selectedTheme = await _context.Themes.FirstOrDefaultAsync(a => a.ThemeSelected);

            await _context.Themes.ForEachAsync(a => a.ThemeSelected = false);

            var newThemeSelected = await _context.Themes.FindAsync(themeId);

            if (newThemeSelected != null)
            {
                newThemeSelected.ThemeSelected = true;
                await _context.SaveChangesAsync();

                var selectedThemeName = _context.Themes
                    .Where(a => a.ThemeSelected)
                    .Select(a => a.ThemeName)
                    .SingleOrDefault();

                var themeGlobal = new ThemeGlobal(selectedThemeName);

                ViewData["Themes"] = new SelectList(_context.Themes, "Id", "ThemeName", selectedTheme?.Id);

                return RedirectToAction(nameof(Index), await _context.Themes.ToListAsync());
            }

            // Handle the case where the selected theme is not found
            return NotFound();
        }
    }
}
