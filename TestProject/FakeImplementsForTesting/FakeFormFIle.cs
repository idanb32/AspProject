using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace TestProject.FakeImplementsForTesting
{
    //This implement is to test input of a user of an image
    class FakeFormFIle : IFormFile
    {
        public string ContentType => throw new NotImplementedException();

        public string ContentDisposition => throw new NotImplementedException();

        public IHeaderDictionary Headers => throw new NotImplementedException();

        public long Length => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public string FileName => "testing.jpg";

        public void CopyTo(Stream target)
        {
            string path = @$"C:\\Users\\user\\source\\repos\\ASPProject\\ASPProject\\wwwroot\\UTPics\\{this.FileName}";
            using (var source = File.Open(path, FileMode.Open))
            {
                source.CopyTo(target);
            }
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Stream OpenReadStream()
        {
            throw new NotImplementedException();
        }
    }
}
