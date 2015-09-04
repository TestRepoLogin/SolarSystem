using System.ComponentModel.DataAnnotations;
using DataLayer.Entities;

namespace SolarSystemWeb.Models.Entities
{
    public class SpaceObjectDto : SimpleModel
    {
        [Required(ErrorMessage = "Нужно указать тип")]
        public int TypeId { get; set; }

        public string TypeName { get; set; }

        [Required(ErrorMessage = "Нужно указать описание")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Нужно указать массу")]
        [Range(1, double.MaxValue, ErrorMessage = "Значение должно быть больше нуля")]
        public double Mass { get; set; }

        [Required(ErrorMessage = "Нужно указать радиус орбиты")]
        [Range(1, double.MaxValue, ErrorMessage = "Значение должно быть больше нуля")]
        public double Distance { get; set; }

        [Required(ErrorMessage = "Нужно указать радиус")]
        [Range(1, double.MaxValue, ErrorMessage = "Значение должно быть больше нуля")]
        public double Radius { get; set; }

        [Required(ErrorMessage = "Нужно указать, чей это спутник")]
        public int OwnerId { get; set; }

        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Нужно указать ссылку на дополнительную информацию")]
        public string WikiLink { get; set; }

        public bool IsSun => Id > 0 && OwnerId == Id;
    }
}