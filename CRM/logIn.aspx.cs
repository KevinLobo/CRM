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
        MySqlConnection con = new MySqlConnection(@"Data Source = localhost;port=3306;Initial"
        +" Catalog=CRM;User Id=root;password = '' ");
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Session["username"] as string))
            {
                Response.Redirect("principal.aspx");
            }
        }

        protected void logIn_Click(object sender, EventArgs e)
        {
            con.Open();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from users where UserName ='"+ userName.Text + "' and Password='"+password.Text+"'";
            cmd.ExecuteNonQuery();
            DataTable dt = new DataTable();

            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows) {
                Session["username"] = dr["UserName"].ToString();
                Response.Redirect("principal.aspx");


            }

            mensaje.Text = "Usuario o contraseña invalida";


            con.Close();

        }

        void ShowMessage(string msg)
        {
            Response.Write("<script>alert('" + msg + "');</script>");

        }

        protected void Register_Click(object sender, EventArgs e)
        {
            try
            {

                if (!(txtUsuarioRegistro.Text.Trim() == "" ||
                        txtPasswordRegistro.Text.Trim() == "" ||
                        txtPasswordRegistroConfirmar.Text.Trim() == ""))
                {
                    if (txtPasswordRegistro.Text == txtPasswordRegistroConfirmar.Text)
                    {
                        con.Open();
                        MySqlCommand cmd = con.CreateCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "INSERT INTO users (UserName, Password) VALUES('" +
                        txtUsuarioRegistro.Text + "', '" + txtPasswordRegistro.Text + "')";
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