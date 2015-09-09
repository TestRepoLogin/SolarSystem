using System.ComponentModel.DataAnnotations;
using DataLayer.Entities;

namespace SolarSystemWeb.Models.Entities
{
    public class SpaceObjectTypeDto : SimpleModel
    {
        public SpaceObjectTypeDto ()
        {
            Plural = "";
        }

        [Required(ErrorMessage = "Нужно указать форму множественного числа")]
        public string Plural { get; set; }

        public bool IsSun => Plural == null;
    }
}