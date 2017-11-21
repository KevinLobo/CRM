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
    public partial class propuesta : System.Web.UI.Page
    {
        IBaseDatos con;
        string error = "";
        double filasPorPagina = 10;
        double paginas = 0;
        int paginaActual = 1;
        string conexion = @"Data Source = sql9.freesqldatabase.com;port=3306;Initial"
            + " Catalog=sql9203199;User Id=sql9203199;password = '4xtW6PBmRm' ";
        public propuesta()
        {
            con = new baseDatos(conexion);
        }

        public propuesta(fakeBaseDatos fakeDB)
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

                    LlenarListaPaginas();
                    BindGridView();
                    CargarSession();
                    lblIdsProductos.Visible = false;

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
            lblVendedor.Text = Session["username"].ToString();
        }

        void clear()
        {
            txtIdProducto.Text = string.Empty;
            txtEmpresa.Text = string.Empty;
            txtPersona.Text = string.Empty;
            txtEmpresa.Visible = false;
            txtPersona.Visible = false;
            rbEmpresa.Checked = false;
            rbPersona.Checked = false;
            rbAprovado.Checked = false;
            rbRechazado.Checked = false;
            txtPrecio.Text = string.Empty;
            txtDescuento.Text = string.Empty;
            txtComision.Text = string.Empty;
            lblPrecioFinal.Text = string.Empty;
            txtRespuesta.Text = string.Empty;
            datetimepicker.Text = string.Empty;
            lblNombreProducto.Text = string.Empty;
            lblCliente.Text = string.Empty;

            lblError.Text = string.Empty;
            btnSubmit.Visible = true;
            btnSubmit.Enabled = true;
            btnUpdate.Visible = false;
            btnUpdate.Enabled = false;
            lblError.Visible = false;
            lblIdVenta.Visible = false;
            txtIdProducto.Focus();
        }


        private void LlenarListaPaginas()
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Select  count(*) from propuesta");
                IDataReader reader = con.getSalida();
                double totalPersonas = 0;
                if (reader.Read())
                {
                    totalPersonas = Convert.ToDouble(reader["count(*)"]);
                }

                lbltotalcount.Text = totalPersonas.ToString();
                paginas = totalPersonas / filasPorPagina;
                paginas = Math.Ceiling(paginas);

                CurrentPageLabel.Text = "Página " + paginaActual + " de " +
                    paginas + ".";

                paginaDropDown.Items.Clear();
                if (paginas != 0)
                {
                    for (int i = 0; i < paginas; i++)
                    {
                        paginaDropDown.Items.Add((i + 1).ToString());
                    }
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

        protected void VerificarProducto(TextBox entrada, Label lblDatos, TextBox pTxtPrecio)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Select * from producto where id ='" + entrada.Text + "'");
                IDataReader reader = con.getSalida();

                if (reader.Read())
                {
                    lblDatos.ForeColor = System.Drawing.Color.Green;
                    lblDatos.Text += Convert.ToString(reader["nombre"]) + "<br />";
                    if (pTxtPrecio.Text != "")
                    {
                        pTxtPrecio.Text = Convert.ToString(Convert.ToInt32(pTxtPrecio.Text) + Convert.ToInt32(reader["precio"]));
                    }
                    else
                    {
                        pTxtPrecio.Text = Convert.ToString(reader["precio"]);
                    }

                    lblIdsProductos.Text += entrada.Text + "-";
                    lblNoProducto.Text = "";

                }
                else
                {
                    lblNoProducto.ForeColor = System.Drawing.Color.Red;
                    lblNoProducto.Text = "No se encontro el producto";
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

        protected void CambioID(object sender, EventArgs e)
        {
            VerificarProducto(txtIdProducto, lblNombreProducto, txtPrecio);
            CalcularPrecioFinal(txtPrecio,txtDescuento,lblPrecioFinal);
        }



        //----------------Precio/descuento/comision
        public void EstaEnRango(TextBox txtEntrada, double rangoMayor)
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

        public void CalcularPrecioFinal(TextBox pTxtPrecio, TextBox pTxtDescuento, Label pLabelPrecioFinal)
        {
            if (pTxtPrecio.Text != "" && pTxtDescuento.Text != "")
            {
                double precioFinal = Convert.ToDouble(pTxtPrecio.Text);
                double descuento = Convert.ToDouble(pTxtDescuento.Text);

                descuento = 100 - descuento;

                precioFinal *= descuento / 100;

                pLabelPrecioFinal.Text = precioFinal.ToString();
            }

        }

        protected void CambioComision(object sender, EventArgs e)
        {
            EstaEnRango(txtComision, 100);
        }

        protected void CambioDescuento(object sender, EventArgs e)
        {
            EstaEnRango(txtDescuento, 100);
            CalcularPrecioFinal(txtPrecio, txtDescuento, lblPrecioFinal);
        }

        //---------Tabla/Grid-----------------
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

        protected string revisarDatosLLenos(TextBox pTxtIdProducto,TextBox pDatetimepicker, TextBox pTxtPrecio,
            TextBox pTxtDescuento, TextBox pTxtComision, TextBox pTxtRespuesta, Label pLblCliente,
            RadioButton pRbAprovado, RadioButton pRbRechazado, Label pLblError)
        {
            error = "";
            pLblError.Text = "";
            //venta
            if (pTxtIdProducto.Text.Trim() == "")
            {
                error += "*El campo producto no puede estar vacio.<br />";
            }
            if (pTxtIdProducto.Text.Trim().Length > 8)
            {
                error += "*El campo producto no puede tener mas de 8 caracteres.<br />";
            }
            //fecha
            if (pDatetimepicker.Text.Trim() == "")
            {
                error += "*El campo fecha no puede estar vacio.<br />";
            }

            //Precio
            if (pTxtPrecio.Text.Trim() == "")
            {
                error += "*El campo precio no puede estar vacio.<br />";
            }

            if (!IsDigitsOnly(pTxtPrecio.Text.Trim()))
            {
                error += "*El campo precio debe tener solo numeros.<br />";
            }

            //descuento
            if (pTxtDescuento.Text.Trim() == "")
            {
                error += "*El campo descuento no puede estar vacio.<br />";
            }
            //El descuento no verifica el largo, debido a que ya se hace previamente

            //comision
            if (pTxtComision.Text.Trim() == "")
            {
                error += "*El campo comisión no puede estar vacio.<br />";
            }
            //El descuento no verifica el largo, debido a que ya se hace previamente

            //respuesta
            if (pTxtRespuesta.Text.Trim() == "")
            {
                error += "*El campo respuesta no puede estar vacio.<br />";
            }
            if (pTxtRespuesta.Text.Trim().Length > 200)
            {
                error += "*El campo venta no puede tener mas de 120 caracteres.<br />";
            }

            if (pLblCliente.Text == "No se encontro el cliente" || pLblCliente.Text == "")
            {
                error += "*No se selecciono un cliente valido.<br />";
            }

            if (!pRbAprovado.Checked && !pRbRechazado.Checked)
            {
                error += "*No se selecciono un estado.<br />";
            }


            pLblError.Text = error;
            pLblError.Visible = true;
            return error;

        }

        // ---------------GridView-------------------
        private void BindGridView()
        {
            try
            {
                CargarPropuesta(paginaDropDown.SelectedIndex, GridViewEmpresa);

            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
        }

        public void CargarPropuesta(int pIndex, GridView pGrid)
        {
            System.Diagnostics.Debug.WriteLine(pIndex);
            con.Abrir();
            if (pIndex == -1)
            {
                pIndex = 0;
            }
            double limite = pIndex * filasPorPagina;
            con.cargarQuery("SELECT propuesta.Id as ID, Fecha, Descuento, Comision, Precio, Vendedor, Estado, Entidad.Nombre as Cliente, Respuesta from " +
                "propuesta inner join Entidad on Entidad.id = propuesta.idEntidad limit "
                + pIndex + "," + filasPorPagina + "");
            IDataReader reader = con.getSalida();
            DataTable table = new DataTable();
            table.Load(reader);
            pGrid.DataSource = table;
            pGrid.DataBind();
            con.Cerrar();
        }

        protected void GridViewPropuesta_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {

                string id = GridViewEmpresa.DataKeys[e.RowIndex].Value.ToString();
                BorrarPropuesta(id);
                ShowMessage("Propuesta eliminada");
                GridViewEmpresa.EditIndex = -1;
                LlenarListaPaginas();
                BindGridView();
            }
            catch (MySqlException ex)
            {
                ShowMessage(ex.Message);
            }
 

        }

        public bool BorrarPropuesta(string id)
        {
            try
            {
                con.Abrir();
                con.cargarQuery("Delete From ProductoXPropuesta where idPropuesta='" + id + "'");
                con.getSalida().Close();
                con.Cerrar();
                con.Abrir();
                con.cargarQuery("Delete From propuesta where id='" + id + "'");
                con.getSalida().Close();
                con.Cerrar();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        // ---------------Persona/Empresa-------------------
        protected void VerificarCliente(TextBox entrada, bool persona)
        {
            try
            {
                con.Abrir();
                string query = "Select id, nombre from Entidad where ";
                if (persona)
                {
                    query += "cedula = " + entrada.Text;
                }
                else
                {
                    query += "id = " + entrada.Text + " and cedula is NULL";
                }
                System.Diagnostics.Debug.WriteLine(query);
                con.cargarQuery(query);
                IDataReader reader = con.getSalida();

                if (reader.Read())
                {
                    lblIdEntidad.ForeColor = System.Drawing.Color.Green;
                    lblIdEntidad.Text = Convert.ToString(reader["id"]);

                    lblCliente.ForeColor = System.Drawing.Color.Green;
                    lblCliente.Text = Convert.ToString(reader["nombre"]);
                }
                else
                {
                    lblCliente.ForeColor = System.Drawing.Color.Red;
                    lblCliente.Text = "No se encontro el cliente";
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

        protected void VerificarPersona(object sender, EventArgs e)
        {
            txtPersona.Text = txtPersona.Text.Trim();
            VerificarCliente(txtPersona, true);
        }

        protected void VerificarEmpresa(object sender, EventArgs e)
        {
            txtEmpresa.Text = txtEmpresa.Text.Trim();
            VerificarCliente(txtEmpresa, false);
        }

        protected void MostrarEmpresa(object sender, EventArgs e)
        {
            txtPersona.Visible = false;
            txtPersona.Text = "";
            lblCliente.Text = "";
            txtEmpresa.Visible = true;
        }

        protected void MostrarPersona(object sender, EventArgs e)
        {
            txtEmpresa.Visible = false;
            txtEmpresa.Text = "";
            lblCliente.Text = "";
            txtPersona.Visible = true;
        }


        // ---------------Botones-------------------
        protected void BtnUpdate_Click(object sender, EventArgs e)
        {
            
        }

        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            clear();
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            string error = revisarDatosLLenos(txtIdProducto, datetimepicker, txtPrecio, txtDescuento,
                txtComision, txtRespuesta, lblCliente, rbAprovado, rbRechazado, lblError);
            if (error == "")
            {
                try
                {
                    string estado = "";
                    if (rbAprovado.Checked)
                    {
                        estado = "1";
                    }
                    else
                    {
                        estado = "0";
                    }

                    InsertarPropuesta(txtIdProducto.Text, datetimepicker.Text, txtPrecio.Text,
                        txtDescuento.Text, txtComision.Text, lblIdEntidad.Text, txtEmpresa.Text,
                        lblVendedor.Text, txtRespuesta.Text, lblIdsProductos, estado);
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
        }

        public bool InsertarPropuesta(string pTxtIDProducto, string pFecha, string pPrecio, string pDescuento,
            string pComision, string idCliente, string pEmpresa, string pVendedor, string pRespuesta, Label idProductos, string estado)
        {
            try
            {



                con.Abrir();
                string query = "INSERT INTO propuesta (fecha, precio, descuento, comision, idEntidad, vendedor, estado, respuesta) " +
                    "VALUES ('" + pFecha.Trim() + "', '" + pPrecio.Trim() + "', '" + pDescuento.Trim() + "', '" + pComision.Trim() +
                    "', " + idCliente + ", '" + pVendedor.Trim() + "', " + estado + ", '"  + pRespuesta.Trim() + "');";
                System.Diagnostics.Debug.WriteLine(query);
                con.cargarQuery(query);

                con.getSalida().Close();

                long latestId = con.LatestId();

                string queryProductos = "INSERT INTO ProductoXPropuesta (idProducto, idPropuesta) VALUES ";

                System.Diagnostics.Debug.WriteLine(idProductos.Text);

                idProductos.Text = idProductos.Text.Remove(idProductos.Text.Length - 1);

                string[] productos = idProductos.Text.Split('-');

                foreach (string idProducto in productos)
                {
                    System.Diagnostics.Debug.WriteLine(idProducto);
                    queryProductos += "(" + idProducto + ", " + latestId.ToString() + "),";
                }

                queryProductos = queryProductos.Remove(queryProductos.Length - 1);
                queryProductos += ";";

                System.Diagnostics.Debug.WriteLine(queryProductos);

                con.cargarQuery(queryProductos);

                con.getSalida().Close();

                con.Cerrar();
                return true;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }
    }
}