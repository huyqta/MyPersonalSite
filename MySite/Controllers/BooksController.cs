using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MySite.Models;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;

namespace MySite.Controllers
{
    public class BooksController : BaseController
    {
        private Books db = new Books();

        // GET: Books
        public async Task<ActionResult> Index()
        {
            return View(await db.DsBooks.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.DsBooks.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Author,Publisher,Description,UrlRead,UrlDownload,UrlImage,UrlThumbnail")] Book book)
        {
            //WebConsole.GoogleDriverConsole w = new WebConsole.GoogleDriverConsole();
            //w.UploadFile();
            if (ModelState.IsValid)
            {
                book.Id = Guid.NewGuid();
                db.DsBooks.Add(book);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.DsBooks.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Author,Publisher,Description,UrlRead,UrlDownload,UrlImage,UrlThumbnail")] Book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = await db.DsBooks.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            Book book = await db.DsBooks.FindAsync(id);
            db.DsBooks.Remove(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public void UploadFileToGoogleDrive()
        {
            var fileName = @"C:\test\111.jpg";
            Google.Apis.Services.BaseClientService.Initializer initializer = new Google.Apis.Services.BaseClientService.Initializer();
            
            DriveService driveService = new DriveService();
            
            var fileMetadata = new File()
            {
                Name = fileName
            };
            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open))
            {
                request = driveService.Files.Create(fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
