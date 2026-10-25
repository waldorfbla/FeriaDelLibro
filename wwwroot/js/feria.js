document.addEventListener("DOMContentLoaded", function () {
    // Total estimado en vivo (vista Reservar)
    const cantidad = document.getElementById("Cantidad");
    const total = document.getElementById("totalEstimado");

    if (cantidad && total) {
        const precio = parseFloat(cantidad.dataset.precio);
        const formato = new Intl.NumberFormat("es-CO", {
            style: "currency", currency: "COP", maximumFractionDigits: 0
        });

        const calcular = () => {
            const n = parseInt(cantidad.value) || 0;
            let subtotal = precio * n;
            if (n >= 3) subtotal *= 0.9;
            total.textContent = formato.format(subtotal);
        };

        cantidad.addEventListener("input", calcular);
        calcular();
    }

    // Confirmación antes de enviar (vista Crear)
    const formLibro = document.getElementById("formLibro");
    if (formLibro) {
        formLibro.addEventListener("submit", function (e) {
            if (!confirm("¿Deseas registrar este libro en el catálogo?")) {
                e.preventDefault();
            }
        });
    }
});