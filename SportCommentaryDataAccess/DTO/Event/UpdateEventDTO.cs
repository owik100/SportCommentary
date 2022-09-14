﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SportCommentaryDataAccess.DTO.Event
{
    public class UpdateEventDTO
    {
        public int EventID { get; set; }
        [Required(ErrorMessage = "{0} jest wymagana.")]
        [StringLength(150, ErrorMessage = " {0} musi być co najmniej {2} i maksymalnie {1} długa.", MinimumLength = 2)]
        [Display(Name = "Nazwa wydarzenia")]
        public string Name { get; set; }
        public string? Icon { get; set; }
        public int? SportTypeID { get; set; }
    }
}
