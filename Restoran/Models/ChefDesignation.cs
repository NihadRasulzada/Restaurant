namespace Restoran.Models
{
    public class ChefDesignation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeactive { get; set; }
        public ICollection<Chef> Chefs { get; set; }
    }
}
