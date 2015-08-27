using DataLayer;
using DataLayer.Repositories;
using SolarSystemWeb.Models.Entities;

namespace SolarSystemWeb.Models.Repositories
{
    public class SpaceObjectTypeRepository : AbstractRepository<SpaceObjectTypeDto, SpaceObjectType> 
    {
        protected override SpaceObjectType FromModelToDataConverter(SpaceObjectTypeDto model)
        {
            return model != null ? 
                new SpaceObjectType
                {
                   Id = model.Id,
                   Name = model.Name,
                   Plural = model.Plural
                } : null;
        }

        protected override SpaceObjectTypeDto FromDataToModelConverter(SpaceObjectType data)
        {
            return data != null ?  
                new SpaceObjectTypeDto
                {
                    Id = data.Id,
                    Name = data.Name,
                    Plural = data.Plural
                } : null;
        }
    }
}