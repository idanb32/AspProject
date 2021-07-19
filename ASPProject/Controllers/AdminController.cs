using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASPProject.Models;
using ASPProject.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace ASPProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly IRepository myRepo;
        private readonly IWebHostEnvironment hostEnvironment;
        //We get the webHostEnvironment to get the location of our pics dirctory
        public AdminController(IRepository myRepo, IWebHostEnvironment hostEnvironment)
        {
            this.myRepo = myRepo;
            this.hostEnvironment = hostEnvironment;
        }
        //Display all the animals in the admin table, and we make sure that we have all the categories for the drop down, and it selected by default to show all
        public IActionResult AdminCatalog()
        {
            ViewBag.selected = 0;
            ViewBag.Categories = myRepo.GetAllCategories();
            return View(myRepo.GetAllAnimals());
        }
        //We use this to get what categories the user chose and display animals only from that spacific categorie
        [HttpPost]
        public IActionResult AdminCatalog(int id)
        {
            ViewBag.selected = id;
            ViewBag.Categories = myRepo.GetAllCategories();
            if (id != 0)
                return View("AdminCatalog", myRepo.OnlyOneCategorie(id));
            else
                return View("AdminCatalog", myRepo.GetAllAnimals());
        }
        //We use this action to change a spacific animal, by getting the spacific animal via the service, and use that data and inseret it to the form for the user to deciede what props are going to stay the same and what are going to change
        [HttpGet]
        public IActionResult ChangeAnimal(int id)
        {
            ViewBag.Categories = myRepo.GetAllCategories();
            Animal changeMe = myRepo.GetAnimal(id);
            if (changeMe != null)
                return View("MakeAnimal", changeMe);
            else
                return View("MakeAnimal");
        }
        //We use this action to change a spacific animal after the user posted his props, and only if it passed the validation
        [HttpPost]
        public IActionResult ChangeAnimal(int id, Animal animal)
        {
            ViewBag.Categories = myRepo.GetAllCategories();
            if (ModelState.IsValid)
            {
                if (animal.ImgFile != null)
                {
                    string removeMePath = Path.Combine(hostEnvironment.WebRootPath, "Pics", myRepo.GetAnimal(id).PictureName);
                    if (System.IO.File.Exists(removeMePath))
                    {
                        System.IO.File.Delete(removeMePath);
                    }
                    string wwwRootPath = hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(animal.ImgFile.FileName);
                    string fileExtention = Path.GetExtension(animal.ImgFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssffff") + fileExtention;
                    animal.PictureName = fileName;
                    string path = Path.Combine(wwwRootPath + "/Pics", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        animal.ImgFile.CopyTo(fileStream);
                    }
                }
                else
                {
                    animal.PictureName = myRepo.GetAnimal(id).PictureName;
                }
                myRepo.ChangeAnimal(id, animal);
                return View("AdminCatalog", myRepo.GetAllAnimals());
            }
            animal.AnimalId = id;
            return View("MakeAnimal", animal);
        }
        //We use this action to make a new animal(get us to the new animal form)
        [HttpGet]
        public IActionResult MakeNewAnimal()
        {
            ViewBag.Categories = myRepo.GetAllCategories();
            return View("MakeAnimal");
        }
        //We use this action to make a new animal after the user posted his choies of props of the animal, and we do it only if it passed the validation
        [HttpPost]
        public IActionResult MakeNewAnimal(Animal animal)
        {
            ViewBag.Categories = myRepo.GetAllCategories();
            if (ModelState.IsValid)
            {
                if (animal.ImgFile != null)
                {
                    string wwwRootPath = hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(animal.ImgFile.FileName);
                    string fileExtention = Path.GetExtension(animal.ImgFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssffff") + fileExtention;
                    animal.PictureName = fileName;
                    string path = Path.Combine(wwwRootPath + "/Pics", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        animal.ImgFile.CopyTo(fileStream);
                    }
                    myRepo.AddAnimal(animal);
                    return View("AdminCatalog", myRepo.GetAllAnimals());
                }
            }
            if (animal.ImgFile == null)
                ViewBag.ShowEror = "Must insert an image";
            else
            {
                ViewBag.ShowEror = null;
                ViewBag.img = animal.ImgFile;
            }
            return View("MakeAnimal");
        }
        //We use this action to delete a spacific animal that the user chose to deltete via our service.
        public IActionResult DeleteAnimal(int id)
        {
            Animal removeMe = myRepo.GetAnimal(id);
            string imgPath = Path.Combine(hostEnvironment.WebRootPath, "Pics", removeMe.PictureName);
            if (System.IO.File.Exists(imgPath))
            {
                System.IO.File.Delete(imgPath);
            }
            myRepo.RemoveAnimal(removeMe);
            ViewBag.Categories = myRepo.GetAllCategories();
            return View("AdminCatalog", myRepo.GetAllAnimals());
        }

    }
}
