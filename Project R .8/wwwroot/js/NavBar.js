document.addEventListener("DOMContentLoaded", function () {
    const btn = document.getElementById("menuButton");

    btn.addEventListener("click", function () {
        document.getElementById("menu").classList.toggle("hidden");
        document.getElementById("bigMenu").classList.toggle("widthChanger");
        document.getElementById("menuButton").classList.toggle("widthChanger");
    });
});