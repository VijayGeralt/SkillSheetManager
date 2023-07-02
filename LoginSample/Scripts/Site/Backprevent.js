function preventBack() { window.history.forward(); }
setTimeout("preventBack()", 0);
console.log("admin");
window.onunload = function () { null };