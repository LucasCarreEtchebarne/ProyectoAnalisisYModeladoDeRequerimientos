// Los mensajes de éxito se cierran solos después de 5 segundos
document.querySelectorAll('.alert-success.alert-dismissible').forEach(function (alerta) {
    setTimeout(function () {
        bootstrap.Alert.getOrCreateInstance(alerta).close();
    }, 5000);
});

// Modal de confirmación para formularios con data-confirmar
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
