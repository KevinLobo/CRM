using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM
{
    [TestFixture]
    class Test : WebForm2
    {
        //--------------------------------------Pruebas persona---------------------------------------------------
        [TestCase]
        //Prueba revisar los datos cuando estos estan bien
        public void datosPersonaCorrectos()
        {
            Assert.AreEqual("",revisarDatosLLenos("116040499","Miguel Jimenez Torres","San Jose Costa Rica","84840496","miguelfenix16@gmail.com"));
        }

    }
}