using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoApp.Application.Services.Task;
using ToDoApp.Domain.Model.Task;
using ToDoApp.HttpManager;

namespace ToDoApp.Controllers;

[Authorize]
public class TaskController(
    TaskService taskService) : Controller
{
    private ulong GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return ulong.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    #region Weekly Calendar

    [HttpGet]
    public async Task<IActionResult> WeeklyCalendar(DateTime? weekStartDate)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return RedirectToAction("Login", "Account");

        // اگر تاریخ شروع هفته مشخص نشده باشد، هفته جاری را نمایش بده
        var startDate = weekStartDate?.Date ?? GetWeekStartDate(DateTime.Now);

        var weeklyData = await taskService.GetWeeklyTasks(userId, startDate);
        
        // برای استفاده در View
        ViewBag.WeekStartDate = startDate;
        ViewBag.Categories = await taskService.GetAllCategories();

        return View(weeklyData);
    }

    [HttpGet]
    public async Task<IActionResult> GetWeeklyTasksJson(DateTime weekStartDate)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return JsonResponseStatus.Error("کاربر یافت نشد");

        var weeklyData = await taskService.GetWeeklyTasks(userId, weekStartDate);
        return Json(weeklyData);
    }

    private DateTime GetWeekStartDate(DateTime date)
    {
        // محاسبه شروع هفته (شنبه)
        var dayOfWeek = (int)date.DayOfWeek;
        // در .NET: Sunday=0, Monday=1, ..., Saturday=6
        // ما می‌خواهیم شنبه = شروع هفته
        var diff = dayOfWeek == 6 ? 0 : (6 - dayOfWeek); // 6 = Saturday
        return date.Date.AddDays(-diff);
    }

    #endregion

    #region Create Task

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTask(CreateTaskDto model)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return JsonResponseStatus.Error("کاربر یافت نشد");

        if (!ModelState.IsValid)
            return JsonResponseStatus.Error("اطلاعات وارد شده معتبر نمی‌باشد");

        var result = await taskService.CreateTask(model, userId);
        if (result)
            return JsonResponseStatus.Success("Task با موفقیت ایجاد شد");

        return JsonResponseStatus.Error("خطا در ایجاد Task");
    }

    #endregion

    #region Edit Task

    [HttpGet]
    public async Task<IActionResult> EditTask(ulong id)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return RedirectToAction("Login", "Account");

        var task = await taskService.FillEditTaskDto(id, userId);
        if (task == null)
        {
            TempData["ErrorMessage"] = "Task مورد نظر یافت نشد";
            return RedirectToAction(nameof(WeeklyCalendar));
        }

        ViewBag.Categories = await taskService.GetAllCategories();
        return View(task);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTask(EditTaskDto model)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return RedirectToAction("Login", "Account");

        if (!ModelState.IsValid)
        {
            ViewBag.Categories = await taskService.GetAllCategories();
            TempData["ErrorMessage"] = "اطلاعات وارد شده معتبر نمی‌باشد";
            return View(model);
        }

        var result = await taskService.EditTask(model, userId);
        if (result)
        {
            TempData["SuccessMessage"] = "Task با موفقیت ویرایش شد";
            return RedirectToAction(nameof(WeeklyCalendar));
        }

        TempData["ErrorMessage"] = "خطا در ویرایش Task";
        ViewBag.Categories = await taskService.GetAllCategories();
        return View(model);
    }

    #endregion

    #region Delete Task

    [HttpPost]
    public async Task<IActionResult> DeleteTask(ulong taskId)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return JsonResponseStatus.Error("کاربر یافت نشد");

        var result = await taskService.DeleteTask(taskId, userId);
        if (result)
            return JsonResponseStatus.Success("Task deleted successfully");

        return JsonResponseStatus.Error("خطا در حذف Task");
    }

    #endregion

    #region Toggle Completion

    [HttpPost]
    public async Task<IActionResult> ToggleTaskCompletion(ulong taskId)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return JsonResponseStatus.Error("کاربر یافت نشد");

        var result = await taskService.ToggleTaskCompletion(taskId, userId);
        if (result)
            return JsonResponseStatus.Success("Task status updated");

        return JsonResponseStatus.Error("خطا در به‌روزرسانی وضعیت Task");
    }

    #endregion

    #region Update Task DateTime (for drag & drop)

    [HttpPost]
    public async Task<IActionResult> UpdateTaskDateTime(ulong taskId, string taskDateTime)
    {
        var userId = GetCurrentUserId();
        if (userId == 0)
            return JsonResponseStatus.Error("کاربر یافت نشد");

        if (!DateTime.TryParse(taskDateTime, out var dateTime))
            return JsonResponseStatus.Error("تاریخ معتبر نیست");

        var task = await taskService.FillEditTaskDto(taskId, userId);
        if (task == null)
            return JsonResponseStatus.Error("Task یافت نشد");

        task.TaskDateTime = dateTime;
        var result = await taskService.EditTask(task, userId);
        
        if (result)
            return JsonResponseStatus.Success("تاریخ Task به‌روزرسانی شد");

        return JsonResponseStatus.Error("خطا در به‌روزرسانی تاریخ");
    }

    #endregion
}

