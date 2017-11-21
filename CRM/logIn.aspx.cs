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
    public partial class Default : System.Web.UI.Page
    {

        IBaseDatos con2 = new baseDatos(@"Data Source = sql9.freesqldatabase.com;port=3306;Initial"
            + " Catalog=sql9203199;User Id=sql9203199;password = '4xtW6PBmRm' ");

        MySqlConnection con = new MySqlConnection(@"Data Source = sql9.freesqldatabase.com;port=3306;Initial"
            + " Catalog=sql9203199;User Id=sql9203199;password = '4xtW6PBmRm' ");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Session["username"] as string))
            {
                Response.Redirect("principal.aspx");
            }
            try
            {
                if (!Page.IsPostBack)
                {
                    CargarPersonas();
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }

        }

        void CargarPersonas()
        {
            try
            {
                con2.Abrir();
                con2.cargarQuery("Select nombre, id from Entidad");
                IDataReader reader = con2.getSalida();

                DataTable personas = new DataTable();
                personas.Load(reader);
                personaDropdown.DataSource = personas;
                personaDropdown.DataTextField = "nombre";
                personaDropdown.DataValueField = "id";
                personaDropdown.DataBind();

            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
            finally
            {
                con2.Cerrar();
            }
        }

        protected void logIn_Click(object sender, EventArgs e)
        {
            con.Open();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from users where UserName ='" + userName.Text + "' and Password='" + password.Text + "'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                Session["username"] = dr["UserName"].ToString();
                Session["cliente"] = dr["tipo"].ToString(); //0 Vendedor, 1 Cliente
                Session["id"] = dr["idEntidad"];
                Response.Redirect("principal.aspx");
            }

            mensaje.Text = "Usuario o contraseña invalida";


            con.Close();

        }

        void ShowMessage(string msg)
        {
            Response.Write("<script>alert('" + msg + "');</script>");

        }

        public bool EsUsuarioValido(string username)
        {
            try
            {
                con2.Abrir();
                con2.cargarQuery("Select COUNT(*) as clientes from users where UserName ='" + username + "'");
                IDataReader reader = con2.getSalida();
                while (reader.Read())
                {
                    string usuarios = reader.GetString(0);
                    if (usuarios == "0")
                    {
                        con2.Cerrar();
                        return true;
                    }
                    else
                    {
                        con2.Cerrar();
                        return false;
                    }
                }
            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
            con2.Cerrar();
            return false;
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            try
            {

                if (!(txtUsuarioRegistro.Text.Trim() == "" ||
                        txtPasswordRegistro.Text.Trim() == "" ||
                        txtPasswordRegistroConfirmar.Text.Trim() == ""))
                {
                    if (EsUsuarioValido(txtUsuarioRegistro.Text.Trim()))
                    {
                        if (txtPasswordRegistro.Text == txtPasswordRegistroConfirmar.Text)
                        {
                            con.Open();
                            MySqlCommand cmd = con.CreateCommand();
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "INSERT INTO users (UserName, Password, tipo, idEntidad) VALUES('" +
                            txtUsuarioRegistro.Text + "', '" + txtPasswordRegistro.Text + "', 0, '" + personaDropdown.SelectedValue + "')";
                            cmd.ExecuteNonQuery();
                            mensaje.Text = "";
                        }
                        else
                        {
                            mensaje.Text = "Los dos campos de contraseña no coinciden";
                        }
                    }
                    else
                    {
                        mensaje.Text = "El usuario ingresado no está disponible.";
                    }

                }
                else
                {
                    mensaje.Text = "Todos los campos son obligatorios";
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
    }
}