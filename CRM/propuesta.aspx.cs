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
    public partial class propuesta : System.Web.UI.Page
    {
        MySqlConnection con = new MySqlConnection(@"Data Source = localhost;port=3306;Initial"
        + " Catalog=CRM;User Id=root;password = '' ");
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

        void CargarSession()
        {
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
            rbAprovado.Checked = false;
            rbRechazado.Checked = false;
            txtPrecio.Text = string.Empty;
            txtDescuento.Text = string.Empty;
            txtComision.Text = string.Empty;
            lblPrecioFinal.Text = string.Empty;
            txtRespuesta.Text = string.Empty;
            datetimepicker.Text = string.Empty;
            lblNombreProducto.Text = string.Empty;
            lblCliente.Text = string.Empty;

            lblError.Text = string.Empty;
            btnSubmit.Visible = true;
            btnSubmit.Enabled = true;
            btnUpdate.Visible = false;
            btnUpdate.Enabled = false;
            lblError.Visible = false;
            lblIdVenta.Visible = false;
            txtIdProducto.Focus();
        }


        private void LlenarListaPaginas()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                MySqlCommand cmd = new MySqlCommand("Select  count(*) from propuesta", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                double totalPersonas = Convert.ToDouble(ds.Tables[0].Rows[0]["count(*)"]);
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
                if (con.State == ConnectionState.Open)
                {
                    con.Close();

                }
            }

        }

        protected void VerificarProducto(TextBox entrada, Label lblDatos, TextBox pTxtPrecio)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                MySqlCommand cmd = new MySqlCommand("Select * from producto where id ='" + entrada.Text + "'", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblDatos.ForeColor = System.Drawing.Color.Green;
                    lblDatos.Text = Convert.ToString(ds.Tables[0].Rows[0]["nombre"]);
                    pTxtPrecio.Text = Convert.ToString(ds.Tables[0].Rows[0]["precio"]);
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
                if (con.State == ConnectionState.Open)
                {
                    con.Close();

                }
            }
        }

        protected void CambioID(object sender, EventArgs e)
        {
            VerificarProducto(txtIdProducto, lblNombreProducto, txtPrecio);
            CalcularPrecioFinal();
        }



        //----------------Precio/descuento/comision
        void EstaEnRango(TextBox txtEntrada, double rangoMayor)
        {
            if (txtEntrada.Text != "")
            {
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
            if (txtPrecio.Text != "" && txtDescuento.Text != "")
            {
                double precioFinal = Convert.ToDouble(txtPrecio.Text);
                double descuento = Convert.ToDouble(txtDescuento.Text);

                descuento = 100 - descuento;

                precioFinal *= descuento / 100;

                lblPrecioFinal.Text = precioFinal.ToString();
            }

        }

        protected void CambioComision(object sender, EventArgs e)
        {
            EstaEnRango(txtComision, 100);
        }

        protected void CambioDescuento(object sender, EventArgs e)
        {
            EstaEnRango(txtDescuento, 100);
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

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        protected bool revisarDatosLLenos()
        {
            error = "";
            lblError.Text = "";
            bool salida = true;
            //venta
            if (txtIdProducto.Text.Trim() == "")
            {
                error += "*El campo producto no puede estar vacio.<br />";
                salida = false;
            }
            if (txtIdProducto.Text.Trim().Length > 8)
            {
                error += "*El campo producto no puede tener mas de 8 caracteres.<br />";
                salida = false;
            }
            //fecha
            if (datetimepicker.Text.Trim() == "")
            {
                error += "*El campo fecha no puede estar vacio.<br />";
                salida = false;
            }

            //Precio
            if (txtPrecio.Text.Trim() == "")
            {
                error += "*El campo precio no puede estar vacio.<br />";
                salida = false;
            }

            //descuento
            if (txtDescuento.Text.Trim() == "")
            {
                error += "*El campo descuento no puede estar vacio.<br />";
                salida = false;
            }
            //El descuento no verifica el largo, debido a que ya se hace previamente

            //comision
            if (txtComision.Text.Trim() == "")
            {
                error += "*El campo comisión no puede estar vacio.<br />";
                salida = false;
            }
            //El descuento no verifica el largo, debido a que ya se hace previamente

            //respuesta
            if (txtRespuesta.Text.Trim() == "")
            {
                error += "*El campo respuesta no puede estar vacio.<br />";
                salida = false;
            }
            if (txtRespuesta.Text.Trim().Length > 200)
            {
                error += "*El campo venta no puede tener mas de 120 caracteres.<br />";
                salida = false;
            }

            if (lblCliente.Text == "No se encontro el cliente" || lblCliente.Text == "")
            {
                error += "*No se selecciono un cliente valido.<br />";
                salida = false;
            }

            if (!rbAprovado.Checked && !rbRechazado.Checked)
            {
                error += "*No se selecciono un estado.<br />";
                salida = false;
            }


            lblError.Text = error;
            lblError.Visible = true;
            return salida;

        }

        // ---------------GridView-------------------
        private void BindGridView()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                double limite = paginaDropDown.SelectedIndex * filasPorPagina;
                MySqlCommand cmd = new MySqlCommand("SELECT propuesta.id,producto.nombre," +
                    "propuesta.fecha,propuesta.precio,propuesta.descuento,propuesta.vendedor,propuesta.respuesta" +
                    " FROM propuesta INNER JOIN producto ON propuesta.idProducto = producto.ID limit "
                    + limite + "," + filasPorPagina + "", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                GridViewEmpresa.DataSource = ds;
                GridViewEmpresa.DataBind();

            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();

                }
            }
        }

        protected void GridViewEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            clear();
            GridViewRow row = GridViewEmpresa.SelectedRow;
            txtIdProducto.Text = row.Cells[3].Text;
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `propuesta` where id = " +
                row.Cells[2].Text + "", con);
            lblIdVenta.Text = row.Cells[2].Text;
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);

            txtIdProducto.Text = Convert.ToString(ds.Tables[0].Rows[0]["idProducto"]);
            lblPrecioFinal.Text = Convert.ToString(ds.Tables[0].Rows[0]["precio"]);
            txtDescuento.Text = Convert.ToString(ds.Tables[0].Rows[0]["descuento"]);
            txtComision.Text = Convert.ToString(ds.Tables[0].Rows[0]["comision"]);
            datetimepicker.Text = Convert.ToString(ds.Tables[0].Rows[0]["fecha"]);
            txtRespuesta.Text = Convert.ToString(ds.Tables[0].Rows[0]["respuesta"]);
            string empresaID = Convert.ToString(ds.Tables[0].Rows[0]["empresaID"]);
            string personaID = Convert.ToString(ds.Tables[0].Rows[0]["personaVenta"]);
            int estado = Convert.ToInt32(ds.Tables[0].Rows[0]["estado"]);
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

            if (estado == 1) rbAprovado.Checked = true;
            if (estado == 0) rbRechazado.Checked = true;


            VerificarProducto(txtIdProducto, lblNombreProducto, txtPrecio);
            lblIdVenta.Visible = true;
            btnSubmit.Visible = false;
            btnSubmit.Enabled = false;
            btnUpdate.Visible = true;
            btnUpdate.Enabled = true;
        }

        protected void GridViewPropuesta_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                con.Open();
                string id = GridViewEmpresa.DataKeys[e.RowIndex].Value.ToString();
                MySqlCommand cmd = new MySqlCommand("Delete From propuesta where id='" + id + "'", con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                ShowMessage("Propuesta eliminada");
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
                con.Close();
            }

        }

        // ---------------Persona/Empresa-------------------
        protected void VerificarCliente(TextBox entrada, string columnaNombre, string tablaNombre)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                MySqlCommand cmd = new MySqlCommand("Select nombre from " + tablaNombre + " where " +
                    columnaNombre + " ='" + entrada.Text + "'", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblCliente.ForeColor = System.Drawing.Color.Green;
                    lblCliente.Text = Convert.ToString(ds.Tables[0].Rows[0]["nombre"]);
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
                if (con.State == ConnectionState.Open)
                {
                    con.Close();

                }
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
            if (revisarDatosLLenos())
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("UPDATE propuesta SET idProducto = @Producto," +
                        "fecha = @Fecha, precio = @Precio, descuento = @Descuento, comision = @Comision," +
                        "empresaID = @Empresa, personaVenta = @Persona, vendedor = @Vendedor," +
                        "estado = @estado, respuesta = @Respuesta WHERE propuesta.id = "+lblIdVenta.Text+";", con);
                    cmd.Parameters.AddWithValue("@Producto", txtIdProducto.Text.Trim());
                    cmd.Parameters.AddWithValue("@Fecha", datetimepicker.Text.Trim());
                    cmd.Parameters.AddWithValue("@Precio", txtPrecio.Text.Trim());
                    cmd.Parameters.AddWithValue("@Descuento", txtDescuento.Text.Trim());
                    cmd.Parameters.AddWithValue("@Comision", txtComision.Text.Trim());

                    if (txtPersona.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@Persona", txtPersona.Text.Trim());
                        cmd.Parameters.AddWithValue("@Empresa", null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Persona", null);
                        cmd.Parameters.AddWithValue("@Empresa", txtEmpresa.Text.Trim());
                    }
                    cmd.Parameters.AddWithValue("@Vendedor", lblVendedor.Text);
                    cmd.Parameters.AddWithValue("@Respuesta", txtRespuesta.Text.Trim());

                    if (rbAprovado.Checked)
                    {
                        cmd.Parameters.AddWithValue("@estado", "1");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@estado", "0");
                    }


                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    ShowMessage("Actualización correcta.");
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
                    con.Close();
                }
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {

            if (revisarDatosLLenos())
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO propuesta" +
                        " (idProducto, fecha, precio, descuento, comision" +
                        ", personaVenta, empresaID, vendedor,estado, respuesta) " +
                        "VALUES (@Producto, @Fecha, @Precio, @Descuento, @Comision," +
                        " @Persona, @Empresa, @Vendedor,@estado,@Respuesta);", con);
                    cmd.Parameters.AddWithValue("@Producto", txtIdProducto.Text.Trim());
                    cmd.Parameters.AddWithValue("@Fecha", datetimepicker.Text.Trim());
                    cmd.Parameters.AddWithValue("@Precio", txtPrecio.Text.Trim());
                    cmd.Parameters.AddWithValue("@Descuento", txtDescuento.Text.Trim());
                    cmd.Parameters.AddWithValue("@Comision", txtComision.Text.Trim());

                    if (txtPersona.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@Persona", txtPersona.Text.Trim());
                        cmd.Parameters.AddWithValue("@Empresa", null);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Persona", null);
                        cmd.Parameters.AddWithValue("@Empresa", txtEmpresa.Text.Trim());
                    }
                    cmd.Parameters.AddWithValue("@Vendedor", lblVendedor.Text);
                    cmd.Parameters.AddWithValue("@Respuesta", txtRespuesta.Text.Trim());

                    if (rbAprovado.Checked)
                    {
                        cmd.Parameters.AddWithValue("@estado", "1");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@estado", "0");
                    }
                    

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
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
                    con.Close();
                }
            }
        }
    }
}