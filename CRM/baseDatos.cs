using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace CRM
{
    public class baseDatos:IBaseDatos
    {
        private MySqlConnection conexion;
        private MySqlCommand command;
        private MySqlDataReader reader;

        public baseDatos(string pConexion)
        {
            conexion = new MySqlConnection(pConexion);
            command = new MySqlCommand();
            command.Connection = conexion;
        }

        public Boolean Abrir()
        {
            try
            {
                conexion.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean Cerrar()
        {
            try
            {
                conexion.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean cargarQuery(String pQuery)
        {
            try
            {
                command.CommandText = pQuery;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IDataReader getSalida() {
            reader = command.ExecuteReader();
            return reader;

        }
    }
}