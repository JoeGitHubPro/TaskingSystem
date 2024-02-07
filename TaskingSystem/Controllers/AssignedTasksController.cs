using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskingSystem.Data;
using TaskingSystem.Models;
using TaskingSystem.ViewModels;

namespace TaskingSystem.Controllers
{
    [Authorize]
    public class AssignedTasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;


        public AssignedTasksController(ApplicationDbContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: AssignedTasks
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.IsInRole(Roles.User))
            {
                var applicationDbContextUser = await _context.AssignedTasks
                    .Include(a => a.AssignedTaskStudent)
                    .Include(a => a.AssignmentHeadLine)
                    .Where(a => a.AssignedTaskStudent.UserName == User.Identity.Name)
                    .OrderByDescending(a => a.AssignedTaskDate)
                    .ToListAsync();

                ViewData["Courses"] = new SelectList(_context.Courses, "CourseCode", "CourseName");
                return View(applicationDbContextUser);
            }

            var applicationDbContext = await _context.AssignedTasks
                .Include(a => a.AssignedTaskStudent)
                .Include(a => a.AssignmentHeadLine)
                .OrderByDescending(a => a.AssignedTaskDate)
                .ToListAsync();

            ViewData["Courses"] = new SelectList(_context.Courses, "CourseCode", "CourseName");
            return View(applicationDbContext);
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.Manger}")]
        public async Task<IActionResult> Index([Bind("Course,Student")] AssignmentSearchViewModel model)
        {
            var applicationDbContext = await _context.AssignedTasks
                .Include(a => a.AssignedTaskStudent)
                .Include(a => a.AssignmentHeadLine)
                .Where(a => a.AssignmentHeadLine.CourseCode == model.Course)
                .OrderByDescending(a => a.AssignedTaskDate)
                .ToListAsync();

            if (model.Student is not null)
                applicationDbContext = applicationDbContext.Where(a => a.AssignedTaskStudent.UserName.Contains(model.Student)).ToList();


            ViewData["Courses"] = new SelectList(_context.Courses, "CourseCode", "CourseName");
            return View(applicationDbContext);
        }
        [HttpPost]
        [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin},{Roles.Manger}")]
        public async Task<IActionResult> Index(string course)
        {
            var applicationDbContext = await _context.AssignedTasks
                .Include(a => a.AssignedTaskStudent)
                .Include(a => a.AssignmentHeadLine)
                .Where(a => a.AssignmentHeadLine.CourseCode == course)
                .OrderByDescending(a => a.AssignedTaskDate)
                .ToListAsync();


            ViewData["Courses"] = new SelectList(_context.Courses, "CourseCode", "CourseName");
            return View(applicationDbContext);
        }



        // GET: AssignedTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignedTask = await _context.AssignedTasks
                .Include(a => a.AssignedTaskStudent)
                .Include(a => a.AssignmentHeadLine)
                .FirstOrDefaultAsync(m => m.AssignedTaskId == id);
            if (assignedTask == null)
            {
                return NotFound();
            }

            return View(assignedTask);
        }

        // GET: AssignedTasks/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CourseTaskes"] = new SelectList(_context.Courses, "CourseCode", "CourseName");

            // ViewData["Task"] = new SelectList(_context.AssignmentHeadLines, "AssignmentId", "AssignmentName");
            return View();
        }

        public async Task<IActionResult> GetAssignmentHeadLines(string CourseCode)
        {
            var Tasks = await _context.AssignmentHeadLines.Where(a => a.CourseCode == CourseCode).Select(a => new { a.AssignmentId, a.AssignmentName }).ToListAsync();
            return Ok(Tasks);
        }
        // POST: AssignedTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssignedTaskId,TaskId,TaskURL,AssignedTaskDate,AssignedTaskStudentId,File")] AssignedTask assignedTask)
        {
            if (assignedTask.File == null)
            {
                ViewData["Task"] = new SelectList(_context.AssignmentHeadLines, "AssignmentId", "AssignmentName", assignedTask.TaskId);
                return View(assignedTask);
            }

            assignedTask.AssignedTaskStudentId = _context.Users.Where(a => a.UserName == User.Identity.Name).Select(a => a.Id).SingleOrDefault();
            assignedTask.AssignedTaskDate = DateTime.UtcNow.AddHours(2);
            assignedTask.TaskURL = UploadFile(assignedTask.File);

            if (assignedTask.AssignedTaskStudentId is not null
                && assignedTask.TaskURL is not null
                && assignedTask.AssignedTaskDate is not null
                && assignedTask.TaskId != 0)
            {
                _context.Add(assignedTask);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }

            ViewData["Task"] = new SelectList(_context.AssignmentHeadLines, "AssignmentId", "AssignmentName", assignedTask.TaskId);
            return View(assignedTask);
        }



        // GET: AssignedTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignedTask = await _context.AssignedTasks.FindAsync(id);
            if (assignedTask == null)
            {
                return NotFound();
            }
            ViewData["AssignedTaskStudentId"] = new SelectList(_context.Users, "Id", "UserName", assignedTask.AssignedTaskStudentId);
            ViewData["TaskId"] = new SelectList(_context.AssignmentHeadLines, "AssignmentId", "AssignmentName", assignedTask.TaskId);
            TempData["CurrentTask"] = assignedTask.TaskURL;
            return View(assignedTask);
        }

        // POST: AssignedTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AssignedTaskId,TaskId,TaskURL,AssignedTaskDate,AssignedTaskStudentId,File")] AssignedTask assignedTask)
        {
            if (id != assignedTask.AssignedTaskId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (assignedTask.TaskURL is not null && assignedTask.File is not null)
                    {
                        assignedTask.TaskURL = assignedTask.File.FileName;
                        if (TempData["CurrentTask"] != assignedTask.TaskURL)
                        {
                            System.IO.File.Delete($"wwwroot/AssignmentFiles/{assignedTask.TaskURL}");
                            assignedTask.TaskURL = UploadFile(assignedTask.File);
                            TempData.Remove("CurrentTask");
                        }
                    }
                    _context.Update(assignedTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssignedTaskExists(assignedTask.AssignedTaskId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedTaskStudentId"] = new SelectList(_context.Users, "Id", "Id", assignedTask.AssignedTaskStudentId);
            ViewData["TaskId"] = new SelectList(_context.AssignmentHeadLines, "AssignmentId", "AssignmentId", assignedTask.TaskId);
            return View(assignedTask);
        }

        // GET: AssignedTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assignedTask = await _context.AssignedTasks
                .Include(a => a.AssignedTaskStudent)
                .Include(a => a.AssignmentHeadLine)
                .FirstOrDefaultAsync(m => m.AssignedTaskId == id);
            if (assignedTask == null)
            {
                return NotFound();
            }

            return View(assignedTask);
        }

        // POST: AssignedTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assignedTask = await _context.AssignedTasks.FindAsync(id);
            if (assignedTask != null)
            {
                if (assignedTask.TaskURL is not null && assignedTask.File is not null)
                    System.IO.File.Delete($"wwwroot/AssignmentFiles/{assignedTask.TaskURL}");

                _context.AssignedTasks.Remove(assignedTask);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssignedTaskExists(int id)
        {
            return _context.AssignedTasks.Any(e => e.AssignedTaskId == id);
        }
        private string UploadFile(IFormFile File)
        {
            var uniqueFileName = GetUniqueFileName(File.FileName);
            var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "AssignmentFiles");
            var filePath = Path.Combine(uploads, uniqueFileName);
            File.CopyTo(new FileStream(filePath, FileMode.Create));
            return uniqueFileName;
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }
    }
}
