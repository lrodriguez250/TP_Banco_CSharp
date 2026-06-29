<?php
session_start();

$conn = mysqli_connect("localhost", "root", "root", "mi_banco_db");

if (!$conn) {
    die("Error de conexión: " . mysqli_connect_error());
}

$usuario = trim($_POST['usuario'] ?? '');
$password = trim($_POST['password'] ?? '');
$documento = trim($_POST['documento'] ?? '');

$sql = "
SELECT * FROM usuarios
WHERE usuario = '$usuario'
AND password = '$password'
AND documento = '$documento'
LIMIT 1
";

$result = mysqli_query($conn, $sql);

if (mysqli_num_rows($result) > 0) {
    $_SESSION['usuario'] = $usuario;
    header("Location: resumen.php");
    exit;
} else {
    echo "❌ Usuario o contraseña incorrectos";
}
?>