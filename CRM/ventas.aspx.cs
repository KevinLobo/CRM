﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

namespace CRM
{
    public partial class ventas : System.Web.UI.Page
    {
        IBaseDatos con;
        string conexion = @"Data Source = localhost;port=3306;Initial"
            + " Catalog=CRM;User Id=root;password = '' ";
        public ventas()
        {
            con = new baseDatos(conexion);
        }

        public ventas(IBaseDatos bd)
        {
            con = new baseDatos("sd");
        }
        string error = "";
        double filasPorPagina = 10;
        double paginas = 0;
        int paginaActual = 1;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Session["username"] as string))
            {
                Response.Redirect("logIn.aspx");
            }
            try
            {
                if (!Page.IsPostBack)
                {

                    LlenarListaPaginas();
                    BindGridView();
                    CargarSession();


                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }

        }


        void ShowMessage(string msg)
        {
            Response.Write("<script>alert('" + msg + "');</script>");

        }

        void CargarSession() {
            lblVendedor.Text = Session["username"].ToString();
        }

        void clear()
        {
            txtIdProducto.Text = string.Empty;
            txtEmpresa.Text = string.Empty;
            txtPersona.Text = string.Empty;
            txtEmpresa.Visible = false;
            txtPersona.Visible = false;
            rbEmpresa.Checked = false;
            rbPersona.Checked = false;
            txtPrecio.Text = string.Empty;
            txtDescuento.Text = string.Empty;
            txtComision.Text = string.Empty;
            lblPrecioFinal.Text = string.Empty;
            txtRespuesta.Text = string.Empty;
            datetimepicker.Text = string.Empty;
            lblNombreProducto.Text = string.Empty;
            lblCliente.Text = string.Empty;

            btnCancel.Text = "Cancelar";
            lblError.Text = string.Empty;
            btnSubmit.Visible = true;
            btnSubmit.Enabled = true;
            lblError.Visible = false;
            lblIdVenta.Visible = false;
            txtIdProducto.Focus();
        }


        private void LlenarListaPaginas()
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Select  count(*) from venta");
                IDataReader reader = con.getSalida();
                double totalPersonas = 0;
                if (reader.Read()){
                    totalPersonas = Convert.ToDouble(reader["count(*)"]);
                }

                lbltotalcount.Text = totalPersonas.ToString();
                paginas = totalPersonas / filasPorPagina;
                paginas = Math.Ceiling(paginas);

                CurrentPageLabel.Text = "Página " + paginaActual + " de " +
                    paginas + ".";

                paginaDropDown.Items.Clear();
                for (int i = 0; i < paginas; i++)
                {
                    paginaDropDown.Items.Add((i + 1).ToString());
                }
                paginaDropDown.SelectedIndex = paginaActual - 1;
            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
            finally
            {
                con.Cerrar();
            }

        }

        protected void VerificarProducto(TextBox entrada, Label lblDatos, TextBox pTxtPrecio)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Select * from producto where id ='" + entrada.Text + "'");
                IDataReader reader = con.getSalida();

                if (reader.Read())
                {
                    lblDatos.ForeColor = System.Drawing.Color.Green;
                    lblDatos.Text = Convert.ToString(reader["nombre"]);
                    pTxtPrecio.Text = Convert.ToString(reader["precio"]);
                }
                else
                {
                    lblDatos.ForeColor = System.Drawing.Color.Red;
                    lblDatos.Text = "No se encontro el producto";
                }

            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
            finally
            {
                con.Cerrar();
            }
        }

        protected void CambioID(object sender, EventArgs e)
        {
            VerificarProducto(txtIdProducto, lblNombreProducto, txtPrecio);
            CalcularPrecioFinal();
        }

        //----------------Precio/descuento/comision
        public void EstaEnRango(TextBox txtEntrada,double rangoMayor)
        {
            if (txtEntrada.Text != "") {
                double entrada = Convert.ToDouble(txtEntrada.Text);
                if (entrada > rangoMayor)
                {
                    txtEntrada.Text = Convert.ToString(rangoMayor);
                }
                if (entrada < 0)
                {
                    txtEntrada.Text = "0";
                }
            }
            else
            {
                txtEntrada.Text = "0";
            }
        }

        void CalcularPrecioFinal()
        {
            if (txtPrecio.Text != "" && txtDescuento.Text != "" ) {
                double precioFinal = Convert.ToDouble(txtPrecio.Text);
                double descuento = Convert.ToDouble(txtDescuento.Text);

                descuento = 100-descuento;

                precioFinal *= descuento / 100;

                lblPrecioFinal.Text = precioFinal.ToString();
            }

        }

        protected void CambioComision(object sender, EventArgs e)
        {
            EstaEnRango(txtComision,100);
        }

        protected void CambioDescuento(object sender, EventArgs e)
        {
            EstaEnRango(txtDescuento,100);
            CalcularPrecioFinal();
        }

