<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reporteErrores.aspx.cs" Inherits="CRM.reporteErrores" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Reporte de Errores</title>
    <link rel="stylesheet" href="/Content/bootstrap.css" />
    <link rel="stylesheet" href="/Content/bootstrap-datetimepicker.css" />

  <script type="text/javascript" src="/scripts/jquery.min.js"></script>
  <script type="text/javascript" src="/scripts/moment.js"></script>
  <script type="text/javascript" src="/scripts/bootstrap.min.js"></script>


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

               <div class="form-group ">
                    <label for="dropdownVentas">Venta por Reportar:</label>
                    <asp:DropDownList ID="ventaDropdown" AutoPostBack="true"
                        runat="server"
                        CssClass="form-control" />
                </div>


                
                <div class="form-group ">
                    <label for="txtReporte">Reporte:</label>
                    <p>Por favor ingrese su reporte de error de la manera más detallada posible. Ingrese el nombre de los productos con los cuales tiene problema.</p>
                    <asp:TextBox ID="txtReporte" runat="server" placeholder="Reporte" class="form-control"
                        TextMode="multiline" Rows="3"></asp:TextBox>
                    
                </div>


                <div class="form-group ">
                    <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                </div>

                <div class="form-group ">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn-success" OnClick="BtnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-danger" OnClick="BtnCancel_Click" />
                </div>
            </div>
        </div>
    </form>
<%-- Inicializa calendario --%>
    <script type="text/javascript">
        $(function () {
            $('#datetimepicker').datetimepicker();
        });
    </script>    

<%-- Revisa si el caracter insertado pertenece a un numero --%>
    <script type="text/javascript">
        function isNumberKey(evt, obj) {

            var charCode = (evt.which) ? evt.which : event.keyCode
            var value = obj.value;
            var dotcontains = value.indexOf(",") != -1;
            if (dotcontains)
                if (charCode == 44) return false;
            if (charCode == 44) return true;
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;
            return true;
        }
    </script>
</body>
</html>
