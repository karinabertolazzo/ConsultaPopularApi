using ConsultaPopularApi.Data;
using ConsultaPopularApi.DTOs;
using ConsultaPopularApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsultaPopularApi.Controllers
{
   
    
        [Route("api/[controller]")]
        [ApiController]
        public class ConsultasController : ControllerBase
        {
            private readonly AppDbContext _context;

            public ConsultasController(AppDbContext context)
            {
                _context = context;
            }

        // GET: api/Consultas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Consulta>>> GetConsultas()
        {
            var consultas = await _context.Consultas
                .Include(c => c.ClinicaEspecialidade)
                    .ThenInclude(ce => ce.Clinica)
                .Include(c => c.ClinicaEspecialidade)
                    .ThenInclude(ce => ce.Especialidade)
                .Include(c => c.Paciente)
                .ToListAsync();

            return consultas;
        }

        // GET: api/Consultas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Consulta>> GetConsulta(int id)
        {
            var consulta = await _context.Consultas
                .Include(c => c.ClinicaEspecialidade)
                    .ThenInclude(ce => ce.Clinica)
                .Include(c => c.ClinicaEspecialidade)
                    .ThenInclude(ce => ce.Especialidade)
                .Include(c => c.Paciente)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (consulta == null)
                return NotFound();

            return consulta;
        }

        // POST: api/Consultas
        // Cria uma vaga de consulta (não agendada ainda)
        [HttpPost]
        public async Task<ActionResult<Consulta>> PostConsulta(ConsultaCreateDTO dto)
        {
            var novaConsulta = new Consulta
            {
                DataHora = dto.DataHora,
                ClinicaEspecialidadeId = dto.ClinicaEspecialidadeId,
                Agendada = false
            };

            _context.Consultas.Add(novaConsulta);
            await _context.SaveChangesAsync();

            // Recarrega a consulta com os relacionamentos desejados
            var consultaComRelacionamentos = await _context.Consultas
                .Include(c => c.ClinicaEspecialidade)
                    .ThenInclude(ce => ce.Clinica)
                    .Include(c => c.ClinicaEspecialidade)
                    .ThenInclude(ce => ce.Especialidade)
                       .Include(c => c.Paciente)
                .FirstOrDefaultAsync(c => c.Id == novaConsulta.Id);

            return CreatedAtAction(nameof(GetConsulta), new { id = novaConsulta.Id }, consultaComRelacionamentos);
        }


        // PUT: api/Consultas/Agendar/5
        // Agendar uma consulta, associando um paciente
        [HttpPut("Agendar/{id}")]
            public async Task<IActionResult> AgendarConsulta(int id, [FromBody] int pacienteId)
            {
                var consulta = await _context.Consultas.FindAsync(id);
                if (consulta == null)
                    return NotFound();

                if (consulta.Agendada)
                    return BadRequest("Consulta já está agendada.");

                consulta.PacienteId = pacienteId;
                consulta.Agendada = true;

                await _context.SaveChangesAsync();
                return NoContent();
            }

            // PUT: api/Consultas/Desmarcar/5
            // Desmarcar uma consulta, removendo paciente
            [HttpPut("Desmarcar/{id}")]
            public async Task<IActionResult> DesmarcarConsulta(int id)
            {
                var consulta = await _context.Consultas.FindAsync(id);
                if (consulta == null)
                    return NotFound();

                if (!consulta.Agendada)
                    return BadRequest("Consulta não está agendada.");

                consulta.PacienteId = null;
                consulta.Agendada = false;

                await _context.SaveChangesAsync();
                return NoContent();
            }

        // PUT: api/Consultas/Atualizar/5
        [HttpPut("Atualizar/{id}")]
        public async Task<IActionResult> AtualizarConsulta(int id, [FromBody] Consulta consultaAtualizada)
        {
            if (id != consultaAtualizada.Id)
                return BadRequest();

            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
                return NotFound();

            // Atualize os campos que desejar
            consulta.DataHora = consultaAtualizada.DataHora;
            consulta.ClinicaEspecialidadeId = consultaAtualizada.ClinicaEspecialidadeId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Consultas.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }
        // DELETE: api/Consultas/5
        [HttpDelete("{id}")]
            public async Task<IActionResult> DeleteConsulta(int id)
            {
                var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
                return NotFound();

                _context.Consultas.Remove(consulta);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

