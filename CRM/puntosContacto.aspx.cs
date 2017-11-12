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
    public partial class puntosContacto : System.Web.UI.Page
    {

        IBaseDatos con;
        string conexion = @"Data Source = sql9.freesqldatabase.com;port=3306;Initial"
            + " Catalog=sql9203199;User Id=sql9203199;password = '4xtW6PBmRm' ";
        public puntosContacto()
        {
            con = new baseDatos(conexion);
        }

        public puntosContacto(fakeBaseDatos fakeDB)
        {
            con = fakeDB;
        }
        
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
                    CargarSession();
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

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("principal.aspx");
        }

        private void LlenarListaPaginas()
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Select count(*) from users where tipo = 0");
                IDataReader reader = con.getSalida();
                double totalPersonas = 0;
                if (reader.Read())
                {
                    totalPersonas = Convert.ToDouble(reader["count(*)"]);
                }

                lbltotalcount.Text = totalPersonas.ToString();
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

        // ---------------GridView-------------------
        private void BindGridView()
        {
            try
            {
                CargarVenta(GridViewEmpresa);

            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }

        }

        public void CargarVenta(GridView pGrid)
        {
            con.Abrir();
            con.cargarQuery("select Nombre, Direccion, Telefono, Correo from users left join " + 
                "Entidad on Entidad.id = users.idEntidad where users.tipo = 0");
            IDataReader reader = con.getSalida();
            DataTable table = new DataTable();
            table.Load(reader);
            pGrid.DataSource = table;
            pGrid.DataBind();
            con.Cerrar();
        }

        protected void GridViewEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        
    }
}