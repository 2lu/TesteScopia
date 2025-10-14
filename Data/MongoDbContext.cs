using MongoDB.Driver;
using Skopia.Api.Models;
using Skopia.Api.Models.Enums;

namespace Skopia.Api.Data;

/// <summary>
/// Contexto do MongoDB para acesso às coleções
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var connectionString = configuration.GetValue<string>("MongoDB:ConnectionString");
        var databaseName = configuration.GetValue<string>("MongoDB:DatabaseName");

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Projeto> Projetos => _database.GetCollection<Projeto>("projeto");
    public IMongoCollection<Tarefa> Tarefas => _database.GetCollection<Tarefa>("tarefa");
}
