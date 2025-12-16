// ========================================
// ?? Dark Mode Management
// ========================================

class ThemeManager {
    constructor() {
        this.theme = localStorage.getItem('theme') || 'light';
        this.init();
    }

    init() {
        this.applyTheme();
        this.setupListeners();
    }

    applyTheme() {
        document.documentElement.setAttribute('data-theme', this.theme);
        localStorage.setItem('theme', this.theme);
        
        // Update toggle button
        const toggles = document.querySelectorAll('.dark-mode-toggle');
        toggles.forEach(toggle => {
            toggle.classList.toggle('active', this.theme === 'dark');
        });
    }

    toggle() {
        this.theme = this.theme === 'light' ? 'dark' : 'light';
        this.applyTheme();
    }

    setupListeners() {
        document.addEventListener('click', (e) => {
            if (e.target.closest('.dark-mode-toggle')) {
                this.toggle();
            }
        });
    }
}

// Initialize theme manager
const themeManager = new ThemeManager();

// ========================================
// ?? Task Management
// ========================================

class TaskManager {
    constructor() {
        this.init();
    }

    init() {
        this.setupCheckboxes();
        this.setupAnimations();
    }

    setupCheckboxes() {
        document.addEventListener('click', (e) => {
            const checkbox = e.target.closest('.modern-checkbox');
            if (checkbox) {
                this.toggleCheckbox(checkbox);
            }
        });
    }

    toggleCheckbox(checkbox) {
        checkbox.classList.toggle('checked');
        const taskCard = checkbox.closest('.task-card');
        if (taskCard) {
            taskCard.classList.toggle('completed');
            this.animateCompletion(taskCard);
        }
    }

    animateCompletion(element) {
        element.style.animation = 'none';
        setTimeout(() => {
            element.style.animation = '';
            element.classList.add('animate-fade-in');
        }, 10);
    }

    setupAnimations() {
        // Intersection Observer for scroll animations
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('animate-fade-in');
                }
            });
        }, { threshold: 0.1 });

        document.querySelectorAll('.task-card, .stat-card').forEach(el => {
            observer.observe(el);
        });
    }
}

// Initialize task manager
const taskManager = new TaskManager();

// ========================================
// ?? Toast Notifications
// ========================================

class ToastManager {
    constructor() {
        this.container = this.createContainer();
    }

    createContainer() {
        let container = document.getElementById('toast-container');
        if (!container) {
            container = document.createElement('div');
            container.id = 'toast-container';
            container.style.cssText = `
                position: fixed;
                top: 1rem;
                left: 50%;
                transform: translateX(-50%);
                z-index: 9999;
                display: flex;
                flex-direction: column;
                gap: 0.5rem;
                pointer-events: none;
            `;
            document.body.appendChild(container);
        }
        return container;
    }

    show(message, type = 'info', duration = 3000) {
        const toast = document.createElement('div');
        toast.className = 'toast';
        
        const icons = {
            success: '?',
            error: '?',
            warning: '?',
            info: '?'
        };

        const colors = {
            success: 'var(--success)',
            error: 'var(--danger)',
            warning: 'var(--warning)',
            info: 'var(--info)'
        };

        toast.style.cssText = `
            background: var(--surface);
            color: var(--text-primary);
            padding: 1rem 1.5rem;
            border-radius: var(--radius-lg);
            box-shadow: var(--shadow-xl);
            display: flex;
            align-items: center;
            gap: 0.75rem;
            min-width: 300px;
            border-right: 4px solid ${colors[type]};
            pointer-events: all;
            animation: slideInRight 0.3s ease-out;
        `;

        toast.innerHTML = `
            <span style="
                width: 2rem;
                height: 2rem;
                border-radius: 50%;
                background: ${colors[type]};
                color: white;
                display: flex;
                align-items: center;
                justify-content: center;
                font-weight: bold;
            ">${icons[type]}</span>
            <span style="flex: 1;">${message}</span>
        `;

        this.container.appendChild(toast);

        setTimeout(() => {
            toast.style.animation = 'slideInLeft 0.3s ease-out reverse';
            setTimeout(() => toast.remove(), 300);
        }, duration);
    }

    success(message, duration) {
        this.show(message, 'success', duration);
    }

