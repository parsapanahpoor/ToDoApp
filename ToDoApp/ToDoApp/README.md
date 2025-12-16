# ?? ToDoApp - „œ?—?  ÂÊ‘„‰œ ò«—Â«

<div align="center">

![ToDoApp Logo](https://img.shields.io/badge/ToDoApp-2.0-6366f1?style=for-the-badge&logo=todoist)
![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet)
![Status](https://img.shields.io/badge/Status-Production_Ready-10b981?style=for-the-badge)

**»Â —?‰ Ê “?»« —?‰ —«Â »—«? „œ?—?  ò«—Â«? —Ê“«‰Â**

[œ„Ê](#) | [„” ‰œ« ](./UI_UX_TRANSFORMATION.md) | [‰’»](#‰’») | [—«Â‰„«](#«” ›«œÂ)

</div>

---

## ? Ê?éê?ùÂ«

### ?? **ÿ—«Õ? „œ—‰ Ê “?»«**
- ÿ—«Õ? ò«„·« ”›«—‘? »« Design System Õ—›Âù«?
- ›Ê‰  Vazirmatn »—«? ŒÊ«‰«?? »Â —
- Glassmorphism Ê Gradient Effects
- «‰?„?‘‰ùÂ«? ‰—„ Ê Ã–«»

### ?? **Dark Mode**
- Å‘ ?»«‰? ò«„· «“ Õ«·   «—?ò
-  €??— ŒÊœò«— »« Toggle
- –Œ?—Â  —Ã?Õ«  ò«—»—

### ?? **Dashboard Å?‘—› Â**
- ¬„«— Ê ê“«—‘ ·ÕŸÂù«?
- ‰„«?‘  ”òùÂ«? «„—Ê“
- Progress Bar «‰?„?‘‰ùœ«—
- œ” Âù»‰œ?ùÂ«? ÂÊ‘„‰œ

### ? **⁄„·ò—œ ”—?⁄**
- Keyboard Shortcuts (Ctrl+K, Ctrl+N, Ctrl+D)
- Toast Notifications
- Loading States
- Smooth Animations

### ?? **Responsive Design**
- Mobile-First Approach
- ÿ—«Õ? Ê«ò‰‘ùê—« »—«?  „«„ œ” ê«ÂùÂ«
- œò„ÂùÂ«? Touch-friendly

### ?? **«„‰? **
- BCrypt »—«? Â‘ —„“ ⁄»Ê—
- Authorization ò«„·
- CSRF Protection
- Secure Authentication

---

## ?? ‘—Ê⁄ ”—?⁄

### Å?‘ù‰?«“Â«

```bash
- .NET 8 SDK
- SQL Server 2019+
- Visual Studio 2022 ?« VS Code
```

### ‰’»

```bash
# 1. Clone repository
git clone https://github.com/parsapanahpoor/ToDoApp.git
cd ToDoApp

# 2. Restore packages
dotnet restore

# 3. Update connection string in appsettings.json
# Server=YOUR_SERVER;Database=ToDoApp;...

# 4. Apply migrations
dotnet ef database update

# 5. Run application
dotnet run
```

### œ” —”?

```
?? Application: https://localhost:5001
?? Admin Panel: https://localhost:5001/Admin
```

---

## ?? «” ›«œÂ

### ò·?œÂ«? „?«‰»—

| ò·?œ | ⁄„·ò—œ |
|------|--------|
| `Ctrl+K` | Ã” ÃÊ? ”—?⁄ |
| `Ctrl+N` | «›“Êœ‰  ”ò Ãœ?œ |
| `Ctrl+D` |  €??— Dark Mode |

### Components

#### 1. «›“Êœ‰ ò«—  ¬„«—?

```html
<div class="stat-card">
    <div class="stat-icon primary">
        <i class="fa fa-tasks"></i>
    </div>
    <div class="stat-value">15</div>
    <div class="stat-label"> ”òùÂ«? ›⁄«·</div>
</div>
```

#### 2. ‰„«?‘ Toast

```javascript
window.toast.success('„Ê›ﬁ? ù¬„?“ »Êœ!');
window.toast.error('Œÿ«?? —Œ œ«œ');
```

#### 3. Confirm Dialog

```javascript
const confirmed = await window.modernUtils.confirmDialog('¬?« „ÿ„∆‰ Â” ?œø');
if (confirmed) {
    // ⁄„·?« 
}
```

---

## ??? „⁄„«—?

### Backend
```
??? Application/
?   ??? Services/          # Business Logic
?   ??? Interfaces/        # Service Contracts
?   ??? Mappings/          # AutoMapper Profiles
?   ??? Extensions/        # Helper Extensions
??? Domain/
?   ??? Entities/          # Database Models
?   ??? Interfaces/        # Repository Contracts
?   ??? Model/             # DTOs
??? Infra/
?   ??? Repositories/      # Data Access
?   ??? ApplicationDbContext.cs
??? Areas/Admin/           # Admin Panel
```

### Frontend
```
??? wwwroot/
?   ??? css/
?   ?   ??? vazirmatn-font.css
?   ?   ??? modern-design.css    # Design System
?   ??? js/
?       ??? modern-app.js         # Utilities & Interactions
??? Views/
    ??? Home/
    ?   ??? Index.cshtml          # Modern Dashboard
    ??? Account/
    ?   ??? Login.cshtml
    ?   ??? Register.cshtml
    ??? Shared/
        ??? _Layout.cshtml
```

---

## ?? Design System

### —‰êùÂ«

```css
/* Light Mode */
--primary: #6366f1;    /* Indigo */
--secondary: #8b5cf6;  /* Purple */
--success: #10b981;    /* Green */
--danger: #ef4444;     /* Red */

/* Dark Mode */
--background: #0f172a;
--surface: #1e293b;
```

### Typography

```css
font-family: 'Vazirmatn', sans-serif;
font-sizes: 0.875rem - 2rem
font-weights: 400, 500, 600, 700
```

---

## ?? Åò?ÃùÂ«

### Backend
- **ASP.NET Core 8.0** - Framework
- **Entity Framework Core 8.0** - ORM
- **BCrypt.Net-Next** - Password Hashing
- **AutoMapper** - Object Mapping
- **Serilog** - Logging

### Frontend
- **Bootstrap 5.3** - Grid System (compatibility)
- **Font Awesome 6** - Icons
- **Custom Design System** - Modern UI

---

## ??  ‰Ÿ?„« 

### appsettings.json

```json
{
  "ConnectionStrings": {
    "ApplicationDbContextConnection": "Server=...;Database=ToDoApp;..."
  },
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "File", "Args": { "path": "Logs/log-.txt" } }
    ]
  }
}
```

---

## ?? Performance

- **Bundle Size:** ~43 KB (CSS + JS)
- **First Paint:** < 1s
- **Interactive:** < 2s
- **Lighthouse Score:** 95+

---

## ?? „‘«—ò 

«?‰ Å—ÊéÂ Open Source «”  Ê „‘«—ò  ‘„« —« ŒÊ‘ù¬„œ „?ùêÊ??„!

```bash
# 1. Fork ò‰?œ
# 2. Branch Ãœ?œ »”«“?œ
git checkout -b feature/amazing-feature

# 3.  €??—«  —« Commit ò‰?œ
git commit -m 'Add amazing feature'

# 4. Push ò‰?œ
git push origin feature/amazing-feature

# 5. Pull Request »«“ ò‰?œ
```

---

## ?? ‰”ŒÂùÂ«

### v2.0.0 (2025-01-16)
- ?? ÿ—«Õ? ò«„· UI/UX
- ?? «÷«›Â ‘œ‰ Dark Mode
- ?? Dashboard Ãœ?œ
- ? »Â»Êœ Performance
- ?? «›“«?‘ «„‰?  »« BCrypt
- ??? „⁄„«—? Repository Pattern

### v1.0.0 (2024-11-04)
- ?? ‰”ŒÂ «Ê·?Â
- ? CRUD  ”òùÂ«
- ??  ﬁÊ?„ Â› ê?
- ?? „œ?—?  ò«—»—«‰

---

## ?? „ÃÊ“

«?‰ Å—ÊéÂ  Õ  „ÃÊ“ MIT „‰ ‘— ‘œÂ «” .

---

## ??  ?„  Ê”⁄Â

- **Parsa Panahpoor** -  Ê”⁄ÂùœÂ‰œÂ «’·?
- **GitHub Copilot** - ò„ò œ— UI/UX Ê „⁄„«—?

---

## ?? «— »«ÿ

- ?? «?„?·: parsapanahpoor@gmail.com
- ?? Ê»”«? : [parsapanahpoor.com](https://parsapanahpoor.com)
- ?? LinkedIn: [Parsa Panahpoor](https://linkedin.com/in/parsapanahpoor)

---

## ??  ‘ò—

„„‰Ê‰ òÂ «“ ToDoApp «” ›«œÂ „?ùò‰?œ!

«ê— Å—ÊéÂ —« œÊ”  œ«‘ ?œ° ? «” «— »œÂ?œ!

---

<div align="center">

**”«Œ Â ‘œÂ »« ?? œ— «?—«‰**

![Made in Iran](https://img.shields.io/badge/Made_in-Iran-green?style=for-the-badge)

</div>
