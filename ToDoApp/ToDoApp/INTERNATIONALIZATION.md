# ?? Internationalization (i18n) Implementation

## ?? Œ·«’Â

”?” „ ç‰œ“»«‰ê? ò«„· »« Å‘ ?»«‰? «“ ›«—”? Ê «‰ê·?”? Å?«œÂù”«“? ‘œ!

---

## ? Ê?éê?ùÂ«? Å?«œÂù”«“? ‘œÂ:

### 1?? **”?” „ Translation**
- ? ò·«” `Translations` »« œ” Âù»‰œ? „‰Ÿ„
- ? Å‘ ?»«‰? «“ ›«—”? Ê «‰ê·?”?
- ? ”«“„«‰œÂ? ‘œÂ œ— Categories
- ? Type-safe (»œÊ‰ Magic Strings)

### 2?? **Language Middleware**
- ?  ‘Œ?’ ŒÊœò«— “»«‰ «“:
  - Query String (`?lang=fa`)
  - Cookie (–Œ?—Â  —Ã?Õ« )
  - Accept-Language Header
  - Default Language (fa)
- ? –Œ?—Â œ— HttpContext
- ? RTL/LTR Detection
- ? Culture Info Management

### 3?? **Landing Page „œ—‰**
- ? ÿ—«Õ? Õ—›Âù«? Ê Œ·«ﬁ«‰Â
- ? Animated Background
- ? 6 Feature Cards
- ? Responsive Design
- ? Parallax Effect
- ? Language Switcher
- ? Å‘ ?»«‰? ò«„· «“ RTL/LTR

### 4?? **UI Components**
- ? Dashboard »« Translations
- ? Navigation »« Language Switcher
- ? Login Page »« “»«‰ùÂ«? „Œ ·›
- ?  „«„ „ Ê‰ ﬁ«»·  —Ã„Â

---

## ?? ”«Œ «— ›«?·ùÂ«:

```
Application/
??? Resources/
?   ??? Translations.cs               ? ”?” „  —Ã„Â
??? Middleware/
    ??? LanguageMiddleware.cs         ? Middleware ç‰œ“»«‰ê?

Views/
??? Home/
?   ??? Index.cshtml                  ? »« Translations
?   ??? _LandingPage.cshtml           ? ’›ÕÂ Welcome „œ—‰
??? Account/
?   ??? Login.cshtml                  ? »« Translations
??? Shared/
    ??? _Layout.cshtml                ? »« Language Switcher

wwwroot/js/
??? dashboard.js                      ? „œ?—?  Dashboard
```

---

## ?? ‰ÕÊÂ «” ›«œÂ:

### **1. «” ›«œÂ «“ Translations œ— View:**

```csharp
@using ToDoApp.Application.Resources

<h1>@Translations.Common.Welcome</h1>
<p>@Translations.Landing.Subtitle</p>
<button>@Translations.Actions.Save</button>
```

### **2. «” ›«œÂ «“ Translations œ— Controller:**

```csharp
using ToDoApp.Application.Resources;

public IActionResult Index()
{
    ViewBag.Title = Translations.Navigation.Dashboard;
    ViewBag.Message = Translations.Dashboard.Welcome(User.Identity.Name);
    return View();
}
```

### **3. œ—?«›  “»«‰ ›⁄·?:**

```csharp
var language = Translations.CurrentLanguage;  // "fa" or "en"
var isPersian = Translations.IsPersian;       // true/false
var isEnglish = Translations.IsEnglish;       // true/false
```

### **4.  €??— “»«‰:**

```html
<!-- Query String -->
<a href="?lang=fa">›«—”?</a>
<a href="?lang=en">English</a>

<!-- JavaScript -->
<script>
function changeLanguage(lang) {
    window.location.href = '?lang=' + lang;
}
</script>
```

---

## ?? œ” Âù»‰œ? Translations:

### **Common:**
- AppName
- Welcome
- Hello

### **Navigation:**
- Home
- Dashboard
- Calendar
- Tasks
- Categories
- Profile
- Settings
- Logout

### **Landing:**
- Title
- Subtitle
- GetStarted
- Login
- Register
- LearnMore
- HeroTitle
- HeroDescription

### **Features:**
- FastTitle / FastDescription
- ModernTitle / ModernDescription
- ResponsiveTitle / ResponsiveDescription
- SecureTitle / SecureDescription
- OrganizedTitle / OrganizedDescription
- NotificationsTitle / NotificationsDescription

### **Auth:**
- Login
- Register
- PhoneNumber
- Password
- ForgotPassword
- NoAccount
- HasAccount
- BackToHome
- LoginTitle
- LoginSubtitle
- RegisterTitle
- RegisterSubtitle

### **Dashboard:**
- Welcome(name)
- Subtitle
- ActiveTasks
- TodayTasks
- CompletedTasks
- SuccessRate
- WeekProgress
- QuickActions
- DailyTip
- AddTask
- NoTasksToday
- TipMessage

### **Actions:**
- Add
- Edit
- Delete
- Save
- Cancel
- Search
- ViewAll
- Refresh

### **Footer:**
- Rights
- Privacy
- Terms
- Contact

---

## ?? Landing Page Features:

### **Visual Elements:**
1. **Animated Background** - Õ—ò  ‰ﬁÿÂùÂ«
2. **Hero Section** - ⁄‰Ê«‰ Ê  Ê÷?Õ« 
3. **CTA Buttons** - œò„ÂùÂ«? »“—ê Ê “?»«
4. **Feature Cards** - 6 ò«—  »« Glassmorphism
5. **Scroll Indicator** - ‰‘«‰ê— «”ò—Ê·
6. **Parallax Effect** - «›ò  „Ê«“? œ— «”ò—Ê·

