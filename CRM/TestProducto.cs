using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRM
{
    [TestFixture]
    class TestProducto : producto
    {

        Label labelError = new Label();

        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void datosProductoCorrectos()
        {
            Assert.AreEqual("",revisarDatosLLenos("Mesas","30000", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el nombre es muy largo
        public void datosProductoNombreLargo()
        {
            String nombre = new String('a', 81);
            Assert.AreEqual("*El campo nombre no puede tener mas de 80 caracteres.<br />",
                revisarDatosLLenos(nombre, "30000", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el nombre esta vacio
        public void datosProductoNombreVacio()
        {
            Assert.AreEqual("*El campo nombre no puede estar vacio.<br />",
                revisarDatosLLenos("", "30000", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el precio es vacio
        public void datosProductoDireccionVacia()
        {
            Assert.AreEqual("*El campo precio no puede estar vacio.<br />", 
                revisarDatosLLenos("Mesas", "", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el precio no tiene solo numeros
        public void datosProductoPrecioNoSoloNumeros()
        {
            Assert.AreEqual("*El campo precio solo puede contener numeros.<br />",
                revisarDatosLLenos("Mesas", "123abc", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando dos datos estan mal
        public void datosProductoDosErroneos()
        {
            Assert.AreEqual("*El campo nombre no puede estar vacio.<br />*El campo precio no puede estar vacio.<br />",
                revisarDatosLLenos("", "", labelError));
        }

        [TestCase]
        //Prueba si el error y el label de error coinciden en caso de error
        public void labelErrorIgualAError()
        {
            revisarDatosLLenos("", "30000", labelError);
            Assert.That(labelError.Text == "*El campo nombre no puede estar vacio.<br />");
        }

        [TestCase]
        //Prueba la funcion isDigit cuando el dato son solo numeros
        public void isDigitTrue()
        {
            Assert.IsTrue(IsDigitsOnly("123"));
        }

        [TestCase]
        //Prueba la funcion isDigit cuando el dato no son solo numeros
        public void isDigitFalse()
        {
            Assert.IsFalse(IsDigitsOnly("123abc"));
        }

    }

    [TestFixture]
    class TestProductoBD
    {
        fakeBaseDatos fBD = new fakeBaseDatos(true, true, true, true, false);

        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void productoCargarGridView()
        {
            GridView grid = new GridView();
            producto productoI = new producto(fBD);
            productoI.CargarProductos(0, grid);
            Assert.That(grid.Rows[0].Cells[0].Text == "OK");
        }

        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void productoAgregarBaseDatos()
        {
            producto productoI = new producto(fBD);
            Assert.AreEqual(true, productoI.InsertarProducto("", ""));
        }

        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void productoElminarBaseDatos()
        {
            producto productoI = new producto(fBD);
            Assert.AreEqual(true, productoI.BorrarProducto("1"));
        }

        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void productoActualizarBaseDatos()
        {
            producto productoI = new producto(fBD);
            Label lblID = new Label();
            lblID.Text = "1";
            Assert.AreEqual(true, productoI.ActualizarProducto(lblID, "",""));
        }

        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void productoErrorEnBaseDatos()
        {
            fakeBaseDatos fBD = new fakeBaseDatos(true, true, true, true, true);
            producto productoI = new producto(fBD);
            var ex = Assert.Throws<Exception>(() => productoI.BorrarProducto("1"));
            Assert.That(ex.Message, Is.EqualTo("Error al acceder a la base de datos."));
        }
    }
}