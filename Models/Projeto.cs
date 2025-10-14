using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Skopia.Api.Models;

/// <summary>
/// Representa um projeto no sistema
/// </summary>
public class Projeto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("titulo")]
    [BsonRequired]
    public string Titulo { get; set; } = string.Empty;

    [BsonElement("descricao")]
    public string? Descricao { get; set; }

    [BsonElement("dataCriacao")]
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    [BsonElement("dataAtualizacao")]
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
}
