using Skopia.Api.Models;

namespace Skopia.Application.DTOs
{
    /// <summary>
    /// DTO para relatório de desempenho.
    /// </summary>
    public class RelatorioDesempenhoDto
    {
        public RelatorioDesempenhoDto(IEnumerable<Tarefa> tarefasConcluidas, double v)
        {
            TarefasConcluidas = tarefasConcluidas;
            V = v;
        }

        public double Desempenho { get; set; }
        public DateTime DataGeracao { get; set; }
        public IEnumerable<Tarefa> TarefasConcluidas { get; }
        public double V { get; }
    }
}