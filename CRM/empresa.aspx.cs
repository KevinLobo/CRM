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
    public partial class WebForm3 : System.Web.UI.Page
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
            txtDireccion.Text = string.Empty;
            txtTelefono.Text = string.Empty;
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
                
                CurrentPageLabel.Text = "Página " +paginaActual+" de " +
                    paginas + ".";

                paginaDropDown.Items.Clear();
                for (int i = 0; i < paginas; i++)
                {
                    paginaDropDown.Items.Add((i + 1).ToString());
                }
                paginaDropDown.SelectedIndex = paginaActual-1;
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

        protected void CambioPagina(object sender, EventArgs e)
        {
            paginaActual = paginaDropDown.SelectedIndex+1;
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

        protected String revisarDatosLLenos(String pNombre, String pDireccion, String pTelefono)
        {
            error = "";
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
            return error;
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            error = revisarDatosLLenos(txtNombre.Text, txtDireccion.Text, txtTelefono.Text);
            if (error  == "")
            {
                try
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO empresa (Nombre,Direccion,Telefono)" +
                        " VALUES (@Name, @Address, @Mobile);", con);
                    cmd.Parameters.AddWithValue("@Name", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Address", txtDireccion.Text);
                    cmd.Parameters.AddWithValue("@Mobile", txtTelefono.Text);
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
            else
            {
                lblError.Text = error;
            }
        }

        protected void GridViewEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridViewEmpresa.SelectedRow;
            lblId.Visible = true;
            lblId.Text = row.Cells[2].Text;
            txtNombre.Text = row.Cells[3].Text;
            txtDireccion.Text = row.Cells[4].Text;
            txtTelefono.Text = row.Cells[5].Text;
            btnSubmit.Visible = false;
            btnSubmit.Enabled = false;
            btnUpdate.Visible = true;
            btnUpdate.Enabled = true;
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

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            error = revisarDatosLLenos(txtNombre.Text, txtDireccion.Text, txtTelefono.Text);
            if (error == "")
            {
                try
                {
                    con.Open();
                    string ced = lblId.Text;
                    MySqlCommand cmd = new MySqlCommand("UPDATE empresa SET Nombre = @Name,Direccion = @Address," +
                        "Telefono=@Mobile WHERE empresa.id = @id", con);
                    cmd.Parameters.AddWithValue("@Name", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Address", txtDireccion.Text);
                    cmd.Parameters.AddWithValue("@Mobile", txtTelefono.Text);
                    cmd.Parameters.AddWithValue("@id", ced);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    ShowMessage("Empresa actualizada");
                    GridViewEmpresa.EditIndex = -1;
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