// Menüyü açma ve kapama fonksiyonları
function toggleMenu() {
    var menu = document.getElementById("menu");
    menu.classList.toggle("show-menu");
}

// Mouse ile navbar üzerine gelindiğinde menüyü aç
document.querySelector(".menu-icon").addEventListener("mouseenter", function () {
    var menu = document.getElementById("menu");
    menu.classList.add("show-menu");
});

// Mouse navbar üzerinden çıktığında menüyü kapat
document.querySelector(".menu").addEventListener("mouseleave", function () {
    var menu = document.getElementById("menu");
    menu.classList.remove("show-menu");
});