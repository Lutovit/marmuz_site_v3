using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace marmuz_site_v1.Models
{
    public class AppealViewModel
    {
        [Display(Name = "Ваше ФИО:")]
        [Required]
        public string Name { set; get; }


        [Display(Name = "Номер телефона:")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { set; get; }


        [Required]
        [Display(Name = "Адрес электронной почты (Email)")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = "Описание задачи:")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }


        [Display(Name = "Дата отправки сообщения:")]
        public string Date { set; get; }

       
        public AppealViewModel() 
        {
            Date = DateTime.Now.ToString();     
        }

    }

}