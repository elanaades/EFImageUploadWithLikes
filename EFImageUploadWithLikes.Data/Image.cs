namespace EFImageUploadWithLikes.Data
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FileName { get; set; }
        public int Likes { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}