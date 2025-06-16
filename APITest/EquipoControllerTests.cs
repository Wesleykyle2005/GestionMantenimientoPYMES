using API.Controllers;
using API.Repositories.IRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SharedModels.Models;
using SharedModels.ModelsDTO.Equipo;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace APITest;

public class EquipoControllerTests
{
    private readonly Mock<IEquipoRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly EquipoController _controller;

    public EquipoControllerTests()
    {
        _repoMock = new Mock<IEquipoRepository>();
        _mapperMock = new Mock<IMapper>();
        _controller = new EquipoController(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenEquipoExists()
    {
        var equipo = new Equipo { EquipoId = 1, Nombre = "EquipoTest" };
        var dto = new EquipoResponseDTO { EquipoId = 1, Nombre = "EquipoTest" };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(equipo);
        _mapperMock.Setup(m => m.Map<EquipoResponseDTO>(equipo)).Returns(dto);

        var result = await _controller.Get(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, okResult.Value);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenEquipoDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Equipo)null);

        var result = await _controller.Get(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithList()
    {
        var equipos = new List<Equipo> { new Equipo { EquipoId = 1 } };
        var dtos = new List<EquipoResponseDTO> { new EquipoResponseDTO { EquipoId = 1 } };
        _repoMock.Setup(r => r.GetAllAsync(null)).ReturnsAsync(equipos);
        _mapperMock.Setup(m => m.Map<List<EquipoResponseDTO>>(equipos)).Returns(dtos);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, okResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        _controller.ModelState.AddModelError("Nombre", "Required");

        var result = await _controller.Create(new EquipoCreateDTO());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreated_WhenEquipoIsCreated()
    {
        var dto = new EquipoCreateDTO { Nombre = "NuevoEquipo" };
        var equipo = new Equipo { EquipoId = 1, Nombre = "NuevoEquipo" };
        var responseDto = new EquipoResponseDTO { EquipoId = 1, Nombre = "NuevoEquipo" };

        _mapperMock.Setup(m => m.Map<Equipo>(dto)).Returns(equipo);
        _repoMock.Setup(r => r.CreateAsync(equipo)).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<EquipoResponseDTO>(equipo)).Returns(responseDto);

        var result = await _controller.Create(dto);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(responseDto, createdResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var dto = new EquipoUpdateDTO { EquipoId = 2 };
        var result = await _controller.Update(1, dto);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenEquipoDoesNotExist()
    {
        var dto = new EquipoUpdateDTO { EquipoId = 1 };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Equipo)null);

        var result = await _controller.Update(1, dto);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenUpdated()
    {
        var dto = new EquipoUpdateDTO { EquipoId = 1 };
        var equipo = new Equipo { EquipoId = 1 };

        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(equipo);
        _mapperMock.Setup(m => m.Map(dto, equipo));
        _repoMock.Setup(r => r.UpdateAsync(equipo)).Returns(Task.CompletedTask);

        var result = await _controller.Update(1, dto);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenEquipoDoesNotExist()
    {
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Equipo)null);

        var result = await _controller.Delete(1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenDeleted()
    {
        var equipo = new Equipo { EquipoId = 1 };
        _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(equipo);
        _repoMock.Setup(r => r.DeleteAsync(equipo)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task GetByEmpresaId_ReturnsOk_WithList()
    {
        var equipos = new List<Equipo> { new Equipo { EquipoId = 1, EmpresaId = 2 } };
        var dtos = new List<EquipoResponseDTO> { new EquipoResponseDTO { EquipoId = 1 } };
        _repoMock.Setup(r => r.GetByEmpresaIdAsync(2)).ReturnsAsync(equipos);
        _mapperMock.Setup(m => m.Map<List<EquipoResponseDTO>>(equipos)).Returns(dtos);

        var result = await _controller.GetByEmpresaId(2);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, okResult.Value);
    }

    [Fact]
    public async Task GetByEstado_ReturnsOk_WithList()
    {
        var equipos = new List<Equipo> { new Equipo { EquipoId = 1, Estado = "Activo" } };
        var dtos = new List<EquipoResponseDTO> { new EquipoResponseDTO { EquipoId = 1 } };
        _repoMock.Setup(r => r.GetByEstadoAsync("Activo")).ReturnsAsync(equipos);
        _mapperMock.Setup(m => m.Map<List<EquipoResponseDTO>>(equipos)).Returns(dtos);

        var result = await _controller.GetByEstado("Activo");

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, okResult.Value);
    }
}
