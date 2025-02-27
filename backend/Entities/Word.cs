using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Entities;

[Index(nameof(Value), IsUnique = true)]
public class Word
{
  public int Id { get; set; }

  [Required]
  [MaxLength(64)]
  public required string Value { get; set; }
  public required List<ConnectionWord> ConnectionWords { get; set; } = [];
}
