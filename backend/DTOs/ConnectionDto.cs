namespace backend.DTOs;

public class ConnectionDto
{
  public int Id { get; set; }
  public required string ConnectionType { get; set; }
  public List<WordDto> Words { get; set; } = [];
}
