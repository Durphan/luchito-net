using luchito_net.Models;
using luchito_net.Models.Dto.Request;
using luchito_net.Models.Entity;
using luchito_net.Models.Mappers;
using luchito_net.Repository.Interfaces;
using luchito_net.Service;
using Moq;
using Xunit;

namespace luchito_net.Tests;

public class CategoryServiceTest
{

    private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
    private readonly CategoryService _categoryService;

    public CategoryServiceTest()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoryService = new CategoryService(_categoryRepositoryMock.Object);
    }

    [Fact]
    async Task NameNormalizesCorrectlyOnCreation()
    {
        _categoryRepositoryMock.Setup(repo => repo.CreateCategory(It.IsAny<Category>()))
            .ReturnsAsync((Category category) => category);
        CategoryRequestDto categoryDto = new CategoryRequestDto();
        categoryDto.Name = "  beBIDas  ";
        var normalizedName = "Bebidas";
        categoryDto.IsActive = true;
        categoryDto.ParentCategoryID = null;
        var response = await _categoryService.CreateCategory(categoryDto);
        Assert.Equal(normalizedName, response.Name);
    }

    [Fact]
    async Task NameNormalizesCorrectlyOnUpdate()
    {
        _categoryRepositoryMock.Setup(repo => repo.UpdateCategory(It.IsAny<int>(), It.IsAny<Category>()))
            .ReturnsAsync((int id, Category category) => category);
        CategoryRequestDto categoryDto = new CategoryRequestDto();
        categoryDto.Name = "  goLosINas  ";
        var normalizedName = "Golosinas";
        categoryDto.IsActive = true;
        categoryDto.ParentCategoryID = null;
        var response = await _categoryService.UpdateCategory(1, categoryDto);
        Assert.Equal(normalizedName, response.Name);
    }

    [Fact]
    async Task CantCreateCategoryWithEmptyName()
    {
        CategoryRequestDto categoryDto = new CategoryRequestDto();
        categoryDto.Name = "     ";
        categoryDto.IsActive = true;
        categoryDto.ParentCategoryID = null;

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            var response = await _categoryService.CreateCategory(categoryDto);
        });
    }





}