using ConsultaPopularApi.Data;
using ConsultaPopularApi.DTOs;
using ConsultaPopularApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaPopularApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EspecialidadesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EspecialidadesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetEspecialidades()
        {
            var especialidades = _context.Especialidades.ToList();
            return Ok(especialidades);
        }
        [HttpPost]
        public IActionResult CreateEspecialidade([FromBody] EspecialidadeDTO dto)
        {
            var especialidade = new Especialidade
            {
                Nome = dto.Nome
            };

            _context.Especialidades.Add(especialidade);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEspecialidades), new { id = especialidade.Id }, especialidade);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateEspecialidade(int id, [FromBody] Especialidade especialidadeAtualizada)
        {
            var especialidade = _context.Especialidades.Find(id);
            if (especialidade == null)
                return NotFound("Especialidade não encontrada.");

            especialidade.Nome = especialidadeAtualizada.Nome;

            _context.SaveChanges();
            return Ok(especialidade);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteEspecialidade(int id)
        {
            var especialidade = _context.Especialidades.Find(id);
            if (especialidade == null)
                return NotFound("Especialidade não encontrada.");

            _context.Especialidades.Remove(especialidade);
            _context.SaveChanges();
            return NoContent(); 
        }



    }
}
