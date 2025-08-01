let dotNetHelper = null;
function onDocumentClick(e) {
    const dropdown = document.querySelector('.profile-dropdown');
    if (!dropdown) return;
    if (!dropdown.contains(e.target)) {
        if (dotNetHelper) dotNetHelper.invokeMethodAsync('CloseProfileDropdown');
    }
}

export function registerDropdownClose(dotNetObjRef) {
    dotNetHelper = dotNetObjRef;
    document.addEventListener('mousedown', onDocumentClick);
}

export function disposeDropdownClose() {
    document.removeEventListener('mousedown', onDocumentClick);
    dotNetHelper = null;
}