//---------Tabla/Grid-----------------
        protected void CambioPagina(object sender, EventArgs e)
        {
            paginaActual = paginaDropDown.SelectedIndex + 1;
            LlenarListaPaginas();
            BindGridView();
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("principal.aspx");
        }

        public bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        protected string revisarDatosLLenos(TextBox pTxtIdProducto, TextBox pDatetimepicker, TextBox pTxtPrecio,
            TextBox pTxtDescuento, TextBox pTxtComision, TextBox pTxtRespuesta, Label pLblCliente, Label pLblError)
        {
            error = "";
            pLblError.Text = "";
            //venta
            if (pTxtIdProducto.Text.Trim() == "")
            {
                error += "*El campo producto no puede estar vacio.<br />";
            }
            if (pTxtIdProducto.Text.Trim().Length > 8)
            {
                error += "*El campo producto no puede tener mas de 8 caracteres.<br />";
            }
            //fecha
            if (pDatetimepicker.Text.Trim() == "")
            {
                error += "*El campo fecha no puede estar vacio.<br />";
            }

            //Precio
            if (pTxtPrecio.Text.Trim() == "")
            {
                error += "*El campo precio no puede estar vacio.<br />";
            }

            if (!IsDigitsOnly(pTxtPrecio.Text.Trim()))
            {
                error += "*El campo precio debe contener solo numeros.<br />";
            }

            //descuento
            if (pTxtDescuento.Text.Trim() == "")
            {
                error += "*El campo descuento no puede estar vacio.<br />";
            }
            //El descuento no verifica el largo, debido a que ya se hace previamente

            //comision
            if (pTxtComision.Text.Trim() == "")
            {
                error += "*El campo comisión no puede estar vacio.<br />";
            }
            //La comision no verifica el largo, debido a que ya se hace previamente

            //respuesta
            if (pTxtRespuesta.Text.Trim() == "")
            {
                error += "*El campo respuesta no puede estar vacio.<br />";
            }
            if (pTxtRespuesta.Text.Trim().Length > 200)
            {
                error += "*El campo venta no puede tener mas de 120 caracteres.<br />";
            }

            if (pLblCliente.Text == "No se encontro el cliente"|| pLblCliente.Text=="")
            {
                error += "*No se selecciono un cliente valido.<br />";
            }


            pLblError.Text = error;
            pLblError.Visible = true;
            return error;

        }

// ---------------GridView-------------------
        private void BindGridView()
        {
            try
            {
                double limite = paginaDropDown.SelectedIndex * filasPorPagina;
                con.Abrir();
                con.cargarQuery("SELECT venta.id,producto.nombre," +
                    "venta.fecha,venta.precio,venta.descuento,venta.vendedor,venta.respuesta" +
                    " FROM venta INNER JOIN producto ON venta.idProducto = producto.ID limit "
                    + limite + "," + filasPorPagina + "");
                IDataReader rader = con.getSalida();
                DataTable table = new DataTable();
                table.Load(rader);
                GridViewEmpresa.DataSource = table;
                GridViewEmpresa.DataBind();

            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
            finally
            {
                con.Cerrar();
            }
        }

        protected void GridViewEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            GridViewRow row = GridViewEmpresa.SelectedRow;
            con.Abrir();
            con.cargarQuery("SELECT * FROM `venta` where id = " +
                row.Cells[1].Text + "");
            IDataReader reader = con.getSalida();
            string empresaID = "";
            string personaID = "";
            if (reader.Read())
            {

                txtIdProducto.Text = row.Cells[3].Text;
                lblIdVenta.Text = "ID: " + row.Cells[1].Text;
                txtIdProducto.Text = Convert.ToString(reader["idProducto"]);
                lblPrecioFinal.Text = Convert.ToString(reader["precio"]);
                txtDescuento.Text = Convert.ToString(reader["descuento"]);
                txtComision.Text = Convert.ToString(reader["comision"]);
                datetimepicker.Text = Convert.ToString(reader["fecha"]);
                txtRespuesta.Text = Convert.ToString(reader["respuesta"]);
                empresaID = Convert.ToString(reader["empresaID"]);
                personaID = Convert.ToString(reader["personaVenta"]);

            }
            con.Cerrar();
            if (empresaID != "")
            {
                txtEmpresa.Visible = true;
                txtEmpresa.Text = empresaID;
                VerificarCliente(txtEmpresa, "ID", "empresa");
                rbEmpresa.Checked = true;
            }
            else
            {
                rbPersona.Checked = true;
                txtPersona.Visible = true;
                txtPersona.Text = personaID;
                VerificarCliente(txtPersona, "cedula", "persona");
                
            }

            VerificarProducto(txtIdProducto, lblNombreProducto, txtPrecio);
            btnCancel.Text = "Volver";
            lblIdVenta.Visible = true;
            btnSubmit.Visible = false;
            btnSubmit.Enabled = false;
        }

        protected void GridViewEmpresa_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                con.Abrir();
                string id = GridViewEmpresa.DataKeys[e.RowIndex].Value.ToString();
                con.cargarQuery("Delete From empresa where id='" + id + "'");
                con.getSalida().Close();
                ShowMessage("Empresa eliminada");
                GridViewEmpresa.EditIndex = -1;
                LlenarListaPaginas();
                BindGridView();
            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
            finally
            {
                con.Cerrar();
            }

        }

