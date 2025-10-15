using Xunit;
using Moq;
using Skopia.Api.Models;
using Skopia.Api.Models.Enums;
using Skopia.Api.Repositories;
using Skopia.Api.Services;  

namespace Skopia.Api.Teste;

public class ProjetoServiceTestes
{
    private readonly Mock<IProjetoRepository> _projetoRepository;
    private readonly Mock<ITarefaRepository> _tarefaRepository;
    private readonly ProjetoService _sut; // System Under Test

    public ProjetoServiceTestes()
    {
        _projetoRepository = new Mock<IProjetoRepository>();
        _tarefaRepository = new Mock<ITarefaRepository>();
        _sut = new ProjetoService(_projetoRepository.Object, _tarefaRepository.Object);
    }

    [Fact]
    public async Task CriarAsync_ComTituloVazio_DeveLancarArgumentException()
    {
        // Arrange
        var projeto = new Projeto { Titulo = "" };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _sut.CriarAsync(projeto));
        Assert.Equal("O título do projeto é obrigatório.", exception.Message);
    }

    [Fact]
    public async Task CriarAsync_ComDadosValidos_DeveChamarRepositorio()
    {
        // Arrange
        var projeto = new Projeto { Titulo = "Novo Projeto", Descricao = "Descrição" };
        _projetoRepository.Setup(r => r.CriarAsync(projeto)).ReturnsAsync(projeto);

        // Act
        var resultado = await _sut.CriarAsync(projeto);

        // Assert
        Assert.NotNull(resultado);
        _projetoRepository.Verify(r => r.CriarAsync(projeto), Times.Once);
    }

    [Fact]
    public async Task DeletarAsync_ComTarefasPendentes_DeveLancarInvalidOperationException()
    {
        // Arrange
        var projetoId = "projeto123";
        var projetoExistente = new Projeto { Id = projetoId, Titulo = "Projeto com Tarefas" };
        var tarefas = new List<Tarefa>
        {
            new() { Status = StatusTarefa.Pendente },
            new() { Status = StatusTarefa.Concluida }
        };

        _projetoRepository.Setup(r => r.ObterPorIdAsync(projetoId)).ReturnsAsync(projetoExistente);
        _tarefaRepository.Setup(r => r.ObterPorProjetoAsync(projetoId)).ReturnsAsync(tarefas);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.DeletarAsync(projetoId));
        Assert.Equal("Não é possível remover um projeto com tarefas pendentes.", exception.Message);
    }

    [Fact]
    public async Task DeletarAsync_SemTarefasPendentes_DeveChamarRepositorio()
    {
        // Arrange
        var projetoId = "projeto123";
        _projetoRepository.Setup(r => r.ObterPorIdAsync(projetoId)).ReturnsAsync(new Projeto { Id = projetoId });
        _tarefaRepository.Setup(r => r.ObterPorProjetoAsync(projetoId)).ReturnsAsync(new List<Tarefa>());

        // Act
        await _sut.DeletarAsync(projetoId);

        // Assert
        _projetoRepository.Verify(r => r.DeletarAsync(projetoId), Times.Once);
    }
}