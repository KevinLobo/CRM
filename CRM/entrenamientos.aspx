<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="entrenamientos.aspx.cs" Inherits="CRM.entrenamientos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Entrenamientos</title>
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
            <div class="container col-xs-12 col-sm-6 col-md-6 col-lg-3">

                <div class="form-group ">
                    <label for="ID">Cliente:</label>
                    <asp:Label ID="lblCliente" runat="server" ></asp:Label>
                </div>

                <div class="form-group ">
                    <label for="fechaInicio">Rango Inicial de Fecha y Hora:</label>
                    <asp:TextBox ID="fechaInicio" runat="server" class="form-control"></asp:TextBox>
                </div>

                <div class="form-group ">
                    <label for="fechaFinal">Rango Final de Fecha y Hora:</label>
                    <asp:TextBox ID="fechaFinal" runat="server" class="form-control"></asp:TextBox>
                </div>

                <div class="form-group ">
                    <asp:CheckBox ID="chkbxFecha" runat="server"
                        Text="Cualquier Fecha" AutoPostBack="true" autocomplete="off"></asp:CheckBox>
                </div>

                <div class="form-group ">
                    <label for="evento">Nombre de Entrenamiento:</label>
                    <asp:TextBox ID="txtNombreEvento" runat="server" class="form-control"
                        AutoPostBack="true" autocomplete="off"></asp:TextBox>
                </div>

                <div class="form-group ">
                    <asp:CheckBox ID="chkbxAsistido" runat="server"
                        Text="Estoy Suscrito" AutoPostBack="true" autocomplete="off"></asp:CheckBox>
                </div>

                <div class="form-group ">
                    <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                </div>

                <div class="form-group ">
                    <asp:Button ID="btnSubmit" runat="server" Text="Buscar" CssClass="btn-success" OnClick="BtnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" CssClass="btn-danger" OnClick="BtnCancel_Click" />
                </div>
                
            </div>

            <div class="container col-xs-12 col-sm-12 col-md-12 col-lg-9">
                <div class="form-group ">

                    <asp:GridView ID="GridViewEmpresa" runat="server" DataKeyNames="Id"
                        OnSelectedIndexChanged="GridViewEmpresa_SelectedIndexChanged"
                        CssClass="table table-bordered bs-table">
                        <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#ffffcc" />

                        <Columns>
                            <asp:CommandField HeaderText="Suscribirse/Desuscribirse" ShowSelectButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>
    <%-- Inicializa calendario --%>
    <script type="text/javascript">
        $(function () {
            $('#fechaInicio').datetimepicker();
            $('#fechaFinal').datetimepicker();
        });
    </script> 
</body>
</html>
