using Skopia.Api.Models;

namespace Skopia.Api.Repositories;

/// <summary>
/// Interface para repositório de tarefas
/// </summary>
public interface ITarefaRepository
{
    Task<IEnumerable<Tarefa>> ObterTodosAsync();
    Task<IEnumerable<Tarefa>> ObterPorProjetoAsync(string projetoId);
    Task<Tarefa?> ObterPorIdAsync(string id);
    Task<Tarefa> CriarAsync(Tarefa tarefa);
    Task<bool> AtualizarAsync(string id, Tarefa tarefa);
    Task<bool> DeletarAsync(string id);
    Task<int> ContarPorProjetoAsync(string projetoId);
}
