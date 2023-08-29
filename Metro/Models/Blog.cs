namespace Metro.Models
{
    public class Blog : SharedModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Author { get; set; }
        public int Rank { get; set; }   
    }
}
