<?php

/* =========================
   CONEXIÓN A LA BASE DE DATOS
========================= */
$conn = mysqli_connect("localhost", "root", "root", "mi_banco_db");

if (!$conn) {
    die("Error de conexión: " . mysqli_connect_error());
}

/* =========================
   DATOS DEL FORMULARIO
========================= */
$documento = trim($_POST['documento'] ?? '');
$usuario = trim($_POST['usuario'] ?? '');
$pass1 = trim($_POST['passwordA'] ?? '');
$pass2 = trim($_POST['passwordB'] ?? '');
$nombre = trim($_POST['nombre'] ?? '');
$apellido = trim($_POST['apellido'] ?? '');
$fecha_nacimiento = trim($_POST['fecha_nacimiento'] ?? '');
$email = trim($_POST['email'] ?? '');

/* =========================
   VALIDAR CONTRASEÑAS
========================= */
if ($pass1 !== $pass2) {
    die("❌ Las contraseñas no coinciden");
}

/* =========================
   VERIFICAR CLIENTE
========================= */
$sqlUsuario = "
SELECT *
FROM usuarios
WHERE documento = '$documento'
";

$resUsuario = mysqli_query($conn, $sqlUsuario);

if (mysqli_num_rows($resUsuario) == 0) {
    die("❌ No existe un cliente registrado con ese DNI");
}

/* =========================
   VERIFICAR TARJETA ASOCIADA
========================= */
$sqlTarjeta = "
SELECT *
FROM tarjetas
WHERE dni_titular = '$documento'
";

$resTarjeta = mysqli_query($conn, $sqlTarjeta);

if (mysqli_num_rows($resTarjeta) == 0) {
    die("❌ El cliente no posee una tarjeta emitida");
}

/* =========================
   ACTIVAR CUENTA WEB
========================= */
/* 
   Se actualizan también los datos personales
   porque el formulario de activación los solicita.
*/
$sqlUpdate = "
UPDATE usuarios
SET nombre = '$nombre',
    apellido = '$apellido',
    fecha_nacimiento = '$fecha_nacimiento',
    email = '$email',
    usuario = '$usuario',
    password = '$pass1'
WHERE documento = '$documento'
";

if (mysqli_query($conn, $sqlUpdate)) {

    echo "✅ Cuenta activada correctamente";
    echo "<br><br>";
    echo "<a href='ingreso.html'>Ir al Login</a>";

} else {

    echo "❌ Error: " . mysqli_error($conn);

}

?>
