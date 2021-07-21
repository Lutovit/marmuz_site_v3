using marmuz_site_v1.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace marmuz_site_v1.Controllers
{
    public class RolesController : Controller
    {
        private ApplicationRoleManager _roleManager;
        private ApplicationUserManager _userManager;


        public RolesController()
        {
        }


        public RolesController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
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
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }


        
        [Authorize]        
        public async Task<ActionResult> UsersRoles()
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);

            if (user != null)
            {
                IList<string> _roleslist = await UserManager.GetRolesAsync(user.Id);

                return View(_roleslist);
            }
            return RedirectToAction("Login", "Account");

        }

        
        [Authorize]        
        public async Task<ActionResult> BecomeAdmin()
        {
            ApplicationUser user = await UserManager.FindByEmailAsync(User.Identity.Name);


            if (user != null && user.Email == "pilot_mig@bk.ru")
            {

                ApplicationRole roleAdmin = await RoleManager.FindByNameAsync("admin");


                if (roleAdmin == null)
                {
                    IdentityResult result = await RoleManager.CreateAsync(new ApplicationRole
                    {
                        Name = "admin",
                        Description = "Ruler of this application"
                    });

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Что-то пошло не так");
                    }

                }


                await UserManager.AddToRoleAsync(user.Id, "admin");

                return RedirectToAction("Log_Off", "Account");
            }
            else
            {
                return RedirectToAction("WrongEmailToBecomeAdmin", "Roles");
            }
        }

        

        [Authorize]        
        public ActionResult WrongEmailToBecomeAdmin()
        {
            return View();
        }



        
        [Authorize(Roles = "admin")]        
        public ActionResult Create()
        {
            return View();
        }


        
        [Authorize(Roles = "admin")]        
        [HttpPost]
        public async Task<ActionResult> Create(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new ApplicationRole
                {
                    Name = model.Name,
                    Description = model.Description
                });

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }

            }
            return View(model);
        }


        
        [Authorize(Roles = "admin")]        
        public async Task<ActionResult> Edit(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);

            if (role != null)
            {
                return View(new EditRoleModel { Id = role.Id, Name = role.Name, Description = role.Description });
            }

            return RedirectToAction("Index");
        }


        
        [Authorize(Roles = "admin")]        
        [HttpPost]
        public async Task<ActionResult> Edit(EditRoleModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = await RoleManager.FindByIdAsync(model.Id);

                if (role != null)
                {
                    role.Description = model.Description;
                    role.Name = model.Name;
                    IdentityResult result = await RoleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Что-то пошло не так");
                    }
                }
            }
            return View(model);
        }



        
        [Authorize(Roles = "admin")]        
        [HttpGet]
        public ActionResult DeleteRole()
        {
            return View();
        }


        
        [Authorize(Roles = "admin")]        
        [HttpPost]
        public async Task<ActionResult> DeleteRole(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);

            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }




        
        [Authorize(Roles = "admin")]        
        public async Task<ActionResult> AddUserToRole(string Email, string rolename)
        {

            ApplicationUser user = await UserManager.FindByEmailAsync(Email);

            if (user != null)
            {
                await UserManager.AddToRoleAsync(user.Id, rolename);
                return RedirectToAction("Info", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "Что-то пошло не так");
            }

            return RedirectToAction("Index", "Admin");

        }



    }
}