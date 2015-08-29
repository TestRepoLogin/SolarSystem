using System.Collections.Generic;
using DataLayer.Entities;
using SolarSystemWeb.Models.Entities;

namespace SolarSystemWeb.Models.ViewModels
{
    public class DropDownViewModel
    {
        public DropDownViewModel(SpaceObjectTypeDto type, IEnumerable<SimpleModel> items)
        {
            Type = type;
            Items = items;
        }

        public IEnumerable<SimpleModel> Items { get; private set; }

        public SpaceObjectTypeDto Type { get; private set; }
    }
}