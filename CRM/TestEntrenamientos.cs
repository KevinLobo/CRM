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
    public class TestEntrenamientos
    {

        [TestCase("11/22/2017 2:54 PM", true)]
        [TestCase("", false)]
        [TestCase("11/222017 2:54 PM", false)]
        public void FormatoFechaValido_TC(String fecha, Boolean resultado)
        {
            fakeBaseDatos fakeBD = new fakeBaseDatos(true, true, true, true, false, 1);
            entrenamientos entrenamientos = new entrenamientos(fakeBD);

            Assert.AreEqual(resultado, entrenamientos.FormatoFechaValido(fecha));
        }

        [TestCase("01/01/2017 2:54 PM", "11/22/2017 2:55 PM", true)]
        [TestCase("11/22/2017 2:55 PM", "01/01/2017 2:54 PM", false)]
        public void RangoFechasValido_TC(String fechaInicial, String fechaFinal, Boolean resultado)
        {
            fakeBaseDatos fakeBD = new fakeBaseDatos(true, true, true, true, false, 1);
            entrenamientos entrenamientos = new entrenamientos(fakeBD);

            Assert.AreEqual(resultado, entrenamientos.RangoFechasValido(fechaInicial, fechaFinal));
        }

        [TestCase("", "", true, "")]
        [TestCase("", "11/22/2017 2:55 PM", false, "*El campo de fecha inicial no puede estar vacío. <br />")]
        [TestCase("01/01/2017 2:54 PM", "", false, "*El campo de fecha final no puede estar vacío. <br />")]
        [TestCase("01/012017 2:54 PM", "11/22/2017 2:55 PM", false, "*El formato de la fecha inicial no es válido. <br />")]
        [TestCase("01/01/2017 2:54 PM", "1122/2017 2:55 PM", false, "*El formato de la fecha final no es válido. <br />")]
        [TestCase("11/22/2017 2:55 PM", "01/01/2017 2:54 PM", false, "*La fecha inicial debe ser menor que la final. <br />")]
        [TestCase("01/01/2017 2:54 PM", "11/22/2017 2:55 PM", false, "")]
        public void RevisarDatosLlenos_TC(String fechaInicial, String fechaFinal, Boolean cualquierFecha, String resultado)
        {
            fakeBaseDatos fakeBD = new fakeBaseDatos(true, true, true, true, false, 1);
            entrenamientos entrenamientos = new entrenamientos(fakeBD);

            Assert.AreEqual(resultado, entrenamientos.RevisarDatosLlenos(fechaInicial, fechaFinal, cualquierFecha, "", new Label()));
        }

        public void SuscribirseAEntrenamiento_TC()
        {
            fakeBaseDatos fakeBD = new fakeBaseDatos(true, true, true, true, false, 1);
            entrenamientos entrenamientos = new entrenamientos(fakeBD);

            Assert.AreEqual(true, entrenamientos.SuscribirseAEntrenamiento("1"));
        }

        public void DesuscribirseAEntrenamiento_TC()
        {
            fakeBaseDatos fakeBD = new fakeBaseDatos(true, true, true, true, false, 1);
            entrenamientos entrenamientos = new entrenamientos(fakeBD);

            Assert.AreEqual(true, entrenamientos.DesuscribirseAEntrenamiento("1"));
        }


        [TestCase("01/01/2017 2:54 PM", "11/22/2017 2:55 PM", false, "Productos HP", "1", false, true)]
        public void BuscarEntrenamientos_TC(string fechaInicio, string fechaFinal, Boolean cualquierFecha, string nombreEvento, string idCliente, Boolean asistido, Boolean respuesta)
        {
            fakeBaseDatos fakeBD = new fakeBaseDatos(true, true, true, true, false, 1);
            entrenamientos entrenamientos = new entrenamientos(fakeBD);

            Assert.AreEqual(respuesta, entrenamientos.BuscarEventos(fechaInicio, fechaFinal, cualquierFecha, nombreEvento, idCliente, asistido, new GridView()));   
        }
    }
}