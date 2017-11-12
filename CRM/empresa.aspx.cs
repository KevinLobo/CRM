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
        string  conexion = @"Data Source = sql9.freesqldatabase.com;port=3306;Initial"
            + " Catalog=sql9203199;User Id=sql9203199;password = '4xtW6PBmRm' ";
        public empresa() {
            con = new baseDatos(conexion);
        }

        public empresa(fakeBaseDatos fakeDB)
        {
            con = fakeDB;
        }


        string error = "";
        double filasPorPagina = 10;
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


        void clear()
        {
            lblId.Visible = false;
            txtNombre.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtCorreo.Text = string.Empty;
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
                con.cargarQuery("Select count(*) from Entidad where cedula is NULL;");
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
                CargarEmpresa(paginaDropDown.SelectedIndex, GridViewEmpresa);
            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
        }

        public void CargarEmpresa(int pIndex, GridView pGrid)
        {
            con.Abrir();
            double limite = pIndex * filasPorPagina;
            string query = "Select Id, Nombre, Direccion, Telefono, Correo " +
                "from Entidad where cedula is NULL limit " + pIndex + "," + filasPorPagina + ";";
            System.Diagnostics.Debug.WriteLine(query);
            con.cargarQuery(query);
            IDataReader reader = con.getSalida();
            DataTable table = new DataTable();
            table.Load(reader);
            pGrid.DataSource = table;
            pGrid.DataBind();
            con.Cerrar();
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

        public bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        protected String revisarDatosLLenos(String pNombre, String pDireccion, String pTelefono, Label labelError, String pCorreo)
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

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            error = revisarDatosLLenos(txtNombre.Text, txtDireccion.Text, txtTelefono.Text,lblError, txtCorreo.Text);
            if (error  == "")
            {
                try
                {
                    InsertarEmpresa(txtNombre.Text, txtDireccion.Text, txtTelefono.Text, txtCorreo.Text);
                    ShowMessage("Registro correcto");
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

        public bool InsertarEmpresa(string pTxtNombre, string pTxtDireccion, string pTxtTelefono, string pTxtCorreo)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("INSERT INTO Entidad (Nombre,Direccion,Telefono,Correo)" +
                    " VALUES ('" + pTxtNombre.Trim() + "','" + pTxtDireccion.Trim() + "', '" + pTxtTelefono.Trim() + "', '" + pTxtCorreo.Trim() + "');");
                con.getSalida().Close();
                con.Cerrar();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
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
                string id = GridViewEmpresa.DataKeys[e.RowIndex].Value.ToString();
                BorrarEmpresa(id);
                ShowMessage("Empresa eliminada");
                GridViewEmpresa.EditIndex = -1;
                LlenarListaPaginas();
                BindGridView();
            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
        }

        public bool BorrarEmpresa(string id)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Delete From Entidad where id='" + id + "'");
                con.getSalida().Close();
                con.Cerrar();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            error = revisarDatosLLenos(txtNombre.Text, txtDireccion.Text, txtTelefono.Text,lblError, txtCorreo.Text);
            if (error == "")
            {
                try
                {
                    ActualizarEmpresa(lblId, txtNombre.Text, txtDireccion.Text, txtTelefono.Text, txtCorreo.Text);
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

        public bool ActualizarEmpresa(Label pLblID,string pTxtNobmre, string pTxtDireccion, string pTxtTelefono, string pTxtCorreo)
        {
            try
            {
                con.Abrir();
                string id = pLblID.Text;
                con.cargarQuery("UPDATE Entidad SET Nombre = '" + pTxtNobmre.Trim() +
                        "',Direccion ='" + pTxtDireccion.Trim() + "', Telefono='" + pTxtTelefono.Trim() +
                        "', Correo='" + pTxtCorreo.Trim() + "' WHERE Entidad.id = '" + id + "'");
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