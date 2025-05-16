
namespace Restoran.DTOs.MemberDtos
{
    public class UpdateChefDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int ChefDesignationId { get; set; }
        public string TwitterUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string InstagramUrl { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
