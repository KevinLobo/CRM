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
    public partial class producto : System.Web.UI.Page
    {


        IBaseDatos con;
        string conexion = @"Data Source = localhost;port=3306;Initial"
            + " Catalog=CRM;User Id=root;password = '' ";
        public producto()
        {
            con = new baseDatos(conexion);
        }

        public producto(IBaseDatos bd)
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
            txtPrecio.Text = string.Empty;
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
                con.cargarQuery("Select  count(*) from producto");
                IDataReader reader = con.getSalida();
                double totalPersonas = 0;
                if (reader.Read()) {
                    totalPersonas = Convert.ToDouble(reader["count(*)"]);
                }
                lbltotalcount.Text = totalPersonas.ToString();
                paginas = totalPersonas / filasPorPagina;
                paginas = Math.Ceiling(paginas);

                CurrentPageLabel.Text = "Página " + paginaActual + " de " +
                    paginas + ".";

                paginaDropDown.Items.Clear();
                for (int i = 0; i < paginas; i++)
                {
                    paginaDropDown.Items.Add((i + 1).ToString());
                }
                if (totalPersonas > 0)
                {
                    paginaDropDown.SelectedIndex = paginaActual - 1;
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


        private void BindGridView()
        {
            try
            {
                CargarProductos(paginaDropDown.SelectedIndex,GridViewProductos);
            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
        }

        public void CargarProductos(int pIndex, GridView pGrid)
        {
            con.Abrir();
            double limite = pIndex * filasPorPagina;
            con.cargarQuery("Select * from producto limit " + pIndex + "," + filasPorPagina + "");
            IDataReader reader = con.getSalida();
            DataTable table = new DataTable();
            table.Load(reader);
            pGrid.DataSource = table;
            pGrid.DataBind();
            con.Cerrar();
        }

        protected void CambioPagina(object sender, EventArgs e)
        {
            paginaActual = paginaDropDown.SelectedIndex + 1;
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

        protected String revisarDatosLLenos(string nombre,string precio,Label labelError)
        {
            error = "";
            labelError.Text = "";
            if (nombre.Trim() == "")
            {
                error += "*El campo nombre no puede estar vacio.<br />";
            }
            if (nombre.Trim().Length > 80)
            {
                error += "*El campo nombre no puede tener mas de 80 caracteres.<br />";
            }
            if (precio.Trim() == "")
            {
                error += "*El campo precio no puede estar vacio.<br />";
            }
            if (!IsDigitsOnly(precio))
            {
                error += "*El campo precio solo puede contener numeros.<br />";
            }

            labelError.Text = error;
            labelError.Visible = true;
            return error;

        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            error = revisarDatosLLenos(txtNombre.Text, txtPrecio.Text, lblError);
            if (error == "")
            {
                try
                {
                    InsertarProducto(txtNombre.Text, txtPrecio.Text);
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
        }

        public bool InsertarProducto(string pTxtNobmre, string pTxtPrecio)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("INSERT INTO producto (nombre,precio)" +
                        " VALUES ('" + pTxtNobmre.Trim() + "', '" + pTxtPrecio.Trim() + "');");
                con.getSalida().Close();
                con.Cerrar();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }



        protected void GridViewProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = GridViewProductos.SelectedRow;
            lblId.Visible = true;
            lblId.Text = row.Cells[2].Text;
            txtNombre.Text = row.Cells[3].Text;
            txtPrecio.Text = row.Cells[4].Text;
            btnSubmit.Visible = false;
            btnSubmit.Enabled = false;
            btnUpdate.Visible = true;
            btnUpdate.Enabled = true;
        }

        protected void CambioPrecio(object sender, EventArgs e)
        {
            EstaEnRango(txtPrecio, Double.MaxValue);
        }


        void EstaEnRango(TextBox txtEntrada, double rangoMayor)
        {
            if (txtEntrada.Text != "")
            {
                double entrada = Convert.ToDouble(txtEntrada.Text);
                if (entrada > rangoMayor)
                {
                    txtEntrada.Text = Convert.ToString(rangoMayor);
                }
                if (entrada < 0)
                {
                    txtEntrada.Text = "0";
                }
            }
            else
            {
                txtEntrada.Text = "0";
            }
        }

        protected void GridViewProducto_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string id = GridViewProductos.DataKeys[e.RowIndex].Value.ToString();
                BorrarProducto(id);
                ShowMessage("Producto eliminado");
                GridViewProductos.EditIndex = -1;
                LlenarListaPaginas();
                BindGridView();
            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
        }

        public bool BorrarProducto(string id)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Delete From producto where id='" + id + "'");
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
            error = revisarDatosLLenos(txtNombre.Text, txtPrecio.Text, lblError);
            if (error == "")
            {
                try
                {
                    ActualizarProducto(lblId, txtNombre.Text, txtPrecio.Text);
 
                    ShowMessage("Producto actualizado");
                    GridViewProductos.EditIndex = -1;
                    LlenarListaPaginas();
                    BindGridView();
                    clear();
                }
                catch (MySqlException ex)
                {
                    ShowMessage(ex.Message);
                }
            }
        }

        public bool ActualizarProducto(Label pLblID, string pTxtNobmre, string pTxtPrecio)
        {
            try
            {
                con.Abrir();
                string id = pLblID.Text;
                con.cargarQuery("UPDATE producto SET nombre = '" + pTxtNobmre.Trim() + "',precio = '" + pTxtPrecio.Trim() + "' " +
                    " WHERE producto.id = '" + id + "'");
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