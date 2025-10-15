using Xunit;
using Moq;
using Skopia.Api.Models;
using Skopia.Api.Repositories;
using Skopia.Api.Services;

namespace Skopia.Api.Teste;

public class TarefaServiceTestes
{
    private readonly Mock<ITarefaRepository> _tarefaRepository;
    private readonly Mock<IProjetoRepository> _projetoRepository;
    private readonly TarefaService _sut;

    public TarefaServiceTestes()
    {
        _tarefaRepository = new Mock<ITarefaRepository>();
        _projetoRepository = new Mock<IProjetoRepository>();
        _sut = new TarefaService(_tarefaRepository.Object, _projetoRepository.Object);
    }

    [Fact]
    public async Task CriarAsync_QuandoProjetoNaoExiste_DeveLancarKeyNotFoundException()
    {
        // Arrange
        var tarefa = new Tarefa { Titulo = "Nova Tarefa", ProjetoId = "id_inexistente" };
        _projetoRepository.Setup(r => r.ObterPorIdAsync(tarefa.ProjetoId)).ReturnsAsync((Projeto?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.CriarAsync(tarefa));
        Assert.Equal("Projeto não encontrado.", exception.Message);
    }

    [Fact]
    public async Task CriarAsync_QuandoLimiteDeTarefasExcedido_DeveLancarInvalidOperationException()
    {
        // Arrange
        var projetoId = "projeto123";
        var tarefa = new Tarefa { Titulo = "Tarefa 21", ProjetoId = projetoId };
        var projeto = new Projeto { Id = projetoId, Titulo = "Projeto Cheio" };

        _projetoRepository.Setup(r => r.ObterPorIdAsync(projetoId)).ReturnsAsync(projeto);
        _tarefaRepository.Setup(r => r.ContarPorProjetoAsync(projetoId)).ReturnsAsync(20); // Limite é 20

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.CriarAsync(tarefa));
        Assert.Equal("O projeto já atingiu o limite de 20 tarefas.", exception.Message);
    }

    [Fact]
    public async Task CriarAsync_ComDadosValidos_DeveCriarTarefa()
    {
        // Arrange
        var projetoId = "projeto123";
        var tarefa = new Tarefa { Titulo = "Nova Tarefa", ProjetoId = projetoId };
        var projeto = new Projeto { Id = projetoId, Titulo = "Meu Projeto" };

        _projetoRepository.Setup(r => r.ObterPorIdAsync(projetoId)).ReturnsAsync(projeto);
        _tarefaRepository.Setup(r => r.ContarPorProjetoAsync(projetoId)).ReturnsAsync(5);
        _tarefaRepository.Setup(r => r.CriarAsync(tarefa)).ReturnsAsync(tarefa);

        // Act
        var resultado = await _sut.CriarAsync(tarefa);

        // Assert
        Assert.NotNull(resultado);
        _tarefaRepository.Verify(r => r.CriarAsync(tarefa), Times.Once);
    }
}