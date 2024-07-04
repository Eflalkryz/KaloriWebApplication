
function toggleMenu() {
    var menu = document.getElementById("menu");
    menu.classList.toggle("show-menu");
}


document.querySelector(".menu-icon").addEventListener("mouseenter", function () {
    var menu = document.getElementById("menu");
    menu.classList.add("show-menu");
});


document.querySelector(".menu").addEventListener("mouseleave", function () {
    var menu = document.getElementById("menu");
    menu.classList.remove("show-menu");
});