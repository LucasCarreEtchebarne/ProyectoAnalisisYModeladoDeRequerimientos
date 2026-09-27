(function () {
    const modalElemento = document.getElementById('modalConfirmacion');
    if (!modalElemento) return;

    const modal = new bootstrap.Modal(modalElemento);
    const texto = document.getElementById('modalConfirmacionTexto');
    const aceptar = document.getElementById('modalConfirmacionAceptar');
    let formularioPendiente = null;

    document.addEventListener('submit', function (evento) {
        const formulario = evento.target;
        if (!formulario.matches('form[data-confirmar]') || formulario.dataset.confirmado === 'si') return;

        evento.preventDefault();
        formularioPendiente = formulario;
        texto.textContent = formulario.dataset.confirmar;
        aceptar.textContent = formulario.dataset.confirmarBoton || 'Confirmar';
        modal.show();
    });

    aceptar.addEventListener('click', function () {
        if (!formularioPendiente) return;
        formularioPendiente.dataset.confirmado = 'si';
        modal.hide();
        formularioPendiente.submit();
    });
})();
