using MongoDB.Driver;
using Skopia.Api.Data;
using Skopia.Api.Models;
using Skopia.Api.Models.Enums;

namespace Skopia.Api.Repositories;

/// <summary>
/// Repositório para operações de projetos no MongoDB
/// </summary>
public class ProjetoRepository : IProjetoRepository
{
    private readonly IMongoCollection<Projeto> _projetos;

    public ProjetoRepository(MongoDbContext context)
    {
        _projetos = context.Projetos;
    }

    public async Task<IEnumerable<Projeto>> ObterTodosAsync()
    {
        return await _projetos.Find(_ => true).ToListAsync();
    }

    public async Task<Projeto?> ObterPorIdAsync(string id)
    {
        return await _projetos.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Projeto> CriarAsync(Projeto projeto)
    {
        await _projetos.InsertOneAsync(projeto);
        return projeto;
    }

    public async Task<bool> AtualizarAsync(string id, Projeto projeto)
    {
        projeto.DataAtualizacao = DateTime.UtcNow;
        var result = await _projetos.ReplaceOneAsync(p => p.Id == id, projeto);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeletarAsync(string id)
    {
        var result = await _projetos.DeleteOneAsync(p => p.Id == id);
        return result.DeletedCount > 0;
    }
}
