using Skopia.Api.Repositories;
using Skopia.Application.DTOs;
using Skopia.Application.Interfaces;

namespace Skopia.Application.Services;

public class RelatorioService : IRelatorioService
{
    private readonly ITarefaRepository _tarefaRepository;
 
    public RelatorioService(ITarefaRepository tarefaRepository)
    {
        _tarefaRepository = tarefaRepository;
    }

    public async Task<RelatorioDesempenhoDto> ObterRelatorioDesempenhoAsync(string id)
    {
        var tarefasConcluidas = await _tarefaRepository.ObterPorProjetoAsync(id);
        var mediaPorDia = tarefasConcluidas.Count() / 30.0;

        return new RelatorioDesempenhoDto(
            tarefasConcluidas,
            Math.Round(mediaPorDia, 2)
        );
    }
}
