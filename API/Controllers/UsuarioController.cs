using API.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using SharedModels.ModelsDTO.Usuario;

namespace API.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IMapper _mapper;

    public UsuarioController(IUsuarioRepository usuarioRepo, IMapper mapper)
    {
        _usuarioRepo = usuarioRepo;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id);
        if (usuario == null)
            return NotFound(new { Message = "Usuario no encontrado." });

        var dto = _mapper.Map<UsuarioResponseDTO>(usuario);
        return Ok(dto);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var usuarios = await _usuarioRepo.GetAllAsync();
        var dtos = _mapper.Map<List<UsuarioResponseDTO>>(usuarios);
        return Ok(dtos);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] UsuarioCreateDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existe = await _usuarioRepo.ExistsAsync(u => u.Email == dto.Email);
        if (existe)
            return Conflict(new { Message = "Ya existe un usuario con ese email." });

        var usuario = _mapper.Map<Usuario>(dto);
        usuario.Contraseña = HashPassword(dto.Contraseña);

        await _usuarioRepo.CreateAsync(usuario);
        var response = _mapper.Map<UsuarioResponseDTO>(usuario);
        return CreatedAtAction(nameof(Get), new { id = usuario.UsuarioId }, response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpdateDTO dto)
    {
        if (id != dto.UsuarioId)
            return BadRequest(new { Message = "El ID no coincide." });

        var usuario = await _usuarioRepo.GetByIdAsync(id);
        if (usuario == null)
            return NotFound(new { Message = "Usuario no encontrado." });

        _mapper.Map(dto, usuario);

        if (!string.IsNullOrEmpty(dto.Contraseña))
            usuario.Contraseña = HashPassword(dto.Contraseña);

        await _usuarioRepo.UpdateAsync(usuario);
        return NoContent();
    }


    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _usuarioRepo.GetByIdAsync(id);
        if (usuario == null)
            return NotFound(new { Message = "Usuario no encontrado." });

        await _usuarioRepo.DeleteAsync(usuario);
        return NoContent();
    }

    private byte[] HashPassword(string password)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        return sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }


    /// <summary>
    /// Obtiene un usuario por email.
    /// </summary>
    [HttpGet("by-email/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var usuario = await _usuarioRepo.GetByEmailAsync(email);
        if (usuario == null)
            return NotFound(new { Message = "Usuario no encontrado." });

        var dto = _mapper.Map<UsuarioResponseDTO>(usuario);
        return Ok(dto);
    }

    /// <summary>
    /// Obtiene todos los usuarios de una empresa.
    /// </summary>
    [HttpGet("by-empresa/{empresaId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByEmpresaId(int empresaId)
    {
        var usuarios = await _usuarioRepo.GetByEmpresaIdAsync(empresaId);
        var dtos = _mapper.Map<List<UsuarioResponseDTO>>(usuarios);
        return Ok(dtos);
    }
}
