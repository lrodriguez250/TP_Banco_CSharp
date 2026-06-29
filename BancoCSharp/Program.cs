using System;
using MySql.Data.MySqlClient;


class Program
{
    private static string connectionString = "Server=127.0.0.1;Port=3306;Database=mi_banco_db;Uid=root;Pwd=root;";

    static void Main(string[] args)
    {
        bool salir = false;
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("========================================");
            Console.WriteLine("    SISTEMA ADMINISTRATIVO PROGRA3CARD   ");
            Console.WriteLine("========================================");
            Console.WriteLine("1. Emitir Nueva Tarjeta (Alta de Cliente)");
            Console.WriteLine("2. Listar Tarjetas");
            Console.WriteLine("3. Ver Detalle de una Tarjeta / Cliente");
            Console.WriteLine("4. Eliminar Tarjeta (Baja de Sistema)");
            Console.WriteLine("5. Emitir Nueva Liquidación Mensual");
            Console.WriteLine("6. Salir");
            Console.WriteLine("========================================");
            Console.Write("Seleccione una opción: ");

            switch (Console.ReadLine())
            {
                case "1": MenuEmitirTarjeta(); break;
                case "2": MenuListarTarjetas(); break;
                case "3": MenuVerDetalleTarjeta(); break;
                case "4": MenuEliminarTarjeta(); break;
                case "5": MenuEmitirLiquidacion(); break;
                case "6": salir = true; break;
                default:
                    Console.WriteLine("Opción no válida. Presione una tecla para continuar...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Funciones a completar:

    static void MenuListarTarjetas()
    {
        Console.Clear();
        Console.WriteLine("--- LISTADO GENERAL DE TARJETAS ---");
        Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}", "Nro Cuenta", "Nro Tarjeta", "Banco Emisor", "DNI Titular");
        Console.WriteLine("----------------------------------------------------------------------");

        // === A realizar ===
        // Aquí deben implementar un SELECT sobre la tabla 'tarjetas'
        // para recorrer las filas e imprimirlas en la consola.

        ObtenerYMostrarTarjetas();

        Console.WriteLine("\nPresione una tecla para volver al menú...");
        Console.ReadKey();
    }

    static void MenuVerDetalleTarjeta()
    {
        Console.Clear();
        Console.WriteLine("--- DETALLE DE TARJETA Y CLIENTE ---");
        Console.Write("Ingrese el Número de Cuenta a consultar: ");
        int numCuenta = Convert.ToInt32(Console.ReadLine());

        // === A realizar ===
        // Aquí deben realizar un SELECT con un JOIN entre 'tarjetas' y 'usuarios' 
        // filtrando por el numCuenta para traer todos los campos (Nombre, Apellido, Email, Saldo, etc.)

        MostrarDetalleCompleto(numCuenta);

        Console.WriteLine("\nPresione una tecla para volver al menú...");
        Console.ReadKey();
    }

    static void MenuEliminarTarjeta()
    {
        Console.Clear();
        Console.WriteLine("--- ELIMINAR TARJETA DEL SISTEMA ---");
        Console.Write("Ingrese el Número de Cuenta de la tarjeta a dar de baja: ");
        int numCuenta = Convert.ToInt32(Console.ReadLine());

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n⚠️ ADVERTENCIA: Se eliminará la tarjeta, sus liquidaciones y los datos de acceso web vinculados.");
        Console.ResetColor();
        Console.Write("¿Está seguro de continuar? (S/N): ");

        if (Console.ReadLine().ToUpper() == "S")
        {
            // === A realizar ===
            // Aquí deben ejecutar un DELETE sobre la tabla 'tarjetas' donde num_cuenta = numCuenta.
            // Como definimos ON DELETE CASCADE en la base de datos, las liquidaciones se borrarán solas.
            // Opcional: Evaluar si también eliminan al usuario de la tabla 'usuarios' o si lo mantienen.

            bool exito = DarDeBajaTarjeta(numCuenta);

            if (exito)
                Console.WriteLine("\nTarjeta eliminada correctamente del sistema.");
            else
                Console.WriteLine("\nError al intentar eliminar la tarjeta. Verifique el número de cuenta.");
        }
        else
        {
            Console.WriteLine("\nOperación cancelada.");
        }

        Console.WriteLine("\nPresione una tecla para volver al menú...");
        Console.ReadKey();
    }


    // =========================================================================
    // MÉTODOS BASE QUE DEBEN COMPLETAR CON LA LÓGICA 
    // =========================================================================

    static void ObtenerYMostrarTarjetas()
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = "SELECT num_cuenta, numero_tarjeta, banco_emisor, dni_titular FROM tarjetas";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("{0,-12} {1,-18} {2,-20} {3,-15}",
                    reader["num_cuenta"],
                    reader["numero_tarjeta"],
                    reader["banco_emisor"],
                    reader["dni_titular"]);
            }

            reader.Close();
        }
    }

    static void MostrarDetalleCompleto(int cuenta)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = @"
                    SELECT u.nombre, u.apellido, u.email,
                    t.num_cuenta, t.numero_tarjeta, t.saldo, t.estado
                    FROM usuarios u
                    INNER JOIN tarjetas t ON u.documento = t.dni_titular
                    WHERE t.num_cuenta = @cuenta";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@cuenta", cuenta);

            MySqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine("\n--- DETALLE COMPLETO ---");
                Console.WriteLine("Nombre: " + reader["nombre"] + " " + reader["apellido"]);
                Console.WriteLine("Email: " + reader["email"]);
                Console.WriteLine("Cuenta: " + reader["num_cuenta"]);
                Console.WriteLine("Tarjeta: " + reader["numero_tarjeta"]);
                Console.WriteLine("Saldo: $" + reader["saldo"]);
                Console.WriteLine("Estado: " + reader["estado"]);
            }
            else
            {
                Console.WriteLine("No se encontró la cuenta.");
            }

            reader.Close();
        }
    }

    static bool DarDeBajaTarjeta(int cuenta)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = "DELETE FROM tarjetas WHERE num_cuenta = @cuenta";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@cuenta", cuenta);

            int filas = cmd.ExecuteNonQuery();

            return filas > 0;
        }

    }

    static void MenuEmitirTarjeta()
    {

        Console.Clear();
        Console.WriteLine("--- EMITIR TARJETA ---");

        Console.Write("DNI: ");
        string dni = Console.ReadLine();

        Console.Write("Cuenta: ");
        int cuenta = Convert.ToInt32(Console.ReadLine());

        Console.Write("Número tarjeta: ");
        string tarjeta = Console.ReadLine();

        Console.Write("Banco: ");
        string banco = Console.ReadLine();

        Console.Write("Saldo: ");
        decimal saldo = Convert.ToDecimal(Console.ReadLine());

        EmitirTarjeta(dni, cuenta, tarjeta, banco, saldo);

        Console.WriteLine("\n✔ Tarjeta creada correctamente");
        Console.ReadKey();

    }
    static void EmitirTarjeta(string dni, int cuenta, string tarjeta, string banco, decimal saldo)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            // 1. VERIFICAR USUARIO
            string checkUser = "SELECT COUNT(*) FROM usuarios WHERE documento = @dni";
            MySqlCommand cmdCheck = new MySqlCommand(checkUser, conn);
            cmdCheck.Parameters.AddWithValue("@dni", dni);

            int existe = Convert.ToInt32(cmdCheck.ExecuteScalar());

            // 2. SI NO EXISTE, CREARLO
            if (existe == 0)
            {
                Console.WriteLine("El usuario no existe, se creará automáticamente...");

                string insertUser = @"
    INSERT INTO usuarios
    (documento, nombre, apellido, email, fecha_nacimiento)
    VALUES
    (@dni, 'SinNombre', 'SinApellido', @email, '2000-01-01')";

                MySqlCommand cmdUser = new MySqlCommand(insertUser, conn);

                cmdUser.Parameters.AddWithValue("@dni", dni);
                cmdUser.Parameters.AddWithValue("@email", "user" + dni + "@temp.com");

                cmdUser.ExecuteNonQuery();
            }

            // 3. CREAR TARJETA
            string insertCard = @"
            INSERT INTO tarjetas 
            (num_cuenta, numero_tarjeta, banco_emisor, estado, saldo, dni_titular)
            VALUES (@cuenta, @tarjeta, @banco, 'Activa', @saldo, @dni)";

            MySqlCommand cmdCard = new MySqlCommand(insertCard, conn);

            cmdCard.Parameters.AddWithValue("@cuenta", cuenta);
            cmdCard.Parameters.AddWithValue("@tarjeta", tarjeta);
            cmdCard.Parameters.AddWithValue("@banco", banco);
            cmdCard.Parameters.AddWithValue("@saldo", saldo);
            cmdCard.Parameters.AddWithValue("@dni", dni);

            cmdCard.ExecuteNonQuery();
        }
    }

    static void MenuEmitirLiquidacion()
    {
        Console.Clear();
        Console.WriteLine("--- EMITIR LIQUIDACIÓN MENSUAL ---");

        Console.Write("Número de cuenta: ");
        int cuenta = Convert.ToInt32(Console.ReadLine());

        Console.Write("Período (YYYY-MM): ");
        string periodo = Console.ReadLine();

        Console.Write("Fecha de vencimiento (YYYY-MM-DD): ");
        string vencimiento = Console.ReadLine();

        Console.Write("Total a pagar: ");
        decimal total = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Pago mínimo: ");
        decimal minimo = Convert.ToDecimal(Console.ReadLine());

        EmitirLiquidacion(cuenta, periodo, vencimiento, total, minimo);

        Console.WriteLine("\n✔ Liquidación emitida correctamente");
        Console.ReadKey();
    }

    static void EmitirLiquidacion(int cuenta, string periodo, string vencimiento, decimal total, decimal minimo)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            conn.Open();

            string query = @"
                INSERT INTO liquidaciones 
                (num_cuenta, periodo, fecha_vencimiento, total_a_pagar, pago_minimo)
                VALUES (@cuenta, @periodo, @vencimiento, @total, @minimo)";

            MySqlCommand cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@cuenta", cuenta);
            cmd.Parameters.AddWithValue("@periodo", periodo);
            cmd.Parameters.AddWithValue("@vencimiento", vencimiento);
            cmd.Parameters.AddWithValue("@total", total);
            cmd.Parameters.AddWithValue("@minimo", minimo);

            cmd.ExecuteNonQuery();
        }
    }


}
