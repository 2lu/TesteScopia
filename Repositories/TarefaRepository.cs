using MongoDB.Driver;
using Skopia.Api.Data;
using Skopia.Api.Models;

namespace Skopia.Api.Repositories;

/// <summary>
/// Repositório para operações de tarefas no MongoDB
/// </summary>
public class TarefaRepository : ITarefaRepository
{
    private readonly IMongoCollection<Tarefa> _tarefas;

    public TarefaRepository(MongoDbContext context)
    {
        _tarefas = context.Tarefas;
    }

    public async Task<IEnumerable<Tarefa>> ObterTodosAsync()
    {
        return await _tarefas.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<Tarefa>> ObterPorProjetoAsync(string projetoId)
    {
        return await _tarefas.Find(t => t.ProjetoId == projetoId).ToListAsync();
    }

    public async Task<Tarefa?> ObterPorIdAsync(string id)
    {
        return await _tarefas.Find(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Tarefa> CriarAsync(Tarefa tarefa)
    {
        await _tarefas.InsertOneAsync(tarefa);
        return tarefa;
    }

    public async Task<bool> AtualizarAsync(string id, Tarefa tarefa)
    {
        tarefa.DataAtualizacao = DateTime.UtcNow;
        var result = await _tarefas.ReplaceOneAsync(t => t.Id == id, tarefa);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeletarAsync(string id)
    {
        var result = await _tarefas.DeleteOneAsync(t => t.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<int> ContarPorProjetoAsync(string projetoId)
    {
        return (int)await _tarefas.CountDocumentsAsync(t => t.ProjetoId == projetoId);
    }
}
