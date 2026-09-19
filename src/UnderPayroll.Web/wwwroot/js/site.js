// Comportamiento común a todas las páginas.

// Los mensajes de resultado se cierran solos a los pocos segundos.
document.querySelectorAll(".flash").forEach(function (alerta) {
    setTimeout(function () {
        bootstrap.Alert.getOrCreateInstance(alerta).close();
    }, 6000);
});

// Alternancia entre tema claro y oscuro. La preferencia se guarda en el
// navegador; el tema inicial se aplica desde el head del layout.
(function () {
    var boton = document.getElementById("themeToggle");
    var raiz = document.documentElement;

    function actualizarEtiqueta() {
        var esOscuro = raiz.getAttribute("data-bs-theme") === "dark";
        boton.textContent = esOscuro ? "Tema claro" : "Tema oscuro";
    }

    boton.addEventListener("click", function () {
        var nuevo = raiz.getAttribute("data-bs-theme") === "dark" ? "light" : "dark";
        raiz.setAttribute("data-bs-theme", nuevo);
        try { localStorage.setItem("tema", nuevo); } catch (e) { }
        actualizarEtiqueta();
    });

    actualizarEtiqueta();
})();
