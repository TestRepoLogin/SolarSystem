using DataLayer.Entities;

namespace SolarSystemWeb.Models.Entities
{
    public class SpaceObjectTypeDto : SimpleModel
    {
        public string Plural { get; set; }

        public bool IsSun => Plural == null;
    }
}