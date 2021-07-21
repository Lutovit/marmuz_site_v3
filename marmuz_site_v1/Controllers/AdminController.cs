using marmuz_site_v1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace marmuz_site_v1.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public AdminController()
        {
        }


        public AdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
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



        [Authorize(Roles = "admin")]
        // GET: Admin
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }


        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Info(string Id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(Id);

            if (user != null)
            {
                IList<string> _rolelist = await UserManager.GetRolesAsync(user.Id);

                AdminInfo model = new AdminInfo { UserName = user.ClientName, UserEmail = user.Email, UserRoleList = _rolelist, CompanyName=user.CompanyName };

                ViewBag.id = user.Id;

                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Что-то пошло не так");
            }

            return RedirectToAction("Index");

        }


        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Edit(string Id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(Id);

            if (user != null)
            {
                IList<string> _userrolelist = await UserManager.GetRolesAsync(user.Id);


                List<ApplicationRole> _appprolelist = RoleManager.Roles.ToList();


                IList<string> _availableroles = new List<string>();

                foreach (ApplicationRole r in _appprolelist)
                {
                    _availableroles.Add(r.Name);
                }


                AdminEdit model = new AdminEdit { UserName = user.ClientName, UserEmail = user.Email, UserRoleList = _userrolelist, RoleList = _availableroles, CompanyName=user.CompanyName };

                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Что-то пошло не так");
            }

            return RedirectToAction("Index");

        }




        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddRoleToUser(string Email, string Rolename)
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(Email);

            if (user != null)
            {
                await UserManager.AddToRoleAsync(user.Id, Rolename);
                return RedirectToAction("Edit", "Admin", new { Id = user.Id });
            }
            else
            {
                ModelState.AddModelError("", "Что-то пошло не так");
            }

            return RedirectToAction("Index");
        }



        [Authorize(Roles = "admin")]
        public async Task<ActionResult> DeleteRoleFromUser(string Email, string Rolename)
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(Email);

            if (user != null)
            {
                await UserManager.RemoveFromRoleAsync(user.Id, Rolename);
                return RedirectToAction("Edit", "Admin", new { Id = user.Id });
            }
            else
            {
                ModelState.AddModelError("", "Что-то пошло не так");
            }

            return RedirectToAction("Index");

        }


        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult DeleteUser()
        {
            return View();
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> DeleteUser(string Id)
        {
            
            ApplicationUser user = await UserManager.FindByIdAsync(Id);

            if (user != null)
            {
                //если удаляемый пользователь - админ удаляет сам себя, то вызывается метод AdminSelfDelete() контролера Account
                if (user.Email == User.Identity.Name)
                {
                    return RedirectToAction("AdminSelfDelete", "Account");
                }


                //в противном слдучае просто удаляем сначала отзывы пользователя, а затем и его самого

                 user = await db.Users.Include(c => c.Comments).FirstOrDefaultAsync(c => c.Id == Id);

                if (user != null)
                {
                    db.Comments.RemoveRange(user.Comments);

                    db.SaveChanges();
                }


                user = await UserManager.FindByIdAsync(Id);

                if (user != null) 
                {
                    IdentityResult result = await UserManager.DeleteAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                }

                
            }
            return RedirectToAction("CantDeleteUser", new { name = user.Email });

        }



        [Authorize(Roles = "admin")]
        public string CantDeleteUser(string name)
        {
            return "Не могу удалить пользователя:  " + name + "  Что-то пошло не так!";
        }




        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }



        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email, ClientName = model.ClientName, CompanyName = model.CompanyName };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);


                if (result.Succeeded)
                {
                    // если создание прошло успешно, то добавляем роль пользователя
                    await UserManager.AddToRoleAsync(user.Id, "user");

                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }



        [Authorize(Roles = "admin")]
        public async Task<ActionResult> FindByEmail(string email)
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                return RedirectToAction("Edit", "Admin", new { Id = user.Id });
            }
            else
            {
                return RedirectToAction("NoUserEmail", "Admin");
            }
        }


        [Authorize(Roles = "admin")]
        public ActionResult NoUserEmail()
        {
            return View();
        }





        [Authorize(Roles = "admin")]
        [HttpGet]
        public ActionResult BanUser()
        {
            return View();
        }




        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> BanUser(string Id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(Id);

            if (user != null)
            {
                
                user.Ban = true;
                await UserManager.UpdateAsync(user);

                BanEmails banEmail = new BanEmails { Email = user.Email };
                db.BanEmails.Add(banEmail);
                await db.SaveChangesAsync();

                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("CantBanUser", new { name = user.Email });

        }



        [Authorize(Roles = "admin")]
        public string CantBanUser(string name)
        {
            return "Не могу забанить пользователя:  " + name + "  Что-то пошло не так!";
        }



        [Authorize(Roles = "admin")]
        public string CantCancelBan(string  email)
        {
            return "Не могу забанить пользователя:  " + email + "  Что-то пошло не так!";
        }





        [Authorize(Roles = "admin")]
        // GET: Admin
        public async Task<ActionResult> BanUsersList()
        {
            return View(await db.BanEmails.ToListAsync());
        }




        [Authorize(Roles = "admin")]
        // GET: Admin
        public async Task<ActionResult> CancelBan(string email)
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(email);

            if (user != null) 
            {                
                user.Ban = false;
                await UserManager.UpdateAsync(user);
            }


            BanEmails banEmail = await db.BanEmails.FirstOrDefaultAsync(c => c.Email == email);

            if (banEmail != null)
            {
                db.BanEmails.Remove(banEmail);
                await db.SaveChangesAsync();

                return RedirectToAction("BanUsersList");
            }
            
            return RedirectToAction("CantCancelBan");
        }
    }
}