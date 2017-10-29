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
        public void datosEmpresaCorrectos()
        {

            Assert.AreEqual("",revisarDatosLLenos("Mesas","30000", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el nombre es muy largo
        public void datosEmpresaNombreLargo()
        {
            String nombre = new String('a', 81);
            Assert.AreEqual("*El campo nombre no puede tener mas de 80 caracteres.<br />",
                revisarDatosLLenos(nombre, "30000", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el nombre esta vacio
        public void datosEmpresaNombreVacio()
        {
            Assert.AreEqual("*El campo nombre no puede estar vacio.<br />",
                revisarDatosLLenos("", "30000", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando la precio es vacio
        public void datosEmpresaDireccionVacia()
        {
            Assert.AreEqual("*El campo precio no puede estar vacio.<br />", 
                revisarDatosLLenos("Mesas", "", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando dos datos estan mal
        public void datosEmpresaDosErroneos()
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
}