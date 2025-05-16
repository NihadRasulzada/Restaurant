namespace Restoran.Models
{
    public class Chef
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string FullName { get; set; }
        public ChefDesignation? ChefDesignation { get; set; }
        public int ChefDesignationId { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
        public bool IsDeactive { get; set; }
    }
}
