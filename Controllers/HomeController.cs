using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DojoDachi.Models;

namespace DojoDachi.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Fullness") == null) {
                HttpContext.Session.SetInt32("Fullness",20);
                HttpContext.Session.SetInt32("Happiness",20);
                HttpContext.Session.SetInt32("Meals",3);
                HttpContext.Session.SetInt32("Energy",85);
                HttpContext.Session.SetString("Status","Default");
                HttpContext.Session.SetString("Message","Welcome to DojoDachi! Please select an action.");
            }
            ViewBag.Full = HttpContext.Session.GetInt32("Fullness");
            ViewBag.Happy = HttpContext.Session.GetInt32("Happiness");
            ViewBag.Meals = HttpContext.Session.GetInt32("Meals");
            ViewBag.Energy = HttpContext.Session.GetInt32("Energy");
            if (ViewBag.Happy >= 100 && ViewBag.Full >= 100 && ViewBag.Energy >= 100) {
                HttpContext.Session.SetString("Status","Cheer");
                HttpContext.Session.SetString("Message","You've won! He likes you! He really likes you!");
                ViewBag.Display1 = "d-none";
                ViewBag.Display2 = "d-block";
            }
            else if (HttpContext.Session.GetString("Status") == "Ded") {
                ViewBag.Display1 = "d-none";
                ViewBag.Display2 = "d-block";
            }
            else {
                ViewBag.Display1 = "d-flex";
                ViewBag.Display2 = "d-none";
            }
            ViewBag.ImgPath = "img/" + HttpContext.Session.GetString("Status") + ".png";
            ViewBag.Message = HttpContext.Session.GetString("Message");
            return View();
        }

        [HttpGet("feed")]
        public IActionResult Feed() {
            if (HttpContext.Session.GetInt32("Meals") > 0) {
                Random rand = new Random();
                HttpContext.Session.SetInt32("Meals",(int)HttpContext.Session.GetInt32("Meals") - 1);
                if (rand.Next(4) == 0) {
                    HttpContext.Session.SetString("Message","Your Dojodachi's a little salty. Meals - 1");
                    HttpContext.Session.SetString("Status","Angry");
                }
                else {
                    int fullamt = rand.Next(5,11);
                    HttpContext.Session.SetInt32("Fullness",(int)HttpContext.Session.GetInt32("Fullness") + fullamt);
                    HttpContext.Session.SetString("Message",$"You fed your DojoDachi! Fullness + {fullamt}, Meals - 1");
                    HttpContext.Session.SetString("Status","Fed");
                }
            }
            else {
                HttpContext.Session.SetString("Message","You're outta meals, brah.");
                HttpContext.Session.SetString("Status","Tired");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("play")]
        public IActionResult Play() {
            if (HttpContext.Session.GetInt32("Energy") >= 5) {
                Random rand = new Random();
                HttpContext.Session.SetInt32("Energy",(int)HttpContext.Session.GetInt32("Energy") - 5);
                if (rand.Next(4) == 0) {
                    HttpContext.Session.SetString("Message","Your Dojodachi's a little salty. Energy - 5.");
                    HttpContext.Session.SetString("Status","Angry");
                }
                else {
                    int happyamt = rand.Next(5,11);
                    HttpContext.Session.SetInt32("Happiness",(int)HttpContext.Session.GetInt32("Happiness") + happyamt);
                    HttpContext.Session.SetString("Message",$"You played with your DojoDachi! Happiness + {happyamt}, Energy - 5");
                    HttpContext.Session.SetString("Status","Cheer");
                }
            }
            else {
                HttpContext.Session.SetString("Message","Your buddy is sleepy, brah. Sleeeeeeeeeeeepppppp. -_- zzz z z z z z  z  z");
                HttpContext.Session.SetString("Status","Tired");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("work")]
        public IActionResult Work() {
            if (HttpContext.Session.GetInt32("Energy") >= 5) {
                Random rand = new Random();
                int mealamt = rand.Next(1,4);
                HttpContext.Session.SetInt32("Meals",(int)HttpContext.Session.GetInt32("Meals") + mealamt);
                HttpContext.Session.SetInt32("Energy",(int)HttpContext.Session.GetInt32("Energy") - 5);
                HttpContext.Session.SetString("Message",$"You participated in corporate greed! Meal + {mealamt}, Energy - 5");
                HttpContext.Session.SetString("Status","Cheer");
            }
            else {
                HttpContext.Session.SetString("Message","Your buddy is sleepy, brah. Sleeeeeeeeeeeepppppp. -_- zzz z z z z z  z  z");
                HttpContext.Session.SetString("Status","Tired");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("sleep")]
        public IActionResult Sleep() {
            HttpContext.Session.SetInt32("Fullness",(int)HttpContext.Session.GetInt32("Fullness") - 5);
            HttpContext.Session.SetInt32("Happiness",(int)HttpContext.Session.GetInt32("Happiness") - 5);
            HttpContext.Session.SetInt32("Energy",(int)HttpContext.Session.GetInt32("Energy") + 15);
            if (HttpContext.Session.GetInt32("Fullness") <= 0) {
                HttpContext.Session.SetString("Message","This is why mom won't let you have a dog.");
                HttpContext.Session.SetString("Status","Ded");
            }
            else if (HttpContext.Session.GetInt32("Happiness") <= 0) {
                HttpContext.Session.SetString("Message","Your 'friend' died of loneliness. Good job.");
                HttpContext.Session.SetString("Status","Ded");
            }
            else {
                HttpContext.Session.SetString("Message","Mmmmmm.... Sleepy sleep.......... Energy + 15, Happiness - 5, Fullness - 5");
                HttpContext.Session.SetString("Status","Asleep");
            }
            return RedirectToAction("Index");
        }

        [HttpGet("/reset")]
        public IActionResult Reset() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
