using API.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using SharedModels.ModelsDTO.Empresa;

namespace API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EmpresaController : ControllerBase
{
    private readonly IEmpresaRepository _empresaRepo;
    private readonly IMapper _mapper;

    public EmpresaController(IEmpresaRepository empresaRepo, IMapper mapper)
    {
        _empresaRepo = empresaRepo;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var empresa = await _empresaRepo.GetByIdAsync(id);
        if (empresa == null)
            return NotFound(new { Message = "Empresa no encontrada." });

        var dto = _mapper.Map<EmpresaResponseDTO>(empresa);
        return Ok(dto);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var empresas = await _empresaRepo.GetAllAsync();
        var dtos = _mapper.Map<List<Empresa>>(empresas);
        return Ok(dtos);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] EmpresaCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existe = await _empresaRepo.ExistsAsync(e => e.Nombre == dto.Nombre);
        if (existe)
            return Conflict(new { Message = "Ya existe una empresa con ese nombre." });

       
        var empresa = _mapper.Map<Empresa>(dto);

       
        await _empresaRepo.CreateAsync(empresa);

       
        var response = _mapper.Map<EmpresaResponseDTO>(empresa);

        return CreatedAtAction(nameof(Get), new { id = empresa.EmpresaId }, response);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] EmpresaUpdateDTO dto)
    {
        if (id != dto.EmpresaId)
            return BadRequest(new { Message = "El ID no coincide." });

        var empresa = await _empresaRepo.GetByIdAsync(id);
        if (empresa == null)
            return NotFound(new { Message = "Empresa no encontrada." });

        _mapper.Map(dto, empresa);
        await _empresaRepo.UpdateAsync(empresa);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var empresa = await _empresaRepo.GetByIdAsync(id);
        if (empresa == null)
            return NotFound(new { Message = "Empresa no encontrada." });

        await _empresaRepo.DeleteAsync(empresa);
        return NoContent();
    }

    /// <summary>
    /// Obtiene una empresa por su nombre exacto.
    /// </summary>
    [HttpGet("by-nombre/{nombre}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNombre(string nombre)
    {
        var empresa = await _empresaRepo.GetByNombreAsync(nombre);
        if (empresa == null)
            return NotFound(new { Message = "Empresa no encontrada." });

        var dto = _mapper.Map<EmpresaResponseDTO>(empresa);
        return Ok(dto);
    }

    /// <summary>
    /// Obtiene todas las empresas con sus usuarios relacionados.
    /// </summary>
    [HttpGet("with-usuarios")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllWithUsuarios()
    {
        var empresas = await _empresaRepo.GetAllWithUsuariosAsync();
        var dtos = _mapper.Map<List<EmpresaResponseDTO>>(empresas);
        return Ok(dtos);
    }

}

