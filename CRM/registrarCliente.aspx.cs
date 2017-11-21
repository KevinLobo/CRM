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
    public partial class registrarCliente : System.Web.UI.Page
    {

        IBaseDatos con;
        string error = "";
        string conexion = @"Data Source = sql9.freesqldatabase.com;port=3306;Initial"
            + " Catalog=sql9203199;User Id=sql9203199;password = '4xtW6PBmRm' ";
        public registrarCliente()
        {
            con = new baseDatos(conexion);
        }

        public registrarCliente(IBaseDatos fakeDB)
        {
            con = fakeDB;
        }

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
                    CargarSession();
                    CargarVentas();
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        void CargarSession()
        {
            try
            {
                con.Abrir();

                con.cargarQuery("Select Nombre from Entidad where id ='" + Session["id"] + "'");

                IDataReader reader = con.getSalida();

                while (reader.Read())
                {
                    lblCliente.Text = reader.GetString(0);
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

        void CargarVentas()
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Select nombre, id from Entidad");
                IDataReader reader = con.getSalida();

                DataTable clientes = new DataTable();
                clientes.Load(reader);
                clienteDropdown.DataSource = clientes;
                clienteDropdown.DataTextField = "nombre";
                clienteDropdown.DataValueField = "id";
                clienteDropdown.DataBind();

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

        void ShowMessage(string msg)
        {
            Response.Write("<script>alert('" + msg + "');</script>");

        }


        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("principal.aspx");
        }

        void Clear()
        {
            txtUsuario.Text = string.Empty;
            txtPasswordRegistro.Text = string.Empty;
            txtPasswordRegistroConfirmar.Text = string.Empty;
            btnCancel.Text = "Cancelar";
            lblError.Text = string.Empty;
            btnSubmit.Visible = true;
            btnSubmit.Enabled = true;
            lblError.Visible = false;
            //txtIdProducto.Focus();
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public bool EsClienteValido(string idEntidad)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Select COUNT(*) as clientes from Entidad where id ='" + idEntidad + "'");
                IDataReader reader = con.getSalida();
                while (reader.Read())
                {
                    string clientes = reader.GetString(0);
                    if (clientes == "0")
                    {
                        con.Cerrar();
                        return false;
                    }
                    else
                    {
                        con.Cerrar();
                        return true;
                    }
                }
            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
            con.Cerrar();
            return false;
        }

        public bool EsUsuarioValido(string username)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Select COUNT(*) as clientes from users where UserName ='" + username + "'");
                IDataReader reader = con.getSalida();
                while (reader.Read())
                {
                    string usuarios = reader.GetString(0);
                    if (usuarios == "0")
                    {
                        con.Cerrar();
                        return true;
                    }
                    else
                    {
                        con.Cerrar();
                        return false;
                    }
                }
            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
            con.Cerrar();
            return false;
        }

        public bool InsertarUsuarioCliente(string username, string password, string idEntidad)
        {
            try
            {
                con.Abrir();

                con.cargarQuery("INSERT INTO users (UserName, Password, tipo, idEntidad) VALUES " +
                "('" + username + "', '" + password + "', 1, '" + idEntidad + "');");

                con.getSalida().Close();
                con.Cerrar();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public string RevisarDatosLlenos(string username, string password, string confirmPassword, string idEntidad, Label labelError)
        {
            error = "";
            labelError.Text = "";


            //respuesta
            if (username == "")
            {
                error += "*El campo de usuario no puede estar vacío.<br />";
            }
            if (username.Length > 40)
            {
                error += "*El campo de usuario no puede tener mas de 40 caracteres.<br />";
            }
            if (!EsUsuarioValido(username))
            {
                error += "*El nombre de usuario ya existe.<br />";
            }
            if (password == "")
            {
                error += "*El campo de contraseña no puede estar vacío.<br />";
            }
            if (password.Length > 128)
            {
                error += "*El campo de contraseña no puede tener más de 128 caracteres.<br />";
            }
            if (confirmPassword == "")
            {
                error += "*El campo de confirmar contraseña no puede estar vacío.<br />";
            }
            if (!password.Equals(confirmPassword))
            {
                error += "*Las contraseñas no coinciden.<br />";
            }
            if (!EsClienteValido(idEntidad))
            {
                error += "*El cliente seleccionado no existe.<br />";
            }


            labelError.Text = error;
            labelError.Visible = true;
            return error;

        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            //string error = RevisarDatosLlenos(txtReporte, lblCliente, lblError);
            if (true)
            {
                try
                {
                    error = RevisarDatosLlenos(txtUsuario.Text.Trim(), txtPasswordRegistro.Text.Trim(), txtPasswordRegistroConfirmar.Text.Trim(), clienteDropdown.SelectedValue, lblError);
                    if (error == "")
                    {
                        InsertarUsuarioCliente(txtUsuario.Text.Trim(), txtPasswordRegistro.Text.Trim(), clienteDropdown.SelectedValue);
                        ShowMessage("Registro de cliente correcto.");
                        Clear();
                    }

                }
                catch (MySqlException ex)
                {
                    ShowMessage(ex.Message);
                }
            }
        }

    }
}