-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost
-- Tiempo de generación: 26-09-2025 a las 22:57:15
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `Inmobiliaria`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Contratos`
--

CREATE TABLE `Contratos` (
  `IdContrato` int(11) NOT NULL,
  `IdInquilino` int(11) NOT NULL,
  `IdInmueble` int(11) NOT NULL,
  `Monto` decimal(10,2) NOT NULL,
  `FechaDesde` date NOT NULL,
  `FechaHasta` date NOT NULL,
  `Estado` tinyint(1) NOT NULL,
  `FechaAnulado` date DEFAULT NULL,
  `CreadoPor` int(11) DEFAULT NULL,
  `AnuladoPor` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `Contratos`
--

INSERT INTO `Contratos` (`IdContrato`, `IdInquilino`, `IdInmueble`, `Monto`, `FechaDesde`, `FechaHasta`, `Estado`, `FechaAnulado`, `CreadoPor`, `AnuladoPor`) VALUES
(1, 1, 1, 200000.00, '2025-08-10', '2026-02-11', 0, '2025-09-28', 7, 6),
(2, 2, 3, 150000.00, '2025-09-03', '2025-12-15', 0, NULL, 7, NULL),
(3, 3, 4, 200000.00, '2025-09-01', '2025-11-10', 0, NULL, 7, NULL),
(4, 4, 2, 100000.00, '2025-10-20', '2026-05-15', 0, NULL, 7, NULL),
(5, 1, 1, 200.00, '2025-09-05', '2025-10-20', 0, '2025-09-26', 7, 6),
(6, 1, 2, 1111.00, '2027-09-23', '2027-11-10', 1, NULL, 7, NULL),
(7, 1, 2, 2111.00, '2027-11-11', '2028-11-10', 0, NULL, 7, NULL),
(8, 1, 2, 2111.00, '2028-11-11', '2029-11-10', 0, NULL, 7, NULL),
(9, 1, 2, 3111.00, '2029-11-11', '2030-11-10', 0, NULL, 7, NULL),
(10, 1, 2, 1111.00, '2027-09-09', '2027-09-09', 0, NULL, 7, NULL),
(11, 1, 2, 1111.00, '2028-09-23', '2029-11-10', 1, NULL, 7, NULL),
(12, 1, 2, 1111.00, '2040-09-22', '2050-11-20', 1, NULL, 7, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Inmuebles`
--

