using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace CRM
{
    public interface IBaseDatos
    {
        Boolean Abrir();
        Boolean Cerrar();
        Boolean cargarQuery(String query);
        IDataReader getSalida();
    }

}