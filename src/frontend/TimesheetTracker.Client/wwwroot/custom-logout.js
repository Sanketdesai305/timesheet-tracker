// custom-logout.js
window.customLogout = function () {
    console.log('customLogout called');
    localStorage.clear();
    sessionStorage.clear();
    window.location.replace('http://localhost:5044');
};
// custom-logout.js

