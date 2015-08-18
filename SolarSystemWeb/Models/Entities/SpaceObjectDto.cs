using DataLayer.Entities;

namespace SolarSystemWeb.Models.Entities
{
    public class SpaceObjectDto : SimpleModel
    {
        public int SpaceObjectTypeId { get; set; }

        public string Description { get; set; }

        public double Mass { get; set; }

        public double Distance { get; set; }

        public double Radius { get; set; }

        //public virtual SpaceObjectType SpaceObjectType { get; set; }
    }
}