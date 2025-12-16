namespace ToDoApp.Application.Resources;

public static class Translations
{
    public static class Common
    {
        public static string AppName => GetText("ToDoApp", "ToDoApp");
        public static string Welcome => GetText("ÎæÔ ÂãÏ?Ï", "Welcome");
        public static string Hello => GetText("ÓáÇã", "Hello");
    }

    public static class Navigation
    {
        public static string Home => GetText("ÎÇäå", "Home");
        public static string Dashboard => GetText("ÏÇÔÈæÑÏ", "Dashboard");
        public static string Calendar => GetText("ÊŞæ?ã", "Calendar");
        public static string Tasks => GetText("ÊÓ˜åÇ", "Tasks");
        public static string Categories => GetText("ÏÓÊåÈäÏ?åÇ", "Categories");
        public static string Profile => GetText("ÑæİÇ?á", "Profile");
        public static string Settings => GetText("ÊäÙ?ãÇÊ", "Settings");
        public static string Logout => GetText("ÎÑæÌ", "Logout");
    }

    public static class Landing
    {
        public static string Title => GetText("Èå ToDoApp ÎæÔ ÂãÏ?Ï", "Welcome to ToDoApp");
        public static string Subtitle => GetText("ÓÇÏåÊÑ?ä æ Ò?ÈÇÊÑ?ä ÑÇå ÈÑÇ? ãÏ?Ñ?Ê ˜ÇÑåÇ? ÑæÒÇäå ÔãÇ", "The easiest and most beautiful way to manage your daily tasks");
        public static string GetStarted => GetText("ÔÑæÚ ˜ä?Ï", "Get Started");
        public static string Login => GetText("æÑæÏ", "Login");
        public static string Register => GetText("ËÈÊ äÇã", "Sign Up");
        public static string LearnMore => GetText("È?ÔÊÑ ÈÏÇä?Ï", "Learn More");
        public static string HeroTitle => GetText("ãÏ?Ñ?Ê åæÔãäÏ ˜ÇÑåÇ? ÑæÒÇäå", "Smart Daily Task Management");
        public static string HeroDescription => GetText("ÈÇ ToDoApp¡ ÊÓ˜åÇ? ÎæÏ ÑÇ ÓÇÒãÇäÏå? ˜ä?Ï¡ Çæáæ?ÊÈäÏ? ˜ä?Ï æ Èå ãæİŞ?Ê ÈÑÓ?Ï", "Organize, prioritize and succeed with ToDoApp");
    }

    public static class Features
    {
        public static string FastTitle => GetText("ÓÑ?Ú æ ÂÓÇä", "Fast & Easy");
        public static string FastDescription => GetText("ÑÇÈØ ˜ÇÑÈÑ? ÓÇÏå æ ÓÑ?Ú ÈÑÇ? ãÏ?Ñ?Ê ÈåÊÑ æŞÊ ÔãÇ", "Simple and fast interface for better time management");
        
        public static string ModernTitle => GetText("ØÑÇÍ? ãÏÑä", "Modern Design");
        public static string ModernDescription => GetText("UI/UX Ò?ÈÇ æ ˜ÇÑÈÑÓäÏ ÈÇ Dark Mode", "Beautiful UI/UX with Dark Mode support");
        
        public static string ResponsiveTitle => GetText("æÇ˜äÔÑÇ", "Responsive");
        public static string ResponsiveDescription => GetText("ÏÑ ÊãÇã ÏÓÊÇååÇ Èå ÎæÈ? ˜ÇÑ ã?˜äÏ", "Works perfectly on all devices");
        
        public static string SecureTitle => GetText("Çãä", "Secure");
        public static string SecureDescription => GetText("ÏÇÏååÇ? ÔãÇ ÈÇ Çãä?Ê ÈÇáÇ ãÍÇİÙÊ ã?ÔæäÏ", "Your data is protected with high security");
        
        public static string OrganizedTitle => GetText("ÓÇÒãÇäÏå? ÔÏå", "Organized");
        public static string OrganizedDescription => GetText("ÏÓÊåÈäÏ?¡ Çæáæ?ÊÈäÏ? æ ÊŞæ?ã åİÊ?", "Categories, priorities and weekly calendar");
        
        public static string NotificationsTitle => GetText("?ÇÏÂæÑ? åæÔãäÏ", "Smart Reminders");
        public static string NotificationsDescription => GetText("åÑÒ ˜ÇÑ? ÑÇ İÑÇãæÔ ä˜ä?Ï", "Never forget a task");
    }

