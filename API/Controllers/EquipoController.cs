using API.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using SharedModels.ModelsDTO.Equipo;

namespace API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EquipoController : ControllerBase
{
    private readonly IEquipoRepository _equipoRepo;
    private readonly IMapper _mapper;

    public EquipoController(IEquipoRepository equipoRepo, IMapper mapper)
    {
        _equipoRepo = equipoRepo;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var equipo = await _equipoRepo.GetByIdAsync(id);
        if (equipo == null)
            return NotFound(new { Message = "Equipo no encontrado." });

        var dto = _mapper.Map<EquipoResponseDTO>(equipo);
        return Ok(dto);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var equipos = await _equipoRepo.GetAllAsync();
        var dtos = _mapper.Map<List<EquipoResponseDTO>>(equipos);
        return Ok(dtos);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] EquipoCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var equipo = _mapper.Map<Equipo>(dto);
        await _equipoRepo.CreateAsync(equipo);
        var response = _mapper.Map<EquipoResponseDTO>(equipo);
        return CreatedAtAction(nameof(Get), new { id = equipo.EquipoId }, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] EquipoUpdateDTO dto)
    {
        if (id != dto.EquipoId)
            return BadRequest(new { Message = "El ID no coincide." });

        var equipo = await _equipoRepo.GetByIdAsync(id);
        if (equipo == null)
            return NotFound(new { Message = "Equipo no encontrado." });

        _mapper.Map(dto, equipo);
        await _equipoRepo.UpdateAsync(equipo);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var equipo = await _equipoRepo.GetByIdAsync(id);
        if (equipo == null)
            return NotFound(new { Message = "Equipo no encontrado." });

        await _equipoRepo.DeleteAsync(equipo);
        return NoContent();
    }

    /// <summary>
    /// Obtiene todos los equipos de una empresa.
    /// </summary>
    [HttpGet("by-empresa/{empresaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmpresaId(int empresaId)
    {
        var equipos = await _equipoRepo.GetByEmpresaIdAsync(empresaId);
        var dtos = _mapper.Map<List<EquipoResponseDTO>>(equipos);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene todos los equipos por estado.
    /// </summary>
    [HttpGet("by-estado/{estado}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEstado(string estado)
    {
        var equipos = await _equipoRepo.GetByEstadoAsync(estado);
        var dtos = _mapper.Map<List<EquipoResponseDTO>>(equipos);
        return Ok(dtos);
    }


}
