using API.Controllers;
using API.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SharedModels.Models;
using SharedModels.ModelsDTO.Mantenimiento;

namespace APITest;

public class MantenimientoControllerTests
{
    private readonly Mock<IMantenimientoRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly MantenimientoController _controller;

    public MantenimientoControllerTests()
    {
        _repoMock = new Mock<IMantenimientoRepository>();
        _mapperMock = new Mock<IMapper>();
        _controller = new MantenimientoController(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenMantenimientoExists()
    {
        var mantenimiento = new Mantenimiento { MantenimientoId = 1, Descripcion = "Test" };
        var dto = new MantenimientoResponseDTO { MantenimientoId = 1, Descripcion = "Test" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(mantenimiento);
        _mapperMock.Setup(m => m.Map<MantenimientoResponseDTO>(mantenimiento)).Returns(dto);

        var result = await _controller.Get(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, okResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenMantenimientoDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Mantenimiento)null);

        var result = await _controller.Get(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithList()
    {
        var mantenimientos = new List<Mantenimiento> { new Mantenimiento { MantenimientoId = 1 } };
        var dtos = new List<MantenimientoResponseDTO> { new MantenimientoResponseDTO { MantenimientoId = 1 } };
        _repoMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(mantenimientos);
        _mapperMock.Setup(m => m.Map<List<MantenimientoResponseDTO>>(mantenimientos)).Returns(dtos);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, okResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        _controller.ModelState.AddModelError("Descripcion", "Required");

        var result = await _controller.Create(new MantenimientoCreateDTO());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenMantenimientoIsCreated()
    {
        var dto = new MantenimientoCreateDTO { Descripcion = "Nuevo" };
        var mantenimiento = new Mantenimiento { MantenimientoId = 1, Descripcion = "Nuevo" };
        var responseDto = new MantenimientoResponseDTO { MantenimientoId = 1, Descripcion = "Nuevo" };

        _mapperMock.Setup(m => m.Map<Mantenimiento>(dto)).Returns(mantenimiento);
        _repoMock.Setup(r => r.CreateAsync(mantenimiento)).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<MantenimientoResponseDTO>(mantenimiento)).Returns(responseDto);

        var result = await _controller.Create(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(responseDto, createdResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var dto = new MantenimientoUpdateDTO { MantenimientoId = 2 };
        var result = await _controller.Update(1, dto);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenMantenimientoDoesNotExist()
    {
        var dto = new MantenimientoUpdateDTO { MantenimientoId = 1 };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Mantenimiento)null);

        var result = await _controller.Update(1, dto);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenUpdated()
    {
        var dto = new MantenimientoUpdateDTO { MantenimientoId = 1 };
        var mantenimiento = new Mantenimiento { MantenimientoId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(mantenimiento);
        _mapperMock.Setup(m => m.Map(dto, mantenimiento));
        _repoMock.Setup(r => r.UpdateAsync(mantenimiento)).Returns(Task.CompletedTask);

        var result = await _controller.Update(1, dto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenMantenimientoDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Mantenimiento)null);

        var result = await _controller.Delete(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeleted()
    {
        var mantenimiento = new Mantenimiento { MantenimientoId = 1 };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(mantenimiento);
        _repoMock.Setup(r => r.DeleteAsync(mantenimiento)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetByEquipoId_ReturnsOk_WithList()
    {
        var mantenimientos = new List<Mantenimiento> { new Mantenimiento { MantenimientoId = 1, EquipoId = 2 } };
        var dtos = new List<MantenimientoResponseDTO> { new MantenimientoResponseDTO { MantenimientoId = 1 } };
        _repoMock.Setup(r => r.GetByEquipoIdAsync(2)).ReturnsAsync(mantenimientos);
        _mapperMock.Setup(m => m.Map<List<MantenimientoResponseDTO>>(mantenimientos)).Returns(dtos);

        var result = await _controller.GetByEquipoId(2);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, okResult.Value);
    }

    [Fact]
    public async Task GetByEstado_ReturnsOk_WithList()
    {
        var mantenimientos = new List<Mantenimiento> { new Mantenimiento { MantenimientoId = 1, Estado = "Pendiente" } };
        var dtos = new List<MantenimientoResponseDTO> { new MantenimientoResponseDTO { MantenimientoId = 1 } };
        _repoMock.Setup(r => r.GetByEstadoAsync("Pendiente")).ReturnsAsync(mantenimientos);
        _mapperMock.Setup(m => m.Map<List<MantenimientoResponseDTO>>(mantenimientos)).Returns(dtos);

        var result = await _controller.GetByEstado("Pendiente");

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, okResult.Value);
    }

    [Fact]
    public async Task GetByEmpresaId_ReturnsOk_WithList()
    {
        var mantenimientos = new List<Mantenimiento> { new Mantenimiento { MantenimientoId = 1, EquipoId = 2 } };
        var dtos = new List<MantenimientoResponseDTO> { new MantenimientoResponseDTO { MantenimientoId = 1 } };
        _repoMock.Setup(r => r.GetByEmpresaIdAsync(3)).ReturnsAsync(mantenimientos);
        _mapperMock.Setup(m => m.Map<List<MantenimientoResponseDTO>>(mantenimientos)).Returns(dtos);

        var result = await _controller.GetByEmpresaId(3);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, okResult.Value);
    }

    [Fact]
    public async Task GetProximos_ReturnsOk_WithList()
    {
        var fechaLimite = DateTime.Now.AddDays(7);
        var mantenimientos = new List<Mantenimiento> { new Mantenimiento { MantenimientoId = 1 } };
        var dtos = new List<MantenimientoResponseDTO> { new MantenimientoResponseDTO { MantenimientoId = 1 } };
        _repoMock.Setup(r => r.GetProximosAsync(fechaLimite)).ReturnsAsync(mantenimientos);
        _mapperMock.Setup(m => m.Map<List<MantenimientoResponseDTO>>(mantenimientos)).Returns(dtos);

        var result = await _controller.GetProximos(fechaLimite);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, okResult.Value);
    }
}
