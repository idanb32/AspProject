using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPProject.Models;

namespace ASPProject.Repositories
{
    public interface IRepository
    {
        //Get and send to the controller the top two most commented animals so we could show them at the home page
        public Animal[] TopTwoComments();
        //Get us a list of all the animals that are in one categorey 
        public List<Animal> OnlyOneCategorie(int chosen);
        //Get us a spacific animal
        public Animal GetAnimal(int animalId);
        //Add a comment to spacific animal
        public void AddComment(string addMe, int animalId);
        //Remove a spacific animal
        public void RemoveAnimal(Animal removeMe);
        //Add a new animal
        public void AddAnimal(Animal addMe);
        //Change the animal props to the new one we get (while keeping the animal id)
        public void ChangeAnimal(int animalId,Animal NewMe);
        //Get us a list to display all the animal comments
        public List<string> AnimalComments(int animalId);
        //Get us all the animals
        public List<Animal> GetAllAnimals();
        //Get us the categorie name of an animal
        public string GetCategorie(int animalId);
        //Get us a list of all the categories so we can use with our drop down
        public IEnumerable<Categorie> GetAllCategories();
    }
}
