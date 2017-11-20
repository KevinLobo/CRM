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
    public partial class entrenamientos : System.Web.UI.Page
    {

        IBaseDatos con;
        string conexion = @"Data Source = sql9.freesqldatabase.com;port=3306;Initial"
            + " Catalog=sql9203199;User Id=sql9203199;password = '4xtW6PBmRm' ";
        public entrenamientos()
        {
            con = new baseDatos(conexion);
        }

        public entrenamientos(fakeBaseDatos fakeDB)
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
                    //LlenarListaPaginas();
                    //BindGridView();

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

        void Clear()
        {
            chkbxFecha.Checked = false;
            chkbxAsistido.Checked = false;
            txtNombreEvento.Text = string.Empty;
            fechaInicio.Text = string.Empty;
            fechaFinal.Text = string.Empty;
            btnCancel.Text = "Cancelar";
            lblError.Text = string.Empty;
            btnSubmit.Visible = true;
            btnSubmit.Enabled = true;
            lblError.Visible = false;
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

        public string RevisarDatosLlenos(string fechaInicio, string fechaFinal, Boolean cualquierFecha, string nombreEvento, Label labelError)
        {


            error = "";
            labelError.Text = "";
            bool fechasValidas = true;

            System.Diagnostics.Debug.WriteLine("Fecha: " + fechaInicio);
            System.Diagnostics.Debug.WriteLine("Cualquiera: "+cualquierFecha);
            if (!cualquierFecha)
            {
                if (fechaInicio == "")
                {
                    error += "*El campo de fecha inicial no puede estar vacío. <br />";
                    fechasValidas = false;
                }

                if (fechaFinal == "")
                {
                    error += "*El campo de fecha final no puede estar vacío. <br />";
                    fechasValidas = false;
                }

                if (fechaInicio != "" && !FormatoFechaValido(fechaInicio))
                {
                    error += "*El formato de la fecha inicial no es válido. <br />";
                    fechasValidas = false;
                }

                if (fechaFinal != "" && !FormatoFechaValido(fechaFinal))
                {
                    error += "*El formato de la fecha final no es válido. <br />";
                    fechasValidas = false;
                }

                if(fechasValidas && !RangoFechasValido(fechaInicio, fechaFinal))
                {
                    error += "*La fecha inicial debe ser menor que la final. <br />";
                }
            }

            labelError.Text = error;
            labelError.Visible = true;
            return error;
        }

        public bool FormatoFechaValido(string fecha)
        {
            DateTime dDate;

            if (DateTime.TryParse(fecha, out dDate))
            {
                return true;
            }
            return false;
        }

        public bool RangoFechasValido(string fechaInicial, string fechaFinal)
        {
            DateTime Inicio = DateTime.Parse(fechaInicial);
            DateTime Final = DateTime.Parse(fechaFinal);
            return Inicio < Final;

        }

        protected void GridViewEmpresa_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridViewEmpresa.SelectedRow;
            Boolean suscribirse = true;

            try
            {
                con.Abrir();

                con.cargarQuery("SELECT Count(*) from ParticipanteXEvento where idEvento = "+
                    row.Cells[1].Text + " and idEntidad = "+Session["id"]);
                

                IDataReader reader = con.getSalida();

                while (reader.Read())
                {
                    if (reader.GetInt32(0) == 1)
                    {
                        suscribirse = false;
                    }
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
            if (suscribirse)
            {
                ShowMessage(SuscribirseAEntrenamiento(row.Cells[1].Text, Session["id"].ToString()));
            } else
            {
                ShowMessage(DesuscribirseAEntrenamiento(row.Cells[1].Text, Session["id"].ToString()));
            }
        }

        public String SuscribirseAEntrenamiento(string idEvento, string idSession)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("INSERT INTO ParticipanteXEvento (idEvento, idEntidad) VALUES("+idEvento+","+idSession+ ")");
                con.getSalida().Close();

                con.Cerrar();
                return "Se ha suscrito correctamente al evento seleccionado.";

            }
            catch (MySqlException ex)
            {
                return ex.Message;
            }
        }

        public String DesuscribirseAEntrenamiento(string idEvento, string idSession)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("DELETE FROM ParticipanteXEvento where idEvento = " + idEvento + " AND idEntidad = " + idSession + ";");
                con.getSalida().Close();

                con.Cerrar();
                return "Se ha desuscrito correctamente al evento seleccionado.";

            }
            catch (MySqlException ex)
            {
                return ex.Message;
            }
        }
    


        public bool BuscarEventos(string fechaInicio, string fechaFinal, Boolean cualquierFecha, string nombreEvento, string idCliente, Boolean asistido, GridView gridViewEmpresa)
        {
            string query = "SELECT Id, Nombre, Descripcion, fechaHora as FechaYHora from Entrenamiento";
            if (asistido)
            {
                query += " inner join ParticipanteXEvento ON ParticipanteXEvento.idEvento = Entrenamiento.id where ParticipanteXEvento.idEntidad = " +
                    idCliente;
            }
            if (!cualquierFecha && asistido)
            {
                query += " AND fechaHora BETWEEN STR_TO_DATE('" + fechaInicio + "', '%m/%d/%Y %h:%i %p') AND STR_TO_DATE('" +
                    fechaFinal + "', '%m/%d/%Y %h:%i %p')";
            }
            if (!cualquierFecha && !asistido)
            {
                query += " where fechaHora BETWEEN STR_TO_DATE('" + fechaInicio + "', '%m/%d/%Y %h:%i %p') AND STR_TO_DATE('" +
                    fechaFinal + "', '%m/%d/%Y %h:%i %p')";
            }
            if (nombreEvento != "" && !cualquierFecha)
            {
                query += " AND Entrenamiento.nombre like '%" + nombreEvento + "%'";
            }

            if (nombreEvento != "" && cualquierFecha)
            {
                query += " where Entrenamiento.nombre like '%" + nombreEvento + "%'";
            }


            query += ";";
            System.Diagnostics.Debug.WriteLine(query);

            try
            {
                con.Abrir();
                con.cargarQuery(query);
                IDataReader reader = con.getSalida();
                DataTable table = new DataTable();
                table.Load(reader);
                gridViewEmpresa.DataSource = table;
                gridViewEmpresa.DataBind();
                con.Cerrar();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }


        //Botones

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string error = RevisarDatosLlenos(fechaInicio.Text, fechaFinal.Text, chkbxFecha.Checked, txtNombreEvento.Text, lblError);

            if (error == "")
            {
                try
                {

                    BuscarEventos(fechaInicio.Text, fechaFinal.Text, chkbxFecha.Checked, txtNombreEvento.Text, Session["id"].ToString(),
                    chkbxAsistido.Checked, GridViewEmpresa);

                }
                catch (MySqlException ex)
                {
                    ShowMessage(ex.Message);
                }
            }            
        }
    }
}