using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace marmuz_site_v1.Models
{
    public class ApplicationRole : IdentityRole
    {

        public ApplicationRole() 
        {
        }


        [Display(Name = "Описание роли")]
        public string Description { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }
}