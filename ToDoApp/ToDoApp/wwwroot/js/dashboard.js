// ========================================
// ?? Dashboard Management
// ========================================

class DashboardManager {
    constructor() {
        this.init();
    }

    async init() {
        await this.loadDashboardData();
        this.setupEventListeners();
    }

    async loadDashboardData() {
        try {
            // Simulated data - œ— ‰”ŒÂ Ê«ﬁ⁄? «“ API œ—?«›  „?ù‘Êœ
            const stats = {
                totalTasks: 15,
                todayTasks: 3,
                completedTasks: 12,
                completedThisWeek: 8,
                totalThisWeek: 12
            };

            // Update stats
            this.updateStats(stats);
            
            // Load today's tasks
            await this.loadTodayTasks();
            
            // Load categories
            await this.loadCategories();
        } catch (error) {
            console.error('Error loading dashboard data:', error);
            window.toast?.error('Œÿ« œ— »«—ê–«—? œ«œÂùÂ«');
        }
    }

    updateStats(stats) {
        document.getElementById('totalTasks').textContent = stats.totalTasks;
        document.getElementById('todayTasks').textContent = stats.todayTasks;
        document.getElementById('completedTasks').textContent = stats.completedTasks;
        
        const completionRate = Math.round((stats.completedTasks / stats.totalTasks) * 100);
        document.getElementById('completionRate').textContent = completionRate + '%';

        // Update week progress
        const weekProgress = Math.round((stats.completedThisWeek / stats.totalThisWeek) * 100);
        document.getElementById('weekProgress').textContent = weekProgress + '%';
        document.getElementById('completedThisWeek').textContent = stats.completedThisWeek;
        document.getElementById('totalThisWeek').textContent = stats.totalThisWeek;
        
        const progressBar = document.getElementById('weekProgressBar');
        progressBar.dataset.percent = weekProgress;
        window.modernUtils?.animateProgress(progressBar, weekProgress);
    }

    async loadTodayTasks() {
        // Simulated tasks - œ— production «“ API „?ù¬?œ
        const tasks = [
            { id: 1, title: 'Ã·”Â »«  ?„', priority: 'high', time: '10:00', completed: false },
            { id: 2, title: 'òœ‰Ê?”? Å—ÊéÂ', priority: 'medium', time: '14:00', completed: false },
            { id: 3, title: 'Ê—“‘ ò—œ‰', priority: 'low', time: '18:00', completed: true }
        ];

        const container = document.getElementById('todayTasksList');
        
        if (tasks.length === 0) {
            container.innerHTML = `
                <div style="text-align: center; padding: 2rem; color: var(--text-secondary);">
                    <div style="font-size: 3rem; margin-bottom: 1rem;">??</div>
                    <p>${window.translations?.noTasksToday || ' ”ò? »—«? «„—Ê“ ÊÃÊœ ‰œ«—œ!'}</p>
                    <button class="btn-modern btn-primary mt-2" onclick="dashboardManager.openQuickAddTask()">
                        <i class="fa fa-plus"></i>
                        ${window.translations?.addFirstTask || '«›“Êœ‰ «Ê·?‰  ”ò'}
                    </button>
                </div>
            `;
            return;
        }

        container.innerHTML = tasks.map(task => `
            <div class="task-card ${task.completed ? 'completed' : ''}">
                <div class="task-priority priority-${task.priority}"></div>
                <div class="flex items-center gap-md" style="padding-right: 1rem;">
                    <div class="modern-checkbox ${task.completed ? 'checked' : ''}" 
                         onclick="dashboardManager.toggleTask(${task.id})"></div>
                    <div style="flex: 1;">
                        <div class="task-title" style="font-weight: 500; color: var(--text-primary); margin-bottom: 0.25rem;">
                            ${task.title}
                        </div>
                        <div style="font-size: 0.875rem; color: var(--text-secondary);">
                            <i class="fa fa-clock-o"></i> ${task.time}
                        </div>
                    </div>
                    <button class="btn-modern btn-icon btn-ghost" onclick="dashboardManager.editTask(${task.id})">
                        <i class="fa fa-pencil"></i>
                    </button>
                </div>
            </div>
        `).join('');
    }

    async loadCategories() {
        const categories = [
            { id: 1, title: '?? ò«—?', count: 8, color: '#6366f1' },
            { id: 2, title: '??? Ê—“‘', count: 3, color: '#10b981' },
            { id: 3, title: '?? „ÿ«·⁄Â', count: 5, color: '#f59e0b' }
        ];

        const container = document.getElementById('categoriesList');
        container.innerHTML = categories.map(cat => `
            <button class="btn-modern btn-secondary w-full" 
                    style="justify-content: space-between;"
                    onclick="dashboardManager.filterByCategory(${cat.id})">
                <span>${cat.title}</span>
                <span class="badge badge-primary">${cat.count}</span>
            </button>
        `).join('');
    }

    toggleTask(taskId) {
        console.log('Toggle task:', taskId);
        window.toast?.success('Ê÷⁄?   ”ò  €??— ò—œ');
        // «?‰Ã« »«?œ API call »“‰?œ
    }

    editTask(taskId) {
        console.log('Edit task:', taskId);
        window.location.href = '/Task/Edit/' + taskId;
    }

    openQuickAddTask() {
        window.location.href = '/Task/WeeklyCalendar';
    }

    filterByCategory(categoryId) {
        window.toast?.info('›?· — »— «”«” œ” Âù»‰œ?');
        // Implement category filtering
    }

    setupEventListeners() {
        // Refresh data every 5 minutes
        setInterval(() => {
            this.loadDashboardData();
        }, 5 * 60 * 1000);
    }
}

// Global functions for onclick handlers
function toggleTask(taskId) {
    dashboardManager.toggleTask(taskId);
}

function editTask(taskId) {
    dashboardManager.editTask(taskId);
}

function openQuickAddTask() {
    dashboardManager.openQuickAddTask();
}

function showAllTasks() {
    window.location.href = '/Task/WeeklyCalendar';
}

function showCategories() {
    window.toast?.info('«?‰ ﬁ«»·?  »Â “Êœ? «÷«›Â „?ù‘Êœ');
}

function filterByCategory(categoryId) {
    dashboardManager.filterByCategory(categoryId);
}

// Initialize dashboard on page load
let dashboardManager;
document.addEventListener('DOMContentLoaded', () => {
    dashboardManager = new DashboardManager();
});

console.log('?? Dashboard Manager loaded successfully!');
