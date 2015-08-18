using DataLayer;
using DataLayer.Repositories;
using SolarSystemWeb.Models.Entities;

namespace SolarSystemWeb.Models.Repositories
{
    public class SpaceObjectRepository : AbstractRepository<SpaceObjectDto, SpaceObject> 
    {
        protected override SpaceObject FromModelToDataConverter(SpaceObjectDto model)
        {
            return new SpaceObject
            {
                 Id = model.Id,
                 Name = model.Name,
                 Description = model.Description,
                 Mass = model.Mass,
                 Radius = model.Radius,
                 Distance = model.Distance
            };
        }

        protected override SpaceObjectDto FromDataToModelConverter(SpaceObject data)
        {
            return new SpaceObjectDto
            {
                Id = data.Id,
                Name = data.Name,
                Description = data.Description,
                Mass = data.Mass,
                Radius = data.Radius,
                Distance = data.Distance
            };
        }
    }
}