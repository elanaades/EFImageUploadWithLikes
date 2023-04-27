using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFImageUploadWithLikes.Data
{
    public class ImageRespository
    {
        private string _connectionString;

        public ImageRespository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Image> GetImages()
        {
            using var context = new ImageDBContext(_connectionString);
            return context.Images.ToList();
        }

        public void AddImage(Image image)
        {
            using var context = new ImageDBContext(_connectionString);
            context.Images.Add(image);
            context.SaveChanges();
        }

        public Image GetImageById(int id)
        {
            using var context = new ImageDBContext(_connectionString);
            return context.Images.FirstOrDefault(i => i.Id == id);
        }

        public void UpdateImageLikes(Image image)
        {
            using var context = new ImageDBContext(_connectionString);
            context.Images.Attach(image);
            context.Entry(image).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
