using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SportCommentaryDataAccess.DTO.SingleCommentary
{
    public class CreateSingleCommentaryDTO
    {
        [ForeignKey("Commentary")]
        public int CommentaryID { get; set; }
        [ForeignKey("Event")]
        public int EventID { get; set; }
        [Required(ErrorMessage = "{0} jest wymagana.")]
        [StringLength(150, ErrorMessage = " {0} musi być co najmniej {2} i maksymalnie {1} długa.", MinimumLength = 2)]
        [Display(Name = "Nazwa")]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }

        public SportCommentaryDataAccess.Entities.Event Event { get; set; }
    }
}
