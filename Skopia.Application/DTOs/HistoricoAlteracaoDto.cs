namespace Skopia.Application.DTOs;

/// <summary>
/// DTO para histórico de alterações
/// </summary>
public record HistoricoAlteracaoDto(
    DateTime DataModificacao,
    string UsuarioId,
    string CampoModificado,
    string ValorAnterior,
    string ValorNovo,
    string Descricao
);
