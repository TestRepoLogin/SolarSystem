using DataLayer;
using DataLayer.Repositories;
using SolarSystemWeb.Models.Entities;

namespace SolarSystemWeb.Models.Repositories
{
    public class SpaceObjectTypeRepository : AbstractRepository<SpaceObjectTypeDto, SpaceObjectType> 
    {
        protected override SpaceObjectType FromModelToDataConverter(SpaceObjectTypeDto model)
        {
            return new SpaceObjectType
            {
               
            };
        }

        protected override SpaceObjectTypeDto FromDataToModelConverter(SpaceObjectType data)
        {
            return new SpaceObjectTypeDto();
        }
    }
}