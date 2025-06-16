using API.Controllers;
using API.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SharedModels.Models;
using SharedModels.ModelsDTO.Empresa;
using Moq;

namespace APITest;

public class EmpresaControllerTests
{
    private readonly Mock<IEmpresaRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EmpresaController _controller;

    public EmpresaControllerTests()
    {
        _repoMock = new Mock<IEmpresaRepository>();
        _mapperMock = new Mock<IMapper>();
        _controller = new EmpresaController(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenEmpresaExists()
    {
        // Arrange
        var empresa = new Empresa { EmpresaId = 1, Nombre = "Test" };
        var dto = new EmpresaResponseDTO { EmpresaId = 1, Nombre = "Test" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(empresa);
        _mapperMock.Setup(m => m.Map<EmpresaResponseDTO>(empresa)).Returns(dto);

        // Act
        var result = await _controller.Get(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, okResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenEmpresaDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Empresa)null);

        var result = await _controller.Get(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithList()
    {
        var empresas = new List<Empresa> { new Empresa { EmpresaId = 1 } };
        var dtos = new List<EmpresaResponseDTO> { new EmpresaResponseDTO { EmpresaId = 1 } };
        _repoMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(empresas);
        _mapperMock.Setup(m => m.Map<List<EmpresaResponseDTO>>(empresas)).Returns(dtos);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, okResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        _controller.ModelState.AddModelError("Nombre", "Required");

        var result = await _controller.Create(new EmpresaCreateDTO());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsConflict_WhenEmpresaExists()
    {
        var dto = new EmpresaCreateDTO { Nombre = "Test" };
        _repoMock.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Empresa, bool>>>())).ReturnsAsync(true);

        var result = await _controller.Create(dto);

        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenEmpresaIsCreated()
    {
        var dto = new EmpresaCreateDTO { Nombre = "Test" };
        var empresa = new Empresa { EmpresaId = 1, Nombre = "Test" };
        var responseDto = new EmpresaResponseDTO { EmpresaId = 1, Nombre = "Test" };

        _repoMock.Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Empresa, bool>>>())).ReturnsAsync(false);
        _mapperMock.Setup(m => m.Map<Empresa>(dto)).Returns(empresa);
        _repoMock.Setup(r => r.CreateAsync(empresa)).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<EmpresaResponseDTO>(empresa)).Returns(responseDto);

        var result = await _controller.Create(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(responseDto, createdResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var dto = new EmpresaUpdateDTO { EmpresaId = 2 };
        var result = await _controller.Update(1, dto);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenEmpresaDoesNotExist()
    {
        var dto = new EmpresaUpdateDTO { EmpresaId = 1 };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Empresa)null);

        var result = await _controller.Update(1, dto);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenUpdated()
    {
        var dto = new EmpresaUpdateDTO { EmpresaId = 1 };
        var empresa = new Empresa { EmpresaId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(empresa);
        _mapperMock.Setup(m => m.Map(dto, empresa));
        _repoMock.Setup(r => r.UpdateAsync(empresa)).Returns(Task.CompletedTask);

        var result = await _controller.Update(1, dto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenEmpresaDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Empresa)null);

        var result = await _controller.Delete(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeleted()
    {
        var empresa = new Empresa { EmpresaId = 1 };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(empresa);
        _repoMock.Setup(r => r.DeleteAsync(empresa)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetByNombre_ReturnsOk_WhenEmpresaExists()
    {
        var empresa = new Empresa { EmpresaId = 1, Nombre = "Test" };
        var dto = new EmpresaResponseDTO { EmpresaId = 1, Nombre = "Test" };
        _repoMock.Setup(r => r.GetByNombreAsync("Test")).ReturnsAsync(empresa);
        _mapperMock.Setup(m => m.Map<EmpresaResponseDTO>(empresa)).Returns(dto);

        var result = await _controller.GetByNombre("Test");

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, okResult.Value);
    }

    [Fact]
    public async Task GetByNombre_ReturnsNotFound_WhenEmpresaDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByNombreAsync("Test")).ReturnsAsync((Empresa)null);

        var result = await _controller.GetByNombre("Test");

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetAllWithUsuarios_ReturnsOk_WithList()
    {
        var empresas = new List<Empresa> { new Empresa { EmpresaId = 1 } };
        var dtos = new List<EmpresaResponseDTO> { new EmpresaResponseDTO { EmpresaId = 1 } };
        _repoMock.Setup(r => r.GetAllWithUsuariosAsync()).ReturnsAsync(empresas);
        _mapperMock.Setup(m => m.Map<List<EmpresaResponseDTO>>(empresas)).Returns(dtos);

        var result = await _controller.GetAllWithUsuarios();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, okResult.Value);
    }
}
