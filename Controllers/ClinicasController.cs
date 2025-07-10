using ConsultaPopularApi.Data;
using ConsultaPopularApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ConsultaPopularApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ClinicasController> _logger; 


        public ClinicasController(AppDbContext context, ILogger<ClinicasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/clinicas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clinica>>> GetClinicas()
        {
            return await _context.Clinicas.Include(c => c.ClinicaEspecialidades)
                 .ThenInclude(ce => ce.Especialidade)
        .ToListAsync();
        }

        // GET: api/clinicas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Clinica>> GetClinica(int id)
        {
            var clinica = await _context.Clinicas.Include(c => c.ClinicaEspecialidades)
                  .ThenInclude(ce => ce.Especialidade)
        .FirstOrDefaultAsync(c => c.Id == id);

            if (clinica == null)
                return NotFound();

            return clinica;
        }

        // POST: api/clinicas
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Clinica>>> PostClinica(Clinica clinica)
        {
            // Validações iniciais
            var validationResult = ValidateClinica(clinica);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning("Validação falhou: {@erros}", validationResult.Errors);

                return BadRequest(new ApiResponse<Clinica>
                {
                    Success = false,
                    Message = "Erro de validação",
                    Errors = validationResult.Errors
                });
            }

            try
            {
                if (clinica.ClinicaEspecialidades != null)
                {
                    foreach (var ce in clinica.ClinicaEspecialidades)
                    {
                        ce.Clinica = null;
                        ce.Especialidade = null;
                    }
                }

                _context.Clinicas.Add(clinica);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetClinica), new { id = clinica.Id },
                    new ApiResponse<Clinica>
                    {
                        Success = true,
                        Message = "Clínica cadastrada com sucesso!",
                        Data = clinica
                    });
            }
            catch (DbUpdateException ex)
            {
                // Tratamento específico para erros de banco de dados
                string errorMessage = "Erro ao salvar os dados.";
                string detalhesErro = ex.InnerException?.Message ?? ex.Message;

                Console.WriteLine("⚠️ Erro ao salvar clínica:");
                Console.WriteLine($"Mensagem: {ex.Message}");
                Console.WriteLine($"InnerException: {ex.InnerException?.ToString()}");

                if (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2627)
                {
                    errorMessage = "Já existe uma clínica com este CNPJ ou email cadastrado.";
                }

                return BadRequest(new ApiResponse<Clinica>
                {
                    Success = false,
                    Message = errorMessage,
                    ErrorDetails = "DatabaseError" 
                });
            }
            catch (Exception ex)
            {
                _logger.LogInformation("📥 Dados recebidos no PostClinica:");
                _logger.LogInformation(JsonSerializer.Serialize(clinica));
                _logger.LogError(ex, "Erro ao cadastrar clínica");

                return StatusCode(500, new ApiResponse<Clinica>
                {
                    Success = false,
                    Message = "Ocorreu um erro inesperado. Por favor, tente novamente mais tarde.",
                    ErrorDetails = "InternalServerError"
                });
            }
        }


        private ValidationResult ValidateClinica(Clinica clinica)
        {
            var errors = new List<string>();

            if (!string.IsNullOrEmpty(clinica.Telefone))
            {
                var telefoneRegex = new Regex(@"^\(?\d{2}\)?[\s-]?\d{4,5}-?\d{4}$");
                if (!telefoneRegex.IsMatch(clinica.Telefone))
                    errors.Add("Telefone inválido. Formato esperado: (00) 0000-0000 ou (00) 00000-0000");
            }

            if (!string.IsNullOrEmpty(clinica.Email))
            {
                var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                if (!emailRegex.IsMatch(clinica.Email))
                    errors.Add("Email inválido. Insira um email no formato: usuario@exemplo.com");
            }

            if (!string.IsNullOrEmpty(clinica.Cnpj))
            {
                var cnpjRegex = new Regex(@"^\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}$");
                if (!cnpjRegex.IsMatch(clinica.Cnpj))
                    errors.Add("CNPJ inválido. Formato esperado: 00.000.000/0000-00");
            }

            return new ValidationResult
            {
                IsValid = errors.Count == 0,
                Errors = errors
            };
        }


        // PUT: api/clinicas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClinica(int id, Clinica clinica)
        {
            if (id != clinica.Id)
                return BadRequest();

            var clinicaDb = await _context.Clinicas
                .Include(c => c.ClinicaEspecialidades)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (clinicaDb == null)
                return NotFound();

            // Atualiza campos simples
            clinicaDb.Nome = clinica.Nome;
            clinicaDb.Telefone = clinica.Telefone;
            clinicaDb.Email = clinica.Email;
            clinicaDb.Cnpj = clinica.Cnpj;
            clinicaDb.HorarioFuncionamento = clinica.HorarioFuncionamento;

            // Atualiza endereço
            clinicaDb.Endereco.Rua = clinica.Endereco.Rua;
            clinicaDb.Endereco.Numero = clinica.Endereco.Numero;
            clinicaDb.Endereco.Complemento = clinica.Endereco.Complemento;
            clinicaDb.Endereco.Bairro = clinica.Endereco.Bairro;
            clinicaDb.Endereco.Cidade = clinica.Endereco.Cidade;
            clinicaDb.Endereco.Estado = clinica.Endereco.Estado;
            clinicaDb.Endereco.Cep = clinica.Endereco.Cep;

            // Atualiza especialidades

            // Remove as antigas
            _context.ClinicaEspecialidades.RemoveRange(clinicaDb.ClinicaEspecialidades);

            // Adiciona as novas, criando objetos com ClinicaId e EspecialidadeId
            clinicaDb.ClinicaEspecialidades = clinica.ClinicaEspecialidades
                .Select(ce => new ClinicaEspecialidade
                {
                    ClinicaId = clinicaDb.Id,
                    EspecialidadeId = ce.EspecialidadeId
                }).ToList();

            await _context.SaveChangesAsync();

            return Ok(clinicaDb);
        }


        // DELETE: api/clinicas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClinica(int id)
        {
            try
            {
                // Verifica se a clínica existe e inclui os relacionamentos que precisam ser tratados
                var clinica = await _context.Clinicas
                    .Include(c => c.ClinicaEspecialidades) 
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (clinica == null)
                {
                    return NotFound(new { message = "Clínica não encontrada" });
                }

                // Remove os relacionamentos primeiro, se existirem
                if (clinica.ClinicaEspecialidades != null && clinica.ClinicaEspecialidades.Any())
                {
                    _context.ClinicaEspecialidades.RemoveRange(clinica.ClinicaEspecialidades);
                }

                // Remove a clínica
                _context.Clinicas.Remove(clinica);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateException ex)
            {
              
                return StatusCode(500, new { message = "Não foi possível excluir a clínica devido a restrições no banco de dados", details = ex.InnerException?.Message });
            }
            catch (Exception ex)
            {
                // Outros erros
                return StatusCode(500, new { message = "Erro interno ao excluir clínica", details = ex.Message });
            }
        }
    }
}
