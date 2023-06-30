function impr() {
    var tabla = document.getElementsByTagName("tr");
    for (var i = 1; i < tabla.length; i++) {
        var nombre = tabla[i].getElementsByTagName("td")[0].textContent;
        var apellidos = tabla[i].getElementsByTagName("td")[1].textContent;
        var fechanac = tabla[i].getElementsByTagName("td")[2].textContent;
        var rut = tabla[i].getElementsByTagName("td")[3].textContent;
        var email = tabla[i].getElementsByTagName("td")[4].textContent;

        console.log("Nombre:", nombre);
        console.log("Apellidos:", apellidos);
        console.log("Fecha de Nacimiento:", fechanac);
        console.log("RUT:", rut);
        console.log("Email:", email);

        


    }
}

document.getElementById("botoncito").addEventListener("click", impr);