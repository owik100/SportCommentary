using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportCommentaryDataAccess.DTO.SportType
{
    public class SportTypeDTO
    {
        public int SportTypeID { get; set; }
        [Required(ErrorMessage="{0} jest wymagana.")]
        [StringLength(150, ErrorMessage = " {0} musi być co najmniej {2} i maksymalnie {1} długa.", MinimumLength = 2)]
        [Display(Name = "Nazwa sportu")]
        public string Name { get; set; }
        public ICollection<SportCommentaryDataAccess.Entities.Commentary>? Commentaries { get; set; }
    }
}
