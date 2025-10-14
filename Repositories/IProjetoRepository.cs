using Skopia.Api.Models;

namespace Skopia.Api.Repositories;

/// <summary>
/// Interface para repositório de projetos
/// </summary>
public interface IProjetoRepository
{
    Task<IEnumerable<Projeto>> ObterTodosAsync();
    Task<Projeto?> ObterPorIdAsync(string id);
    Task<Projeto> CriarAsync(Projeto projeto);
    Task<bool> AtualizarAsync(string id, Projeto projeto);
    Task<bool> DeletarAsync(string id);
}
