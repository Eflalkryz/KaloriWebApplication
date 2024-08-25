namespace KaloriWebApplication.Models.Concrete
{
    public class fotoUpload
    {
        public IFormFile image { get; set; }
        public string SelectedRadio { get; set; }
        public List<string> OCRText { get; set; }
    }
}
