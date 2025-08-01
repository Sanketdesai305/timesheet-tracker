export function registerCalendarDropdownClose(dotNetHelper) {
    function onClick(event) {
        // Only close if click is outside the dropdown
        const dropdown = document.querySelector('.sidebar-dropdown');
        if (dropdown && !dropdown.contains(event.target)) {
            dotNetHelper.invokeMethodAsync('CloseCalendarDropdownFromJs');
        }
    }
    document.addEventListener('mousedown', onClick);
    window._calendarDropdownCleanup = () => {
        document.removeEventListener('mousedown', onClick);
    };
}

export function disposeCalendarDropdownClose() {
    if (window._calendarDropdownCleanup) {
        window._calendarDropdownCleanup();
        window._calendarDropdownCleanup = null;
    }
}
