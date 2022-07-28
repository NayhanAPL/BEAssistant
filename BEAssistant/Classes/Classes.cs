using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace BEAssistant
{
    //-------------------------------------INVERSIONES  

    public enum ConstFrecuencia
    {
        Diaria,
        Semanal,
        Quincenal,
        Mensual,
        Semestral,
        Anual
    }
    public enum ConstCategoria
    {
        Independiente,
        Proporcional
    }
    public enum ExtraCategoria
    {
        Beneficiosas,
        Inprevistas
    }
    public enum TiposConstante
    {
        Herramientas,
        Renta,
        MateriaPrima,
        Empleados,
        Licencias,
        ServiciosExternos,
        Transportacion,
        Equipos,
        Otros
    }
    public enum TiposAcumulativa
    {
        MateriaPrima,
        Transportacion,
        ServiciosExternos,
        Herramientas,
        Equipos,
        Otros
    }
    public enum TiposExtra
    {
        Accidente,
        Reparacion,
        Sustitucion,
        Otros,
        Productividad,
        Ahorro,
        Calidad,
        Rendimiento
    }


    public class InConstante
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public ConstFrecuencia Frecuencia { get; set; }
        public ConstCategoria Categoria { get; set; }
        public TiposConstante Tipo { get; set; }
    }
    public class InAcumulativa
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public TiposAcumulativa Tipo { get; set; }
    }
    public class InExtraordinaria
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public ExtraCategoria Categoria { get; set; }
        public TiposExtra Tipo { get; set; }
    }


    public class ReConstante
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Costo { get; set; }
        public double Unidades { get; set; }
        public int IdInv { get; set; }
        public DateTime Fecha { get; set; }
    }
    public class ReAcumulativa
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Costo { get; set; }
        public double Unidades { get; set; }
        public int IdInv { get; set; }
        public DateTime Fecha { get; set; }
    }
    public class ReExtraordinaria
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Costo { get; set; }
        public double Unidades { get; set; }
        public int IdInv { get; set; }
        public DateTime Fecha { get; set; }
    }
    public class Deudas
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdInv { get; set; }
        public string Denominacion { get; set; }
        public double Costo { get; set; }
        public double Unidades { get; set; }
        public DateTime FechaIcicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Descripcion { get; set; }
    }
    public class DependenciasConstantes
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdInv { get; set; }
        public int IdItem { get; set; }
        public string Item { get; set; }
        public string Clase { get; set; }
        public int Porcentaje { get; set; }
    }


    //-----------------------------------------PRODUCTOS


    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Denominacion { get; set; }
        public int Tiempo { get; set; }
        public double Precio { get; set; }
        public int Dificultad { get; set; }
        public int Demanda { get; set; }
        public double Inversion { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public double Dimensiones { get; set; }
    }
    public class MateriasPrimas
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public double CantidadPorProducto { get; set; }
        public double PrecioUnidad { get; set; }
    }
    public class Dimention
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double IdProducto { get; set; }
        public string Denominacion { get; set; }
        public double Dimen1 { get; set; }
        public double Dimen2 { get; set; }
        public double Dimen3 { get; set; }
    }
    public class Venta
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Precio { get; set; }
        public double Unidades { get; set; }
        public int IdPro { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
    }
    public class Procesos
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdPro { get; set; }
        public DateTime FechaIni { get; set; }
        public int TimeEsperado { get; set; }
        public int TimeResultado { get; set; }
        public int Cantidad { get; set; }
        public string Descripcion { get; set; }
        public bool EnProceso { get; set; }
    }

    //-----------------------------------------CIERRES

    public class Caducidad
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdReg { get; set; }
        public string TipoInv { get; set; }
        public bool Caduco { get; set; }
    }
    public class CierresDiarios
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Ingreso { get; set; }
        public double GastoC { get; set; }
        public double GastoA { get; set; }
        public double GastoE { get; set; }
        public DateTime Fecha { get; set; }
    }
    public class CierresMensual
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Ganancia { get; set; }
        public double GastoC { get; set; }
        public double GastoA { get; set; }
        public double GastoE { get; set; }
        public DateTime Fecha { get; set; }
    }
    public class Stocker
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdInv { get; set; }
        public int Duracion { get; set; }
        public double CantActual { get; set; }
        public string TipoInv { get; set; }
        public string Categoria { get; set; }
        public int UnidadesEstimadas { get; set; }
    }
    public class StockProductos
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int IdPro { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
    }
    public class Inventario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Materias { get; set; }
        public double ConsUtil { get; set; }
        public double ConsCaduco { get; set; }
        public double ConsExedente { get; set; }
        public DateTime Fecha { get; set; }
    }

    //-----------------------------------------OTROS
    public class FechaInicio
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime inicio { get; set; }
    }

    public class Contacto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Numero { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string Categoria { get; set; }
        public string Direccion { get; set; }
        public string Descripcion { get; set; }
    }

    public class Opciones
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}

