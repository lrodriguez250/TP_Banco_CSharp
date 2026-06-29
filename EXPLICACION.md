# TP Programación III - Progra3Card

## Integrantes

* Lorenzo Rodriguez Wilson

---

# Descripción General

Este trabajo práctico consiste en el desarrollo de un sistema de administración y consulta de tarjetas de crédito denominado **Progra3Card**.

El sistema está compuesto por tres partes principales:

1. Aplicación administrativa desarrollada en C#.
2. Sitio web desarrollado con PHP y HTML.
3. Base de datos MySQL.

La aplicación administrativa es utilizada por el banco para emitir tarjetas, consultar clientes y generar liquidaciones.

El sitio web es utilizado por los clientes para activar su cuenta, iniciar sesión y consultar su resumen.

Toda la información se almacena y consulta desde una base de datos MySQL compartida entre ambos sistemas.

---

# Tecnologías Utilizadas

* C#
* .NET
* PHP
* HTML
* Tailwind CSS
* MySQL
* Git y GitHub

---

# Estructura de la Base de Datos

La base de datos utilizada es:

mi_banco_db

Principales tablas:

### usuarios

Almacena la información de los clientes.

Campos principales:

* documento
* tipo_documento
* nombre
* apellido
* fecha_nacimiento
* email
* usuario
* password

### tarjetas

Almacena las tarjetas emitidas por el banco.

Campos principales:

* num_cuenta
* numero_tarjeta
* banco_emisor
* estado
* saldo
* dni_titular

### liquidaciones

Almacena los resúmenes mensuales generados para cada tarjeta.

Campos principales:

* num_cuenta
* periodo
* fecha_vencimiento
* total_a_pagar
* pago_minimo

---

# Funcionamiento del Sistema

## 1. Emisión de Tarjeta

Desde la aplicación C# el administrador puede:

* Ingresar DNI del cliente.
* Crear una nueva cuenta.
* Emitir una tarjeta.
* Asignar saldo inicial.

Si el usuario no existe en la tabla usuarios, el sistema lo crea automáticamente con datos temporales.

---

## 2. Activación de Cuenta Web

El cliente accede al formulario de activación.

Completa:

* Documento
* Nombre
* Apellido
* Fecha de nacimiento
* Correo electrónico
* Usuario web
* Contraseña

El sistema valida:

* Que exista un usuario registrado.
* Que posea una tarjeta emitida.

Luego actualiza sus datos personales y habilita el acceso web.

---

## 3. Inicio de Sesión

El cliente ingresa:

* Usuario
* Contraseña

El sistema valida las credenciales contra la tabla usuarios.

Si son correctas, accede al resumen de cuenta.

---

## 4. Consulta de Resumen

Una vez autenticado, el cliente puede visualizar:

* Nombre completo.
* Email.
* Documento.
* Número de cuenta.
* Número de tarjeta.
* Saldo disponible.
* Estado de la tarjeta.
* Última liquidación generada.

---

## 5. Emisión de Liquidaciones

Desde la aplicación administrativa en C# se pueden generar nuevas liquidaciones.

Se registran:

* Período.
* Fecha de vencimiento.
* Total a pagar.
* Pago mínimo.

Las liquidaciones quedan disponibles automáticamente para su consulta desde el sitio web.

---

# Funcionalidades Implementadas en C#

### Menú Administrativo

1. Emitir Nueva Tarjeta.
2. Listar Tarjetas.
3. Ver Detalle de Tarjeta y Cliente.
4. Eliminar Tarjeta.
5. Emitir Liquidación Mensual.
6. Salir.

---

# Flujo Completo Probado

Se verificó el siguiente flujo completo:

1. Creación de cliente desde C#.
2. Emisión de tarjeta.
3. Registro web del cliente.
4. Inicio de sesión.
5. Consulta de resumen.
6. Emisión de liquidación.
7. Visualización de la liquidación desde la web.

---

# Observaciones

Para simplificar el proceso de alta, cuando un cliente no existe en la base de datos, el sistema administrativo crea automáticamente un registro temporal que luego es completado durante la activación web.

De esta manera se garantiza el flujo completo solicitado para el trabajo práctico.

---

# Repositorio

Proyecto desarrollado para la materia Programación III utilizando C#, PHP y MySQL.
