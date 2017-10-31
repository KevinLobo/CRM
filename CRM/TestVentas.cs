using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRM
{
    [TestFixture]
    class TestVentas : ventas
    {
        Label labelError = new Label();
        Label labelCliente = new Label();
        TextBox txtid = new TextBox();
        TextBox txtdate = new TextBox();
        TextBox txtprecio = new TextBox();
        TextBox txtdescuento = new TextBox();
        TextBox txtcomision = new TextBox();
        TextBox txtrespuesta = new TextBox();

        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void datosVentasCorrectos()
        {
            txtid.Text = "01";
            txtdate.Text = "29/10/17";
            txtprecio.Text = "30000";
            txtdescuento.Text = "10";
            txtcomision.Text = "10";
            txtrespuesta.Text = "Respuesta";
            labelCliente.Text = "Cliente";

            Assert.AreEqual("",revisarDatosLLenos(txtid,txtdate,txtprecio,txtdescuento,txtcomision,txtrespuesta,labelCliente,labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el ID esta vacio
        public void datosVentasIdVacio()
        {
            txtid.Text = "";
            txtdate.Text = "29/10/17";
            txtprecio.Text = "30000";
            txtdescuento.Text = "10";
            txtcomision.Text = "10";
            txtrespuesta.Text = "Respuesta";
            labelCliente.Text = "Cliente";

            Assert.AreEqual("*El campo producto no puede estar vacio.<br />", 
                revisarDatosLLenos(txtid, txtdate, txtprecio, txtdescuento, txtcomision, txtrespuesta, labelCliente,
                labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el ID es muy largo
        public void datosVentasIdLargo()
        {
            txtid.Text = "0123456789";
            txtdate.Text = "29/10/17";
            txtprecio.Text = "30000";
            txtdescuento.Text = "10";
            txtcomision.Text = "10";
            txtrespuesta.Text = "Respuesta";
            labelCliente.Text = "Cliente";

            Assert.AreEqual("*El campo producto no puede tener mas de 8 caracteres.<br />", 
                revisarDatosLLenos(txtid, txtdate, txtprecio, txtdescuento, txtcomision, txtrespuesta, labelCliente, 
                labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el Date esta vacio
        public void datosVentasDateVacio()
        {
            txtid.Text = "01";
            txtdate.Text = "";
            txtprecio.Text = "30000";
            txtdescuento.Text = "10";
            txtcomision.Text = "10";
            txtrespuesta.Text = "Respuesta";
            labelCliente.Text = "Cliente";

            Assert.AreEqual("*El campo fecha no puede estar vacio.<br />", 
                revisarDatosLLenos(txtid, txtdate, txtprecio, txtdescuento, txtcomision, txtrespuesta, labelCliente, 
                labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el precio esta vacio
        public void datosVentasPrecioVacio()
        {
            txtid.Text = "01";
            txtdate.Text = "29/10/17";
            txtprecio.Text = "";
            txtdescuento.Text = "10";
            txtcomision.Text = "10";
            txtrespuesta.Text = "Respuesta";
            labelCliente.Text = "Cliente";

            Assert.AreEqual("*El campo precio no puede estar vacio.<br />", 
                revisarDatosLLenos(txtid, txtdate, txtprecio, txtdescuento, txtcomision, txtrespuesta, labelCliente, 
                labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el precio no son solo numeros
        public void datosVentasPrecioNoSoloNumeros()
        {
            txtid.Text = "01";
            txtdate.Text = "29/10/17";
            txtprecio.Text = "123abc";
            txtdescuento.Text = "10";
            txtcomision.Text = "10";
            txtrespuesta.Text = "Respuesta";
            labelCliente.Text = "Cliente";

            Assert.AreEqual("*El campo precio debe contener solo numeros.<br />",
                revisarDatosLLenos(txtid, txtdate, txtprecio, txtdescuento, txtcomision, txtrespuesta, labelCliente,
                labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el descuento esta vacio
        public void datosVentasDescuentoVacio()
        {
            txtid.Text = "01";
            txtdate.Text = "29/10/17";
            txtprecio.Text = "30000";
            txtdescuento.Text = "";
            txtcomision.Text = "10";
            txtrespuesta.Text = "Respuesta";
            labelCliente.Text = "Cliente";

            Assert.AreEqual("*El campo descuento no puede estar vacio.<br />", 
                revisarDatosLLenos(txtid, txtdate, txtprecio, txtdescuento, txtcomision, txtrespuesta, labelCliente, 
                labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando la comision esta vacia
        public void datosVentasComisionVacio()
        {
            txtid.Text = "01";
            txtdate.Text = "29/10/17";
            txtprecio.Text = "30000";
            txtdescuento.Text = "10";
            txtcomision.Text = "";
            txtrespuesta.Text = "Respuesta";
            labelCliente.Text = "Cliente";

            Assert.AreEqual("*El campo comisión no puede estar vacio.<br />", 
                revisarDatosLLenos(txtid, txtdate, txtprecio, txtdescuento, txtcomision, txtrespuesta, labelCliente, 
                labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando la respuesta esta vacia
        public void datosVentasRespuestaVacio()
        {
            txtid.Text = "01";
            txtdate.Text = "29/10/17";
            txtprecio.Text = "30000";
            txtdescuento.Text = "10";
            txtcomision.Text = "10";
            txtrespuesta.Text = "";
            labelCliente.Text = "Cliente";

            Assert.AreEqual("*El campo respuesta no puede estar vacio.<br />", 
                revisarDatosLLenos(txtid, txtdate, txtprecio, txtdescuento, txtcomision, txtrespuesta, labelCliente, 
                labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando la respuesta es muy larga
        public void datosVentasRespuestaLargo()
        {
            txtid.Text = "01";
            txtdate.Text = "29/10/17";
            txtprecio.Text = "30000";
            txtdescuento.Text = "10";
            txtcomision.Text = "10";
            String respuesta = new String('a', 210);
            txtrespuesta.Text = respuesta;
            labelCliente.Text = "Cliente";

            Assert.AreEqual("*El campo venta no puede tener mas de 120 caracteres.<br />", 
                revisarDatosLLenos(txtid, txtdate, txtprecio, txtdescuento, txtcomision, txtrespuesta, labelCliente, 
                labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el cliente esta vacio
        public void datosVentasClienteVacio()
        {
            txtid.Text = "01";
            txtdate.Text = "29/10/17";
            txtprecio.Text = "30000";
            txtdescuento.Text = "10";
            txtcomision.Text = "10";
            txtrespuesta.Text = "Respuesta";
            labelCliente.Text = "";

            Assert.AreEqual("*No se selecciono un cliente valido.<br />", 
                revisarDatosLLenos(txtid, txtdate, txtprecio, txtdescuento, txtcomision, txtrespuesta, labelCliente, 
                labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando dos datos son incorrectos
        public void datosVentasDosDatosMal()
        {
            txtid.Text = "";
            txtdate.Text = "";
            txtprecio.Text = "30000";
            txtdescuento.Text = "10";
            txtcomision.Text = "10";
            txtrespuesta.Text = "Respuesta";
            labelCliente.Text = "Cliente";

            Assert.AreEqual("*El campo producto no puede estar vacio.<br />*El campo fecha no puede estar vacio.<br />",
                revisarDatosLLenos(txtid, txtdate, txtprecio, txtdescuento, txtcomision, txtrespuesta, labelCliente,
                labelError));
        }

        [TestCase]
        //Prueba revisar que el label de error sea igual error
        public void datosVentasLabelError()
        {
            txtid.Text = "01";
            txtdate.Text = "29/10/17";
            txtprecio.Text = "30000";
            txtdescuento.Text = "10";
            txtcomision.Text = "10";
            txtrespuesta.Text = "Respuesta";
            labelCliente.Text = "";

            revisarDatosLLenos(txtid, txtdate, txtprecio, txtdescuento, txtcomision, txtrespuesta, labelCliente,
                 labelError);
            Assert.That(labelError.Text == "*No se selecciono un cliente valido.<br />");
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

        [TestCase]
        //Prueba la funcion esta en rango cuando la entrada esta en el limite maximo
        public void RangoMayorLimite()
        {
            TextBox txtEntrada = new TextBox();
            double entrada = double.MaxValue;
            txtEntrada.Text = entrada.ToString();
            EstaEnRango(txtEntrada, 10);
            Assert.That(txtEntrada.Text == "10");
        }

        [TestCase]
        //Prueba la funcion esta en rango cuando la entrada esta en el limite minimo
        public void RangoMenorLimite()
        {
            TextBox txtEntrada = new TextBox();
            double entrada = double.MinValue;
            txtEntrada.Text = entrada.ToString();
            EstaEnRango(txtEntrada, 10);
            Assert.That(txtEntrada.Text == "0");
        }

        [TestCase]
        //Prueba la funcion esta en rango cuando la entrada es mayor al rango
        public void RangoMayor()
        {
            TextBox txtEntrada = new TextBox();
            txtEntrada.Text = "100";
            EstaEnRango(txtEntrada, 10);
            Assert.That(txtEntrada.Text == "10");
        }

        [TestCase]
        //Prueba la funcion esta en rango cuando la entrada es mnor al rango
        public void RangoMenor()
        {
            TextBox txtEntrada = new TextBox();
            txtEntrada.Text = "10";
            EstaEnRango(txtEntrada, 100);
            Assert.That(txtEntrada.Text == "10");
        }

        [TestCase]
        //Prueba la funcion esta en rango cuando la entrada es menor a cero
        public void MenorCero()
        {
            TextBox txtEntrada = new TextBox();
            txtEntrada.Text = "-100";
            EstaEnRango(txtEntrada, 10);
            Assert.That(txtEntrada.Text == "0");
        }

        [TestCase]
        //Prueba la funcion esta en rango cuando la entrada es vacia
        public void EntradaVacia()
        {
            TextBox txtEntrada = new TextBox();
            txtEntrada.Text = "";
            EstaEnRango(txtEntrada, 10);
            Assert.That(txtEntrada.Text == "0");
        }
    }
}