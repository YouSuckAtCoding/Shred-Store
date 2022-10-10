using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShredStore.Controllers
{
    public class ShredStoreController : Controller
    {
        // GET: ShredStoreController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ShredStoreController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ShredStoreController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShredStoreController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShredStoreController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShredStoreController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShredStoreController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShredStoreController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
