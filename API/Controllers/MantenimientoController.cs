using API.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using SharedModels.ModelsDTO.Mantenimiento;

namespace API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MantenimientoController : ControllerBase
{
    private readonly IMantenimientoRepository _mantenimientoRepo;
    private readonly IMapper _mapper;

    public MantenimientoController(IMantenimientoRepository mantenimientoRepo, IMapper mapper)
    {
        _mantenimientoRepo = mantenimientoRepo;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var mantenimiento = await _mantenimientoRepo.GetByIdAsync(id);
        if (mantenimiento == null)
            return NotFound(new { Message = "Mantenimiento no encontrado." });

        var dto = _mapper.Map<MantenimientoResponseDTO>(mantenimiento);
        return Ok(dto);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var mantenimientos = await _mantenimientoRepo.GetAllAsync();
        var dtos = _mapper.Map<List<MantenimientoResponseDTO>>(mantenimientos);
        return Ok(dtos);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] MantenimientoCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var mantenimiento = _mapper.Map<Mantenimiento>(dto);
        await _mantenimientoRepo.CreateAsync(mantenimiento);
        var response = _mapper.Map<MantenimientoResponseDTO>(mantenimiento);
        return CreatedAtAction(nameof(Get), new { id = mantenimiento.MantenimientoId }, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] MantenimientoUpdateDTO dto)
    {
        if (id != dto.MantenimientoId)
            return BadRequest(new { Message = "El ID no coincide." });

        var mantenimiento = await _mantenimientoRepo.GetByIdAsync(id);
        if (mantenimiento == null)
            return NotFound(new { Message = "Mantenimiento no encontrado." });

        _mapper.Map(dto, mantenimiento);
        await _mantenimientoRepo.UpdateAsync(mantenimiento);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var mantenimiento = await _mantenimientoRepo.GetByIdAsync(id);
        if (mantenimiento == null)
            return NotFound(new { Message = "Mantenimiento no encontrado." });

        await _mantenimientoRepo.DeleteAsync(mantenimiento);
        return NoContent();
    }

    /// <summary>
    /// Obtiene todos los mantenimientos de un equipo.
    /// </summary>
    [HttpGet("by-equipo/{equipoId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEquipoId(int equipoId)
    {
        var mantenimientos = await _mantenimientoRepo.GetByEquipoIdAsync(equipoId);
        var dtos = _mapper.Map<List<MantenimientoResponseDTO>>(mantenimientos);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene todos los mantenimientos por estado.
    /// </summary>
    [HttpGet("by-estado/{estado}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEstado(string estado)
    {
        var mantenimientos = await _mantenimientoRepo.GetByEstadoAsync(estado);
        var dtos = _mapper.Map<List<MantenimientoResponseDTO>>(mantenimientos);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene todos los mantenimientos de una empresa.
    /// </summary>
    [HttpGet("by-empresa/{empresaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmpresaId(int empresaId)
    {
        var mantenimientos = await _mantenimientoRepo.GetByEmpresaIdAsync(empresaId);
        var dtos = _mapper.Map<List<MantenimientoResponseDTO>>(mantenimientos);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene los mantenimientos próximos a vencer (por fecha límite).
    /// </summary>
    [HttpGet("proximos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProximos([FromQuery] DateTime fechaLimite)
    {
        var mantenimientos = await _mantenimientoRepo.GetProximosAsync(fechaLimite);
        var dtos = _mapper.Map<List<MantenimientoResponseDTO>>(mantenimientos);
        return Ok(dtos);
    }
}
