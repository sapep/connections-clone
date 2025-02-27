namespace backend.Entities;

public class ConnectionWord
{
  public int ConnectionId { get; set; }
  public required Connection Connection { get; set; }

  public int WordId { get; set; }
  public required Word Word { get; set; }
}
