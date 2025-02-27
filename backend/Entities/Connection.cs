using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Entities;

[Index(nameof(ConnectionType), IsUnique = true)]
public class Connection
{
  public int Id { get; set; }
  public required List<ConnectionWord> ConnectionWords { get; set; } = [];

  [Required]
  [MaxLength(64)]
  public required string ConnectionType { get; set; }
}
