using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PTS.UI.Models
{
    public class BaseViewModel
    {
        public int Id { get; set; }

        //[RegularExpression(@"^[a-zA-Z0-9_ ]*$", ErrorMessage = "White space found in Name field.")]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }

    public class CATScoreViewModel : BaseViewModel
    {
        [Required]
        [Display(Name = "Text")]
        public string Text { get; set; }
    }
    public class InvestigationViewModel : BaseViewModel
    {
        [Required]
        [Display(Name = "Investigation Type")]
        public int InvestigationTypeId { get; set; }

        [Display(Name = "Investigation Type Name")]
        public string InvestigationTypeName { get; set; }
    }
}