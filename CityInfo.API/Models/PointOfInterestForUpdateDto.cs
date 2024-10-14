using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models;

public class PointOfInterestForUpdateDto
{
    [Required(ErrorMessage = "You should provide a name value.")]
    [MaxLength(60)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(250)]
    public string? Description { get; set; }
}
