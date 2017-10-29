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
    public partial class empresa : System.Web.UI.Page
    {
        IBaseDatos con;
        string  conexion =@"Data Source = localhost;port=3306;Initial"
            + " Catalog=CRM;User Id=root;password = '' ";
        public empresa() {
            con = new baseDatos(conexion);
        }

        public empresa(IBaseDatos bd)
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
                con.Abrir();
                con.cargarQuery("Select  count(*) from empresa");
                IDataReader reader = con.getSalida();
                double totalPersonas = 0;
                if (reader.Read())
                {
                    totalPersonas = Convert.ToDouble(reader["count(*)"]);
                }
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
                con.Cerrar();
            }

        }


        private void BindGridView()
        {
            try
            {
                con.Abrir();
                double limite = paginaDropDown.SelectedIndex * filasPorPagina;
                con.cargarQuery("Select * from empresa limit " + limite + "," + filasPorPagina + "");
                IDataReader reader = con.getSalida();
                DataTable table = new DataTable();
                table.Load(reader);
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

        protected String revisarDatosLLenos(String pNombre, String pDireccion, String pTelefono, Label labelError)
        {
            error = "";
            labelError.Text = "";
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
            labelError.Text = error;
            labelError.Visible = true;
            return error;
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            error = revisarDatosLLenos(txtNombre.Text, txtDireccion.Text, txtTelefono.Text,lblError);
            if (error  == "")
            {
                try
                {
                    con.Abrir();
                    con.cargarQuery("INSERT INTO empresa (Nombre,Direccion,Telefono)" +
                        " VALUES ('" + txtNombre.Text + "','" + txtDireccion.Text + "', '" + txtTelefono.Text + "');");
                    con.getSalida().Close();
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
                    con.Cerrar();
                }
            }
            else
            {
                lblError.Text = error;
            }
        }

        protected void GridViewEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            filaSeleccionada(lblId, txtNombre, txtTelefono, txtDireccion, btnSubmit, btnUpdate, GridViewEmpresa);
        }

        protected void filaSeleccionada(Label pLblID, TextBox pTxtNombre, TextBox pTxtTelefono,
            TextBox pTxtDireccion,Button pBtnSubmit, Button pBtnUpdate, GridView pGV)
        {
            GridViewRow row = pGV.SelectedRow;

            pLblID.Visible = true;
            pLblID.Text = row.Cells[2].Text;
            pTxtNombre.Text = row.Cells[3].Text;
            pTxtDireccion.Text = row.Cells[4].Text;
            pTxtTelefono.Text = row.Cells[5].Text;
            pBtnSubmit.Visible = false;
            pBtnSubmit.Enabled = false;
            pBtnUpdate.Visible = true;
            pBtnUpdate.Enabled = true;

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

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            error = revisarDatosLLenos(txtNombre.Text, txtDireccion.Text, txtTelefono.Text,lblError);
            if (error == "")
            {
                try
                {
                    con.Abrir();
                    string ced = lblId.Text;
                    string query = "UPDATE empresa SET Nombre = '" + txtNombre.Text +
                        "',Direccion ='" + txtDireccion.Text + "', Telefono='" + txtTelefono.Text +
                        "' WHERE empresa.id = '" + ced + "'";
                    con.cargarQuery(query);
                    con.getSalida().Close();
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
                    con.Cerrar();
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