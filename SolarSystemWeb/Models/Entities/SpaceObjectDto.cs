using System;
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
        
        [Range(1, long.MaxValue, ErrorMessage = "Значение должно быть больше нуля")]
        public long? OrbitPeriod { get; set; }

        [Required(ErrorMessage = "Нужно указать продолжительность суток")]
        [Range(1, long.MaxValue, ErrorMessage = "Значение должно быть больше нуля")]
        public long SiderealPeriod { get; set; }

        [Required(ErrorMessage = "Нужно указать, чей это спутник")]
        public int OwnerId { get; set; }

        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Нужно указать ссылку на дополнительную информацию")]
        public string WikiLink { get; set; }

        public byte[] MainImage { get; set; }

        public byte[] OrbitImage { get; set; }

        public bool IsSun => Id > 0 && OwnerId == Id;

        public TimeSpan OrbitPeriodSpan => TimeSpan.FromSeconds(OrbitPeriod ?? 0);

        public TimeSpan SiderealPeriodSpan => TimeSpan.FromSeconds(SiderealPeriod);        
    }    
}