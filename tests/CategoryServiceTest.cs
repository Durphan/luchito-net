using luchito_net.Config.DataProvider;
using luchito_net.Models.Dto.Request;
using luchito_net.Repository;
using luchito_net.Repository.Interfaces;
using luchito_net.Service;
using luchito_net.Tests.Factory;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace luchito_net.Tests;

public class CategoryServiceTest : IDisposable
{
    private readonly InitializeDatabase _context;
    private readonly ICategoryRepository _categoryRepository;
    private readonly CategoryService _categoryService;

    public CategoryServiceTest()
    {
        _context = MemoryDatabaseFactory.CreateInMemoryDatabaseOptions();
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _categoryRepository = new CategoryRepository(_context, NullLogger<CategoryRepository>.Instance);

        _categoryService = new CategoryService(_categoryRepository);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    async Task NameNormalizesCorrectlyOnCreation()
    {
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
        CategoryRequestDto createDto = new CategoryRequestDto();
        createDto.Name = "Initial Category";
        createDto.IsActive = true;
        createDto.ParentCategoryID = null;
        var created = await _categoryService.CreateCategory(createDto);

        CategoryRequestDto updateDto = new CategoryRequestDto();
        updateDto.Name = "  goLosINas  ";
        var normalizedName = "Golosinas";
        updateDto.IsActive = true;
        updateDto.ParentCategoryID = null;
        var response = await _categoryService.UpdateCategory(created.Id, updateDto);
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