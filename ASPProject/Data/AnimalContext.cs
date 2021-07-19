using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ASPProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ASPProject.Data
{
    public class AnimalContext : DbContext
    {
        public AnimalContext(DbContextOptions<AnimalContext> options) : base(options)
        {
        }
        public DbSet<Animal> animals { get; set; }
        public DbSet<Categorie> categories { get; set; }
        public DbSet<Comment> comments { get; set; }
        //Copy all the images from ONE directory to another directory its public for the UT
        public void Copy(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);
            foreach (var file in Directory.GetFiles(sourceDir))
            {
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
            }
        }
        //Deletes all the files in one directory its public for the UT
        public void Delete(string deleteFromHere)
        {
            DirectoryInfo di = new DirectoryInfo(deleteFromHere);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string copyFromHere = @"C:\Users\user\source\repos\ASPProject\ASPProject\wwwroot\CantChangeMePics";
            string copyToHereAfterItsEmpyy = @"C:\Users\user\source\repos\ASPProject\ASPProject\wwwroot\Pics";
            //On creation we make sure that we have our basics imgs only and nothing from the last run of the program, we do that by deleting all the files in our Pics folder that is kept use to save imges from users
            Delete(copyToHereAfterItsEmpyy);
            //And then we copy all the basic imges from a diffrent dirctory so we could have the basic program on each run
            Copy(copyFromHere, copyToHereAfterItsEmpyy);
            modelBuilder.Entity<Categorie>().HasData
                (
                new Categorie { CategoryId = 1, Name = "Birds" },
                new Categorie { CategoryId = 2, Name = "Fishs" },
                new Categorie { CategoryId = 3, Name = "Mammals" },
                new Categorie { CategoryId = 4, Name= "Reptiles" }
                );
            modelBuilder.Entity<Animal>().HasData
                (
                new Animal {AnimalId=1,Age=5,CategoryId=1,Name="BulBul",Descrition="A common bird in israel",PictureName= "bulbulPic.jpg" },
                new Animal {AnimalId=2,Age=3,CategoryId=2,Name="Shark",Descrition="A dangerous hungry fish",PictureName= "sharkPic.jpg" },
                new Animal {AnimalId=3,Age=15,CategoryId=3,Name="Dog",Descrition="The mans best friend",PictureName= "dogPic.jpg" },
                new Animal {AnimalId=4,Age=2,CategoryId=4,Name="Zepha",Descrition="A common dangrous snake",PictureName= "zephaPic.jpg" },
                new Animal {AnimalId=5,Age=5,CategoryId=3,Name="Cat",Descrition="A populer pet ",PictureName= "catPic.jpg" },
                new Animal {AnimalId=6,Age=5,CategoryId=1,Name="Eagal",Descrition="The sign of freedom",PictureName= "eagelePic.jpg" }
                )
                ;
            modelBuilder.Entity<Comment>().HasData
                (
                new Comment { CommentId=1,AnimalId=1,CommentData="BulBul ha ha ha"},
                new Comment { CommentId=2,AnimalId=1,CommentData="Funny name ha ha ha"},
                new Comment { CommentId=3,AnimalId=3,CommentData="I love dogs "},
                new Comment { CommentId=4,AnimalId=3,CommentData="Dogs are the best"},
                new Comment { CommentId=5,AnimalId=3,CommentData="My dog is the best"},
                new Comment { CommentId=6,AnimalId=5,CommentData="Cats are adorabol"},
                new Comment { CommentId=7,AnimalId=5,CommentData="Cats are lazy"},
                new Comment { CommentId=8,AnimalId=5,CommentData="My cat sleeps all the time"},
                new Comment { CommentId=9,AnimalId=5,CommentData="'A populer pet' not in my city we hate cats over here"},
                new Comment { CommentId=10,AnimalId=3,CommentData="I have the same kind of dog"}
                );
        }
    }
}
