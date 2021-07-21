using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using marmuz_site_v1.Models;
using Microsoft.AspNet.Identity.Owin;

namespace marmuz_site_v1.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public CommentsController()
        {
        
        }

        public CommentsController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }



        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }



        public async Task<ActionResult> TestComments() 
        {
            ApplicationUser user = await UserManager.FindByEmailAsync("pilot_mig@bk.ru");

            if (user != null) 
            {
                    List<Comment> list = new List<Comment>
                {
                    new Comment{ ApplicationUserId = user.Id, Text = "Это первый коммент на нашем сайте и конечно же я хочу сказать что сайта ебанавротухуенный!"},
                    new Comment{ ApplicationUserId = user.Id, Text = "Это второй коммент на нашем сайте и мне все построили и еще и денег дали!"},
                    new Comment{ ApplicationUserId = user.Id, Text = "Это третий коммент на нашем сайте и а меня обманули пидоры, нихера не построили и еще на бабки кинули ссуки!"}
                };

                db.Comments.AddRange(list);
                await db.SaveChangesAsync();
            }


            return RedirectToAction("Index");
        }

        


        // GET: Comments
        public async Task<ActionResult> Index()
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);

            if (user != null)
            {
                IList<string> _rolelist = await UserManager.GetRolesAsync(user.Id);

                bool isAdmin = false;

                if (_rolelist.Contains("admin")) 
                {
                    isAdmin = true;
                }

                if (isAdmin) 
                {
                    return RedirectToAction("AdminsCommentsViewing");
                }
            }

            return View(await db.Comments.Include(c => c.User).OrderBy(c=>c.Date).ToListAsync());
        }


        public async Task<ActionResult> AdminsCommentsViewing()
        {
            return View(await db.Comments.Include(c => c.User).OrderBy(c => c.Date).ToListAsync());
        }




        [Authorize(Roles = "admin")]
        // GET: Comments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Comment comment = await db.Comments.Include(c=>c.User).Where(c=>c.Id==id).FirstOrDefaultAsync();
            Comment comment = await db.Comments.Include(c => c.User).FirstOrDefaultAsync(c=>c.Id == id);

            if (comment == null)
            {
                return HttpNotFound();
            }

            return View(comment);
        }






        [Authorize]
        // GET: Comments/Create
        public async Task<ActionResult> Create()
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);

            if (user != null) 
            {
                CommentViewModel cm = new CommentViewModel
                {
                    Date = DateTime.Now.ToString(),
                    ClientName = user.ClientName
                };

                return View(cm);
            }

            return RedirectToAction("Create");
        }



               

        // POST: Comments/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(/*[Bind(Include = "Id,Date,Text,ApplicationUserId")] Comment comment*/ CommentViewModel cm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);
                Comment comment = new Comment() { };

                if (user != null) 
                {
                    comment.ApplicationUserId = user.Id;
                    comment.Text = cm.Text;

                    db.Comments.Add(comment);

                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }

            }
            //return View(cm);
            return RedirectToAction("Create");
        }
                          




        [Authorize]
        // GET: Comments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = await db.Comments.Include(c => c.User).FirstOrDefaultAsync(c=>c.Id==id);

            if (comment == null)
            {
                return HttpNotFound();
            }


            ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);
            bool isAdmin = false;

            if (user != null)
            {
                IList<string> _rolelist = await UserManager.GetRolesAsync(user.Id);
                
                if (_rolelist.Contains("admin"))
                {
                    isAdmin = true;
                }
            }


            if (user != null && comment.ApplicationUserId != user.Id && isAdmin == false)
            {
                return View("YouCanEditOnlyYourComment");
            }



            CommentEditViewModel cem = new CommentEditViewModel
            {
                Id = comment.Id,
                ApplicationUserId = comment.ApplicationUserId,
                DateOfCreate = comment.Date.ToString(),
                
                ClientName = comment.User.ClientName,
                Text = comment.Text
            };

            return View(cem);
        }



        // POST: Comments/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(/*[Bind(Include = "Id,Date,Text,ApplicationUserId")] Comment comment*/ CommentEditViewModel cem)
        {
            if (ModelState.IsValid)
            {
                Comment comment = await db.Comments.FindAsync(cem.Id);                

                if (comment == null)
                {
                    return HttpNotFound();
                }

                comment.isEdited = true;
                comment.DateOfLastEdit = DateTime.Now;
                comment.Text = cem.Text;


                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(cem);
        }









        // GET: Comments/Delete/5

        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            

            Comment comment = await db.Comments.Include(c=>c.User).FirstOrDefaultAsync(c=>c.Id==id);            

            if (comment == null)
            {
                return HttpNotFound();
            }

            ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);
            bool isAdmin = false;

            if (user != null)
            {
                IList<string> _rolelist = await UserManager.GetRolesAsync(user.Id);

                if (_rolelist.Contains("admin"))
                {
                    isAdmin = true;
                }
            }

            if (user != null && comment.ApplicationUserId != user.Id && isAdmin == false) 
            {
                return View("YouCanDelOnlyYourComment");
            }


            return View(comment);
        }



        // POST: Comments/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Comment comment = await db.Comments.FindAsync(id);
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
