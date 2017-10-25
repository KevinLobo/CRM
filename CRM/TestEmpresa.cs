using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM
{
    [TestFixture]
    class TestEmpresa : WebForm3
    {
        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void datosEmpresaCorrectos()
        {
            Assert.AreEqual("",revisarDatosLLenos("KimberlyClark","San Jose Costa Rica","22222222"));
        }

        [TestCase]
        //Prueba revisar los datos cuando el nombre es muy largo
        public void datosEmpresaNombreLargo()
        {
            String nombre = new String('a', 210);
            Assert.AreEqual("*El campo nombre no puede tener mas de 80 caracteres.<br />", revisarDatosLLenos(nombre, "San Jose Costa Rica", "84840496"));
        }

        [TestCase]
        //Prueba revisar los datos cuando el nombre esta vacio
        public void datosEmpresaNombreVacio()
        {
            Assert.AreEqual("*El campo nombre no puede estar vacio.<br />", revisarDatosLLenos("", "San Jose Costa Rica", "84840496"));
        }

        [TestCase]
        //Prueba revisar los datos cuando la direccion es muy larga
        public void datosEmpresaDireccionLarga()
        {
            String direccion = new String('a', 210);
            Assert.AreEqual("*El campo direccion no puede tener más de 200 caracteres.<br />", revisarDatosLLenos("KimberlyClark", direccion, "84840496"));
        }

        [TestCase]
        //Prueba revisar los datos cuando la direccion es vacia
        public void datosEmpresaDireccionVacia()
        {
            Assert.AreEqual("*El campo direccion no puede estar vacio.<br />", revisarDatosLLenos("KimberlyClark", "", "84840496"));
        }

        [TestCase]
        //Prueba revisar los datos cuando el telefono es muy largo
        public void datosEmpresaTelefonoLargo()
        {
            Assert.AreEqual("*El campo telefono no puede tener más de 8 caracteres.<br />", revisarDatosLLenos("KimberlyClark", "San Jose Costa Rica", "8484049600"));
        }

        [TestCase]
        //Prueba revisar los datos cuando el telefono esta vacio
        public void datosEmpresaTelefonoVacio()
        {
            Assert.AreEqual("*El campo telefono no puede estar vacio.<br />", revisarDatosLLenos("KimberlyClark", "San Jose Costa Rica", ""));
        }

        [TestCase]
        //Prueba revisar los datos cuando el telefono no tiene solo numeros
        public void datosEmpresaTelefonoLetras()
        {
            Assert.AreEqual("*El campo telefono solo puede contener numeros.<br />", revisarDatosLLenos("KimberlyClark", "San Jose Costa Rica", "abc40496"));
        }

        [TestCase]
        //Prueba revisar los datos cuando dos datos estan mal
        public void datosEmpresaDosErroneos()
        {
            Assert.AreEqual("*El campo direccion no puede estar vacio.<br />*El campo telefono no puede tener más de 8 caracteres.<br />", revisarDatosLLenos("KimberlyClark", "", "8484049600"));
        }
    }
}