// ---------------Persona/Empresa-------------------
        protected void VerificarCliente(TextBox entrada,string columnaNombre,string tablaNombre)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Select nombre from " + tablaNombre + " where " +
                    columnaNombre + " ='" + entrada.Text + "'");
                IDataReader reader = con.getSalida();

                if (reader.Read())
                {
                    lblCliente.ForeColor = System.Drawing.Color.Green;
                    lblCliente.Text = Convert.ToString(reader["nombre"]);
                }
                else
                {
                    lblCliente.ForeColor = System.Drawing.Color.Red;
                    lblCliente.Text = "No se encontro el cliente";
                }

            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
            finally
            {
                con.Cerrar();
            }

        }

        protected void VerificarPersona(object sender, EventArgs e)
        {
            txtPersona.Text = txtPersona.Text.Trim();
            VerificarCliente(txtPersona, "cedula", "persona");
        }

        protected void VerificarEmpresa(object sender, EventArgs e)
        {
            txtEmpresa.Text = txtEmpresa.Text.Trim();
            VerificarCliente(txtEmpresa, "ID", "empresa");
        }

        protected void MostrarEmpresa(object sender, EventArgs e)
        {
            txtPersona.Visible = false;
            txtPersona.Text = "";
            lblCliente.Text = "";
            txtEmpresa.Visible = true;
        }

        protected void MostrarPersona(object sender, EventArgs e)
        {
            txtEmpresa.Visible = false;
            txtEmpresa.Text = "";
            lblCliente.Text = "";
            txtPersona.Visible = true;
        }


        // ---------------Botones-------------------
        protected void BtnUpdate_Click(object sender, EventArgs e)
        {

        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string error = revisarDatosLLenos(txtIdProducto,datetimepicker,txtPrecio,txtDescuento,
                txtComision,txtRespuesta,lblCliente,lblError);
            if (error == "")
            {
                try
                {
                    con.Abrir();
                    if (txtPersona.Text != "")
                    {
                        con.cargarQuery("INSERT INTO venta (idProducto, fecha, precio, descuento, comision" +
                        ", personaVenta, empresaID, vendedor, respuesta) VALUES " +
                        "('" + txtIdProducto.Text.Trim() + "', '" + datetimepicker.Text.Trim() +
                        "', '" + txtPrecio.Text.Trim() + "', '" + txtDescuento.Text.Trim() +
                        "', '" + txtComision.Text.Trim() + "'," + " '"+ txtPersona.Text + "', null, '"
                        + lblVendedor.Text + "', '"+ txtRespuesta.Text.Trim() + "');");
                    }
                    else
                    {
                        con.cargarQuery("INSERT INTO venta (idProducto, fecha, precio, descuento, comision" +
                        ", personaVenta, empresaID, vendedor, respuesta) VALUES " +
                        "('" + txtIdProducto.Text.Trim() + "', '" + datetimepicker.Text.Trim() +
                        "', '" + txtPrecio.Text.Trim() + "', '" + txtDescuento.Text.Trim() +
                        "', '" + txtComision.Text.Trim() + "'," + " null, '" + txtEmpresa.Text + "', '"
                        + lblVendedor.Text + "', '" + txtRespuesta.Text.Trim() + "');");
                    }

                    con.getSalida().Close();
                    ShowMessage("Registro correcto.");
                    clear();
                    LlenarListaPaginas();
                    BindGridView();
                }
                catch (MySqlException ex)
                {
                    ShowMessage(ex.Message);
                }
                finally
                {
                    con.Cerrar();
                }
            }
        }
    }
}