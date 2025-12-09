using Microsoft.EntityFrameworkCore;
using ToDoApp.Domain.Model.Task;
using ToDoApp.Infra;

namespace ToDoApp.Application.Services.Task;

public class CategoryService(ApplicationDbContext context)
{
    public async Task<FilterCategoriesDto> FilterCategories(FilterCategoriesDto filter)
    {
        var query = context.Categories
            .Where(s => !s.IsDelete)
            .OrderByDescending(p => p.CreateDate)
            .AsQueryable();

        #region Filter

        if (!string.IsNullOrEmpty(filter.Title))
            query = query.Where(s => s.Title.Contains(filter.Title));

        #endregion

        await filter.Paging(query);

        return filter;
    }

    public async Task<bool> CreateCategory(CreateCategoryDto model)
    {
        if (string.IsNullOrEmpty(model.Title))
            return false;

        if (await context.Categories.AnyAsync(p => p.Title == model.Title && !p.IsDelete))
            return false;

        var newCategory = new Domain.Entities.Task.Category
        {
            Title = model.Title,
            Description = model.Description,
            Color = model.Color
        };

        await context.Categories.AddAsync(newCategory);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<EditCategoryDto> FillEditCategoryDto(ulong id)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDelete);
        
        if (category == null)
            return null;

        return new EditCategoryDto
        {
            Id = id,
            Title = category.Title,
            Description = category.Description,
            Color = category.Color
        };
    }

    public async Task<bool> EditCategory(EditCategoryDto edit)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == edit.Id && !c.IsDelete);
        
        if (category == null)
            return false;

        if (await context.Categories.AnyAsync(p => p.Title == edit.Title && p.Id != edit.Id && !p.IsDelete))
            return false;

        category.Title = edit.Title;
        category.Description = edit.Description;
        category.Color = edit.Color;
        category.Update();

        context.Categories.Update(category);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteCategory(ulong categoryId)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == categoryId && !c.IsDelete);
        
        if (category == null)
            return false;

        category.IsDelete = true;
        category.Update();

        context.Categories.Update(category);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<List<Domain.Entities.Task.Category>> GetAllCategories()
    {
        return await context.Categories
            .Where(c => !c.IsDelete)
            .OrderBy(c => c.Title)
            .ToListAsync();
    }
}

