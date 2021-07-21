using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using marmuz_site_v1.Models;

namespace marmuz_site_v1.Controllers
{
    public class ContentController : Controller
    {

        private string CrEmail ="RazumProekt2021@gmail.com"; //Credentials email
        private string CrPass = "RazumProect2021_RazumProect2021";  //Credentials pass

        private string emailFrom = "RazumProekt2021@gmail.com";

        private string emailTo = "Razumproekt@gmail.com";
        private string emailCopyTo = "Marmuzevich@gmail.com";  




        // GET: Content
        [HttpGet]
        public ActionResult Appeal()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Appeal(AppealViewModel m)
        {
            
            if (ModelState.IsValid)
            {
                Task t1;

                try
                {
                  t1 = Task.Run(()=>SendMessageBySMTP(m));                    
                }
                catch 
                {
                    return RedirectToAction("AppealError");
                }

                await Task.WhenAll(new[] {t1});

                return RedirectToAction("AppealAccepted");
            }

            return View();        
        }


        public ActionResult AppealAccepted()
        {
            return View();
        }


        public ActionResult AppealError()
        {
            return View();
        }



        public void SendMessageBySMTP(AppealViewModel model)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress(emailFrom, "Razum Proekt");

            // кому отправляем
            MailAddress to = new MailAddress(emailTo);

            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);

            //отправка копии
            m.CC.Add(emailCopyTo);  

            // тема письма
            m.Subject = "Новая заявка!";

            m.IsBodyHtml = true;

            // текст письма
            m.Body = "<h3>Сообщение от:   </h3>" + model.Name + "." + "<h3>Номер телефона:   </h3>" + model.PhoneNumber + "." + "<h3>Email:   </h3>" + model.Email + "." +
             "<h3>Отправлено:   </h3>" + model.Date + "." + "<h3>Обращение:   </h3>" + model.Description;

            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                smtp.UseDefaultCredentials = false;

                // логин и пароль
                smtp.Credentials = new NetworkCredential(CrEmail, CrPass);

                smtp.EnableSsl = true;

                smtp.Send(m);
            }

        }

    }
}