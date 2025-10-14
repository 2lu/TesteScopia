using Skopia.Application.DTOs;

namespace Skopia.Application.Interfaces;

/// <summary>
/// Interface para serviço de relatórios (ISP - Interface Segregation Principle)
/// </summary>
public interface IRelatorioService
{
    Task<RelatorioDesempenhoDto> ObterRelatorioDesempenhoAsync(string usuarioId);
}
