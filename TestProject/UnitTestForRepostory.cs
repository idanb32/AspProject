using System.Collections.Generic;
using System.Linq;
using ASPProject.Data;
using ASPProject.Models;
using ASPProject.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class UnitTestForRepostory
    {
        private DbContextOptions<AnimalContext> options;
        private AnimalContext context;
        private AnimalRepostory myRep;
        public UnitTestForRepostory()
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
        }
        [TestMethod]
        public void TestByOrder()
        {
            TestGetAllAnimal();
            TestAddAnimal();
            TestTopTwoComments();
            TestComments();
            TestAddComment();
            TestAllCategories();
            TestGetAnimal();
            TestOneCategorie();
            TestRemoveAnimal();
            TestChangeAnimal();
        }
        public void TestGetAllAnimal()
        {
            var amountOfAnimals = myRep.GetAllAnimals().Count();
            var resault = 6;
            Assert.AreEqual(amountOfAnimals, resault);
        }
        public void TestAddAnimal()
        {
            myRep.AddAnimal(new Animal { });
            var amountOfAnimals = myRep.GetAllAnimals().Count();
            var resault = 7;
            Assert.AreEqual(amountOfAnimals, resault);
        }
        public void TestTopTwoComments()
        {
            Animal[] topTwo = myRep.TopTwoComments();
            Assert.IsTrue(topTwo[0].Name == "Dog" && topTwo[1].Name == "Cat");
        }
        public void TestComments()
        {
            var coments = myRep.AnimalComments(3).Count();
            var resault = 5;
            Assert.IsTrue(coments == resault || coments == resault + 1);
        }
        public void TestAddComment()
        {
            myRep.AddComment("new Comment", 3);
            var coments = myRep.AnimalComments(3).Count();
            var resault = 6;
            Assert.IsTrue(coments == resault);
        }
        public void TestAllCategories()
        {
            var categoriesNum = myRep.GetAllCategories().Count();
            var resault = 4;
            Assert.IsTrue(categoriesNum == resault);
        }
        public void TestGetAnimal()
        {
            var dogName = myRep.GetAnimal(3).Name;
            var resault = "Dog";
            Assert.IsTrue(dogName == resault);
        }
        public void TestOneCategorie()
        {
            var animalsInCategorie = myRep.OnlyOneCategorie(1);
            var resault = "BulBul";
            var resaultTwo = "Eagal";
            Assert.IsTrue(animalsInCategorie[0].Name == resault && animalsInCategorie[1].Name == resaultTwo);
        }
        public void TestRemoveAnimal()
        {
            myRep.RemoveAnimal(myRep.GetAnimal(7));
            var numOfAnimals = myRep.GetAllAnimals().Count();
            var resault = 6;
            Assert.IsTrue(numOfAnimals == resault);
        }
        public void TestChangeAnimal()
        {
            var newAnimal = myRep.GetAnimal(1);
            var resault = newAnimal.Name = "BulBulNewName";
            myRep.ChangeAnimal(1, newAnimal);
            Assert.AreEqual(resault, myRep.GetAnimal(1).Name);
        }
    }
}
