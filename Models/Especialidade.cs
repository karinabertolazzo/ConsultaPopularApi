using System.Text.Json.Serialization;

namespace ConsultaPopularApi.Models
{
    public class Especialidade
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;


        [JsonIgnore] 
        public ICollection<ClinicaEspecialidade> ClinicaEspecialidades { get; set; } = new List<ClinicaEspecialidade>();
    }
}
