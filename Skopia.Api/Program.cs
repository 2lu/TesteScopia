using Skopia.Application.Interfaces;
using Skopia.Application.Services;
using Skopia.Infrastructure.Configuration;
using Skopia.Api.Repositories;
using Skopia.Api.Services;

namespace Skopia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar MongoDB
            var mongoDbSettings = builder.Configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>()
                ?? throw new InvalidOperationException("Configurações do MongoDB não encontradas.");

            builder.Services.AddSingleton(mongoDbSettings);


            builder.Services.AddScoped<IProjetoRepository, ProjetoRepository>();
            builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
            builder.Services.AddScoped<IProjetoService, ProjetoService>();
            builder.Services.AddScoped<ITarefaService, TarefaService>();
            builder.Services.AddScoped<IRelatorioService, RelatorioService>();

            // Adicionar Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new()
                {
                    Title = "Skopia API",
                    Version = "v1",
                    Description = "API projetos / tarefas"
                });
            });

            var app = builder.Build();

            // Configurar Swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Skopia API v1");
                    options.RoutePrefix = string.Empty; // Swagger na raiz
                });
            }

            app.UseHttpsRedirection();

            // Endpoints de Projetos
            app.MapGet("/api/projetos/", async (IProjetoService service) =>
            {
                var projetos = await service.ObterTodosAsync();
                return Results.Ok(projetos);
            })
            .WithName("ListarProjetosPorUsuario")
            .WithTags("projeto");

            app.MapGet("/api/projetos/{id}", async (string id, IProjetoService service) =>
            {
                var projeto = await service.ObterPorIdAsync(id);
                return projeto == null ? Results.NotFound("Projeto não encontrado.") : Results.Ok(projeto);
            })
            .WithName("ObterProjetoPorId")
            .WithTags("projeto");

            app.MapPost("/api/projetos", async (Models.Projeto dto, IProjetoService service) =>
            {
                try
                {
                    var projeto = await service.CriarAsync(dto);
                    return Results.Created($"/api/projetos/{projeto.Id}", projeto);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
            })
            .WithName("CriarProjeto")
            .WithTags("projeto");

            app.MapDelete("/api/projetos/{id}", async (string id, IProjetoService service) =>
            {
                try
                {
                    await service.DeletarAsync(id);
                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
            })
            .WithName("RemoverProjeto")
            .WithTags("projeto");

            // Endpoints de Tarefas
            app.MapGet("/api/tarefas/projeto/{projetoId}", async (string projetoId, ITarefaService service) =>
            {
                var tarefas = await service.ObterPorProjetoAsync(projetoId);
                return Results.Ok(tarefas);
            })
            .WithName("ListarTarefasPorProjeto")
            .WithTags("tarefa");

            app.MapGet("/api/tarefas/{id}", async (string id, ITarefaService service) =>
            {
                var tarefa = await service.ObterPorIdAsync(id);
                return tarefa == null ? Results.NotFound("Tarefa não encontrada.") : Results.Ok(tarefa);
            })
            .WithName("ObterTarefaPorId")
            .WithTags("tarefa");

            app.MapPost("/api/tarefas", async (Models.Tarefa dto, ITarefaService service) =>
            {
                try
                {
                    var tarefa = await service.CriarAsync(dto);
                    return Results.Created($"/api/tarefas/{tarefa.Id}", tarefa);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
            })
            .WithName("CriarTarefa")
            .WithTags("tarefa");

            app.MapPut("/api/tarefas/{id}", async (string id,Models.Tarefa dto, ITarefaService service) =>
            {
                try
                {
                    var tarefa = await service.AtualizarAsync(id, dto);
                    return Results.Ok(tarefa);
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { erro = ex.Message });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
            })
            .WithName("AtualizarTarefa")
            .WithTags("tarefa");

            app.MapDelete("/api/tarefas/{id}", async (string id, ITarefaService service) =>
            {
                try
                {
                    await service.DeletarAsync(id);
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
            })
            .WithName("RemoverTarefa")
            .WithTags("tarefa");

            // Endpoints de Relatórios
            app.MapGet("/api/relatorios/desempenho/{usuarioId}", async (string usuarioId, IRelatorioService service) =>
            {
                try
                {
                    var relatorio = await service.ObterRelatorioDesempenhoAsync(usuarioId);
                    return Results.Ok(relatorio);
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Results.Unauthorized();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
            })
            .WithName("ObterRelatorioDesempenho")
            .WithTags("Relatórios");

            app.Run();
        }
    }
}
