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
            WebForm2 persona =new WebForm2();
            //persona.setCedula("116040499");
            //Assert.AreEqual(true, )
            //persona.setCedula("116040499");
            //base.setCorreo("miguelfenix16@gmail.com");
            //base.setNombre("Miguel Jimenez Torres");
            //base.setDireccion("San Jose Costa Rica");
            //base.setTelefono("84840496");
            Assert.IsTrue(base.revisarDatosLLenos());
        }

    }
}