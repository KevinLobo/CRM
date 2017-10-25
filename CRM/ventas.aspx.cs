using System;
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

        void CargarSession() {
            lblVendedor.Text = Session["username"].ToString();
        }

        void clear()
        {
            txtVenta.Text = string.Empty;
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

            lblCliente.Text = string.Empty;

            btnCancel.Text = "Cancelar";
            lblError.Text = string.Empty;
            btnSubmit.Visible = true;
            btnSubmit.Enabled = true;
            lblError.Visible = false;
            lblIdVenta.Visible = false;
            txtVenta.Focus();
        }


        private void LlenarListaPaginas()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                MySqlCommand cmd = new MySqlCommand("Select  count(*) from venta", con);
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




//----------------Precio/descuento/comision
        void EstaEnRango(TextBox txtEntrada,double rangoMayor)
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

        protected void CambioPrecio(object sender, EventArgs e)
        {
            EstaEnRango(txtPrecio, Double.MaxValue);
            CalcularPrecioFinal();
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
            if (txtVenta.Text.Trim() == "")
            {
                error += "*El campo venta no puede estar vacio.<br />";
                salida = false;
            }
            if (txtVenta.Text.Trim().Length > 120)
            {
                error += "*El campo venta no puede tener mas de 120 caracteres.<br />";
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
            //El precio no verifica el largo, debido a que ya se hace previamente

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

            if (lblCliente.Text == "No se encontro el cliente"|| lblCliente.Text=="")
            {
                error += "*No se selecciono un cliente valido.<br />";
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
                MySqlCommand cmd = new MySqlCommand("SELECT `id`,`nombreVenta`,`fecha`,`precio`," +
                    "`descuento`,`vendedor`,`respuesta` FROM `venta` limit " + limite + "," +
                    filasPorPagina + "", con);
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
            txtVenta.Text = row.Cells[3].Text;
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `venta` where id = " + 
                row.Cells[1].Text + "", con);
            lblIdVenta.Text = "ID: " + row.Cells[1].Text;
            MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adp.Fill(ds);

            txtVenta.Text = Convert.ToString(ds.Tables[0].Rows[0]["nombreVenta"]);
            txtPrecio.Text = Convert.ToString(ds.Tables[0].Rows[0]["precio"]);
            txtDescuento.Text = Convert.ToString(ds.Tables[0].Rows[0]["descuento"]);
            txtComision.Text = Convert.ToString(ds.Tables[0].Rows[0]["comision"]);
            datetimepicker.Text = Convert.ToString(ds.Tables[0].Rows[0]["fecha"]);
            txtRespuesta.Text = Convert.ToString(ds.Tables[0].Rows[0]["respuesta"]);
            string empresaID = Convert.ToString(ds.Tables[0].Rows[0]["empresaID"]);
            string personaID = Convert.ToString(ds.Tables[0].Rows[0]["personaVenta"]);
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


            CalcularPrecioFinal();
            btnCancel.Text = "Volver";
            lblIdVenta.Visible = true;
            btnSubmit.Visible = false;
            btnSubmit.Enabled = false;
        }

        protected void GridViewEmpresa_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                con.Open();
                string id = GridViewEmpresa.DataKeys[e.RowIndex].Value.ToString();
                MySqlCommand cmd = new MySqlCommand("Delete From empresa where id='" + id + "'", con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
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
                con.Close();
            }

        }

// ---------------Persona/Empresa-------------------
        protected void VerificarCliente(TextBox entrada,string columnaNombre,string tablaNombre)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                MySqlCommand cmd = new MySqlCommand("Select nombre from "+ tablaNombre + " where "+ 
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

                    clear();
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
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO venta" +
                        " (nombreVenta, fecha, precio, descuento, comision" +
                        ", personaVenta, empresaID, vendedor, respuesta) " +
                        "VALUES (@Producto, @Fecha, @Precio, @Descuento, @Comision," +
                        " @Persona, @Empresa, @Vendedor, @Respuesta);", con);
                    cmd.Parameters.AddWithValue("@Producto", txtVenta.Text.Trim());
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