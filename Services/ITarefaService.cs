using Skopia.Api.Models;

namespace Skopia.Api.Services;

/// <summary>
/// Interface para serviço de tarefas
/// </summary>
public interface ITarefaService
{
    Task<IEnumerable<Tarefa>> ObterTodosAsync();
    Task<IEnumerable<Tarefa>> ObterPorProjetoAsync(string projetoId);
    Task<Tarefa?> ObterPorIdAsync(string id);
    Task<Tarefa> CriarAsync(Tarefa tarefa);
    Task<bool> AtualizarAsync(string id, Tarefa tarefa);
    Task<bool> DeletarAsync(string id);
}
