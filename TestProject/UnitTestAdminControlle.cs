using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASPProject.Controllers;
using ASPProject.Data;
using ASPProject.Models;
using ASPProject.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestProject.FakeImplementsForTesting;

namespace TestProject
{
    [TestClass]
    public class UnitTestAdminController

    {
        private AdminController testControler;
        private DbContextOptions<AnimalContext> options;
        private AnimalContext context;
        private AnimalRepostory myRep;
        private IWebHostEnvironment myHost;

        public UnitTestAdminController()
        {
            options = new DbContextOptionsBuilder<AnimalContext>().UseInMemoryDatabase(databaseName: "DbContextDatabase").Options;
            context = new AnimalContext(options);
            if (context.animals.Count() == 0)
                context.animals.AddRange(new List<Animal>{
                new Animal { AnimalId = 1, Age = 5, CategoryId = 1, Name = "BulBul", Descrition = "A common bird in israel", PictureName = "bulbulPic.jpg" },
                new Animal { AnimalId = 2, Age = 3, CategoryId = 2, Name = "Shark", Descrition = "A dangerous hungry fish", PictureName = "sharkPic.jpg" },
                new Animal { AnimalId = 3, Age = 15, CategoryId = 3, Name = "Dog", Descrition = "The mans best friend", PictureName = "dogPic.jpg" },
                new Animal { AnimalId = 4, Age = 2, CategoryId = 4, Name = "Zepha", Descrition = "A common dangrous snake", PictureName = "zephaPic.jpg" },
                new Animal { AnimalId = 5, Age = 5, CategoryId = 3, Name = "Cat", Descrition = "A populer pet ", PictureName = "catPic.jpg" },
                new Animal { AnimalId = 6, Age = 5, CategoryId = 1, Name = "Eagal", Descrition = "The sign of freedom", PictureName = "eagelePic.jpg" }});
            if (context.categories.Count() == 0)
                context.categories.AddRange(new List<Categorie> {  new Categorie { CategoryId = 1, Name = "Birds" },
                new Categorie { CategoryId = 2, Name = "Fishs" },
                new Categorie { CategoryId = 3, Name = "Mammals" },
                new Categorie { CategoryId = 4, Name= "Reptiles" }});
            if (context.comments.Count() == 0)
                context.comments.AddRange(new List<Comment> {new Comment { CommentId=1,AnimalId=1,CommentData="BulBul ha ha ha"},
                new Comment { CommentId=2,AnimalId=1,CommentData="Funny name ha ha ha"},
                new Comment { CommentId=3,AnimalId=3,CommentData="I love dogs "},
                new Comment { CommentId=4,AnimalId=3,CommentData="Dogs are the best"},
                new Comment { CommentId=5,AnimalId=3,CommentData="My dog is the best"},
                new Comment { CommentId=6,AnimalId=5,CommentData="Cats are adorabol"},
                new Comment { CommentId=7,AnimalId=5,CommentData="Cats are lazy"},
                new Comment { CommentId=8,AnimalId=5,CommentData="My cat sleeps all the time"},
                new Comment { CommentId=9,AnimalId=5,CommentData="'A populer pet' not in my city we hate cats over here"},
                new Comment { CommentId=10,AnimalId=3,CommentData="I have the same kind of dog"},
                new Comment { CommentId=11,AnimalId=3,CommentData="My dog is the best"}
            });
            context.SaveChanges();
            myRep = new AnimalRepostory(context);
            //I use it just to get the path to wwwroot, so i made one that only returns the wwwroot
            myHost = new FakeWebHost();
            testControler = new AdminController(myRep, myHost);
        }
        [TestMethod]
        public void TestByOrder()
        {
            TestAdmiChangeAnimalDontReal();
            TestAdminCatalog();
            TestAdminCatalogSpacificCategorie();
            TestAdminCatalogChooseAllCategories();
            TestAdmiChangeAnimal();
            TestAdmiChangeAnimalPost();
            TestAdminMakeNewAnimalInputError();
            TestAdminMakeNewAnimal();
            TestAdminDeleteAnimal();

        }
        public void TestAdminCatalog()
        {
            var viewResult = testControler.AdminCatalog() as ViewResult;
            var shownAnimals = (List<Animal>)viewResult.Model;
            var result = myRep.GetAllAnimals();
            bool flag = true;
            for (int i = 0; i < shownAnimals.Count(); i++)
            {
                if (result[i].AnimalId != shownAnimals[i].AnimalId)
                {
                    flag = false;
                    break;
                }
            }
            Assert.IsTrue(flag);
        }
        public void TestAdminCatalogSpacificCategorie()
        {
            var thisCaegorie = 1;
            var viewResult = testControler.AdminCatalog(thisCaegorie) as ViewResult;
            var shownAnimals = (List<Animal>)viewResult.Model;
            var result = myRep.OnlyOneCategorie(thisCaegorie);
            bool flag = true;
            for (int i = 0; i < shownAnimals.Count(); i++)
            {
                if ((result[i].AnimalId != shownAnimals[i].AnimalId) || result[i].CategoryId != thisCaegorie)
                {
                    flag = false;
                    break;
                }
            }
            Assert.IsTrue(flag);
        }
        public void TestAdminCatalogChooseAllCategories()
        {
            //This categorie is all
            var thisCaegorie = 0;
            var viewResult = testControler.AdminCatalog(thisCaegorie) as ViewResult;
            var shownAnimals = (List<Animal>)viewResult.Model;
            var result = myRep.GetAllAnimals();
            bool flag = true;
            for (int i = 0; i < shownAnimals.Count(); i++)
            {
                if ((result[i].AnimalId != shownAnimals[i].AnimalId))
                {
                    flag = false;
                    break;
                }
            }
            Assert.IsTrue(flag);
        }
        public void TestAdmiChangeAnimal()
        {
            //This animal is bulbul
            var thisAnimal = 1;
            var viewResult = testControler.ChangeAnimal(thisAnimal) as ViewResult;
            var shownAnimal = (Animal)viewResult.Model;
            var result = myRep.GetAnimal(1);
            Assert.IsTrue(result.Name== "BulBul"&& shownAnimal.AnimalId==result.AnimalId);
        }
        public void TestAdmiChangeAnimalDontReal()
        {
            //This animal isnt real
            var thisAnimalId = 11;
            var viewResult = testControler.ChangeAnimal(thisAnimalId) as ViewResult;
            var shownAnimal = (Animal)viewResult.Model;
            Assert.IsTrue(shownAnimal == null);
        }
        public void TestAdmiChangeAnimalPost()
        {
            //This animal is bulbul
            var thisAnimal = 1;
            var newBulBul = myRep.GetAnimal(thisAnimal);
            newBulBul.ImgFile = null;
            newBulBul.Name = "New BulBul";
            var viewResult = testControler.ChangeAnimal(thisAnimal,newBulBul) as ViewResult;
            var result = myRep.GetAnimal(thisAnimal);
            Assert.IsTrue(result.Name== "New BulBul");
        }
        public void TestAdminMakeNewAnimalInputError()
        {
            var newAnimal = new Animal { ImgFile = null, Age = 10, CategoryId = 1, Name = "Nahlieli", Descrition = "desc" };
            var viewResult = testControler.MakeNewAnimal(newAnimal) as ViewResult;
            var animalList = myRep.GetAllAnimals();
            Assert.IsTrue(animalList[animalList.Count() - 1].Name != "Nahlieli");

        }
        public void TestAdminMakeNewAnimal()
        {
            var inputImg = new FakeFormFIle();
            var newAnimal = new Animal { ImgFile=inputImg,Age=10,CategoryId=1,Name="Nahlieli",Descrition="desc"};
            var viewResult = testControler.MakeNewAnimal(newAnimal) as ViewResult;
            var animalList = myRep.GetAllAnimals();
            Assert.IsTrue(animalList[animalList.Count() - 1].Name == "Nahlieli");
        }
        public void TestAdminDeleteAnimal()
        {
            //This is bulbul
            var deleteAnimalId = 1;
            var viewResult = testControler.DeleteAnimal(deleteAnimalId) as ViewResult;
            var animalList = myRep.GetAllAnimals();
            bool flag = true;
            foreach (var item in animalList)
            {
                if (item.Name == "BulBul")
                    flag = false;
            }
            Assert.IsTrue(flag);
        }
    }
}