    error(message, duration) {
        this.show(message, 'error', duration);
    }

    warning(message, duration) {
        this.show(message, 'warning', duration);
    }

    info(message, duration) {
        this.show(message, 'info', duration);
    }
}

// Global toast instance
window.toast = new ToastManager();

// ========================================
// ?? Smooth Scroll
// ========================================

document.querySelectorAll('a[href^="#"]').forEach(anchor => {
    anchor.addEventListener('click', function (e) {
        e.preventDefault();
        const target = document.querySelector(this.getAttribute('href'));
        if (target) {
            target.scrollIntoView({
                behavior: 'smooth',
                block: 'start'
            });
        }
    });
});

// ========================================
// ?? Keyboard Shortcuts
// ========================================

document.addEventListener('keydown', (e) => {
    // Ctrl/Cmd + K - Quick search
    if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
        e.preventDefault();
        const searchInput = document.querySelector('input[type="search"]');
        if (searchInput) {
            searchInput.focus();
        }
    }

    // Ctrl/Cmd + N - New task
    if ((e.ctrlKey || e.metaKey) && e.key === 'n') {
        e.preventDefault();
        const addButton = document.querySelector('.fab');
        if (addButton) {
            addButton.click();
        }
    }

    // Ctrl/Cmd + D - Toggle dark mode
    if ((e.ctrlKey || e.metaKey) && e.key === 'd') {
        e.preventDefault();
        themeManager.toggle();
    }
});

// ========================================
// ?? Progress Animation
// ========================================

function animateProgress(element, targetPercent) {
    const fill = element.querySelector('.progress-fill');
    if (!fill) return;

    let currentPercent = 0;
    const increment = targetPercent / 50;
    
    const animation = setInterval(() => {
        currentPercent += increment;
        if (currentPercent >= targetPercent) {
            currentPercent = targetPercent;
            clearInterval(animation);
        }
        fill.style.width = currentPercent + '%';
    }, 20);
}

// Animate all progress bars on load
window.addEventListener('load', () => {
    document.querySelectorAll('.progress-bar').forEach(bar => {
        const targetPercent = parseFloat(bar.dataset.percent) || 0;
        animateProgress(bar, targetPercent);
    });
});

// ========================================
// ?? Utility Functions
// ========================================

// Format date to Persian
function formatPersianDate(date) {
    return new Intl.DateTimeFormat('fa-IR', {
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    }).format(new Date(date));
}

// Confirm dialog
async function confirmDialog(message) {
    return new Promise((resolve) => {
        const overlay = document.createElement('div');
        overlay.style.cssText = `
            position: fixed;
            inset: 0;
            background: rgba(0, 0, 0, 0.5);
            backdrop-filter: blur(4px);
            display: flex;
            align-items: center;
            justify-content: center;
            z-index: 9999;
            animation: fadeIn 0.2s ease-out;
        `;

        const dialog = document.createElement('div');
        dialog.style.cssText = `
            background: var(--surface);
            padding: 2rem;
            border-radius: var(--radius-xl);
            max-width: 400px;
            box-shadow: var(--shadow-2xl);
            animation: fadeIn 0.3s ease-out;
        `;

        dialog.innerHTML = `
            <h3 style="margin-bottom: 1rem; color: var(--text-primary);">สร??ฯ</h3>
            <p style="margin-bottom: 1.5rem; color: var(--text-secondary);">${message}</p>
            <div style="display: flex; gap: 0.5rem; justify-content: flex-end;">
                <button class="btn-modern btn-secondary" onclick="this.closest('.confirm-overlay').remove(); window.confirmResolve(false);">
                    แÛๆ
                </button>
                <button class="btn-modern btn-primary" onclick="this.closest('.confirm-overlay').remove(); window.confirmResolve(true);">
                    สร??ฯ
                </button>
            </div>
        `;

        overlay.className = 'confirm-overlay';
        overlay.appendChild(dialog);
        document.body.appendChild(overlay);

        window.confirmResolve = resolve;
    });
}

// Export utility functions
window.modernUtils = {
    toast,
    confirmDialog,
    formatPersianDate,
    animateProgress
};

console.log('?? Modern Design System loaded successfully!');
