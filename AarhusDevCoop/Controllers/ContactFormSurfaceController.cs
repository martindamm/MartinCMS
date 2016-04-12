using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using AarhusDevCoop.ViewModels;
using Umbraco.Core.Models;

namespace AarhusDevCoop.Controllers
{
    public class ContactFormSurfaceController : SurfaceController
    {
        // GET: ContactFormSurface
        public ActionResult Index()
        {
            return PartialView("ContactForm", new ContactForm());
        }

        // Handle form submit
        [HttpPost]
        public ActionResult HandleFormSubmit(ContactForm model)
        {
            if (!ModelState.IsValid) { return CurrentUmbracoPage(); }

            // send email
            MailMessage message = new MailMessage();
            message.To.Add("martindamm@live.dk");
            message.Subject = model.Subject;
            message.From = new MailAddress(model.Email, model.Name);
            message.Body = model.Message;

            // variabel til at gemme beskeder fra contactform
            // i Members sectionen i backend
            var comment = Services.ContentService.CreateContent(model.Subject, CurrentPage.Id, "comment");
            comment.SetValue("commentName", model.Name);
            comment.SetValue("email", model.Email);
            comment.SetValue("subject", model.Subject);
            comment.SetValue("message", model.Message);

            Services.ContentService.Save(comment);

            TempData["success"] = true;
            return RedirectToCurrentUmbracoPage();
        }
    }
}