using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace marmuz_site_v1.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {

        public string ClientName { set; get; }
        public string CompanyName { set; get; }
        public bool Ban { set; get; }

        public ICollection<Comment> Comments { set; get; }

        public ApplicationUser() 
        {
            Comments = new List<Comment>();
            Ban = false;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }




    public class Comment 
    {
        public int Id { set; get; }


        public DateTime Date { set; get; }


        public bool isEdited { set; get; }

        public DateTime DateOfLastEdit { set; get; }


        public string Text { set; get; }

        public string ApplicationUserId { set; get; }

        public ApplicationUser User { set; get; }

        public Comment()
        {
            Date = DateTime.Now;
            DateOfLastEdit = DateTime.Now;
            isEdited = false;
        }    
    }


    public class BanEmails 
    {
        public int Id { set; get; }
        public string Email { set; get; }
    }




    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Comment> Comments { set; get; }
        public DbSet<BanEmails> BanEmails { set; get; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}