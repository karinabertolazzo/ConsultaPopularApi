using System.Text.Json.Serialization;

namespace ConsultaPopularApi.Models
{
    public class ClinicaEspecialidade
    {
        public int Id { get; set; }

        // Relacionamento com Clínica
        public int ClinicaId { get; set; }
    
        public Clinica? Clinica { get; set; }

             
        [JsonIgnore]
        public List<Consulta>? Consultas { get; set; }  
        public int EspecialidadeId { get; set; } 
        public Especialidade? Especialidade { get; set; } 
    }
}
