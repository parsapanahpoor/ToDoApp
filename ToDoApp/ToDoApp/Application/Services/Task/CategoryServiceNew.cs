using Microsoft.Extensions.Logging;
using ToDoApp.Application.Interfaces;
using ToDoApp.Domain.Common;
using ToDoApp.Domain.Entities.Task;
using ToDoApp.Domain.Interfaces;
using ToDoApp.Domain.Model.Task;

namespace ToDoApp.Application.Services.Task;

public class CategoryServiceNew : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CategoryServiceNew> _logger;

    public CategoryServiceNew(
        ICategoryRepository categoryRepository,
        IUnitOfWork unitOfWork,
        ILogger<CategoryServiceNew> logger)
    {
        _categoryRepository = categoryRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    #region Filter Categories

    public async Task<FilterCategoriesDto> FilterCategories(FilterCategoriesDto filter, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Filtering categories with parameters: {@Filter}", filter);

            var query = _categoryRepository.GetQueryable()
                .OrderByDescending(p => p.CreateDate)
                .AsQueryable();

            #region Filter

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(s => s.Title.Contains(filter.Title));

            #endregion

            await filter.Paging(query);

            _logger.LogInformation("Successfully filtered {Count} categories", filter.Entities?.Count ?? 0);

            return filter;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error filtering categories");
            throw;
        }
    }

    #endregion

    #region Create Category

    public async Task<Result<ulong>> CreateCategory(CreateCategoryDto model, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Creating new category with title: {Title}", model.Title);

            // Validation: Check if title is provided
            if (string.IsNullOrWhiteSpace(model.Title))
            {
                _logger.LogWarning("Category title is empty");
                return Result<ulong>.Failure("«?Ã«œ œ” Âù»‰œ? »« Œÿ« „Ê«ÃÂ ‘œ", "⁄‰Ê«‰ œ” Âù»‰œ? «·“«„? «” ");
            }

            // Validation: Check if title already exists
            if (await _categoryRepository.IsTitleExistsAsync(model.Title, cancellationToken: cancellationToken))
            {
                _logger.LogWarning("Category title {Title} already exists", model.Title);
                return Result<ulong>.Failure("«?Ã«œ œ” Âù»‰œ? »« Œÿ« „Ê«ÃÂ ‘œ", "⁄‰Ê«‰ œ” Âù»‰œ?  ò—«—? «” ");
            }

            var newCategory = new Category
            {
                Title = model.Title,
                Description = model.Description,
                Color = model.Color ?? "#3498db",
                CreateDate = DateTime.UtcNow
            };

            await _categoryRepository.AddAsync(newCategory, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully created category {Title} with ID {CategoryId}", model.Title, newCategory.Id);

            return Result<ulong>.Success(newCategory.Id, "œ” Âù»‰œ? »« „Ê›ﬁ?  «?Ã«œ ‘œ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category {Title}", model.Title);
            return Result<ulong>.Failure("Œÿ« œ— «?Ã«œ œ” Âù»‰œ?", ex.Message);
        }
    }

    #endregion

    #region Delete Category

    public async Task<Result> DeleteCategory(ulong categoryId, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Deleting category {CategoryId}", categoryId);

            var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken);

            if (category == null || category.IsDelete)
            {
                _logger.LogWarning("Category {CategoryId} not found", categoryId);
                return Result.Failure("œ” Âù»‰œ? ?«›  ‰‘œ");
            }

            category.IsDelete = true;
            category.Update();

            _categoryRepository.Update(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Successfully deleted category {CategoryId}", categoryId);

            return Result.Success("œ” Âù»‰œ? »« „Ê›ﬁ?  Õ–› ‘œ");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category {CategoryId}", categoryId);
            return Result.Failure("Œÿ« œ— Õ–› œ” Âù»‰œ?", ex.Message);
        }
    }

    #endregion
}
