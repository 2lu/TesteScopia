using Skopia.Api.Models;
using Skopia.Api.Repositories;

namespace Skopia.Api.Services;

/// <summary>
/// Serviço com lógica de negócio para projetos
/// </summary>
public class ProjetoService : IProjetoService
{
    private readonly IProjetoRepository _projetoRepository;
    private readonly ITarefaRepository _tarefaRepository;

    public ProjetoService(IProjetoRepository projetoRepository, ITarefaRepository tarefaRepository)
    {
        _projetoRepository = projetoRepository;
        _tarefaRepository = tarefaRepository;
    }

    public async Task<IEnumerable<Projeto>> ObterTodosAsync()
    {
        return await _projetoRepository.ObterTodosAsync();
    }

    public async Task<Projeto?> ObterPorIdAsync(string id)
    {
        return await _projetoRepository.ObterPorIdAsync(id);
    }

    public async Task<Projeto> CriarAsync(Projeto projeto)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(projeto.Titulo))
            throw new ArgumentException("O título do projeto é obrigatório.");

        if (projeto.Titulo.Length > 100)
            throw new ArgumentException("O título do projeto não pode ter mais de 100 caracteres.");

        if (!string.IsNullOrEmpty(projeto.Descricao) && projeto.Descricao.Length > 500)
            throw new ArgumentException("A descrição do projeto não pode ter mais de 500 caracteres.");

        return await _projetoRepository.CriarAsync(projeto);
    }

    public async Task<bool> AtualizarAsync(string id, Projeto projeto)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(projeto.Titulo))
            throw new ArgumentException("O título do projeto é obrigatório.");

        if (projeto.Titulo.Length > 100)
            throw new ArgumentException("O título do projeto não pode ter mais de 100 caracteres.");

        if (!string.IsNullOrEmpty(projeto.Descricao) && projeto.Descricao.Length > 500)
            throw new ArgumentException("A descrição do projeto não pode ter mais de 500 caracteres.");

        var projetoExistente = await _projetoRepository.ObterPorIdAsync(id);
        if (projetoExistente == null)
            throw new KeyNotFoundException("Projeto não encontrado.");

        return await _projetoRepository.AtualizarAsync(id, projeto);
    }

    public async Task<bool> DeletarAsync(string id)
    {
        var projetoExistente = await _projetoRepository.ObterPorIdAsync(id);
        if (projetoExistente == null)
            throw new KeyNotFoundException("Projeto não encontrado.");

        // Verifica se há tarefas pendentes
        var tarefas = await _tarefaRepository.ObterPorProjetoAsync(id);
        var tarefasPendentes = tarefas.Where(t => t.Status != Models.Enums.StatusTarefa.Concluida);
        
        if (tarefasPendentes.Any())
            throw new InvalidOperationException("Não é possível remover um projeto com tarefas pendentes.");

        return await _projetoRepository.DeletarAsync(id);
    }
}