    public static class Auth
    {
        public static string Login => GetText("æÑæÏ", "Login");
        public static string Register => GetText("ËÈÊ äÇã", "Sign Up");
        public static string PhoneNumber => GetText("ÔãÇÑå ãæÈÇ?á", "Phone Number");
        public static string Password => GetText("ÑãÒ ÚÈæÑ", "Password");
        public static string ForgotPassword => GetText("ÑãÒ ÚÈæÑ ÑÇ İÑÇãæÔ ˜ÑÏåÇ?Ï¿", "Forgot password?");
        public static string NoAccount => GetText("ÍÓÇÈ ˜ÇÑÈÑ? äÏÇÑ?Ï¿", "Don't have an account?");
        public static string HasAccount => GetText("ŞÈáÇğ ËÈÊ äÇã ˜ÑÏåÇ?Ï¿", "Already registered?");
        public static string BackToHome => GetText("ÈÇÒÔÊ Èå ÕİÍå ÇÕá?", "Back to Home");
        public static string LoginTitle => GetText("ÎæÔ ÂãÏ?Ï", "Welcome Back");
        public static string LoginSubtitle => GetText("ÈÑÇ? ÇÏÇãå æÇÑÏ ÍÓÇÈ ÎæÏ Ôæ?Ï", "Sign in to continue");
        public static string RegisterTitle => GetText("ËÈÊ äÇã", "Create Account");
        public static string RegisterSubtitle => GetText("ÈÑÇ? ÔÑæÚ ÍÓÇÈ ˜ÇÑÈÑ? ÈÓÇÒ?Ï", "Create an account to get started");
    }

    public static class Dashboard
    {
        public static string Welcome(string name) => GetText($"ÓáÇã¡ {name}", $"Hello, {name}");
        public static string Subtitle => GetText("È?Ç??Ï ÇãÑæÒ ÑÇ ÑÇäÑ? ÔÑæÚ ˜ä?ã!", "Let's make today productive!");
        public static string ActiveTasks => GetText("ÊÓ˜åÇ? İÚÇá", "Active Tasks");
        public static string TodayTasks => GetText("ÊÓ˜åÇ? ÇãÑæÒ", "Today's Tasks");
        public static string CompletedTasks => GetText("Ê˜ã?á ÔÏå", "Completed");
        public static string SuccessRate => GetText("ÏÑÕÏ ãæİŞ?Ê", "Success Rate");
        public static string WeekProgress => GetText("?ÔÑİÊ Ç?ä åİÊå", "This Week's Progress");
        public static string QuickActions => GetText("ÏÓÊÑÓ? ÓÑ?Ú", "Quick Actions");
        public static string DailyTip => GetText("ä˜Êå ÑæÒ", "Daily Tip");
        public static string AddTask => GetText("ÇİÒæÏä ÊÓ˜", "Add Task");
        public static string NoTasksToday => GetText("ÊÓ˜? ÈÑÇ? ÇãÑæÒ æÌæÏ äÏÇÑÏ!", "No tasks for today!");
        public static string TipMessage => GetText("ÈÑÇ? ÈåÑåæÑ? È?ÔÊÑ¡ ÊÓ˜åÇ? ãåã ÑÇ ÏÑ ÇÈÊÏÇ? ÑæÒ ÇäÌÇã Ïå?Ï!", "For better productivity, do important tasks early in the day!");
    }

    public static class Actions
    {
        public static string Add => GetText("ÇİÒæÏä", "Add");
        public static string Edit => GetText("æ?ÑÇ?Ô", "Edit");
        public static string Delete => GetText("ÍĞİ", "Delete");
        public static string Save => GetText("ĞÎ?Ñå", "Save");
        public static string Cancel => GetText("áÛæ", "Cancel");
        public static string Search => GetText("ÌÓÊÌæ", "Search");
        public static string ViewAll => GetText("ãÔÇåÏå åãå", "View All");
        public static string Refresh => GetText("ÈåÑæÒÑÓÇä?", "Refresh");
    }

    public static class Footer
    {
        public static string Rights => GetText("ÊãÇã? ÍŞæŞ ãÍİæÙ ÇÓÊ", "All rights reserved");
        public static string Privacy => GetText("ÍÑ?ã ÎÕæÕ?", "Privacy");
        public static string Terms => GetText("ÔÑÇ?Ø ÇÓÊİÇÏå", "Terms");
        public static string Contact => GetText("ÊãÇÓ ÈÇ ãÇ", "Contact Us");
    }

    // Helper method to get text based on current language
    private static string GetText(string persianText, string englishText)
    {
        var language = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
        return language.StartsWith("fa") ? persianText : englishText;
    }

    // Method to set language
    public static void SetLanguage(string languageCode)
    {
        var culture = new System.Globalization.CultureInfo(languageCode);
        System.Threading.Thread.CurrentThread.CurrentCulture = culture;
        System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
    }

    // Get current language
    public static string CurrentLanguage => System.Threading.Thread.CurrentThread.CurrentCulture.Name.StartsWith("fa") ? "fa" : "en";
    public static bool IsPersian => CurrentLanguage == "fa";
    public static bool IsEnglish => CurrentLanguage == "en";
}
