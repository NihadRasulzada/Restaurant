using Restoran.DTOs.MemberDesignationDtos;

namespace Restoran.DTOs.MemberDtos
{
    public class ChefDto
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public string FullName { get; set; }
        public string DesignationName { get; set; }
        public bool IsDeactive { get; set; }
    }
}
