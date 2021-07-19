using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ASPProject.Controllers
{
    public class AnimalController : Controller
    {
        private readonly IRepository myRepo;
        public AnimalController(IRepository myRepo)
        {
            this.myRepo = myRepo;
        }
        //We use this as our home page and default page, as we can see in the pipe line in the configure method, and we display the two most comented animals via our service
        public IActionResult Index()
        {
            return View(myRepo.TopTwoComments());
        }
        //We use this action to display our catalog of animals, making sure we insert the categories props for our dropdown
        public IActionResult Catalog()
        {
            ViewBag.selected = 0;
            ViewBag.Categories = myRepo.GetAllCategories();
            return View(myRepo.GetAllAnimals());
        }
        //We use this action when we get an input from the user about a spacific categories he wants to display, and we display it via our service
        [HttpPost]
        public IActionResult Catalog(int id)
        {
            ViewBag.selected = id;
            ViewBag.Categories = myRepo.GetAllCategories();
            if (id != 0)
                return View("Catalog", myRepo.OnlyOneCategorie(id));
            else
                return View("Catalog", myRepo.GetAllAnimals());
        }
        //We use this action to show a spacific animal details(all of their props) to the user by his choice, and we are in a get attribute beacuse we use this page to upload comments about the animal as well 
        [HttpGet]
        public IActionResult MoreDetails(int id)
        {
            ViewBag.Comments = myRepo.AnimalComments(id);
            ViewBag.Categorie = myRepo.GetCategorie(id);
            return View(myRepo.GetAnimal(id));
        }
        //We use this action to post the comments that are displayed about a spacific animal
        [HttpPost]
        public IActionResult MoreDetails(int id, string comment)
        {
            if (comment != null)
            {
                myRepo.AddComment(comment, id);
            }
            ViewBag.Comments = myRepo.AnimalComments(id);
            ViewBag.Categorie = myRepo.GetCategorie(id);
            return View(myRepo.GetAnimal(id));
        }

    }
}