CREATE TABLE `Inmuebles` (
  `IdInmueble` int(11) NOT NULL,
  `IdTipo` int(11) NOT NULL,
  `Uso` enum('comercial','residencial') NOT NULL,
  `Ambientes` int(11) NOT NULL,
  `Direccion` varchar(250) NOT NULL,
  `Precio` decimal(10,2) NOT NULL,
  `Coordenadas` varchar(250) NOT NULL,
  `Disponible` tinyint(1) NOT NULL DEFAULT 1,
  `Estado` tinyint(1) NOT NULL,
  `IdPropietario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `Inmuebles`
--

INSERT INTO `Inmuebles` (`IdInmueble`, `IdTipo`, `Uso`, `Ambientes`, `Direccion`, `Precio`, `Coordenadas`, `Disponible`, `Estado`, `IdPropietario`) VALUES
(1, 2, 'residencial', 3, 'Av. Rivadavia 4521, Caballito, CABA', 120000.00, '-34.618921, -58.440385', 0, 1, 1),
(2, 1, 'residencial', 2, 'San Martín 233, San Miguel de Tucumán', 100000.00, '-26.828544, -65.205899', 1, 1, 2),
(3, 5, 'comercial', 10, 'Av. Pellegrini 1840, Rosario, Santa Fe', 205000.50, '-32.954812, -60.645937', 1, 1, 4),
(4, 4, 'comercial', 3, 'Bv. San Juan 950, Córdoba Capital', 150000.00, '-31.417851, -64.187922', 1, 1, 5),
(5, 3, 'comercial', 2, 'Calle Sarmiento 120, Mendoza', 300000.00, '-32.889732, -68.845907', 1, 1, 5);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Inquilinos`
--

CREATE TABLE `Inquilinos` (
  `IdInquilino` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Dni` varchar(50) NOT NULL,
  `Telefono` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `Inquilinos`
--

INSERT INTO `Inquilinos` (`IdInquilino`, `Nombre`, `Apellido`, `Dni`, `Telefono`, `Email`, `Estado`) VALUES
(1, 'Lucía', 'Fernández', '32456123', '11-4567-8932', 'lucia.fernandez@example.com', 1),
(2, 'Martín', 'Gómez', '29873456', '11-6789-1245', 'martin.gomez@example.com', 1),
(3, 'Sofía', 'Pereyra', '41234567', '11-5123-6789', 'sofia.pereyra@example.com', 1),
(4, 'Diego', 'Ramírez', '35678901', '11-4234-7890', 'diego.ramirez@example.com', 1),
(5, 'Rebeca', 'Aguilera', '40234789', '11-3345-9876', 'camila.torres@example.com', 1),
(8, 'rebecaaaaaaaaaaaa', 'sadlksjdlkasjd', '11111111111', 'sdafasdf', 'adsfsadf', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Pagos`
--

CREATE TABLE `Pagos` (
  `IdPago` int(11) NOT NULL,
  `NumeroPago` varchar(150) DEFAULT NULL,
  `Concepto` varchar(150) NOT NULL,
  `Monto` decimal(10,0) NOT NULL,
  `Fecha` date NOT NULL,
  `CorrespondeAMes` date DEFAULT NULL,
  `IdContrato` int(11) NOT NULL,
  `Estado` tinyint(1) NOT NULL,
  `CreadoPor` int(11) DEFAULT NULL,
  `AnuladoPor` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `Pagos`
--

INSERT INTO `Pagos` (`IdPago`, `NumeroPago`, `Concepto`, `Monto`, `Fecha`, `CorrespondeAMes`, `IdContrato`, `Estado`, `CreadoPor`, `AnuladoPor`) VALUES
(1, '1', 'Alquiler agosto 2025', 200000, '2025-09-05', '2025-08-01', 1, 1, 7, NULL),
(2, '2', 'Alquiler septiembre 2025', 200000, '2025-10-04', '2025-09-01', 2, 1, 7, NULL),
(5, 'REC-2025-005', 'Alquiler septiembre 2025', 150000, '2025-09-03', '2025-09-01', 2, 1, 7, NULL),
(6, 'REC-2025-006', 'Reparaciones', 5000, '2025-12-04', NULL, 3, 1, 7, NULL),
(7, 'REC-2025-007', 'Alquiler noviembre 2025', 90000, '2025-12-01', '2025-11-01', 3, 0, 7, NULL),
(8, '1', 'ytertyret', 234, '0001-01-01', NULL, 1, 1, 7, NULL),
(9, '123', 'eawerwq', 23412, '0001-01-01', NULL, 1, 1, 7, NULL),
(10, '1', '234', 23143, '2025-09-24', NULL, 1, 1, 7, NULL),
(11, '10', 'Multa', 600000, '2025-09-25', NULL, 1, 1, 7, NULL),
(12, '124', 'Multa', 600000, '2025-09-25', NULL, 1, 1, 7, NULL),
(13, '125', 'Multa', 600000, '2025-09-25', NULL, 1, 1, 7, NULL),
(14, '126', 'Se le rompio un ceramico', 1200, '2025-09-25', NULL, 1, 1, 7, NULL),
(15, '127', 'Multa', 400000, '2025-09-26', NULL, 1, 1, 7, NULL),
(16, '128', 'mensual', 20000, '2025-09-26', '2025-09-01', 1, 1, 7, NULL),
(17, '1', 'Multa', 400, '2025-09-26', NULL, 5, 1, 7, NULL),
(18, '1', 'a', 12, '2025-09-26', NULL, 11, 1, 7, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Propietarios`
--

CREATE TABLE `Propietarios` (
  `IdPropietario` int(11) NOT NULL,
  `Nombre` varchar(100) NOT NULL,
  `Apellido` varchar(100) NOT NULL,
  `Dni` varchar(30) NOT NULL,
  `Email` varchar(150) NOT NULL,
  `Telefono` varchar(20) NOT NULL,
  `Direccion` varchar(150) NOT NULL,
  `Estado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `Propietarios`
--

INSERT INTO `Propietarios` (`IdPropietario`, `Nombre`, `Apellido`, `Dni`, `Email`, `Telefono`, `Direccion`, `Estado`) VALUES
(1, 'Alejandro', 'Medina', '27890123', 'alejandro.medina@example.com', '11-4587-2390', 'Av. Corrientes 1450, CABA', 1),
(2, 'Mariana', 'Suárez', '33456218', 'mariana.suarez@example.com', '11-6123-4789', 'Calle Belgrano 235, Rosario, Santa Fe', 1),
(3, 'Ricardo', 'López', '30124567', 'veronica.herrera@ejemplo.com', '11-5789-0345', 'San Martín 432, Córdoba Capital', 1),
(4, 'Esteban', 'Ríos', '38974561', 'esteban.rios@example.com', '11-5567-9821', 'Italia 1200, Mendoza', 1),
(5, 'Verónica', 'Herrera', '41239876', 'veronica.herrera@example.com', '11-4456-7823', 'San Martín 432, Córdoba Capital', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Tipos`
--

CREATE TABLE `Tipos` (
  `IdTipo` int(11) NOT NULL,
  `Descripcion` varchar(100) NOT NULL,
  `Estado` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `Tipos`
--

INSERT INTO `Tipos` (`IdTipo`, `Descripcion`, `Estado`) VALUES
(1, 'Casa', 1),
(2, 'Departamento', 1),
(3, 'Galpón', 1),
(4, 'Depósito', 1),
(5, 'Oficina', 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `Usuarios`
--

CREATE TABLE `Usuarios` (
  `IdUsuario` int(11) NOT NULL,
  `Email` varchar(150) NOT NULL,
  `Rol` int(11) NOT NULL,
  `Estado` tinyint(1) NOT NULL,
  `Avatar` varchar(200) NOT NULL,
  `Clave` varchar(200) NOT NULL,
  `Nombre` varchar(200) NOT NULL,
  `Apellido` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `Usuarios`
--

INSERT INTO `Usuarios` (`IdUsuario`, `Email`, `Rol`, `Estado`, `Avatar`, `Clave`, `Nombre`, `Apellido`) VALUES
(6, 'cupcakemateroooo@mail.com', 1, 1, '', 'iEopoApDig4xHpQ9viKDy9NUMqcfGf0DBupUxH5Rz5s=', 'cupcakesssss', 'materaaaa'),
(7, 'empl@mail.com', 2, 1, '/uploads/avatar_7.jpg', 'iEopoApDig4xHpQ9viKDy9NUMqcfGf0DBupUxH5Rz5s=', 'empl', 'eado');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `Contratos`
--
ALTER TABLE `Contratos`
  ADD PRIMARY KEY (`IdContrato`),
  ADD KEY `IdInquilino` (`IdInquilino`),
  ADD KEY `IdInmueble` (`IdInmueble`),
  ADD KEY `CreadoPor` (`CreadoPor`),
  ADD KEY `AnuladoPor` (`AnuladoPor`);

--
-- Indices de la tabla `Inmuebles`
--
ALTER TABLE `Inmuebles`
  ADD PRIMARY KEY (`IdInmueble`),
  ADD KEY `IdTipo` (`IdTipo`),
  ADD KEY `IdPropietario` (`IdPropietario`);

--
-- Indices de la tabla `Inquilinos`
--
ALTER TABLE `Inquilinos`
  ADD PRIMARY KEY (`IdInquilino`);

--
-- Indices de la tabla `Pagos`
--
ALTER TABLE `Pagos`
  ADD PRIMARY KEY (`IdPago`),
  ADD KEY `IdContrato` (`IdContrato`),
  ADD KEY `AnuladoPor` (`AnuladoPor`),
  ADD KEY `CreadoPor` (`CreadoPor`);

--
-- Indices de la tabla `Propietarios`
--
ALTER TABLE `Propietarios`
  ADD PRIMARY KEY (`IdPropietario`);

--
-- Indices de la tabla `Tipos`
--
ALTER TABLE `Tipos`
  ADD PRIMARY KEY (`IdTipo`);

--
-- Indices de la tabla `Usuarios`
--
ALTER TABLE `Usuarios`
  ADD PRIMARY KEY (`IdUsuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `Contratos`
--
ALTER TABLE `Contratos`
  MODIFY `IdContrato` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `Inmuebles`
--
ALTER TABLE `Inmuebles`
  MODIFY `IdInmueble` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `Inquilinos`
--
ALTER TABLE `Inquilinos`
  MODIFY `IdInquilino` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `Pagos`
--
ALTER TABLE `Pagos`
  MODIFY `IdPago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT de la tabla `Propietarios`
--
ALTER TABLE `Propietarios`
  MODIFY `IdPropietario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `Tipos`
--
ALTER TABLE `Tipos`
  MODIFY `IdTipo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `Usuarios`
--
ALTER TABLE `Usuarios`
  MODIFY `IdUsuario` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `Contratos`
--
ALTER TABLE `Contratos`
  ADD CONSTRAINT `Contratos_ibfk_1` FOREIGN KEY (`IdInquilino`) REFERENCES `Inquilinos` (`IdInquilino`),
  ADD CONSTRAINT `Contratos_ibfk_2` FOREIGN KEY (`IdInmueble`) REFERENCES `Inmuebles` (`IdInmueble`),
  ADD CONSTRAINT `Contratos_ibfk_3` FOREIGN KEY (`CreadoPor`) REFERENCES `Usuarios` (`IdUsuario`),
  ADD CONSTRAINT `Contratos_ibfk_4` FOREIGN KEY (`AnuladoPor`) REFERENCES `Usuarios` (`IdUsuario`);

--
-- Filtros para la tabla `Inmuebles`
--
ALTER TABLE `Inmuebles`
  ADD CONSTRAINT `Inmuebles_ibfk_1` FOREIGN KEY (`IdTipo`) REFERENCES `Tipos` (`IdTipo`),
  ADD CONSTRAINT `Inmuebles_ibfk_2` FOREIGN KEY (`IdPropietario`) REFERENCES `Propietarios` (`IdPropietario`);

--
-- Filtros para la tabla `Pagos`
--
ALTER TABLE `Pagos`
  ADD CONSTRAINT `Pagos_ibfk_1` FOREIGN KEY (`IdContrato`) REFERENCES `Contratos` (`IdContrato`),
  ADD CONSTRAINT `Pagos_ibfk_2` FOREIGN KEY (`AnuladoPor`) REFERENCES `Usuarios` (`IdUsuario`),
  ADD CONSTRAINT `Pagos_ibfk_3` FOREIGN KEY (`CreadoPor`) REFERENCES `Usuarios` (`IdUsuario`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
