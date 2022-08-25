using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportCommentaryDataAccess.DTO.SportType
{
    public class CreateSportTypeDTO
    {
        [Display(Name = "Nazwa sportu")]
        [Required(ErrorMessage = "{0} jest wymagana.")]
        [MinLength(2, ErrorMessage = "{0} musi mieć co najmniej {1} znaki.")]
        [MaxLength(100, ErrorMessage = "{0} musi mieć maksymalnie {1} znaków.")]
        public string Name { get; set; }
    }
}
