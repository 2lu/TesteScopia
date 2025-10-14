namespace Skopia.Infrastructure.Configuration;

/// <summary>
/// Configurações do MongoDB
/// </summary>
public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string ProjetosCollection { get; set; } = "projeto";
    public string TarefasCollection { get; set; } = "tarefa";
}
