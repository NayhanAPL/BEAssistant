using Microsoft.EntityFrameworkCore;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BEAssistant.DataBase
{
    public class Database
    {
        readonly SQLiteAsyncConnection _database;
        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<InConstante>().Wait();
            _database.CreateTableAsync<InExtraordinaria>().Wait();
            _database.CreateTableAsync<InAcumulativa>().Wait();
            _database.CreateTableAsync<ReConstante>().Wait();
            _database.CreateTableAsync<ReExtraordinaria>().Wait();
            _database.CreateTableAsync<ReAcumulativa>().Wait();
            _database.CreateTableAsync<Product>().Wait();
            _database.CreateTableAsync<MateriasPrimas>().Wait();
            _database.CreateTableAsync<Dimention>().Wait();
            _database.CreateTableAsync<Venta>().Wait();
            _database.CreateTableAsync<Deudas>().Wait(); 
            _database.CreateTableAsync<CierresDiarios>().Wait(); 
            _database.CreateTableAsync<CierresMensual>().Wait(); 
            _database.CreateTableAsync<FechaInicio>().Wait();
            _database.CreateTableAsync<DependenciasConstantes>().Wait();
            _database.CreateTableAsync<Stocker>().Wait();
            _database.CreateTableAsync<Contacto>().Wait();
            _database.CreateTableAsync<Opciones>().Wait();
            _database.CreateTableAsync<Inventario>().Wait();
            _database.CreateTableAsync<Procesos>().Wait();
        }
        
        //---------------------------------InvConstante-------------------------------------------

        //consulta completa InvConstante----------------------------------------------------------
        public Task<List<InConstante>> GetInvConstante()
        {
            return _database.Table<InConstante>().ToListAsync();
        }
        //consulta por id de InvConstante---------------------------------------------------------
        public Task<InConstante> GetIdInvConstante(int Id)
        {
            return _database.Table<InConstante>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el InvConstante en la db--------------------------------------------------------
        public Task<int> SaveInvConstante(InConstante U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el InvConstante en la db------------------------------------
        public Task<int> SaveUpInvConstante(InConstante U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla InvConstante-----------------------------------------------
        public Task<int> DeleteInvConstante(InConstante InverConstante)
        {
            var x = _database.DeleteAsync(InverConstante);
            return x;
        }
        // devuelve por tipo en InConstante--------------------------------------------------------
        public async Task<List<InConstante>> GetByTipoInvConstante(int tipo)
        {
            return await _database.QueryAsync<InConstante>($"Select * from InConstante where Tipo = '{tipo}'");
        }
        // devuelve por nombre en InvConstante------------------------------------------------------
        public async Task<List<InConstante>> GetByNameInvConstante(string name)
        {
            return await _database.QueryAsync<InConstante>($"Select * from InConstante where Nombre = '{name}'");
        }
        // devuelve el ultimo elemento guardado---------------------------------------------------
        public async Task<List<InConstante>> GetLastItemInvConstante()
        {
            return await _database.QueryAsync<InConstante>($"Select * from InConstante order by Id desc limit 1");
        }


        //---------------------------------InvExtraordinaria-------------------------------------------

        //consulta completa InvExtraordinaria----------------------------------------------------------
        public Task<List<InExtraordinaria>> GetInvExtraordinaria()
        {
            return _database.Table<InExtraordinaria>().ToListAsync();
        }
        //consulta por id de InvExtraordinaria---------------------------------------------------------
        public Task<InExtraordinaria> GetIdInvExtraordinaria(int Id)
        {
            return _database.Table<InExtraordinaria>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el InvExtraordinaria en la db--------------------------------------------------------
        public Task<int> SaveInvExtraordinaria(InExtraordinaria U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el InvExtraordinaria en la db------------------------------------
        public Task<int> SaveUpInvExtraordinaria(InExtraordinaria U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla InvConstante----------------------------------------------------
        public Task<int> DeleteInvExtraordinaria(InExtraordinaria InvExtraordinaria)
        {
            var x = _database.DeleteAsync(InvExtraordinaria);
            return x;
        }


        //---------------------------------InvAcumulativa-------------------------------------------

        //consulta completa InvAcumulativa----------------------------------------------------------
        public Task<List<InAcumulativa>> GetInvAcumulativa()
        {
            return _database.Table<InAcumulativa>().ToListAsync();
        }
        //consulta por id de InvAcumulativa---------------------------------------------------------
        public Task<InAcumulativa> GetIdInvAcumulativa(int Id)
        {
            return _database.Table<InAcumulativa>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el InvAcumulativa en la db--------------------------------------------------------
        public Task<int> SaveInvAcumulativa(InAcumulativa U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el InvAcumulativa en la db------------------------------------
        public Task<int> SaveUpInvAcumulativa(InAcumulativa U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla InvAcumulativa-----------------------------------------------
        public Task<int> DeleteInvAcumulativa(InAcumulativa InvAcumulativa)
        {
            var x = _database.DeleteAsync(InvAcumulativa);
            return x;
        }
        // devuelve por tipo en InvAcumulativa---------------------------------------------------
        public async Task<List<InAcumulativa>> GetByTipoInvAcumulativa(int tipo)
        {
            var query = string.Format("Select * from InAcumulativa where Tipo = '{0}'", tipo);
            return await _database.QueryAsync<InAcumulativa>(query);
        }
        // devuelve por nombre en InvAcumulativa--------------------------
        public async Task<List<InAcumulativa>> GetByNameInvAcumulativa(string name)
        {
            return await _database.QueryAsync<InAcumulativa>($"Select * from InAcumulativa where Nombre = '{name}'");
        }
        // devuelve el ultimo elemento guardado---------------------------------------------------
        public async Task<List<InAcumulativa>> GetLastItemInvAcumulativa()
        {
            return await _database.QueryAsync<InAcumulativa>($"Select * from InAcumulativa order by Id desc limit 1");
        }


        //---------------------------------RegConstante-------------------------------------------

        //consulta completa RegConstante----------------------------------------------------------
        public Task<List<ReConstante>> GetRegConstante()
        {
            return _database.Table<ReConstante>().ToListAsync();
        }
        //consulta por id de RegConstante---------------------------------------------------------
        public Task<ReConstante> GetIdRegConstante(int Id)
        {
            return _database.Table<ReConstante>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el RegConstante en la db--------------------------------------------------------
        public Task<int> SaveRegConstante(ReConstante U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el RegConstante en la db------------------------------------
        public Task<int> SaveUpRegConstante(ReConstante U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla RegConstante-----------------------------------------------
        public Task<int> DeleteRegConstante(ReConstante RegConstante)
        {
            var x = _database.DeleteAsync(RegConstante);
            return x;
        }
        // devuelve el elemento por el id de la inversion-----------------------------------------
        public async Task<List<ReConstante>> GetIdInvRegConstante(int ID)
        {
            return await _database.QueryAsync<ReConstante>($"Select * from ReConstante where IdInv = {ID}");
        }
        // devuelve por fecha en InConstante--------------------------
        public async Task<List<ReConstante>> GetByFechaReConstante(DateTime time)
        {
            return await _database.QueryAsync<ReConstante>($"Select * from ReConstante where Fecha = '{time}'");
        }
        // devuelve las inverciones por su tipo en ReConstante--------------------------
        public async Task<List<ReConstante>> GetByTipoReConstante(int tipo)
        {
            return await _database.QueryAsync<ReConstante>($"Select * from ReConstante where IdInv in ( Select Id from InConstante where Tipo = '{tipo}')");
        }
        // devuelve el ultimo elemento guardado---------------------------------------------------
        public async Task<List<ReConstante>> GetLastItemRegConstante()
        {
            return await _database.QueryAsync<ReConstante>($"Select * from ReConstante order by Id desc limit 1");
        }


        //---------------------------------RegExtraordinaria-------------------------------------------

        //consulta completa RegExtraordinaria----------------------------------------------------------
        public Task<List<ReExtraordinaria>> GetRegExtraordinaria()
        {
            return _database.Table<ReExtraordinaria>().ToListAsync();
        }
        //consulta por id de RegExtraordinaria---------------------------------------------------------
        public Task<ReExtraordinaria> GetIdRegExtraordinaria(int Id)
        {
            return _database.Table<ReExtraordinaria>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el RegExtraordinaria en la db--------------------------------------------------------
        public Task<int> SaveRegExtraordinaria(ReExtraordinaria U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el RegExtraordinaria en la db------------------------------------
        public Task<int> SaveUpRegExtraordinaria(ReExtraordinaria U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla RegExtraordinaria-----------------------------------------------
        public Task<int> DeleteRegExtraordinaria(ReExtraordinaria RegExtraordinaria)
        {
            var x = _database.DeleteAsync(RegExtraordinaria);
            return x;
        }
        // devuelve el elemento por el id de la inversion
        public async Task<List<ReExtraordinaria>> GetIdInvRegExtraordinaria(int ID)
        {
            return await _database.QueryAsync<ReExtraordinaria>($"Select * from ReExtraordinaria where IdInv = {ID}");
        }
        // devuelve por fecha en InExtraordinaria--------------------------
        public async Task<List<ReExtraordinaria>> GetByFechaReExtraordinaria(DateTime time)
        {
            return await _database.QueryAsync<ReExtraordinaria>($"Select * from ReExtraordinaria where Fecha = '{time}'");
        }


        //---------------------------------RegAcumulativa-------------------------------------------

        //consulta completa RegAcumulativa----------------------------------------------------------
        public Task<List<ReAcumulativa>> GetRegAcumulativa()
        {
            return _database.Table<ReAcumulativa>().ToListAsync();
        }
        //consulta por id de RegAcumulativa---------------------------------------------------------
        public Task<ReAcumulativa> GetIdRegAcumulativa(int Id)
        {
            return _database.Table<ReAcumulativa>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el RegAcumulativa en la db--------------------------------------------------------
        public Task<int> SaveRegAcumulativa(ReAcumulativa U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el RegAcumulativa en la db------------------------------------
        public Task<int> SaveUpRegAcumulativa(ReAcumulativa U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla RegAcumulativa-----------------------------------------------
        public Task<int> DeleteRegAcumulativa(ReAcumulativa RegAcumulativa)
        {
            var x = _database.DeleteAsync(RegAcumulativa);
            return x;
        }
        // devuelve el elemento por el id de la inversion
        public async Task<List<ReAcumulativa>> GetIdInvRegAcumulativa(int ID)
        {
            return await _database.QueryAsync<ReAcumulativa>($"Select * from ReAcumulativa where IdInv = {ID}");
        }
        // devuelve por fecha en ReAcumulativa--------------------------
        public async Task<List<ReAcumulativa>> GetByFechaReAcumulativa(DateTime time)
        {
            return await _database.QueryAsync<ReAcumulativa>($"Select * from ReAcumulativa where Fecha = '{time}'");
        }
        // devuelve las inverciones por su tipo en ReAcumulativa--------------------------
        public async Task<List<ReAcumulativa>> GetByTipoReAcumulativa(int tipo)
        {
            return await _database.QueryAsync<ReAcumulativa>($"Select * from ReAcumulativa where IdInv in ( Select Id from InAcumulativa where Tipo = '{tipo}')");
        }
        // devuelve el ultimo elemento guardado---------------------------------------------------
        public async Task<List<ReAcumulativa>> GetLastItemRegAcumulativa()
        {
            return await _database.QueryAsync<ReAcumulativa>($"Select * from ReAcumulativa order by Id desc limit 1");
        }

        //---------------------------------Stock-------------------------------------------

        //consulta completa Stock----------------------------------------------------------
        public Task<List<Stocker>> GetStock()
        {
            return _database.Table<Stocker>().ToListAsync();
        }
        //consulta por id de Stock---------------------------------------------------------
        public Task<Stocker> GetIdStock(int Id)
        {
            return _database.Table<Stocker>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el Stock en la db--------------------------------------------------------
        public Task<int> SaveStock(Stocker U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el Stock en la db------------------------------------
        public Task<int> SaveUpStock(Stocker U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla Stock-----------------------------------------------
        public Task<int> DeleteStock(Stocker Stock)
        {
            var x = _database.DeleteAsync(Stock);
            return x;
        }
        // devuelve por tipo en Stock--------------------------------------------------------
        public async Task<List<Stocker>> GetByTipoInvStock(string tipo)
        {
            return await _database.QueryAsync<Stocker>($"Select * from Stocker where TipoInv = '{tipo}'");
        }
        // devuelve el elemento por el id de la inversion Acumulativa
        public async Task<List<Stocker>> GetIdInvAStock(int ID)
        {
            return await _database.QueryAsync<Stocker>($"Select * from Stocker where IdInv = {ID} and TipoInv = 'Acumulativa'");
        }
        // devuelve el elemento por el id de la inversion Constante
        public async Task<List<Stocker>> GetIdInvCStock(int ID)
        {
            return await _database.QueryAsync<Stocker>($"Select * from Stocker where IdInv = {ID} and TipoInv = 'Constante'");
        }

        //---------------------------------Productos-------------------------------------------

        //consulta completa Productos----------------------------------------------------------
        public Task<List<Product>> GetProductos()
        {
            return _database.Table<Product>().ToListAsync();
        }
        //consulta por id de Productos---------------------------------------------------------
        public Task<Product> GetIdProductos(int Id)
        {
            return _database.Table<Product>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el Productos en la db--------------------------------------------------------
        public Task<int> SaveProductos(Product U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el Productos en la db------------------------------------
        public Task<int> SaveUpProductos(Product U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla Productos-----------------------------------------------
        public Task<int> DeleteProductos(Product Productos)
        {
            var x = _database.DeleteAsync(Productos);
            return x;
        }
        // devuelve todos los elementos que tengan la denominacion que se pasa-----------------
        public async Task<List<Product>> GetByDenominacionProductos(string deno)
        {
            return await _database.QueryAsync<Product>($"Select * from Product where Denominacion = '{deno}'");
        }
        // devuelve el elemento que contiene el nombre que se pasa-----------------------------
        public async Task<List<Product>> GetByNombreProductos(string name)
        {
            return await _database.QueryAsync<Product>($"Select * from Product where Nombre = '{name}'");
        }
        // devuelve el ultimo elemento guardado-----------------------------
        public async Task<List<Product>> GetLastItemProductos()
        {
            return await _database.QueryAsync<Product>($"Select * from Product order by Id desc limit 1");
        }
        // devuelve el ultimo elemento guardado-----------------------------
        public async Task<List<Product>> GetLastItemByDenomProductos(string denom)
        {
            return await _database.QueryAsync<Product>($"Select * from Product where Denominacion = '{denom}' order by Id desc limit 1");
        }


        //---------------------------------MateriaPrima-------------------------------------------

        //consulta completa MateriaPrima----------------------------------------------------------
        public Task<List<MateriasPrimas>> GetMateriaPrima()
        {
            return _database.Table<MateriasPrimas>().ToListAsync();
        }
        //consulta por id de MateriaPrima---------------------------------------------------------
        public Task<MateriasPrimas> GetIdMateriaPrima(int Id)
        {
            return _database.Table<MateriasPrimas>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el MateriaPrima en la db--------------------------------------------------------
        public Task<int> SaveMateriaPrima(MateriasPrimas U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el MateriaPrima en la db------------------------------------
        public Task<int> SaveUpMateriaPrima(MateriasPrimas U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla MateriaPrima--------------------------------------------------
        public Task<int> DeleteMateriaPrima(MateriasPrimas MateriaPrima)
        {
            var x = _database.DeleteAsync(MateriaPrima);
            return x;
        }
        // devuelve los elementos que contengan el id de producto dado----------------------------
        public async Task<List<MateriasPrimas>> GetByIdProMateriaPrima(int id)
        {
            return await _database.QueryAsync<MateriasPrimas>($"Select * from MateriasPrimas where IdProducto = {id}");
        }
        // devuelve los elementos que contengan el nombre de materia prima dado----------------------------
        public async Task<List<MateriasPrimas>> GetByNameMateriaPrima(string nombre)
        {
            return await _database.QueryAsync<MateriasPrimas>($"Select * from MateriasPrimas where Nombre = '{nombre}'");
        }
        // devuelve las materias primas sin precios por unidad definidos--------------------------
        public async Task<List<MateriasPrimas>> GetByPrecUniVacioMateriaPrima()
        {
            return await _database.QueryAsync<MateriasPrimas>($"Select * from MateriasPrimas where PrecioUnidad = 0");
        }


        //---------------------------------Dimensiones-------------------------------------------

        //consulta completa Dimensiones----------------------------------------------------------
        public Task<List<Dimention>> GetDimensiones()
        {
            return _database.Table<Dimention>().ToListAsync();
        }
        //consulta por id de Dimensiones---------------------------------------------------------
        public Task<Dimention> GetIdDimensiones(int Id)
        {
            return _database.Table<Dimention>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el Dimensiones en la db--------------------------------------------------------
        public Task<int> SaveDimensiones(Dimention U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el Dimensiones en la db------------------------------------
        public Task<int> SaveUpDimensiones(Dimention U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla Dimensiones--------------------------------------------------
        public Task<int> DeleteDimensiones(Dimention Dimensiones)
        {
            var x = _database.DeleteAsync(Dimensiones);
            return x;
        }
        // devuelve los elementos que contengan el id de producto dado----------------------------
        public async Task<List<Dimention>> GetByIdProDimensiones(int id)
        {
            return await _database.QueryAsync<Dimention>($"Select * from Dimention where IdProducto = {id}");
        }
        // devuelve los elementos que contengan la denominacion dada----------------------------
        public async Task<List<Dimention>> GetByDenomProDimensiones(int denom)
        {
            return await _database.QueryAsync<Dimention>($"Select * from Dimention where Denominacion = '{denom}'");
        }

        //---------------------------------Venta-------------------------------------------

        //consulta completa Venta----------------------------------------------------------
        public Task<List<Venta>> GetVenta()
        {
            return _database.Table<Venta>().ToListAsync();
        }
        //consulta por id de Venta---------------------------------------------------------
        public Task<Venta> GetIdVenta(int Id)
        {
            return _database.Table<Venta>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el Venta en la db--------------------------------------------------------
        public Task<int> SaveVenta(Venta U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el Venta en la db------------------------------------
        public Task<int> SaveUpVenta(Venta U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla Venta-----------------------------------------------
        public Task<int> DeleteVenta(Venta Venta)
        {
            var x = _database.DeleteAsync(Venta);
            return x;
        }
        // devuelve el elemento por el id del producto-----------------------------------------
        public async Task<List<Venta>> GetByIdProVenta(int ID)
        {
            return await _database.QueryAsync<Venta>($"Select * from Venta where IdPro = {ID}");
        }
        // devuelve el ultimo elemento guardado de un producto especifico-----------------------------
        public async Task<List<Venta>> GetLastVentaByNameProductos(int id)
        {
            return await _database.QueryAsync<Venta>($"Select * from Venta where IdPro = '{id}' order by Id desc limit 1");
        }
        // devuelve por fecha en Venta--------------------------
        public async Task<List<Venta>> GetByFechaVenta(DateTime time)
        {
            return await _database.QueryAsync<Venta>($"Select * from Venta where Fecha = '{time}'");
        }
        // devuelve por Denominacion en Venta--------------------------
        public async Task<List<Venta>> GetByDenomVenta(string denom)
        {
            return await _database.QueryAsync<Venta>($"Select * from Venta where IdPro in ( Select Id from Product where Denominacion = '{denom}')");
        }


        //---------------------------------Deuda-------------------------------------------

        //consulta completa Deuda----------------------------------------------------------
        public Task<List<Deudas>> GetDeuda()
        {
            return _database.Table<Deudas>().ToListAsync();
        }
        //consulta por id de Deuda---------------------------------------------------------
        public Task<Deudas> GetIdDeuda(int Id)
        {
            return _database.Table<Deudas>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el Deuda en la db--------------------------------------------------------
        public Task<int> SaveDeuda(Deudas U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el Deuda en la db------------------------------------
        public Task<int> SaveUpDeuda(Deudas U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla Deuda-----------------------------------------------
        public Task<int> DeleteDeuda(Deudas Deuda)
        {
            var x = _database.DeleteAsync(Deuda);
            return x;
        }
        // devuelve el elemento por el id de la inversion----------------------------------
        public async Task<List<Deudas>> GetIdInvDeuda(int ID)
        {
            return await _database.QueryAsync<Deudas>($"Select * from Deudas where IdInv = {ID}");
        }

        //---------------------------------DependenciaConstante-------------------------------------------

        //consulta por id de DependenciaConstante---------------------------------------------------------
        public Task<DependenciasConstantes> GetIdDependenciaConstante(int Id)
        {
            return _database.Table<DependenciasConstantes>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el DependenciaConstante en la db--------------------------------------------------------
        public Task<int> SaveDependenciaConstante(DependenciasConstantes U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el InvConstante en la db------------------------------------
        public Task<int> SaveUpDependenciaConstante(DependenciasConstantes U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla InvConstante-----------------------------------------------
        public Task<int> DeleteDependenciaConstante(DependenciasConstantes DependenciaConstante)
        {
            var x = _database.DeleteAsync(DependenciaConstante);
            return x;
        }
        // devuelve el elemento por el id de la inversion----------------------------------
        public async Task<List<DependenciasConstantes>> GetIdInvDependenciaConstante(int ID)
        {
            return await _database.QueryAsync<DependenciasConstantes>($"Select * from DependenciasConstantes where IdInv = {ID}");
        }

        //---------------------------------CierreDiario-------------------------------------------

        //consulta completa CierreDiario----------------------------------------------------------
        public Task<List<CierresDiarios>> GetCierreDiario()
        {
            return _database.Table<CierresDiarios>().ToListAsync();
        }
        //consulta por id de CierreDiario---------------------------------------------------------
        public Task<CierresDiarios> GetIdCierreDiario(int Id)
        {
            return _database.Table<CierresDiarios>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el CierreDiario en la db--------------------------------------------------------
        public Task<int> SaveCierreDiario(CierresDiarios U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el CierreDiario en la db------------------------------------
        public Task<int> SaveUpCierreDiario(CierresDiarios U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla CierreDiario-----------------------------------------------
        public Task<int> DeleteCierreDiario(CierresDiarios CierreDiario)
        {
            var x = _database.DeleteAsync(CierreDiario);
            return x;
        }
        // devuelve el ultimo elemento guardado---------------------------------------------------
        public async Task<List<CierresDiarios>> GetLastItemCierreDiario()
        {
            return await _database.QueryAsync<CierresDiarios>($"Select * from CierresDiarios order by Id desc limit 1");
        }
        // devuelve elementos por cantidad desendente--------------------------------------------------
        public async Task<List<CierresDiarios>> GetByDescCantCierreDiario(int index)
        {
            return await _database.QueryAsync<CierresDiarios>($"Select * from CierresDiarios order by Id desc limit {index}");
        }

        //---------------------------------CierreMensual-------------------------------------------

        //consulta completa CierreMensual----------------------------------------------------------
        public Task<List<CierresMensual>> GetCierreMensual()
        {
            return _database.Table<CierresMensual>().ToListAsync();
        }
        //consulta por id de CierreMensual---------------------------------------------------------
        public Task<CierresMensual> GetIdCierreMensual(int Id)
        {
            return _database.Table<CierresMensual>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el CierreMensual en la db--------------------------------------------------------
        public Task<int> SaveCierreMensual(CierresMensual U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el CierreMensual en la db------------------------------------
        public Task<int> SaveUpCierreMensual(CierresMensual U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla CierreMensual-----------------------------------------------
        public Task<int> DeleteCierreMensual(CierresMensual CierreMensual)
        {
            var x = _database.DeleteAsync(CierreMensual);
            return x;
        }
        // devuelve el ultimo elemento guardado---------------------------------------------------
        public async Task<List<CierresMensual>> GetLastItemCierreMensual()
        {
            return await _database.QueryAsync<CierresMensual>($"Select * from CierresMensual order by Id desc limit 1");
        }


        //---------------------------------FechaInicio-------------------------------------------

        //consulta por id de FechaInicio---------------------------------------------------------
        public Task<FechaInicio> GetIdFechaInicio(int Id)
        {
            return _database.Table<FechaInicio>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el FechaInicio en la db--------------------------------------------------------
        public Task<int> SaveFechaInicio(FechaInicio U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }


        //---------------------------------Contacts-------------------------------------------

        //consulta completa Contacts----------------------------------------------------------
        public Task<List<Contacto>> GetContacts()
        {
            return _database.Table<Contacto>().ToListAsync();
        }
        //consulta por id de Contacts---------------------------------------------------------
        public Task<Contacto> GetIdContacts(int Id)
        {
            return _database.Table<Contacto>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el Contacts en la db--------------------------------------------------------
        public Task<int> SaveContacts(Contacto U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el Contacts en la db------------------------------------
        public Task<int> SaveUpContacts(Contacto U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla Contacts-----------------------------------------------
        public Task<int> DeleteContacts(Contacto Contacto)
        {
            var x = _database.DeleteAsync(Contacto);
            return x;
        }

        //---------------------------------Opciones-------------------------------------------

        //consulta completa Opciones----------------------------------------------------------
        public Task<List<Opciones>> GetOpciones()
        {
            return _database.Table<Opciones>().ToListAsync();
        }
        //consulta por id de Opciones---------------------------------------------------------
        public Task<Opciones> GetIdOpciones(int Id)
        {
            return _database.Table<Opciones>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el Opciones en la db--------------------------------------------------------
        public Task<int> SaveOpciones(Opciones U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el Opciones en la db------------------------------------
        public Task<int> SaveUpOpciones(Opciones U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // devuelve por nombre en Opciones--------------------------
        public async Task<List<Opciones>> GetByNameOpciones(string name)
        {
            return await _database.QueryAsync<Opciones>($"Select * from Opciones where Nombre = '{name}'");
        }


        //---------------------------------Inventario-------------------------------------------

        //consulta completa Inventario----------------------------------------------------------
        public Task<List<Inventario>> GetInventario()
        {
            return _database.Table<Inventario>().ToListAsync();
        }
        //consulta por id de InvConstante---------------------------------------------------------
        public Task<Inventario> GetIdInventario(int Id)
        {
            return _database.Table<Inventario>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el Inventario en la db--------------------------------------------------------
        public Task<int> SaveInventario(Inventario U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el Inventario en la db------------------------------------
        public Task<int> SaveUpInventario(Inventario U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla InvConstante-----------------------------------------------
        public Task<int> DeleteInventario(Inventario Inventario)
        {
            var x = _database.DeleteAsync(Inventario);
            return x;
        }
        // devuelve el ultimo elemento guardado---------------------------------------------------
        public async Task<List<Inventario>> GetLastItemInventario()
        {
            return await _database.QueryAsync<Inventario>($"Select * from Inventario order by Id desc limit 1");
        }


        //---------------------------------Procesos-------------------------------------------

        //consulta completa Procesos----------------------------------------------------------
        public Task<List<Procesos>> GetProcesos()
        {
            return _database.Table<Procesos>().ToListAsync();
        }
        //consulta por id de Procesos---------------------------------------------------------
        public Task<Procesos> GetIdProcesos(int Id)
        {
            return _database.Table<Procesos>().Where(a => a.Id == Id).FirstOrDefaultAsync();
        }
        //annadir el Procesos en la db--------------------------------------------------------
        public Task<int> SaveProcesos(Procesos U)
        {
            if (U.Id == 0)
                return _database.InsertAsync(U);
            else return null;
        }
        //guardar la actualizacion de el Procesos en la db------------------------------------
        public Task<int> SaveUpProcesos(Procesos U)
        {
            if (U.Id != 0)
                return _database.UpdateAsync(U);
            else return _database.InsertAsync(U);
        }
        // borrar una fila de la tabla Procesos-----------------------------------------------
        public Task<int> DeleteProcesos(Procesos Procesos)
        {
            var x = _database.DeleteAsync(Procesos);
            return x;
        }
        // devuelve el elemento por el id del producto-----------------------------------------
        public async Task<List<Procesos>> GetByIdProProcesos(int ID)
        {
            return await _database.QueryAsync<Procesos>($"Select * from Procesos where IdPro = {ID}");
        }
        // devuelve los que estan en proceso en Procesos--------------------------------------------------------
        public async Task<List<Procesos>> GetActivosProcesos(bool enProceso)
        {
            return await _database.QueryAsync<Procesos>($"Select * from Procesos where EnProceso = {enProceso}");
        }
    }
}
