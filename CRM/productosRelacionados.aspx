<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="productosRelacionados.aspx.cs" Inherits="CRM.productosRelacionados" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Productos Relacionados</title>
    <link rel="stylesheet" href="/Content/bootstrap.css" />
    <link rel="stylesheet" href="/Content/bootstrap-datetimepicker.css" />

  <script type="text/javascript" src="/scripts/jquery.min.js"></script>
  <script type="text/javascript" src="/scripts/moment.js"></script>
  <script type="text/javascript" src="/scripts/bootstrap.min.js"></script>
    <script type="text/javascript" src="/scripts/bootstrap-datetimepicker.js"></script>


    <style type="text/css">
        .auto-style1 {
            width: 131px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <nav class="navbar navbar-inverse">
            <div class="container-fluid">
                <div class="navbar-header">
                    <a class="navbar-brand" href="#">CRM</a>
                </div>
                <% if (Session["cliente"].ToString() == "True")
            {%>
            <!-- #include file="navbarCliente.html" -->
        <%}
                else
                {%>
        <!-- #include file="navbar.htm" -->
                <%}%>
                <ul class="nav navbar-nav navbar-right">
                    <li>
                        <asp:LinkButton ID="LinkButton2" runat="server" OnClick="LinkButton1_Click">
           Cerrar Sesión
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>
        </nav>
         

<%-- Contenido --%>

        <div class="container">
            <div class="container col-xs-12 col-sm-12 col-md-12 col-lg-12">

            <div class="form-group ">
                <label for="ID">Cliente:</label>
                <asp:Label ID="lblCliente" runat="server" ></asp:Label>
            </div>

            <div class="container col-xs-12 col-sm-12 col-md-12 col-lg-12">
                <div class="form-group ">
                    <h3>
                        <span>Con base en sus compras talvez le interesen los siguientes productos:</span>
                    </h3>

                    <asp:GridView ID="GridViewEmpresa" runat="server"
                        OnSelectedIndexChanged="GridViewEmpresa_SelectedIndexChanged"
                        CssClass="table table-bordered bs-table">
                        <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#ffffcc" />

                    </asp:GridView>
                </div>

                <div class="form-group">
                    <p>Todos los precios de nuestros productos son en dólares estadounidenses ($).</p>
                </div>

                <div class="form-group">
                    <p>Puede <a href="puntosContacto.aspx">contactar</a> a alguno de nuestros vendedores para cotizar un producto.</p>
                </div>
            </div>
        </div>
            </div>
    </form>
</body>
</html>
