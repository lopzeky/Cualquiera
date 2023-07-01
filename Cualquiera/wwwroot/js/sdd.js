function impr1() {
    var tabla = document.getElementsByTagName("tr");
    var datos1 = []

    for (var i = 1; i < tabla.length; i++) {
        var nombre = tabla[i].getElementsByTagName("td")[0].textContent;
        var apellidos = tabla[i].getElementsByTagName("td")[1].textContent;
        var fechanac = tabla[i].getElementsByTagName("td")[2].textContent;
        var rut = tabla[i].getElementsByTagName("td")[3].textContent;
        var email = tabla[i].getElementsByTagName("td")[4].textContent;
        var checkbox = tabla[i].getElementsByTagName("input")[0];
        var dis = checkbox.checked ? checkbox.value : 'off';

        datos1.push([
            nombre, apellidos, fechanac, rut, email, dis
        ]);

        document.getElementById("donForm1").datos1.value = JSON.stringify(datos1);

        document.getElementById("donForm1").submit();

    }

}

document.getElementById("botoncito1").addEventListener("click", impr1);