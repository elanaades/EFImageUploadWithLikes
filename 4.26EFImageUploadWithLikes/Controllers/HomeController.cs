using _4._26EFImageUploadWithLikes.Models;
using EFImageUploadWithLikes.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text.Json;

namespace _4._26EFImageUploadWithLikes.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;
        private IWebHostEnvironment _environment;

        public HomeController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _environment = environment;
        }

        public IActionResult Index()
        {
            var repo = new ImageRespository(_connectionString);
            return View(new HomePageViewModel
            {
                Images = repo.GetImages()
            });
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(Image image, IFormFile imageFile)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            string fullPath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            using var stream = new FileStream(fullPath, FileMode.CreateNew);
            imageFile.CopyTo(stream);
            image.FileName = fileName;

            var repo = new ImageRespository(_connectionString);
            image.DateUploaded = DateTime.Now;
            repo.AddImage(image);
            return Redirect("/");
        }

        public IActionResult ViewImage(int id)
        {
            var repo = new ImageRespository(_connectionString);
            bool alreadyLiked = HttpContext.Session.GetInt32($"Image-{id}-{User.Identity.Name}") != null;

            var vm = new ViewImageViewModel
            {
                Image = repo.GetImageById(id),
                AlreadyLiked = alreadyLiked
            };
            return View(vm);
        }

        public IActionResult GetLikes(int id)
        {
            var repo = new ImageRespository(_connectionString);
            Image image = repo.GetImageById(id);
            return Json(image.Likes);
        }

        [HttpPost]
        public IActionResult UpdateLikes(int id)
        {
            var currentUser = User.Identity.Name;
            var repo = new ImageRespository(_connectionString);
            Image image = repo.GetImageById(id);
            var likesImage = HttpContext.Session.GetInt32($"Image-{id}-{currentUser}");
            
            if (likesImage == null)
            {
                HttpContext.Session.SetInt32($"Image-{id}-{currentUser}", 1);
                image.Likes++;
                repo.UpdateImageLikes(image);
            }

            return Redirect($"ViewImage?id={id}");
        }
    }

    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key)
        {
            string value = session.GetString(key);

            return value == null ? default(T) :
                JsonSerializer.Deserialize<T>(value);
        }
    }
}