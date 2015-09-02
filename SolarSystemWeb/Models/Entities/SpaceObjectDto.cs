using DataLayer.Entities;

namespace SolarSystemWeb.Models.Entities
{
    public class SpaceObjectDto : SimpleModel
    {

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public string Description { get; set; }

        public double Mass { get; set; }

        public double Distance { get; set; }

        public double Radius { get; set; }

        public int OwnerId { get; set; }

        public bool IsSun => OwnerId == Id;
    }
}