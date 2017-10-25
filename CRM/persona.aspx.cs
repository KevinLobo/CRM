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
    public partial class WebForm2 : System.Web.UI.Page
    {
        MySqlConnection con = new MySqlConnection(@"Data Source = localhost;port=3306;Initial"
        + " Catalog=CRM;User Id=root;password = '' ");
        string error = "";
        double personasPorPagina = 10;
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

        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }


        protected String revisarDatosLLenos(String pCedula, String pNombre, String pDireccion,
            String pTelefono, String pCorreo, Label labelError)
        {
            error = "";
            labelError.Text = "";
            if (pCedula == "")
            {
                error += "*El campo cedula no puede estar vacio.<br />";
            }
            if (pCedula.Length > 10)
            {
                error += "*El campo cedula no puede tener mas de 80 caracteres.<br />";
            }
            if (pNombre == "")
            {
                error += "*El campo nombre no puede estar vacio.<br />";
            }
            if (pNombre.Length > 80)
            {
                error += "*El campo nombre no puede tener mas de 80 caracteres.<br />";
            }
            if (pDireccion == "")
            {
                error += "*El campo direccion no puede estar vacio.<br />";
            }
            if (pDireccion.Length > 200)
            {
                error += "*El campo direccion no puede tener más de 200 caracteres.<br />";
            }
            if (pTelefono == "")
            {
                error += "*El campo telefono no puede estar vacio.<br />";
            }
            else
            {
                if (!IsDigitsOnly(pTelefono))
                {
                    error += "*El campo telefono solo puede contener numeros.<br />";
                }
            }
            if (pTelefono.Length > 8)
            {
                error += "*El campo telefono no puede tener más de 8 caracteres.<br />";
            }
            if (pCorreo == "")
            {
                error += "*El campo correo no puede estar vacio.<br />";
            }
            if (pCorreo.Length > 80)
            {
                error += "*El campo correo no puede tener mas de 80 caracteres.<br />";
            }
            labelError.Text = error;
            return error;

        }


        void clear()
        {
            txtCedula.Visible = true;
            lblCedula.Visible = false;
            txtCedula.Text = string.Empty;
            txtNombre.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtCorreo.Text = string.Empty;
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

                MySqlCommand cmd = new MySqlCommand("Select  count(*) from persona", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                double totalPersonas = Convert.ToDouble(ds.Tables[0].Rows[0]["count(*)"]);
                lbltotalcount.Text = totalPersonas.ToString();
                paginas = totalPersonas / personasPorPagina;
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

                double limite = paginaDropDown.SelectedIndex * personasPorPagina;
                MySqlCommand cmd = new MySqlCommand("Select * from persona limit "+limite+","+personasPorPagina+"", con);
                MySqlDataAdapter adp = new MySqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                GridViewPersona.DataSource = ds;
                GridViewPersona.DataBind();

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


        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("principal.aspx");
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            error = revisarDatosLLenos(txtCedula.Text, txtNombre.Text, txtDireccion.Text,
                txtTelefono.Text, txtCorreo.Text, lblError);
            if (error == "")
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO persona (cedula,Nombre,Direccion,Telefono,Correo)" +
                        " VALUES (@ID,@Name, @Address, @Mobile, @Email);", con);
                    cmd.Parameters.AddWithValue("@ID", txtCedula.Text.Trim());
                    cmd.Parameters.AddWithValue("@Name", txtNombre.Text.Trim());
                    cmd.Parameters.AddWithValue("@Address", txtDireccion.Text.Trim());
                    cmd.Parameters.AddWithValue("@Mobile", txtTelefono.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtCorreo.Text.Trim());
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
            else
            {
                lblError.Text = error;
            }
        }

        protected void GridViewPersona_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridViewPersona.SelectedRow;
            txtCedula.Visible = false;
            lblCedula.Visible = true;
            txtCedula.Text = row.Cells[2].Text;
            lblCedula.Text = row.Cells[2].Text;
            txtNombre.Text = row.Cells[3].Text;
            txtDireccion.Text = row.Cells[4].Text;
            txtTelefono.Text = row.Cells[5].Text;
            txtCorreo.Text = row.Cells[6].Text;
            btnSubmit.Visible = false;
            btnSubmit.Enabled = false;
            btnUpdate.Visible = true;
            btnUpdate.Enabled = true;
        }

        protected void GridViewPersona_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                con.Open();
                string ced = GridViewPersona.DataKeys[e.RowIndex].Value.ToString();
                MySqlCommand cmd = new MySqlCommand("Delete From persona where cedula='" + ced + "'", con);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                ShowMessage("Persona eliminada");
                GridViewPersona.EditIndex = -1;
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

        protected void CambioPagina(object sender, EventArgs e)
        {
            paginaActual = paginaDropDown.SelectedIndex + 1;
            LlenarListaPaginas();
            BindGridView();
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            error = revisarDatosLLenos(txtCedula.Text, txtNombre.Text, txtDireccion.Text, 
                txtTelefono.Text, txtCorreo.Text,lblError);
            if (error == "")
            {
                try
                {
                    con.Open();
                    string ced = lblCedula.Text;
                    MySqlCommand cmd = new MySqlCommand("UPDATE persona SET Nombre = @Name,Direccion = @Address," +
                        "Telefono=@Mobile ,Correo = @Email WHERE persona.cedula = @Cedula", con);
                    cmd.Parameters.AddWithValue("@Name", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Address", txtDireccion.Text);
                    cmd.Parameters.AddWithValue("@Mobile", txtTelefono.Text);
                    cmd.Parameters.AddWithValue("@Email", txtCorreo.Text);
                    cmd.Parameters.AddWithValue("@Cedula", ced);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    ShowMessage("Persona actualizada");
                    GridViewPersona.EditIndex = -1;
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
            else
            {
                lblError.Text = error;
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }


    }
}