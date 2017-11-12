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
    public partial class persona : System.Web.UI.Page
    {
        IBaseDatos con;
        string conexion = @"Data Source = sql9.freesqldatabase.com;port=3306;Initial"
            + " Catalog=sql9203199;User Id=sql9203199;password = '4xtW6PBmRm' ";
        public persona()
        {
            con = new baseDatos(conexion);
        }

        public persona(fakeBaseDatos fakeDB)
        {
            con = fakeDB;
        }

        string error = "";
        double personasPorPagina = 10;
        double paginas = 0;
        int paginaActual = 1;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["username"] as string) || Session["cliente"].ToString() == "True")
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

        public bool IsDigitsOnly(string str)
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
            if (pCorreo != "" && !IsValidEmail(pCorreo))
            {
                error += "*El correo es inválido.";
            }
            labelError.Text = error;
            labelError.Visible = true;
            return error;
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
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
                con.Abrir();
                con.cargarQuery("Select count(*) from Entidad where cedula is not NULL;");
                IDataReader reader = con.getSalida();
                double totalPersonas = 0;
                if (reader.Read())
                {
                    totalPersonas = Convert.ToDouble(reader["count(*)"]);
                }
                
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
                con.Cerrar();
            }

        }

        private void BindGridView()
        {
            try
            {
                CargarPersona(paginaDropDown.SelectedIndex, GridViewPersona);
            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
        }

        public void CargarPersona(int pIndex,GridView pGrid)
        {
            con.Abrir();
            double limite = pIndex * personasPorPagina;
            con.cargarQuery("Select Cedula, Nombre, Direccion, Telefono, Correo "+
                "from Entidad where cedula is not NULL limit " + limite + "," + personasPorPagina + "");
            IDataReader reader = con.getSalida();
            DataTable table = new DataTable();
            table.Load(reader);
            pGrid.DataSource = table;
            pGrid.DataBind();
            con.Cerrar();
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
                    InsertarPersona(txtCedula.Text,txtNombre.Text, txtDireccion.Text,
                        txtTelefono.Text, txtCorreo.Text);
                    ShowMessage("Registro correcto.");
                    clear();
                    LlenarListaPaginas();
                    BindGridView();
                }
                catch (MySqlException ex)
                {
                    ShowMessage(ex.Message);
                }
            }
            else
            {
                lblError.Text = error;
            }

        }

        public bool InsertarPersona(string pTxtCedula, string pTxtNombre, string pTxtDireccion , string pTxtTelefono,
            string pTxtCorreo)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("INSERT INTO Entidad (cedula,Nombre,Direccion,Telefono,Correo)" +
                    " VALUES ('" + pTxtCedula.Trim() + "','" + pTxtNombre.Trim() +
                    "', '" + pTxtDireccion.Trim() + "','" + pTxtTelefono.Trim() +
                    "', '" + pTxtCorreo.Trim() + "');");
                con.getSalida().Close();
                con.Cerrar();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
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
                string ced = GridViewPersona.DataKeys[e.RowIndex].Value.ToString();
                BorrarPersona(ced);
                ShowMessage("Persona eliminada");
                GridViewPersona.EditIndex = -1;
                LlenarListaPaginas();
                BindGridView();
            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }

        }

        public bool BorrarPersona(string id)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Delete From Entidad where cedula='" + id + "'");
                con.getSalida().Close();
                con.Cerrar();
                return true;
            }
            catch(MySqlException ex)
            {
                throw ex;
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
                    ActualizarPersona(lblCedula,txtCedula.Text, txtNombre.Text, txtDireccion.Text,
                        txtTelefono.Text, txtCorreo.Text);
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

            }
            else
            {
                lblError.Text = error;
            }
        }

        public bool ActualizarPersona(Label pLblCedula, string pTxtCedula, string pTxtNombre, string pTxtDireccion, string pTxtTelefono,
            string pTxtCorreo)
        {
            try
            {
                con.Abrir();
                string ced = pLblCedula.Text;
                con.cargarQuery("UPDATE Entidad SET Nombre = '" + pTxtNombre.Trim() + "',Direccion = '" + pTxtDireccion.Trim() + "'," +
                    "Telefono='" + pTxtTelefono.Trim() + "' ,Correo = '" + pTxtCorreo.Trim() + "' WHERE Entidad.cedula = '" + pTxtCedula.Trim() + "'");
                con.getSalida().Close();
                con.Cerrar();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }


    }
}