using Skopia.Api.Models;

namespace Skopia.Api.Services;

/// <summary>
/// Interface para serviço de projetos
/// </summary>
public interface IProjetoService
{
    Task<IEnumerable<Projeto>> ObterTodosAsync();
    Task<Projeto?> ObterPorIdAsync(string id);
    Task<Projeto> CriarAsync(Projeto projeto);
    Task<bool> AtualizarAsync(string id, Projeto projeto);
    Task<bool> DeletarAsync(string id);
}