### **Interactive Elements:**
1. **Language Switcher** -  €??— “»«‰
2. **Hover Effects** - «‰?„?‘‰ —Ê? ò«— ùÂ«
3. **Smooth Animations** - fade in Ê bounce

---

## ?? Configuration:

### **Supported Languages:**
```csharp
private static readonly string[] SupportedLanguages = { "fa", "en" };
private const string DefaultLanguage = "fa";
```

### **Cookie Settings:**
```csharp
var cookieOptions = new CookieOptions
{
    Expires = DateTimeOffset.UtcNow.AddYears(1),
    HttpOnly = true,
    Secure = true,
    SameSite = SameSiteMode.Lax,
    Path = "/"
};
```

---

## ?? Language Detection Flow:

```
1. Check Query String (?lang=fa)
   ? Not found
2. Check Cookie (ToDoApp.Language)
   ? Not found
3. Check Accept-Language Header
   ? Not found
4. Use Default Language (fa)
```

---

## ?? RTL/LTR Support:

### **Automatic Detection:**
```csharp
context.Items["IsRTL"] = language == "fa";
```

### **Usage in HTML:**
```html
<html dir="@(Translations.IsPersian ? "rtl" : "ltr")">
```

### **CSS Adjustments:**
```css
/* Conditional Styles */
style="margin-@(isPersian ? "right" : "left"): 2rem;"
style="padding: 0.5rem @(isPersian ? "2.5rem 0.5rem 1rem" : "1rem 0.5rem 2.5rem");"
```

---

## ?? «÷«›Â ò—œ‰ “»«‰ Ãœ?œ:

### **„—Õ·Â 1: «÷«›Â ò—œ‰ »Â SupportedLanguages**
```csharp
private static readonly string[] SupportedLanguages = { "fa", "en", "ar" };
```

### **„—Õ·Â 2: «÷«›Â ò—œ‰ Translation**
```csharp
private static string GetText(string persianText, string englishText, string arabicText)
{
    var language = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
    
    if (language.StartsWith("fa")) return persianText;
    if (language.StartsWith("ar")) return arabicText;
    return englishText; // Default
}
```

### **„—Õ·Â 3: »—Ê“—”«‰? UI**
```html
<a href="?lang=ar">«·⁄—»Ì…</a>
```

---

## ?? „“«?«:

### **? Developer Experience:**
- Type-safe (IntelliSense support)
- ”«œÂ Ê ”«“„«‰œÂ? ‘œÂ
- »œÊ‰ ‰?«“ »Â Resource Files Å?ç?œÂ
- ﬁ«»·  Ê”⁄Â

### **? User Experience:**
-  €??— ”—?⁄ “»«‰
- –Œ?—Â  —Ã?Õ« 
- RTL/LTR ŒÊœò«—
- UI/UX ?òÅ«—çÂ

### **? Performance:**
- »œÊ‰ overhead «÷«›?
- ò‘ ‘œÂ œ— Memory
- ”—?⁄ Ê ò«—¬„œ

---

## ?? Best Practices:

### **1. Â„?‘Â «“ Translations «” ›«œÂ ò‰?œ:**
```csharp
// ? »œ
<h1>ŒÊ‘ ¬„œ?œ</h1>

// ? ŒÊ»
<h1>@Translations.Common.Welcome</h1>
```

### **2. »—«? „ ‰ùÂ«? Dynamic «“ „ œ «” ›«œÂ ò‰?œ:**
```csharp
// ? ŒÊ»
public static string Welcome(string name) => 
    GetText($"”·«„° {name}", $"Hello, {name}");
```

### **3. “»«‰ —« œ— Layout çò ò‰?œ:**
```csharp
@using ToDoApp.Application.Resources
var isPersian = Translations.IsPersian;
```

### **4. Language Switcher —« œ—  „«„ ’›Õ«  ﬁ—«— œÂ?œ:**
```html
<div class="lang-switcher">
    <a href="?lang=fa">›«—”?</a>
    <a href="?lang=en">English</a>
</div>
```

---

## ?? ¬?‰œÂ (Phase 2):

### **Å?‘‰Â«œ« :**
1. **Resource Files** - »—«?  —Ã„ÂùÂ«? Å?ç?œÂù —
2. **Database Translations** - »—«? „Õ Ê«? œ«?‰«„?ò
3. **Translation Management UI** - Ê?—«?‘  —Ã„ÂùÂ« œ— Admin Panel
4. **Pluralization Support** - „œ?—?  Ã„⁄/„›—œ
5. **Date/Time Localization** - ›—„   «—?Œ »Ê„?
6. **Number Formatting** - ›—„  «⁄œ«œ »Ê„?
7. **Currency Formatting** - ‰„«?‘ «—“
8. **More Languages** - ⁄—»?°  —ò?° ...

---

## ?? ‰ ?ÃÂùê?—?:

? ”?” „ ç‰œ“»«‰ê? ò«„· Ê Õ—›Âù«?
? Landing Page „œ—‰ Ê “?»«
? Å‘ ?»«‰? ò«„· «“ RTL/LTR
?  Ã—»Â ò«—»—? ?òÅ«—çÂ
? òœ  „?“ Ê ﬁ«»· ‰êÂœ«—?
? ¬„«œÂ »—«?  Ê”⁄Â »?‘ —

---

** «—?Œ:** 2025-01-16
**‰”ŒÂ:** 2.1.0
**Ê÷⁄? :** ? ò«„· Ê ¬„«œÂ «” ›«œÂ

---

Made with ?? by GitHub Copilot
