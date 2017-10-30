using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRM
{
    [TestFixture]
    class TestPropuesta : propuesta
    {
        TextBox txtId = new TextBox();
        TextBox txtDate = new TextBox();
        TextBox txtPrecio = new TextBox();
        TextBox txtDescuento = new TextBox();
        TextBox txtComision = new TextBox();
        TextBox txtRespuesta = new TextBox();
        Label lblCliente = new Label();
        RadioButton rbAprobado = new RadioButton();
        RadioButton rbRechazado = new RadioButton();
        Label lblError = new Label();

        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void datosPropuestaCorrectos()
        {
            txtId.Text = "01";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "30000";
            txtDescuento.Text = "10";
            txtComision.Text = "10";
            txtRespuesta.Text = "Respuesta";
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = true;

            Assert.AreEqual("", revisarDatosLLenos(txtId,txtDate,txtPrecio,txtDescuento,txtComision,txtRespuesta,lblCliente,rbAprobado,rbRechazado,lblError));
        }

        [TestCase]
        //Prueba revisar los datos cuando ID esta vacio
        public void datosPropuestaIdVacio()
        {
            txtId.Text = "";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "30000";
            txtDescuento.Text = "10";
            txtComision.Text = "10";
            txtRespuesta.Text = "Respuesta";
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = true;

            Assert.AreEqual("*El campo producto no puede estar vacio.<br />", 
                revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente, 
                rbAprobado, rbRechazado, lblError));
        }

        [TestCase]
        //Prueba revisar los datos cuando ID es muy largo
        public void datosPropuestaIdLargo()
        {
            txtId.Text = "0123456789";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "30000";
            txtDescuento.Text = "10";
            txtComision.Text = "10";
            txtRespuesta.Text = "Respuesta";
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = true;

            Assert.AreEqual("*El campo producto no puede tener mas de 8 caracteres.<br />", 
                revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente, 
                rbAprobado, rbRechazado, lblError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el date esta vacio
        public void datosPropuestaDateVacio()
        {
            txtId.Text = "01";
            txtDate.Text = "";
            txtPrecio.Text = "30000";
            txtDescuento.Text = "10";
            txtComision.Text = "10";
            txtRespuesta.Text = "Respuesta";
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = true;

            Assert.AreEqual("*El campo fecha no puede estar vacio.<br />", 
                revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente, 
                rbAprobado, rbRechazado, lblError));
        }

        [TestCase]
        //Prueba revisar los datos cuando precio esta vacio
        public void datosPropuestaPrecioVacio()
        {
            txtId.Text = "01";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "";
            txtDescuento.Text = "10";
            txtComision.Text = "10";
            txtRespuesta.Text = "Respuesta";
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = true;

            Assert.AreEqual("*El campo precio no puede estar vacio.<br />", 
                revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente, 
                rbAprobado, rbRechazado, lblError));
        }

        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void datosPropuestaPrecioNoSoloNumeros()
        {
            txtId.Text = "01";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "123abc";
            txtDescuento.Text = "10";
            txtComision.Text = "10";
            txtRespuesta.Text = "Respuesta";
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = true;

            Assert.AreEqual("*El campo precio debe tener solo numeros.<br />", revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente, rbAprobado, rbRechazado, lblError));
        }

        [TestCase]
        //Prueba revisar los datos cuando descuento esta vacio
        public void datosPropuestaDescuentoVacio()
        {
            txtId.Text = "01";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "30000";
            txtDescuento.Text = "";
            txtComision.Text = "10";
            txtRespuesta.Text = "Respuesta";
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = true;

            Assert.AreEqual("*El campo descuento no puede estar vacio.<br />", 
                revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente, 
                rbAprobado, rbRechazado, lblError));
        }

        [TestCase]
        //Prueba revisar los datos cuando comision esta vacia
        public void datosPropuestaComisionVacio()
        {
            txtId.Text = "01";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "30000";
            txtDescuento.Text = "10";
            txtComision.Text = "";
            txtRespuesta.Text = "Respuesta";
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = true;

            Assert.AreEqual("*El campo comisión no puede estar vacio.<br />", 
                revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente, 
                rbAprobado, rbRechazado, lblError));
        }

        [TestCase]
        //Prueba revisar los datos cuando respuesta esta vacia
        public void datosPropuestaRespuestaVacia()
        {
            txtId.Text = "01";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "30000";
            txtDescuento.Text = "10";
            txtComision.Text = "10";
            txtRespuesta.Text = "";
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = true;

            Assert.AreEqual("*El campo respuesta no puede estar vacio.<br />", 
                revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente, 
                rbAprobado, rbRechazado, lblError));
        }

        [TestCase]
        //Prueba revisar los datos cuando respuesta es muy larga
        public void datosPropuestaRespuestaLarga()
        {
            txtId.Text = "01";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "30000";
            txtDescuento.Text = "10";
            txtComision.Text = "10";
            String respuesta = new String('a', 210);
            txtRespuesta.Text = respuesta;
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = true;

            Assert.AreEqual("*El campo venta no puede tener mas de 120 caracteres.<br />", 
                revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente, 
                rbAprobado, rbRechazado, lblError));
        }

        [TestCase]
        //Prueba revisar los datos cuando cliente esta vacio
        public void datosPropuestaClienteVacio()
        {
            txtId.Text = "01";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "30000";
            txtDescuento.Text = "10";
            txtComision.Text = "10";
            txtRespuesta.Text = "Respuesta";
            lblCliente.Text = "";
            rbAprobado.Checked = true;

            Assert.AreEqual("*No se selecciono un cliente valido.<br />", 
                revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente, 
                rbAprobado, rbRechazado, lblError));
        }

        [TestCase]
        //Prueba revisar los datos cuando no selecciono estado
        public void datosPropuestaEstado()
        {
            txtId.Text = "01";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "30000";
            txtDescuento.Text = "10";
            txtComision.Text = "10";
            txtRespuesta.Text = "Respuesta";
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = false;
            rbRechazado.Checked = false;

            Assert.AreEqual("*No se selecciono un estado.<br />", revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente, rbAprobado, rbRechazado, lblError));
        }

        [TestCase]
        //Prueba revisar los datos cuando dos datos estan incorrectos
        public void datosPropuestaDosDatosIncorrectos()
        {
            txtId.Text = "01";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "";
            txtDescuento.Text = "";
            txtComision.Text = "10";
            txtRespuesta.Text = "Respuesta";
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = true;

            Assert.AreEqual("*El campo precio no puede estar vacio.<br />*El campo descuento no puede estar vacio.<br />", 
                revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente, 
                rbAprobado, rbRechazado, lblError));
        }

        [TestCase]
        //Prueba revisar que el label de error sea igual al error
        public void datosPropuestaLabelError()
        {
            txtId.Text = "";
            txtDate.Text = "29/10/17";
            txtPrecio.Text = "30000";
            txtDescuento.Text = "10";
            txtComision.Text = "10";
            txtRespuesta.Text = "Respuesta";
            lblCliente.Text = "Cliente";
            rbAprobado.Checked = true;
            
            revisarDatosLLenos(txtId, txtDate, txtPrecio, txtDescuento, txtComision, txtRespuesta, lblCliente,
                rbAprobado, rbRechazado, lblError);
            Assert.That(lblError.Text == "*El campo producto no puede estar vacio.<br />");
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