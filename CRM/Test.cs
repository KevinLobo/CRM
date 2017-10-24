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
            Assert.AreEqual("*El campo nombre no puede tener mas de 80 caracteres.<br />", revisarDatosLLenos("116040499", "Sacarias Piedras del Rio Milagro de Jesus Cuarto Rodriguez Cooper 0123456789 abcdefghijklmnopqrstuvwxyz", "San Jose Costa Rica", "84840496", "miguelfenix16@gmail.com"));
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
            Assert.AreEqual("*El campo direccion no puede tener más de 200 caracteres.<br />", revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "aaaaaaaabbbbbbbbccccccccddddddddeeeeeeeeffffffffgggggggghhhhhhhhiiiiiiiijjjjjjjjkkkkkkkkmmmmmmmmnnnnnnnnllllllllooooooooppppppppqqqqqqqqrrrrrrrrssssssssttttttttuuuuuuuuvvvvvvvvwwwwwwwwxxxxxxxxyyyyyyyyzzzzzzzz", "84840496", "miguelfenix16@gmail.com"));
        }

        [TestCase]
        //Prueba revisar los datos cuando la direccion es vacia
        public void datosPersonaDireccionVacia()
        {
            Assert.AreEqual("*El campo direccion no puede estar vacio.<br />", revisarDatosLLenos("116040499", "Miguel Jimenez Torres", "", "84840496", "miguelfenix16@gmail.com"));
        }
    }
}