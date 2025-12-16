using ToDoApp.Domain.Common;
using ToDoApp.Domain.Model.Task;

namespace ToDoApp.Application.Interfaces;

public interface ICategoryService
{
    Task<FilterCategoriesDto> FilterCategories(FilterCategoriesDto filter, CancellationToken cancellationToken = default);
    Task<Result<ulong>> CreateCategory(CreateCategoryDto model, CancellationToken cancellationToken = default);
    Task<Result> DeleteCategory(ulong categoryId, CancellationToken cancellationToken = default);
}
