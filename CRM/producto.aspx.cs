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
    public partial class producto : System.Web.UI.Page
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


        void clear()
        {
            lblId.Visible = false;
            txtNombre.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            lblError.Text = string.Empty;
            btnSubmit.Visible = true;
            btnUpdate.Visible = false;
            btnSubmit.Enabled = true;
            btnUpdate.Enabled = false;
            lblError.Visible = false;
            txtNombre.Focus();
        }


        private void LlenarListaPaginas()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }

                MySqlCommand cmd = new MySqlCommand("Select  count(*) from empresa", con);
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


        private void BindGridView()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                double limite = paginaDropDown.SelectedIndex * filasPorPagina;
                MySqlCommand cmd = new MySqlCommand("Select * from empresa limit " + limite + "," + filasPorPagina + "", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                GridViewProductos.DataSource = ds;
                GridViewProductos.DataBind();

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
            if (txtNombre.Text.Trim() == "")
            {
                error += "*El campo nombre no puede estar vacio.<br />";
                salida = false;
            }
            if (txtNombre.Text.Trim().Length > 80)
            {
                error += "*El campo nombre no puede tener mas de 80 caracteres.<br />";
                salida = false;
            }

            lblError.Text = error;
            lblError.Visible = true;
            return salida;

        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (revisarDatosLLenos())
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO producto (nombre,precio)" +
                        " VALUES (@Name, @precio);", con);
                    cmd.Parameters.AddWithValue("@Name", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Address", txtPrecio.Text);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    ShowMessage("Registro correcto");
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

        protected void GridViewProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridViewProductos.SelectedRow;
            lblId.Visible = true;
            lblId.Text = row.Cells[2].Text;
            txtNombre.Text = row.Cells[3].Text;
            txtPrecio.Text = row.Cells[4].Text;
            btnSubmit.Visible = false;
            btnSubmit.Enabled = false;
            btnUpdate.Visible = true;
            btnUpdate.Enabled = true;
        }

        protected void CambioPrecio(object sender, EventArgs e)
        {
            EstaEnRango(txtPrecio, Double.MaxValue);
        }


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

        protected void GridViewProducto_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                con.Open();
                string id = GridViewProductos.DataKeys[e.RowIndex].Value.ToString();
                MySqlCommand cmd = new MySqlCommand("Delete From producto where id='" + id + "'", con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                ShowMessage("Producto eliminado");
                GridViewProductos.EditIndex = -1;
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

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (revisarDatosLLenos())
            {
                try
                {
                    con.Open();
                    string id = lblId.Text;
                    MySqlCommand cmd = new MySqlCommand("UPDATE producto SET nombre = @Name,precio = @Precio " +
                        " WHERE producto.id = @id", con);
                    cmd.Parameters.AddWithValue("@Name", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Precio", txtPrecio.Text);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    ShowMessage("Producto actualizado");
                    GridViewProductos.EditIndex = -1;
                    LlenarListaPaginas();
                    BindGridView();
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


    }
}