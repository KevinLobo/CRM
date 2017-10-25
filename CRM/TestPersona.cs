using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM
{
    [TestFixture]
    class TestPersona : WebForm2
    {
        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void datosPersonaCorrectos()
        {
            Assert.AreEqual("",revisarDatosLLenos("116040499","Miguel Jimenez Torres","San Jose Costa Rica","84840496","miguelfenix16@gmail.com"));
        }

        [TestCase]
        //Prueba revisar los datos cuando el nombre es muy largo
        public void datosPersonaNombreLargo()
        {
            String nombre = new String('a', 210);
            Assert.AreEqual("*El campo nombre no puede tener mas de 80 caracteres.<br />", revisarDatosLLenos("116040499", nombre, "San Jose Costa Rica", "84840496", "miguelfenix16@gmail.com"));
        }

        [TestCase]
        //Prueba revisar los datos cuando el nombre esta vacio
        public void datosPersonaNombreVacio()
        {
            Assert.AreEqual("*El campo nombre no puede estar vacio.<br />", revisarDatosLLenos("116040499", "", "San Jose Costa Rica", "84840496", "miguelfenix16@gmail.com"));
        }

        [TestCase]
        //Prueba revisar los datos cuando la cedula es muy larga
        public void datosPersonaCedulaLarga()
        {
            Assert.AreEqual("*El campo cedula no puede tener mas de 80 caracteres.<br />", revisarDatosLLenos("01234567890123456789", "Miguel Jimenez Torres", "San Jose Costa Rica", "84840496", "miguelfenix16@gmail.com"));
        }

        //Prueba revisar los datos cuando la cedula es vacia
        public void datosPersonaCedulaVacia()
        {
            Assert.AreEqual("*El campo cedula no puede estar vacio.<br />", revisarDatosLLenos("", "Miguel Jimenez Torres", "San Jose Costa Rica", "84840496", "miguelfenix16@gmail.com"));
        }

        [TestCase]
        //Prueba revisar los datos cuando la direccion es muy larga
        public void datosPersonaDireccionLarga()
        {
            String direccion = new String('a', 210);
            Assert.AreEqual("*El campo direccion no puede tener más de 200 caracteres.<br />", revisarDatosLLenos("116040499", "Miguel Jimenez Torres", direccion, "84840496", "miguelfenix16@gmail.com"));
        }

        [TestCase]
        //Prueba revisar los datos cuando la direccion es vacia
        public void datosPersonaDireccionVacia()
        {
            Assert.AreEqual("*El campo direccion no puede estar vacio.<br />", revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "", "84840496", "miguelfenix16@gmail.com"));
        }

        [TestCase]
        //Prueba revisar los datos cuando el telefono es muy largo
        public void datosPersonaTelefonoLargo()
        {
            Assert.AreEqual("*El campo telefono no puede tener más de 8 caracteres.<br />", revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "San Jose Costa Rica", "8484049600", "miguelfenix16@gmail.com"));
        }

        [TestCase]
        //Prueba revisar los datos cuando el telefono esta vacio
        public void datosPersonaTelefonoVacio()
        {
            Assert.AreEqual("*El campo telefono no puede estar vacio.<br />", revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "San Jose Costa Rica", "", "miguelfenix16@gmail.com"));
        }

        [TestCase]
        //Prueba revisar los datos cuando el telefono no tiene solo numeros
        public void datosPersonaTelefonoLetras()
        {
            Assert.AreEqual("*El campo telefono solo puede contener numeros.<br />", revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "San Jose Costa Rica", "abc40496", "miguelfenix16@gmail.com"));
        }

        [TestCase]
        //Prueba revisar los datos cuando el correo es muy largo
        public void datosPersonaCorreoLargo()
        {
            Assert.AreEqual("*El campo correo no puede tener mas de 80 caracteres.<br />", revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "San Jose Costa Rica", "84840496", "imagusunimottohandsupsooumetuatesutostanduporenaihatakagenandodatewakoroestandasumaraysodfkladsflksjd@gmail.com"));
        }

        [TestCase]
        //Prueba revisar los datos cuando el correo esta vacio
        public void datosPersonaCorreoVacio()
        {
            Assert.AreEqual("*El campo correo no puede estar vacio.<br />", revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "San Jose Costa Rica", "84840496", ""));
        }

        [TestCase]
        //Prueba revisar los datos cuando dos datos estan mal
        public void datosPersonaDosErroneos()
        {
            Assert.AreEqual("*El campo cedula no puede tener mas de 80 caracteres.<br />*El campo telefono no puede tener más de 8 caracteres.<br />", revisarDatosLLenos("11604049900", "Miguel Jimenez Torres", "San Jose Costa Rica", "8484049600", "miguelfenix16@gmail.com"));
        }
    }
}