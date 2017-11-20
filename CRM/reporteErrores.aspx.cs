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
    public partial class reporteErrores : System.Web.UI.Page
    {

        IBaseDatos con;
        string conexion = @"Data Source = sql9.freesqldatabase.com;port=3306;Initial"
            + " Catalog=sql9203199;User Id=sql9203199;password = '4xtW6PBmRm' ";
        public reporteErrores()
        {
            con = new baseDatos(conexion);
        }

        public reporteErrores(IBaseDatos fakeDB)
        {
            con = fakeDB;
        }

        string error = "";

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
                con.cargarQuery("Select CONCAT(id, ' - ', fecha, ' - $', precio) as nombreVenta, id from venta where idEntidad ='" + Session["id"] + "'");
                IDataReader reader = con.getSalida();
                

                
                DataTable ventas = new DataTable();
                ventas.Load(reader);
                ventaDropdown.DataSource = ventas;
                ventaDropdown.DataTextField = "nombreVenta";
                ventaDropdown.DataValueField = "id";
                ventaDropdown.DataBind();

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
            txtReporte.Text = string.Empty;
            btnCancel.Text = "Cancelar";
            lblError.Text = string.Empty;
            btnSubmit.Visible = true;
            btnSubmit.Enabled = true;
            lblError.Visible = false;
            //txtIdProducto.Focus();
        }

        // ---------------Botones-------------------

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
        

        public bool EsVentaValida(string idVenta)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Select COUNT(*) as ventas from venta where id ='" + idVenta + "'");
                IDataReader reader = con.getSalida();
                while (reader.Read())
                {
                    string ventas = reader.GetString(0);
                    if (ventas == "0")
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

        public bool InsertarReporte(DateTime fechaHora, string reporte, string idVenta, string idCliente)
        {
            try
            {
                con.Abrir();

                con.cargarQuery("INSERT INTO Reporte (Fecha, Reporte, idVenta, idEntidad) VALUES " +
                "('" + fechaHora.ToString("dd/MM/yyyy hh:mm:ss") + "', '" + reporte + "'," + idVenta + ", '"+ idCliente + "');");

                con.getSalida().Close();
                con.Cerrar();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public string RevisarDatosLlenos(string reporte, string idVenta, Label labelError)
        {
            error = "";
            labelError.Text = "";
            

            //respuesta
            if (reporte == "")
            {
                error += "*El campo del reporte no puede estar vacio.<br />";
            }
            if (reporte.Length > 1000)
            {
                error += "*El campo de reporte no puede tener mas de 1000 caracteres.<br />";
            }
            if (!EsVentaValida(idVenta))
            {
                error += "*La venta seleccionada no existe.<br />";
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
                    error = RevisarDatosLlenos(txtReporte.Text.Trim(), ventaDropdown.SelectedValue, lblError);
                    if (error == "")
                    {
                        InsertarReporte(DateTime.Now, txtReporte.Text.Trim(), ventaDropdown.SelectedValue, Session["id"].ToString());
                        ShowMessage("Registro correcto.");
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
