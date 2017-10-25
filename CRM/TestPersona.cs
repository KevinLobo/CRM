using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRM
{
    [TestFixture]
    class TestPersona : WebForm2
    {
        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void datosPersonaCorrectos()
        {
            Label labelError = new Label();
            Assert.AreEqual("",revisarDatosLLenos("116040499","Miguel Jimenez Torres",
                "San Jose Costa Rica","84840496","miguelfenix16@gmail.com", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el nombre es muy largo
        public void datosPersonaNombreLargo()
        {
            Label labelError = new Label();
            String nombre = new String('a', 210);
            Assert.AreEqual("*El campo nombre no puede tener mas de 80 caracteres.<br />",
                revisarDatosLLenos("116040499", nombre, "San Jose Costa Rica", "84840496",
                "miguelfenix16@gmail.com", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el nombre esta vacio
        public void datosPersonaNombreVacio()
        {
            Label labelError = new Label();
            Assert.AreEqual("*El campo nombre no puede estar vacio.<br />",
                revisarDatosLLenos("116040499", "", "San Jose Costa Rica", "84840496",
                "miguelfenix16@gmail.com", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando la cedula es muy larga
        public void datosPersonaCedulaLarga()
        {
            Label labelError = new Label();
            Assert.AreEqual("*El campo cedula no puede tener mas de 80 caracteres.<br />",
                revisarDatosLLenos("01234567890123456789", "Miguel Jimenez Torres", "San Jose Costa Rica",
                "84840496", "miguelfenix16@gmail.com", labelError));
        }

        //Prueba revisar los datos cuando la cedula es vacia
        public void datosPersonaCedulaVacia()
        {
            Label labelError = new Label();
            Assert.AreEqual("*El campo cedula no puede estar vacio.<br />",
                revisarDatosLLenos("", "Miguel Jimenez Torres", "San Jose Costa Rica",
                "84840496", "miguelfenix16@gmail.com", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando la direccion es muy larga
        public void datosPersonaDireccionLarga()
        {
            Label labelError = new Label();
            String direccion = new String('a', 210);
            Assert.AreEqual("*El campo direccion no puede tener más de 200 caracteres.<br />",
                revisarDatosLLenos("116040499", "Miguel Jimenez Torres", direccion, "84840496",
                "miguelfenix16@gmail.com", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando la direccion es vacia
        public void datosPersonaDireccionVacia()
        {
            Label labelError = new Label();
            Assert.AreEqual("*El campo direccion no puede estar vacio.<br />",
                revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "", "84840496",
                "miguelfenix16@gmail.com", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el telefono es muy largo
        public void datosPersonaTelefonoLargo()
        {
            Label labelError = new Label();
            Assert.AreEqual("*El campo telefono no puede tener más de 8 caracteres.<br />",
                revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "San Jose Costa Rica",
                "8484049600", "miguelfenix16@gmail.com", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el telefono esta vacio
        public void datosPersonaTelefonoVacio()
        {
            Label labelError = new Label();
            Assert.AreEqual("*El campo telefono no puede estar vacio.<br />", 
                revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "San Jose Costa Rica", 
                "", "miguelfenix16@gmail.com", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el telefono no tiene solo numeros
        public void datosPersonaTelefonoLetras()
        {
            Label labelError = new Label();
            Assert.AreEqual("*El campo telefono solo puede contener numeros.<br />",
                revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "San Jose Costa Rica",
                "abc40496", "miguelfenix16@gmail.com", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el correo es muy largo
        public void datosPersonaCorreoLargo()
        {
            Label labelError = new Label();
            Assert.AreEqual("*El campo correo no puede tener mas de 80 caracteres.<br />",
                revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "San Jose Costa Rica",
                "84840496", "imagusunimottohandsupsooumetuatesutostanduporenaihatakagenandoda" +
                "tewakoroestandasumaraysodfkladsflksjd@gmail.com", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el correo esta vacio
        public void datosPersonaCorreoVacio()
        {
            Label labelError = new Label();
            Assert.AreEqual("*El campo correo no puede estar vacio.<br />",
                revisarDatosLLenos("116040499", "Miguel Jimenez Torres",
                "San Jose Costa Rica", "84840496", "", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando dos datos estan mal
        public void datosPersonaDosErroneos()
        {
            Label labelError = new Label();
            Assert.AreEqual("*El campo cedula no puede tener mas de 80 caracteres.<br />*El campo" +
                " telefono no puede tener más de 8 caracteres.<br />",
                revisarDatosLLenos("11604049900", "Miguel Jimenez Torres", "San Jose Costa Rica", "8484049600",
                "miguelfenix16@gmail.com", labelError));
        }

        [TestCase]
        //Prueba si el error y el label de error coinciden en caso de error
        public void labelErrorIgualAError()
        {
            Label labelError = new Label();
            revisarDatosLLenos("116040499", "Miguel Jimenez Torres",
                "San Jose Costa Rica", "84840496", "", labelError);
            Assert.That(labelError.Text=="*El campo correo no puede estar vacio.<br />");
        }
    }
}