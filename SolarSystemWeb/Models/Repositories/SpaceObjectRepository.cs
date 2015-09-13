using System;
using System.Linq.Expressions;
using DataLayer;
using DataLayer.Repositories;
using SolarSystemWeb.Models.Entities;

namespace SolarSystemWeb.Models.Repositories
{
    public class SpaceObjectRepository : AbstractRepository<SpaceObjectDto, SpaceObject> 
    {
        protected override SpaceObject FromModelToDataConverter(SpaceObjectDto model)
        {
            return model != null ?
                new SpaceObject
                {
                     Id = model.Id,
                     Name = model.Name,
                     Description = model.Description,
                     Mass = model.Mass,
                     Radius = model.Radius,
                     Distance = model.Distance,
                     SpaceObjectTypeId = model.TypeId,
                     OwnerId = model.OwnerId,
                     WikiLink = model.WikiLink,  
                     MainImage = model.MainImage,
                     OrbitImage = model.OrbitImage,
                     OrbitPeriod = model.OrbitPeriod,
                     SiderealPeriod = model.SiderealPeriod
                } : null;
        }

        protected override SpaceObjectDto FromDataToModelConverter(SpaceObject data)
        {            
            return data != null ?  
                new SpaceObjectDto
                {
                    Id = data.Id,
                    Name = data.Name,
                    Description = data.Description,
                    Mass = data.Mass,
                    Radius = data.Radius,
                    Distance = data.Distance,
                    TypeId = data.SpaceObjectTypeId,
                    TypeName = data.SpaceObjectType.Name,
                    OwnerId = data.OwnerId,
                    WikiLink = data.WikiLink,
                    OwnerName = data.SpaceObject2.Name,
                    MainImage = data.MainImage,
                    OrbitImage = data.OrbitImage,
                    OrbitPeriod = data.OrbitPeriod,
                    SiderealPeriod = data.SiderealPeriod
                } : null;
        }        
    }
}