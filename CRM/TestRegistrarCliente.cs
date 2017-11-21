using MySql.Data.MySqlClient;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRM
{
    public class TestRegistrarCliente
    {
        [TestCase("1", "1", true)]
        [TestCase("34", "0", false)]
        public void EsClienteValido_TC(String idIdentidad, String cantidad, Boolean respuesta)
        {
            DataTable tabla = new DataTable();
            DataRow fila = tabla.NewRow();
            tabla.Columns.Add(new DataColumn("COUNT"));
            fila["COUNT"] = cantidad;
            tabla.Rows.Add(fila);
            DataTableReader reader = new DataTableReader(tabla);

            IBaseDatos baseDatosFalsa = Substitute.For<IBaseDatos>();
            baseDatosFalsa.getSalida().Returns(reader);

            registrarCliente registro = new registrarCliente(baseDatosFalsa);
            Assert.AreEqual(respuesta, registro.EsClienteValido(idIdentidad));
        }


        [TestCase("usuarioExistente", "1", false)]
        [TestCase("usuarioNoExistente", "0", true)]
        public void EsUsuarioValido_TC(String usuario, String cantidad, Boolean respuesta)
        {
            DataTable tabla = new DataTable();
            DataRow fila = tabla.NewRow();
            tabla.Columns.Add(new DataColumn("COUNT"));
            fila["COUNT"] = cantidad;
            tabla.Rows.Add(fila);
            DataTableReader reader = new DataTableReader(tabla);

            IBaseDatos baseDatosFalsa = Substitute.For<IBaseDatos>();
            baseDatosFalsa.getSalida().Returns(reader);

            registrarCliente registro = new registrarCliente(baseDatosFalsa);
            Assert.AreEqual(respuesta, registro.EsUsuarioValido(usuario));
        }

        [TestCase("usuarioNoRegistrado", "password", "1")]
        public void InsertarUsuarioCliente_TC(string username, string password, string idEntidad)
        {
            fakeBaseDatos fakeBD = new fakeBaseDatos(true, true, true, true, false, 1);
            registrarCliente registro = new registrarCliente(fakeBD);
            Assert.AreEqual(true, registro.InsertarUsuarioCliente(username, password, idEntidad));
        }

        [TestCase("", "password", "password", "4", "0", "*El campo de usuario no puede estar vacío.<br />*El cliente seleccionado no existe.<br />")]
        [TestCase("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "password", "password", "4", "0", "*El campo de usuario no puede tener mas de 40 caracteres.<br />*El cliente seleccionado no existe.<br />")]
        [TestCase("usuarioExistente", "password", "password", "4", "1", "*El nombre de usuario ya existe.<br />*El cliente seleccionado no existe.<br />")]
        [TestCase("usuario", "", "password", "4", "0", "*El campo de contraseña no puede estar vacío.<br />*Las contraseñas no coinciden.<br />*El cliente seleccionado no existe.<br />")]
        [TestCase("usuario", 
            "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 
            "password", "4", "0", "*El campo de contraseña no puede tener más de 128 caracteres.<br />*Las contraseñas no coinciden.<br />*El cliente seleccionado no existe.<br />")]
        [TestCase("usuario", "password", "", "4", "0", "*El campo de confirmar contraseña no puede estar vacío.<br />*Las contraseñas no coinciden.<br />*El cliente seleccionado no existe.<br />")]
        [TestCase("usuario", "password", "password1", "4", "0", "*Las contraseñas no coinciden.<br />*El cliente seleccionado no existe.<br />")]
        public void RevisarDatosLlenos_TC(string username, string password, string confirmPassword, string idEntidad, string resultadoBD, string resultado)
        {
            DataTable tabla = new DataTable();
            DataRow fila = tabla.NewRow();
            tabla.Columns.Add(new DataColumn("COUNT"));
            fila["COUNT"] = resultadoBD;
            tabla.Rows.Add(fila);
            DataTableReader reader = new DataTableReader(tabla);

            IBaseDatos baseDatosFalsa = Substitute.For<IBaseDatos>();
            baseDatosFalsa.getSalida().Returns(reader);

            registrarCliente registro = new registrarCliente(baseDatosFalsa);
            Assert.AreEqual(resultado, registro.RevisarDatosLlenos(username, password, confirmPassword, idEntidad, new Label()));
        }
    }
}