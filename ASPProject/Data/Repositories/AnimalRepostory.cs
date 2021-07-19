using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ASPProject.Data;
using ASPProject.Models;

namespace ASPProject.Repositories
{
    //Each method explanation is descraibed in the interface
    public class AnimalRepostory : IRepository
    {
        private readonly AnimalContext myDb;

        public AnimalRepostory(AnimalContext Db)
        {
            myDb = Db;
        }
        public void AddAnimal(Animal addMe)
        {
            Animal newAnimal = new Animal { Age = addMe.Age, CategoryId = addMe.CategoryId, Descrition = addMe.Descrition, Name = addMe.Name, PictureName = addMe.PictureName };
            myDb.animals.Add(newAnimal);
            myDb.SaveChanges();
        }
        public void AddComment(string addMe, int animalId)
        {
            Comment myNewComment = new Comment { AnimalId = animalId, CommentData = addMe };
            myDb.comments.Add(myNewComment);
            myDb.SaveChanges();
        }
        public List<string> AnimalComments(int animalId)
        {
            return myDb.comments.Where
                (comment => comment.AnimalId == animalId)
                .Select(item => item.CommentData).ToList();
        }
        public void ChangeAnimal(int animalId, Animal NewMe)
        {
            Animal changMeNow = myDb.animals.Where(item => item.AnimalId == animalId).First();
            changMeNow.Name = NewMe.Name;
            changMeNow.CategoryId = NewMe.CategoryId;
            changMeNow.Descrition = NewMe.Descrition;
            changMeNow.Age = NewMe.Age;
            changMeNow.PictureName = NewMe.PictureName;
            myDb.SaveChanges();
        }
        public Animal GetAnimal(int animalId)
        {
            try
            {
                return myDb.animals.Where(animal => animal.AnimalId == animalId).First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<Animal> GetAllAnimals()
        {
            return myDb.animals.ToList();
        }

        public List<Animal> OnlyOneCategorie(int chosen)
        {
            return myDb.animals.Where(item => item.CategoryId == chosen).ToList();
        }
        public void RemoveAnimal(Animal removeMe)
        {
            IEnumerable<Comment> commentToRemove = myDb.comments.
                Where(item => item.AnimalId == removeMe.AnimalId);
            foreach (var comment in commentToRemove)
            {
                myDb.comments.Remove(comment);
            }
            myDb.Remove(removeMe);
            myDb.SaveChanges();
        }
        public Animal[] TopTwoComments()
        {
            Animal[] topTwo;
            if (myDb.animals.Count() >= 2)
            {
                topTwo = new Animal[2];
            }
            else if (myDb.animals.Count() == 1)
            {
                topTwo = new Animal[1];
                topTwo[0] = myDb.animals.First();
                return topTwo;
            }
            else
            {
                return null;
            }
            int biggestId = 1, secondbigId = 2, countBiggest = 0, countSecond = 0;
            foreach (var item in myDb.animals.ToList())
            {
                int checker = myDb.comments.Where(item2 => item2.AnimalId == item.AnimalId).Count();
                if (checker >= countBiggest)
                {
                    secondbigId = biggestId;
                    countSecond = countBiggest;
                    biggestId = item.AnimalId;
                    countBiggest = checker;
                }
                else if (checker >= countSecond)
                {
                    secondbigId = item.AnimalId;
                    countSecond = checker;
                }
            }
            topTwo[0] = myDb.animals.Where(item => item.AnimalId == biggestId).First();
            topTwo[1] = myDb.animals.Where(item => item.AnimalId == secondbigId).First();
            return topTwo;
        }
        public string GetCategorie(int animalId)
        {
            Animal chosen = GetAnimal(animalId);
            return myDb.categories.Where(item => item.CategoryId == chosen.CategoryId).First().Name;
        }
        public IEnumerable<Categorie> GetAllCategories()
        {
            return myDb.categories;
        }
    }
}

