function impr() {
    var tabla = document.getElementsByTagName("tr");
    var datos = []

    for (var i = 1; i < tabla.length; i++) {
        var nombre = tabla[i].getElementsByTagName("td")[0].textContent;
        var apellidos = tabla[i].getElementsByTagName("td")[1].textContent;
        var fechanac = tabla[i].getElementsByTagName("td")[2].textContent;
        var rut = tabla[i].getElementsByTagName("td")[3].textContent;
        var email = tabla[i].getElementsByTagName("td")[4].textContent;

        datos.push([
            nombre, apellidos, fechanac, rut, email
        ]);

        document.getElementById("donForm").datos.value = JSON.stringify(datos);

        document.getElementById("donForm").submit();
        
    }

}

document.getElementById("botoncito").addEventListener("click", impr);