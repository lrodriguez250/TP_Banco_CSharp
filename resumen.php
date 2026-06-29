<?php
session_start();

if (!isset($_SESSION['usuario'])) {
    die("❌ Acceso denegado");
}

$conn = mysqli_connect("localhost", "root", "root", "mi_banco_db");

if (!$conn) {
    die("Error de conexión: " . mysqli_connect_error());
}

$usuario = $_SESSION['usuario'];

$sql = "
SELECT u.nombre, u.apellido, u.email,
       u.documento,
       t.num_cuenta, t.numero_tarjeta, t.saldo
FROM usuarios u
JOIN tarjetas t ON u.documento = t.dni_titular
WHERE u.usuario = '$usuario'
LIMIT 1
";

$result = mysqli_query($conn, $sql);
$data = mysqli_fetch_assoc($result);

if (!$data) {
    die("❌ No se encontraron datos del usuario");
}

$sql2 = "
SELECT *
FROM liquidaciones
WHERE num_cuenta = {$data['num_cuenta']}
ORDER BY periodo DESC
LIMIT 1
";

$res2 = mysqli_query($conn, $sql2);
$liq = mysqli_fetch_assoc($res2);
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <title>Home Banking</title>
    <script src="https://cdn.tailwindcss.com"></script>
</head>

<body class="bg-gray-100 font-sans">

<!-- HEADER -->
<header class="bg-[#004691] text-white p-5 shadow-md">
    <div class="max-w-5xl mx-auto flex justify-between items-center">
        <h1 class="text-xl font-bold">Mis Tarjetas</h1>
        <p class="text-sm">Usuario: <?php echo $data['nombre']; ?></p>
    </div>
</header>

<!-- CONTENIDO -->
<main class="max-w-5xl mx-auto p-6 space-y-6">

    <!-- TARJETA PRINCIPAL -->
    <div class="bg-white shadow-lg rounded-xl p-6 border-l-4 border-blue-700">
        <h2 class="text-lg font-bold text-gray-700">Resumen de cuenta</h2>

        <div class="grid md:grid-cols-2 gap-4 mt-4 text-sm">
            <p><strong>Nombre:</strong> <?php echo $data['nombre'] . " " . $data['apellido']; ?></p>
            <p><strong>Email:</strong> <?php echo $data['email']; ?></p>
            <p><strong>Documento:</strong> <?php echo $data['documento']; ?></p>
            <p><strong>Nro Cuenta:</strong> <?php echo $data['num_cuenta']; ?></p>
        </div>
    </div>

    <!-- TARJETA BANCARIA -->
    <div class="bg-gradient-to-r from-blue-700 to-blue-500 text-white rounded-xl p-6 shadow-lg">
        <h3 class="text-lg font-semibold">Tarjeta de Crédito</h3>

        <p class="mt-3 tracking-widest text-lg">
            <?php echo chunk_split($data['numero_tarjeta'], 4, ' '); ?>
        </p>

        <div class="mt-4 flex justify-between">
            <p><strong>Saldo:</strong> $<?php echo $data['saldo']; ?></p>
            <p><strong>Estado:</strong> Activa</p>
        </div>
    </div>

    <!-- LIQUIDACIÓN -->
    <div class="bg-white shadow-lg rounded-xl p-6 border-l-4 border-green-600">
        <h3 class="text-lg font-bold text-gray-700">Última liquidación</h3>

        <?php if ($liq) { ?>
            <div class="mt-3 text-sm space-y-2">
                <p><strong>Período:</strong> <?php echo $liq['periodo']; ?></p>
                <p><strong>Total a pagar:</strong> $<?php echo $liq['total_a_pagar']; ?></p>
                <p><strong>Pago mínimo:</strong> $<?php echo $liq['pago_minimo']; ?></p>
            </div>
        <?php } else { ?>
            <p class="text-gray-500 mt-2">No hay liquidaciones disponibles.</p>
        <?php } ?>
    </div>

    <!-- BOTÓN LOGOUT (opcional) -->
    <div class="text-right">
        <a href="ingreso.html" class="text-red-600 font-semibold hover:underline">
            Cerrar sesión
        </a>
    </div>

</main>

</body>
</html>