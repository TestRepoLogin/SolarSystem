using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities
{
    public class SimpleModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Нужно указать название")]
        public string Name { get; set; }
    }
}
