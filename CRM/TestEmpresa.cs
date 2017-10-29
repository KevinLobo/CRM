﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace CRM
{
    [TestFixture]
    class TestEmpresa : empresa
    {

        Label labelError = new Label();
        TextBox tTxtNombre = new TextBox(),
            tTxtDireccion = new TextBox(),
            tTxtTelefono = new TextBox();
        Label tLblId = new Label();
        Button tBtnSubmit = new Button();
        Button tBtnUpdate = new Button();
        [TestCase]
        //Prueba revisar los datos cuando estos estan correctos
        public void datosEmpresaCorrectos()
        {

            Assert.AreEqual("",revisarDatosLLenos("KimberlyClark","San Jose Costa Rica","22222222", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el nombre es muy largo
        public void datosEmpresaNombreLargo()
        {
            String nombre = new String('a', 210);
            Assert.AreEqual("*El campo nombre no puede tener mas de 80 caracteres.<br />",
                revisarDatosLLenos(nombre, "San Jose Costa Rica", "84840496", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el nombre esta vacio
        public void datosEmpresaNombreVacio()
        {
            Assert.AreEqual("*El campo nombre no puede estar vacio.<br />",
                revisarDatosLLenos("", "San Jose Costa Rica", "84840496", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando la direccion es muy larga
        public void datosEmpresaDireccionLarga()
        {
            String direccion = new String('a', 210);
            Assert.AreEqual("*El campo direccion no puede tener más de 200 caracteres.<br />",
                revisarDatosLLenos("KimberlyClark", direccion, "84840496", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando la direccion es vacia
        public void datosEmpresaDireccionVacia()
        {
            Assert.AreEqual("*El campo direccion no puede estar vacio.<br />", 
                revisarDatosLLenos("KimberlyClark", "", "84840496", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el telefono es muy largo
        public void datosEmpresaTelefonoLargo()
        {
            Assert.AreEqual("*El campo telefono no puede tener más de 8 caracteres.<br />",
                revisarDatosLLenos("KimberlyClark", "San Jose Costa Rica", "8484049600", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el telefono esta vacio
        public void datosEmpresaTelefonoVacio()
        {
            Assert.AreEqual("*El campo telefono no puede estar vacio.<br />", 
                revisarDatosLLenos("KimberlyClark", "San Jose Costa Rica", "", labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando el telefono no tiene solo numeros
        public void datosEmpresaTelefonoLetras()
        {
            Assert.AreEqual("*El campo telefono solo puede contener numeros.<br />",
                revisarDatosLLenos("KimberlyClark", "San Jose Costa Rica", "abc40496",labelError));
        }

        [TestCase]
        //Prueba revisar los datos cuando dos datos estan mal
        public void datosEmpresaDosErroneos()
        {
            Assert.AreEqual("*El campo direccion no puede estar vacio.<br />*El campo telefono no puede tener más de 8 caracteres.<br />",
                revisarDatosLLenos("KimberlyClark", "", "8484049600", labelError));
        }

        [TestCase]
        //Prueba si el error y el label de error coinciden en caso de error
        public void labelErrorIgualAError()
        {
            revisarDatosLLenos("KimberlyClark", "San Jose Costa Rica", "abc40496", labelError);
            Assert.That(labelError.Text == "*El campo telefono solo puede contener numeros.<br />");
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
        //Prueba si llena los textBox con datos del gridrow
        public void llenarCamposDelGridRow()
        {
            GridView gridTest = new GridView();
            DataTable dt = new DataTable();
            dt.Columns.Add("Seleccionar");
            dt.Columns.Add("Eliminar");
            dt.Columns.Add("ID");
            dt.Columns.Add("Nombre");
            dt.Columns.Add("Descripcion");
            dt.Columns.Add("Telefono");
            dt.Rows.Add("Seleccionar", "Eliminar", "1", "NombreTest", "DescripcionTest", "TelefonoTest");

            gridTest.DataSource = dt;
            gridTest.DataBind();

            gridTest.SelectedIndex = 0;

            filaSeleccionada(tLblId, tTxtNombre, tTxtTelefono, tTxtDireccion, tBtnSubmit, tBtnUpdate, gridTest);
            
            Assert.That(tLblId.Text == "1" && tTxtNombre.Text == "NombreTest" && tTxtTelefono.Text == "TelefonoTest" &&
                tTxtDireccion.Text == "DescripcionTest" && tLblId.Visible && tBtnUpdate.Visible && !tBtnSubmit.Visible);
        }





        

    }
}