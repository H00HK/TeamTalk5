using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NLog;
using TeamTalkLib;
using TeamTalkLib.Settings;

namespace MockWeb.Controllers
{
    public class HomeController : Controller
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            log.Info("Starting index...");
            ViewBag.Title = "Home Page";
            ViewBag.Link = LinkProvider.GetActiveLink(LinkProvider.getTestSettingsForUser(
                new ChannelSettings(1, ""), new ChannelSettings(2, "test123"), new UserSettings("Antek Sobkowicz", "antek", "antek")));

            ViewBag.Link_javaAdmin = LinkProvider.GetActiveLink(LinkProvider.getTestSettingsForUser(
                 new ChannelSettings(1, ""), new ChannelSettings(2, "test123"), new UserSettings("Java_admin", "user", "password")));

            ViewBag.Link_a1 = LinkProvider.GetActiveLink(LinkProvider.getTestSettingsForUser(
                new ChannelSettings(1, ""), new ChannelSettings(2, "test123"), new UserSettings("API User 18", "New java user 18", "123")));

            ViewBag.Link_a2 = LinkProvider.GetActiveLink(LinkProvider.getTestSettingsForUser(
                new ChannelSettings(1, ""), new ChannelSettings(2, "test123"), new UserSettings("API User 19", "New java user 19", "123")));


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }



    }
}