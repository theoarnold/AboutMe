using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AboutMe.Data
{
    public class ButtonInfo
    {
        [Key]
        public int Id { get; set; } // Primary key

        [StringLength(14)]
        public string ButtonText { get; set; }

        public string ButtonColourHex { get; set; }

        public string ButtonUrl { get; set; }

        [ForeignKey("ApplicationInfo")]
        public int? ApplicationInfoId { get; set; } // Foreign key

        public ApplicationInfo ApplicationInfo { get; set; } // Navigation property
    }

    public class ApplicationInfo
    {
        [Key]
        public int Id { get; set; } // Primary key

        public string Bio { get; set; }

        public string GithubCred { get; set; }

        public string GithubName { get; set; }

        public List<ButtonInfo> Buttons { get; set; }
    }
}
