using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CRM
{
    public class fakeBaseDatos : IBaseDatos
    {
        private Boolean salidaAbrir;
        private Boolean salidaCerrar;
        private Boolean salidaQuery;
        private Boolean error = false;
        private long salidaId;


        public fakeBaseDatos(Boolean pSalidaAbrir, Boolean pSalidaCerrar, Boolean pSalidaQuery,
                            Boolean pSaldaResultados, Boolean pError, long pSalidaId)
        {
            salidaAbrir = pSalidaAbrir;
            salidaCerrar = pSalidaCerrar;
            salidaQuery = pSalidaQuery;
            error = pError;
            salidaId = pSalidaId;
        }

        public Boolean Abrir()
        {
            return salidaAbrir;
        }
        public Boolean Cerrar()
        {
            return salidaCerrar;
        }
        public Boolean cargarQuery(String comando)
        {
            return salidaQuery;
        }

        public IDataReader getSalida()
        {
            DataTable table = new DataTable();
            DataRow row = table.NewRow();
            table.Columns.Add(new DataColumn("Salida"));
            row["Salida"] = "OK";
            table.Rows.Add(row);
            DataTableReader reader = new DataTableReader(table);
            if (!error)
            {
                return reader;
            }
            else
            {
                throw new System.Exception("Error al acceder a la base de datos.");
            }
        }

        public long LatestId()
        {
            return salidaId;
        }
    }
}