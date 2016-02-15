using Microsoft.AspNet.Mvc;
using BookStore.Portal.ViewModel;

namespace BookStore.Portal
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            var model = new BookModel() { Name = "MyBook" };
            return View(model);
        }
    }
}
