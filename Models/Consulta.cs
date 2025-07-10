namespace ConsultaPopularApi.Models
{
    public class Consulta
    {

        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public bool Agendada { get; set; }

        // Relacionamento com ClinicaEspecialidade
        public int? ClinicaEspecialidadeId { get; set; }
        public ClinicaEspecialidade? ClinicaEspecialidade { get; set; }

        public int? PacienteId { get; set; }  
        public Paciente? Paciente { get; set; }
    }
}
