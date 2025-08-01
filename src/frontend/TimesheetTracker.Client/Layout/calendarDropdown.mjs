// Use .mjs extension for ES module compatibility if needed
export function registerCalendarDropdownClose(dotNetHelper) {
    function onClick(event) {
        dotNetHelper.invokeMethodAsync('CloseCalendarDropdownFromJs');
    }
    document.addEventListener('click', onClick);
    window._calendarDropdownCleanup = () => {
        document.removeEventListener('click', onClick);
    };
}

export function disposeCalendarDropdownClose() {
    if (window._calendarDropdownCleanup) {
        window._calendarDropdownCleanup();
        window._calendarDropdownCleanup = null;
    }
}
