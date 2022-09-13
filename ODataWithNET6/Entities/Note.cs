using System.ComponentModel.DataAnnotations;

namespace ODataWithNET6.Entities
{
    public class Note
    {
        public Guid Id { get; set; }
        [Required]
        public string MessageNote { get; set; }
    }
}
