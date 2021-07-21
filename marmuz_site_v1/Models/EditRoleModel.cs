using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace marmuz_site_v1.Models
{
    public class EditRoleModel
    {
        public string Id { get; set; }

        [Display(Name = "Наименование роли:")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Описание роли:")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}