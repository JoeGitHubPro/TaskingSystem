using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskingSystem.Data;
using TaskingSystem.Models;

namespace TaskingSystem.Controllers
{
    [Authorize]
    public class CoursesRegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesRegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CoursesRegistration
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(Roles.SuperAdmin))
            {
                var Context = await _context.StudentsCourses.Include(s => s.Course).Include(s => s.Student).ToListAsync();
                return View(Context);
            }
            var userId = await _context.Users.Where(a => a.UserName == User.Identity.Name).Select(a => a.Id).SingleOrDefaultAsync();
            var applicationDbContext = _context.StudentsCourses.Where(a => a.StudentId == userId).Include(s => s.Course).Include(s => s.Student);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CoursesRegistration/Create
        [Authorize(Roles = Roles.User)]
        public IActionResult Create()
        {
            ViewData["CourseCode"] = new SelectList(_context.Courses, "CourseCode", "CourseCode");
            // ViewData["StudentId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["StudentId"] = new SelectList(_context.Users.Where(a => a.UserName == User.Identity.Name), "Id", "UserName");
            return View();
        }

        // POST: CoursesRegistration/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Roles.User)]
        public async Task<IActionResult> Create([Bind("StudentId,CourseCode")] StudentsCourses studentsCourses)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(studentsCourses);
                    await _context.SaveChangesAsync();

                }
                catch (Exception)
                {

                    return RedirectToAction("Error", "Home", new ErrorViewModel() { Error = "This Record already exsist!" });
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseCode"] = new SelectList(_context.Courses, "CourseCode", "CourseCode", studentsCourses.CourseCode);
            ViewData["StudentId"] = new SelectList(_context.Users.Where(a => a.UserName == User.Identity.Name), "Id", "UserName", studentsCourses.StudentId);
            return View(studentsCourses);
        }



        // GET: CoursesRegistration/Delete/5
        public async Task<IActionResult> Delete(string CourseCode, string StudentId)
        {
            if (CourseCode == null && StudentId == null)
            {
                return NotFound();
            }

            var studentsCourses = await _context.StudentsCourses
                .Include(a => a.Student)
                .Include(a => a.Course)
                .Where(a => a.StudentId == StudentId && a.CourseCode == CourseCode)
                .SingleOrDefaultAsync();

            if (studentsCourses == null)
            {
                return NotFound();
            }

            return View(studentsCourses);
        }

        // POST: CoursesRegistration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string CourseCode, string StudentId)
        {
            var studentsCourses = await _context.StudentsCourses.FindAsync(StudentId, CourseCode);
            if (studentsCourses != null)
            {
                _context.StudentsCourses.Remove(studentsCourses);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentsCoursesExists(string id)
        {
            return _context.StudentsCourses.Any(e => e.StudentId == id);
        }
    }
}
