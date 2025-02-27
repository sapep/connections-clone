using System.Text.Json;
using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class Seed
{
  public record SeedWord(string Value);
  public record SeedConnection(string ConnectionType, List<SeedWord> Words);

  public static async Task SeedConnections(
    IWebHostEnvironment environment,
    AppDbContext context
  )
  {
    // Seeding only allowed in development
    if (!environment.IsDevelopment()) return;

    var connectionData = await File.ReadAllTextAsync("Data/ConnectionsSeedData.json");

    var jsonOptions = new JsonSerializerOptions
    {
      PropertyNameCaseInsensitive = true
    };

    var seedConnections = JsonSerializer.Deserialize<List<SeedConnection>>(connectionData, jsonOptions);

    if (seedConnections == null) return;

    // Load connections including their ConnectionWords
    var existingConnections = await context.Connections
      .Include(c => c.ConnectionWords)
        .ThenInclude(cw => cw.Word)
      .ToDictionaryAsync(c => c.ConnectionType, c => c);

    var existingWords = await context.Words.ToDictionaryAsync(w => w.Value, w => w);

    foreach (var seedConnection in seedConnections)
    {
      if (!existingConnections.TryGetValue(seedConnection.ConnectionType, out var connectionEntity))
      {
        connectionEntity = new Connection
        {
          ConnectionType = seedConnection.ConnectionType,
          ConnectionWords = []
        };

        context.Connections.Add(connectionEntity);
        existingConnections.Add(connectionEntity.ConnectionType, connectionEntity);
      }

      // Filter duplicates using Distinct() (records are compared by value)
      var distinctSeedWords = seedConnection.Words.Distinct().ToList();

      foreach (var seedWord in distinctSeedWords)
      {
        if (!existingWords.TryGetValue(seedWord.Value, out var wordEntity))
        {
          wordEntity = new Word
          {
            Value = seedWord.Value,
            ConnectionWords = []
          };

          context.Words.Add(wordEntity);
          existingWords.Add(wordEntity.Value, wordEntity);
        }

        // Check if the connection already contains this word
        bool alreadyAdded = connectionEntity.ConnectionWords
          .Any(cw => cw.Word != null && cw.Word.Value == seedWord.Value);

        if (!alreadyAdded)
        {
          var connectionWord = new ConnectionWord
          {
            Connection = connectionEntity,
            Word = wordEntity
          };

          connectionEntity.ConnectionWords.Add(connectionWord);
          wordEntity.ConnectionWords.Add(connectionWord);
        }
      }
    }

    await context.SaveChangesAsync();
  }
}
