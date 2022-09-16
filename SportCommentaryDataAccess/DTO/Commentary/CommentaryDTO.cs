using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SportCommentaryDataAccess.DTO.Commentary
{
    public class CommentaryDTO
    {
        public int CommentaryID { get; set; }
        [Required(ErrorMessage = "{0} jest wymagana.")]
        [StringLength(400, ErrorMessage = " {0} musi być co najmniej {2} i maksymalnie {1} długa.", MinimumLength = 2)]
        [Display(Name = "Relacja")]
        public string Caption { get; set; }
        public string Description { get; set; }
        public string CommentaryImage { get; set; }
        public DateTime CommentaryStart { get; set; }
        public bool IsLive { get; set; }
    }
}
