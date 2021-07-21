using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace marmuz_site_v1.Models
{
    public class CommentViewModel
    {
        [Display(Name ="Дата:")]
        public string Date { set; get; }

        [Display(Name = "Ваше имя:")]
        public string ClientName { set; get; }

        [Required]
        [Display(Name = "Ваш отзыв:")]
        [DataType(DataType.MultilineText)]
        public string Text { set; get; }

    }

    public class CommentEditViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { set; get; }

        [HiddenInput(DisplayValue = false)]
        public string ApplicationUserId { set; get; }


        [Display(Name = "Создан:")]
        public string DateOfCreate { set; get; }


        [Display(Name = "Изменен:")]
        public string DateOflastEdit { set; get; }


        [Display(Name = "Ваше имя:")]
        public string ClientName { set; get; }


        [Required]
        [Display(Name = "Ваш отзыв:")]
        [DataType(DataType.MultilineText)]
        public string Text { set; get; }

        public CommentEditViewModel() 
        {
            DateOflastEdit = DateTime.Now.ToString();        
        }

    }
}