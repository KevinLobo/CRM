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
    [TestFixture]
    class TestReporteErrores : reporteErrores
    {

        Label labelError = new Label();
        TextBox tTxtNombre = new TextBox(),
            tTxtDireccion = new TextBox(),
            tTxtTelefono = new TextBox();
        Label tLblId = new Label();
        Button tBtnSubmit = new Button();
        Button tBtnUpdate = new Button();


        [TestCase("1", "1", true)]
        [TestCase("12043", "0", false)]
        public void EsVentaValida_TC(String idVenta, String cantidad, bool respuesta)
        {
            DataTable tabla = new DataTable();
            DataRow fila = tabla.NewRow();
            tabla.Columns.Add(new DataColumn("COUNT"));
            fila["COUNT"] = cantidad;
            tabla.Rows.Add(fila);
            DataTableReader reader = new DataTableReader(tabla);

            IBaseDatos baseDatosFalsa = Substitute.For<IBaseDatos>();
            baseDatosFalsa.getSalida().Returns(reader);

            reporteErrores errores = new reporteErrores(baseDatosFalsa);
            Assert.AreEqual(respuesta, errores.EsVentaValida(idVenta));
        }

        
        /*[TestCase]
        public void EsVentaValida_Excepcion_TC() {
            IBaseDatos baseDatosFalsa = Substitute.For<IBaseDatos>();
            baseDatosFalsa.Abrir().Returns( x => { throw new MySqlException });
        }*/


        [TestCase]
        public void InsertarReporte_TC()
        {
            DateTime fechaHora = DateTime.Now;
            string reporte = "El producto #618 no sirve";
            string idVenta = "1";
            string idCliente = "1";

            fakeBaseDatos fBD = new fakeBaseDatos(true, true, true, true, false, 1);
            reporteErrores errores = new reporteErrores(fBD);
            Assert.AreEqual(true, errores.InsertarReporte(fechaHora, reporte, idVenta, idCliente));
        }

        [TestCase]
        public void RevisarDatosLlenos_ReporteLargo_TC()
        {

            DataTable tabla = new DataTable();
            DataRow fila = tabla.NewRow();
            tabla.Columns.Add(new DataColumn("COUNT"));
            fila["COUNT"] = "1";
            tabla.Rows.Add(fila);
            DataTableReader reader = new DataTableReader(tabla);

            IBaseDatos baseDatosFalsa = Substitute.For<IBaseDatos>();
            baseDatosFalsa.getSalida().Returns(reader);


            String reporte = new String('a', 1001);
            String idVenta = "1";
            
            reporteErrores errores = new reporteErrores(baseDatosFalsa);
            Assert.AreEqual("*El campo de reporte no puede tener mas de 1000 caracteres.<br />", errores.RevisarDatosLlenos(reporte, idVenta, new Label()));
        }

        [TestCase]
        public void RevisarDatosLlenos_ReporteCorto_TC()
        {

            DataTable tabla = new DataTable();
            DataRow fila = tabla.NewRow();
            tabla.Columns.Add(new DataColumn("COUNT"));
            fila["COUNT"] = "1";
            tabla.Rows.Add(fila);
            DataTableReader reader = new DataTableReader(tabla);

            IBaseDatos baseDatosFalsa = Substitute.For<IBaseDatos>();
            baseDatosFalsa.getSalida().Returns(reader);


            String reporte = "";
            String idVenta = "1";

            reporteErrores errores = new reporteErrores(baseDatosFalsa);
            Assert.AreEqual("*El campo del reporte no puede estar vacio.<br />", errores.RevisarDatosLlenos(reporte, idVenta, new Label()));
        }

        [TestCase]
        public void RevisarDatosLlenos_VentaInvalida_TC()
        {

            DataTable tabla = new DataTable();
            DataRow fila = tabla.NewRow();
            tabla.Columns.Add(new DataColumn("COUNT"));
            fila["COUNT"] = "0";
            tabla.Rows.Add(fila);
            DataTableReader reader = new DataTableReader(tabla);

            IBaseDatos baseDatosFalsa = Substitute.For<IBaseDatos>();
            baseDatosFalsa.getSalida().Returns(reader);


            String reporte = "Producto malo";
            String idVenta = "1";

            reporteErrores errores = new reporteErrores(baseDatosFalsa);
            Assert.AreEqual("*La venta seleccionada no existe.<br />", errores.RevisarDatosLlenos(reporte, idVenta, new Label()));
        }

        [TestCase]
        public void RevisarDatosLlenos_Valido_TC()
        {

            DataTable tabla = new DataTable();
            DataRow fila = tabla.NewRow();
            tabla.Columns.Add(new DataColumn("COUNT"));
            fila["COUNT"] = "1";
            tabla.Rows.Add(fila);
            DataTableReader reader = new DataTableReader(tabla);

            IBaseDatos baseDatosFalsa = Substitute.For<IBaseDatos>();
            baseDatosFalsa.getSalida().Returns(reader);


            String reporte = "Producto malo";
            String idVenta = "1";

            reporteErrores errores = new reporteErrores(baseDatosFalsa);
            Assert.AreEqual("", errores.RevisarDatosLlenos(reporte, idVenta, new Label()));
        }
    }
}