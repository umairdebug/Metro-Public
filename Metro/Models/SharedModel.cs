using System.ComponentModel.DataAnnotations;

namespace Metro.Models
{
    public class SharedModel
    {
        public SharedModel()
        {
            Id = Path.GetRandomFileName().Replace(".", ""); // dsjflksjlddfk
            DbEntryTime = DateTime.UtcNow;
            LastModifiedTime = DateTime.UtcNow;
        }
        [ScaffoldColumn(false)]
        public string Id { get; set; }// = Path.GetRandomFileName().Replace(".", ""); // dsjflksjlddfk

        [ScaffoldColumn(false)]
        public DateTime DbEntryTime { get; set; }

        [ScaffoldColumn(false)]
        public DateTime LastModifiedTime { get; set; }
    }
}
