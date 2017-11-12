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
    public partial class productosRelacionados : System.Web.UI.Page
    {
        IBaseDatos con;
        string conexion = @"Data Source = sql9.freesqldatabase.com;port=3306;Initial"
            + " Catalog=sql9203199;User Id=sql9203199;password = '4xtW6PBmRm' ";
        public productosRelacionados()
        {
            con = new baseDatos(conexion);
        }

        public productosRelacionados(fakeBaseDatos fakeDB)
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
            con.cargarQuery("SELECT p2.Id, p2.Nombre, p2.Precio from venta inner join ProductoXVenta on ProductoXVenta.IdVenta = "+
                "venta.id inner join producto as p on ProductoXVenta.IdProducto = p.ID inner join producto as p2 on p.idCategoria = "+
                "p2.idCategoria where venta.idEntidad = "+Session["id"]+" order by rand() limit 5;");
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