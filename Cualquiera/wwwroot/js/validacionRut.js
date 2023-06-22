$(document).ready(function () {
    var rutInput = $('#Rut');

    rutInput.on('input', function () {
        var rut = rutInput.val();

        $.ajax({
            url: '/Secretarios/ValidarRut',
            type: 'POST',
            data: { rut: rut },
            success: function (result) {
                if (result.isValid) {
                    rutInput.siblings('.text-danger').empty();
                } else {
                    rutInput.siblings('.text-danger').text('El Rut ingresado no es válido.');
                }
            }
        });
    });
});