// https://www.w3schools.com/js/js_cookies.asp
function setCookie(cookieName, cookieValue, expireMinute) {
    var d = new Date();
    d.setTime(d.getTime() + (expireMinute * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cookieName + "=" + cookieValue + ";" + expires + ";path=/";
}

// https://www.w3schools.com/js/js_cookies.asp
function getCookie(cookieName) {
    var name = cookieName + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

// https://www.w3schools.com/js/js_cookies.asp
function deleteCookie(cookieName) {
    document.cookie = cookieName + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
}

// https://stackoverflow.com/questions/60494746/blazor-navigation-update-url-without-changing-reloading-page
// https://stackoverflow.com/questions/3338642/updating-address-bar-with-new-url-without-hash-or-reloading-the-page
function changeAddressBarUrl(newUrl) {
    history.pushState(null, "", newUrl);
}

// https://www.w3schools.com/howto/howto_js_add_class.asp
function addClassToElementById(elementId, className) {
    var element = document.getElementById(elementId);
    if (element) {
        element.classList.add(className);
    } else {
        console.log("Element with id=" + elementId + " not found. Error occurred in BlazorExtensions.js file and addClassToElementById method.");
    }
}

function removeClassFromElementById(elementId, className) {
    var element = document.getElementById(elementId);
    if (element) {
        element.classList.remove(className);
    }
    else {
        console.log("Element with id=" + elementId + " not found. Error occurred in BlazorExtensions.js file and removeClassFromElementById method.");
    }
}

// My signature in console
function addSignature() {
    console.log('%c Developed by %cATA Software Development Unit', 'font-weight:bold; font-size:25px; color: blue;', 'font-weight:bold; font-size:25px;letter-spacing:3px;color:green;text-shadow:1px 1px 0px black, 1px -1px 0px black, -1px 1px 0px black, -1px -1px 0x black;');
    console.log('contact us: https://www.linkedin.com/in/farshad-davoudi/');
}

// Clear console
function clearConsole() {
    console.clear();
}

function setFocus(elementId) {
    document.getElementById(elementId).focus();
}