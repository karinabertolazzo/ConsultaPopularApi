using Microsoft.AspNetCore.Mvc;
using ConsultaPopularApi.Data;
using ConsultaPopularApi.Models;
using Microsoft.EntityFrameworkCore;
using ConsultaPopularApi.DTOs;

namespace ConsultaPopularApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicaEspecialidadesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClinicaEspecialidadesController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/ClinicaEspecialidades
        [HttpPost]
        public async Task<ActionResult<ClinicaEspecialidadeResponseDTO>> PostClinicaEspecialidade([FromBody] ClinicaEspecialidadeDTO dto)
        {

            try
            {
                var clinica = await _context.Clinicas.FindAsync(dto.ClinicaId);
                if (clinica == null)
                    return BadRequest("Clínica informada não existe.");

                var especialidade = await _context.Especialidades.FindAsync(dto.EspecialidadeId);
                if (especialidade == null)
                    return BadRequest("Especialidade informada não existe.");

                var ce = new ClinicaEspecialidade
                {
                    ClinicaId = dto.ClinicaId,
                    EspecialidadeId = dto.EspecialidadeId,
                    Especialidade = especialidade
                };

                _context.ClinicaEspecialidades.Add(ce);
                await _context.SaveChangesAsync();

                var response = new ClinicaEspecialidadeResponseDTO
                {
                    Id = ce.Id,
                    ClinicaId = ce.ClinicaId,
                    ClinicaNome = clinica.Nome,
                    EspecialidadeId = ce.EspecialidadeId,
                    EspecialidadeNome = especialidade.Nome
                };

                return CreatedAtAction(nameof(GetClinicaEspecialidade), new { id = ce.Id }, response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex); // ideal usar um logger real
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }




        // GET: api/ClinicaEspecialidades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClinicaEspecialidadeResponseDTO>>> GetClinicaEspecialidades()
        {
            var lista = await _context.ClinicaEspecialidades
       .Include(ce => ce.Clinica)
       .Include(ce => ce.Especialidade)
       .Select(ce => new ClinicaEspecialidadeResponseDTO
       {
           Id = ce.Id,
           ClinicaId = ce.ClinicaId,
           ClinicaNome = ce.Clinica.Nome,
           EspecialidadeId = ce.EspecialidadeId,
           EspecialidadeNome = ce.Especialidade.Nome
       })
       .ToListAsync();

            return lista;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClinicaEspecialidadeResponseDTO>> GetClinicaEspecialidade(int id)
        {
            var ce = await _context.ClinicaEspecialidades
                .Include(c => c.Clinica)
                .Include(c => c.Especialidade)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (ce == null)
                return NotFound();

            var response = new ClinicaEspecialidadeResponseDTO
            {
                Id = ce.Id,
                ClinicaId = ce.ClinicaId,
                ClinicaNome = ce.Clinica?.Nome,
                EspecialidadeId = ce.EspecialidadeId,
                EspecialidadeNome = ce.Especialidade?.Nome
            };

            return Ok(response); // ✅ ISSO AQUI
        }

        // PUT: api/ClinicaEspecialidades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClinicaEspecialidade(int id, [FromBody] AtualizarClinicaEspecialidadeDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da URL não confere com o corpo da requisição.");

            var ce = await _context.ClinicaEspecialidades.FindAsync(id);
            if (ce == null)
                return NotFound();

            ce.ClinicaId = dto.ClinicaId;
            ce.EspecialidadeId = dto.EspecialidadeId;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/ClinicaEspecialidades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClinicaEspecialidade(int id)
        {
            var ce = await _context.ClinicaEspecialidades.FindAsync(id);
            if (ce == null)
                return NotFound();

            _context.ClinicaEspecialidades.Remove(ce);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
