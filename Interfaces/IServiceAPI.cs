using BlazorApp.Models;

namespace BlazorApp.Interfaces
{
    public interface IServiceAPI
    {
        Task<List<Paciente>?> GetPacienteList();
        Task<Paciente?> GetPaciente(int? id);
        Task<bool> CreatePaciente(Paciente paciente);
        Task<bool> UpdatePaciente(Paciente paciente);
        Task<bool> DeletePaciente(int id);
    }
}