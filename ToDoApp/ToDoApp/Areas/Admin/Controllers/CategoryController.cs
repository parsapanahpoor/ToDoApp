using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Services.Task;
using ToDoApp.Domain.Model.Task;
using ToDoApp.HttpManager;

namespace ToDoApp.Areas.Admin.Controllers;

public class CategoryController(
    CategoryService categoryService) :
    AdminBaseController
{
    #region Filter Categories

    public async Task<IActionResult> FilterCategories(FilterCategoriesDto filter)
        => View(await categoryService.FilterCategories(filter));

    #endregion

    #region Create Category

    public IActionResult CreateCategory()
        => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto model)
    {
        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی‌باشد";
            return View(model);
        }

        var result = await categoryService.CreateCategory(model);
        if (result)
        {
            TempData[SuccessMessage] = "دسته‌بندی با موفقیت ایجاد شد";
            return RedirectToAction(nameof(FilterCategories));
        }

        TempData[WarningMessage] = "عنوان دسته‌بندی تکراری است";
        return View(model);
    }

    #endregion

    #region Edit Category

    [HttpGet]
    public async Task<IActionResult> EditCategory(ulong id)
    {
        var result = await categoryService.FillEditCategoryDto(id);
        if (result == null)
        {
            TempData[ErrorMessage] = "دسته‌بندی مورد نظر یافت نشد";
            return RedirectToAction(nameof(FilterCategories));
        }

        return View(result);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(EditCategoryDto edit)
    {
        if (!ModelState.IsValid)
        {
            TempData[ErrorMessage] = "اطلاعات وارد شده معتبر نمی‌باشد";
            return View(edit);
        }

        var result = await categoryService.EditCategory(edit);
        if (result)
        {
            TempData[SuccessMessage] = "دسته‌بندی با موفقیت ویرایش شد";
            return RedirectToAction(nameof(FilterCategories));
        }

        TempData[WarningMessage] = "عنوان دسته‌بندی تکراری است";
        return View(edit);
    }

    #endregion

    #region Delete Category

    public async Task<IActionResult> DeleteCategory(ulong categoryId)
    {
        var result = await categoryService.DeleteCategory(categoryId);
        if (result)
            return JsonResponseStatus.Success();

        return JsonResponseStatus.Error();
    }

    #endregion
}

