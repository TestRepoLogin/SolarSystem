using System.Collections.Generic;
using DataLayer.Entities;

namespace SolarSystemWeb.Models.ViewModels
{
    public class DropDownViewModel
    {
        public DropDownViewModel(string title, IEnumerable<SimpleModel> items)
        {
            Title = title;
            Items = items;
        }

        public IEnumerable<SimpleModel> Items { get; private set; }

        public string Title { get; private set; }
    }
}