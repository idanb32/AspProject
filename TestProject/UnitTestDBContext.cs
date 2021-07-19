using System.IO;
using System.Linq;
using ASPProject.Data;
using ASPProject.Models;
using ASPProject.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class UnitTestDBContext

    {
        private DbContextOptions<AnimalContext> options;
        private AnimalContext context;
        [TestMethod]
        public void TestByOrder()
        {
            Delete();
            Copy();
        }
        public void Delete()
        {
            string copyToHereAfterItsEmpyy = @"C:\Users\user\source\repos\ASPProject\ASPProject\wwwroot\Pics";
            context.Delete(copyToHereAfterItsEmpyy);
            var folder = new DirectoryInfo(copyToHereAfterItsEmpyy);
            if (folder.Exists)
            {
                Assert.IsTrue(folder.GetFileSystemInfos().Length == 0);
            }
        }
        public void Copy()
        {
            string copyToHereAfterItsEmpyy = @"C:\Users\user\source\repos\ASPProject\ASPProject\wwwroot\Pics";
            string copyFromHere = @"C:\Users\user\source\repos\ASPProject\ASPProject\wwwroot\CantChangeMePics";
            context.Copy(copyFromHere,copyToHereAfterItsEmpyy);
            var folder = new DirectoryInfo(copyToHereAfterItsEmpyy);
            if (folder.Exists)
            {
                Assert.IsTrue(folder.GetFileSystemInfos().Length != 0);
            }
        }
        [TestInitialize]
        public void Init()
        {
            options = new DbContextOptionsBuilder<AnimalContext>().UseInMemoryDatabase(databaseName: "DbContextDatabase").Options;
            context = new AnimalContext(options);
        }

    }
}
