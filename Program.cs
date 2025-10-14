using Skopia.Api.Data;
using Skopia.Api.Models;
using Skopia.Api.Models.Enums;
using Skopia.Api.Repositories;
using Skopia.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Skopia API",
        Version = "v1",
        Description = "API para gerenciamento de projetos e tarefas"
    });
});

// Registro de dependências
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IProjetoRepository, ProjetoRepository>();
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<IProjetoService, ProjetoService>();
builder.Services.AddScoped<ITarefaService, TarefaService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configuração do pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// ==================== ENDPOINTS DE PROJETOS ====================

app.MapGet("/api/projetos", async (IProjetoService service) =>
{
    try
    {
        var projetos = await service.ObterTodosAsync();
        return Results.Ok(projetos);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("ObterTodosProjetos")
.WithTags("projeto");

app.MapGet("/api/projetos/{id}", async (string id, IProjetoService service) =>
{
    try
    {
        var projeto = await service.ObterPorIdAsync(id);
        return projeto != null ? Results.Ok(projeto) : Results.NotFound("Projeto não encontrado.");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("ObterProjetoPorId")
.WithTags("projeto");

app.MapPost("/api/projetos", async (Projeto projeto, IProjetoService service) =>
{
    try
    {
        var novoProjeto = await service.CriarAsync(projeto);
        return Results.Created($"/api/projetos/{novoProjeto.Id}", novoProjeto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("CriarProjeto")
.WithTags("projeto");

app.MapPut("/api/projetos/{id}", async (string id, Projeto projeto, IProjetoService service) =>
{
    try
    {
        var atualizado = await service.AtualizarAsync(id, projeto);
        return atualizado ? Results.Ok("Projeto atualizado com sucesso.") : Results.NotFound("Projeto não encontrado.");
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("AtualizarProjeto")
.WithTags("projeto");

app.MapDelete("/api/projetos/{id}", async (string id, IProjetoService service) =>
{
    try
    {
        var deletado = await service.DeletarAsync(id);
        return deletado ? Results.Ok("Projeto deletado com sucesso.") : Results.NotFound("Projeto não encontrado.");
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("DeletarProjeto")
.WithTags("projeto");

// ==================== ENDPOINTS DE TAREFAS ====================

app.MapGet("/api/tarefas", async (ITarefaService service) =>
{
    try
    {
        var tarefas = await service.ObterTodosAsync();
        return Results.Ok(tarefas);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("ObterTodasTarefas")
.WithTags("tarefa");

app.MapGet("/api/tarefas/{id}", async (string id, ITarefaService service) =>
{
    try
    {
        var tarefa = await service.ObterPorIdAsync(id);
        return tarefa != null ? Results.Ok(tarefa) : Results.NotFound("Tarefa não encontrada.");
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("ObterTarefaPorId")
.WithTags("tarefa");

app.MapGet("/api/projetos/{projetoId}/tarefas", async (string projetoId, ITarefaService service) =>
{
    try
    {
        var tarefas = await service.ObterPorProjetoAsync(projetoId);
        return Results.Ok(tarefas);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("ObterTarefasPorProjeto")
.WithTags("tarefa");

app.MapPost("/api/tarefas", async (Tarefa tarefa, ITarefaService service) =>
{
    try
    {
        var novaTarefa = await service.CriarAsync(tarefa);
        return Results.Created($"/api/tarefas/{novaTarefa.Id}", novaTarefa);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (InvalidOperationException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("CriarTarefa")
.WithTags("tarefa");

app.MapPut("/api/tarefas/{id}", async (string id, Tarefa tarefa, ITarefaService service) =>
{
    try
    {
        var atualizado = await service.AtualizarAsync(id, tarefa);
        return atualizado ? Results.Ok("Tarefa atualizada com sucesso.") : Results.NotFound("Tarefa não encontrada.");
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("AtualizarTarefa")
.WithTags("tarefa");

app.MapDelete("/api/tarefas/{id}", async (string id, ITarefaService service) =>
{
    try
    {
        var deletado = await service.DeletarAsync(id);
        return deletado ? Results.Ok("Tarefa deletada com sucesso.") : Results.NotFound("Tarefa não encontrada.");
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("DeletarTarefa")
.WithTags("tarefa");

app.Run();
