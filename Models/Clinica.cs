using System.Text.Json.Serialization;

namespace ConsultaPopularApi.Models
{
    public class Endereco
    {
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }

    }

    public class Clinica
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public Endereco Endereco { get; set; }  
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cnpj { get; set; }
        public string HorarioFuncionamento { get; set; }

        public List<ClinicaEspecialidade>? ClinicaEspecialidades { get; set; }
    }
}
