using System.Collections.Generic;
using System.IO;
using System.Linq;
using ASPProject.Controllers;
using ASPProject.Data;
using ASPProject.Models;
using ASPProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class UnitTestAnimalController

    {
        private AnimalController testControler;
        private DbContextOptions<AnimalContext> options;
        private AnimalContext context;
        private AnimalRepostory myRep;

        public UnitTestAnimalController()
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
            testControler = new AnimalController(myRep);
        }
        [TestMethod]
        public void TestByOrder()
        {
            TestIndex();
            TestCatalog();
            TestCatalogOneCategorie();
            TestCatalogOneCategorieChoseAll();
            TestMoreDetailsModel();
            TestMoreDetailsAddCommentToBulBul();
            TestMoreDetailsAddCommentNull();
            for (int i = 0; i < 6; i++)
            {
                TestMoreDetailsAddCommentToBulBul();
            }
            TestIndexBecomeBulBul();
        }
        public void TestIndex()
        {
            var whatShoudBeShown = myRep.TopTwoComments();
            var viewResult = testControler.Index() as ViewResult;
            var result = (Animal[])viewResult.Model;
            bool flag = true;
            // Added this condition to show that the index does change after we add comments
            if (result[0].AnimalId == 1)
                flag = false;
            for (int i = 0; i < whatShoudBeShown.Length; i++)
            {
                if (result[i].AnimalId != whatShoudBeShown[i].AnimalId)
                {
                    flag = false;
                    break;
                }
               
            }
            Assert.IsTrue(flag);
        }
        public void TestIndexBecomeBulBul()
        {
            var whatShoudBeShown = myRep.TopTwoComments();
            var viewResult = testControler.Index() as ViewResult;
            var result = (Animal[])viewResult.Model;
           
            Assert.IsTrue(whatShoudBeShown[0].AnimalId==result[0].AnimalId &&result[0].AnimalId==1 );
        }

        public void TestCatalog()
        {
            var viewResult = testControler.Catalog() as ViewResult;
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

        public void TestCatalogOneCategorie()
        {
            //This categorie is Birds
            var thisCaegorie = 1;
            var viewResult = testControler.Catalog(thisCaegorie) as ViewResult;
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
        public void TestCatalogOneCategorieChoseAll()
        {
            //This categorie is all
            var thisCaegorie = 0;
            var viewResult = testControler.Catalog(thisCaegorie) as ViewResult;
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
        public void TestMoreDetailsModel()
        {
            //This animal is BulBul
            var thisAnimalId = 1;
            var viewResult = testControler.MoreDetails(thisAnimalId) as ViewResult;
            var shownAnimals = (Animal)viewResult.Model;
            var result = myRep.GetAnimal(thisAnimalId);
            Assert.IsTrue(result.AnimalId == shownAnimals.AnimalId);
        }
        public void TestMoreDetailsAddCommentToBulBul()
        {
            //This animal is BulBul
            var thisAnimalId = 1;
            string newComment = "NewComment";
            var viewResult = testControler.MoreDetails(thisAnimalId, newComment) as ViewResult;
            var resultComments = myRep.AnimalComments(thisAnimalId);
            Assert.IsTrue(resultComments[resultComments.Count() - 1] == newComment);
        }
        public void TestMoreDetailsAddCommentNull()
        {
            //This animal is BulBul
            var thisAnimalId = 1;
            string newComment = null;
            //should stay the same after the controller action
            var resultCommentsLength = myRep.AnimalComments(thisAnimalId).Count();
            var viewResult = testControler.MoreDetails(thisAnimalId, newComment) as ViewResult;
            var resultCommentsLengthAfterAddingNothing = myRep.AnimalComments(thisAnimalId).Count();
            Assert.IsTrue(resultCommentsLength == resultCommentsLengthAfterAddingNothing);
        }

    }
}
