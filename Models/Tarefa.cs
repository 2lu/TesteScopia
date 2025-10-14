using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Skopia.Api.Models.Enums;

namespace Skopia.Api.Models;

/// <summary>
/// Representa uma tarefa vinculada a um projeto
/// </summary>
public class Tarefa
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("titulo")]
    [BsonRequired]
    public string Titulo { get; set; } = string.Empty;

    [BsonElement("descricao")]
    public string? Descricao { get; set; }

    [BsonElement("dataVencimento")]
    public DateTime DataVencimento { get; set; }

    [BsonElement("status")]
    [BsonRepresentation(BsonType.Int32)]
    public StatusTarefa Status { get; set; } = StatusTarefa.Pendente;

    [BsonElement("prioridade")]
    [BsonRepresentation(BsonType.Int32)]
    public PrioridadeTarefa Prioridade { get; set; } = PrioridadeTarefa.Media;

    [BsonElement("projetoId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ProjetoId { get; set; } = string.Empty;

    [BsonElement("dataCriacao")]
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    [BsonElement("dataAtualizacao")]
    public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
}
