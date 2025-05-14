using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementCoreApplication.Areas.Students.Controllers.MVC
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
