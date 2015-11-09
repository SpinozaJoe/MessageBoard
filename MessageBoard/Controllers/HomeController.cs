using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MessageBoard.Models;
using MessageBoard.Services;
using MessageBoard.Data;

namespace MessageBoard.Controllers
{
    public class HomeController : Controller
    {
        private IMailService m_service = null;
        private IMessageBoardRepository m_repository = null;

        public HomeController(IMailService service, IMessageBoardRepository repository)
        {
            m_service = service;
            m_repository = repository;
        }

        public ActionResult Store()
        {
            return View();
        }

        public ActionResult Index()
        {
            /*
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            List<Topic> topics = m_repository.GetTopicsIncludingReplies()
                .OrderByDescending(t => t.Created)
                .Take(25)
                .ToList();

            return View(topics);*/

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(ContactModel contactDetails)
        {
            if (m_service.SendMail("david.dickerson@markit.com", contactDetails.Email, "Thanks for your interest", string.Format("Comment from {1}{0}Website: {2}",
                Environment.NewLine,
                contactDetails.Name,
                contactDetails.Website)))
            {
                ViewBag.MailSent = true;
            }

            return View();
        }

        [Authorize]
        public ActionResult MyMessages()
        {
            return View();
        }

        [Authorize(Users="dave")]
        public ActionResult Moderation()
        {
            return View();
        }
    }
}
