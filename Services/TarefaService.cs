using Skopia.Api.Models;
using Skopia.Api.Models.Enums;
using Skopia.Api.Repositories;

namespace Skopia.Api.Services;

/// <summary>
/// Serviço com lógica de negócio para tarefas
/// </summary>
public class TarefaService : ITarefaService
{
    private readonly ITarefaRepository _tarefaRepository;
    private readonly IProjetoRepository _projetoRepository;
    private const int LIMITE_TAREFAS_POR_PROJETO = 20;

    public TarefaService(ITarefaRepository tarefaRepository, IProjetoRepository projetoRepository)
    {
        _tarefaRepository = tarefaRepository;
        _projetoRepository = projetoRepository;
    }

    public async Task<IEnumerable<Tarefa>> ObterTodosAsync()
    {
        return await _tarefaRepository.ObterTodosAsync();
    }

    public async Task<IEnumerable<Tarefa>> ObterPorProjetoAsync(string projetoId)
    {
        return await _tarefaRepository.ObterPorProjetoAsync(projetoId);
    }

    public async Task<Tarefa?> ObterPorIdAsync(string id)
    {
        return await _tarefaRepository.ObterPorIdAsync(id);
    }

    public async Task<Tarefa> CriarAsync(Tarefa tarefa)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(tarefa.Titulo))
            throw new ArgumentException("O título da tarefa é obrigatório.");

        if (tarefa.Titulo.Length > 100)
            throw new ArgumentException("O título da tarefa não pode ter mais de 100 caracteres.");

        if (!string.IsNullOrEmpty(tarefa.Descricao) && tarefa.Descricao.Length > 500)
            throw new ArgumentException("A descrição da tarefa não pode ter mais de 500 caracteres.");

        // Verifica se o projeto existe
        var projeto = await _projetoRepository.ObterPorIdAsync(tarefa.ProjetoId);
        if (projeto == null)
            throw new KeyNotFoundException("Projeto não encontrado.");

        // Verifica limite de tarefas por projeto
        var quantidadeTarefas = await _tarefaRepository.ContarPorProjetoAsync(tarefa.ProjetoId);
        if (quantidadeTarefas >= LIMITE_TAREFAS_POR_PROJETO)
            throw new InvalidOperationException($"O projeto já atingiu o limite de {LIMITE_TAREFAS_POR_PROJETO} tarefas.");

        return await _tarefaRepository.CriarAsync(tarefa);
    }

    public async Task<bool> AtualizarAsync(string id, Tarefa tarefa)
    {
        // Validações
        if (string.IsNullOrWhiteSpace(tarefa.Titulo))
            throw new ArgumentException("O título da tarefa é obrigatório.");

        if (tarefa.Titulo.Length > 100)
            throw new ArgumentException("O título da tarefa não pode ter mais de 100 caracteres.");

        if (!string.IsNullOrEmpty(tarefa.Descricao) && tarefa.Descricao.Length > 500)
            throw new ArgumentException("A descrição da tarefa não pode ter mais de 500 caracteres.");

        var tarefaExistente = await _tarefaRepository.ObterPorIdAsync(id);
        if (tarefaExistente == null)
            throw new KeyNotFoundException("Tarefa não encontrada.");

        return await _tarefaRepository.AtualizarAsync(id, tarefa);
    }

    public async Task<bool> DeletarAsync(string id)
    {
        var tarefaExistente = await _tarefaRepository.ObterPorIdAsync(id);
        if (tarefaExistente == null)
            throw new KeyNotFoundException("Tarefa não encontrada.");

        return await _tarefaRepository.DeletarAsync(id);
    }
}
