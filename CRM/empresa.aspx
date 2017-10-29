<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="empresa.aspx.cs" Inherits="CRM.empresa" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Empresa</title>
    <link rel="stylesheet" href="CSS/bootstrap.css" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="JS/bootstrap.js"></script>


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
                <ul class="nav navbar-nav">
                    <li><a href="principal.aspx">Principal</a></li>
                    <li class="dropdown active"><a class="dropdown-toggle" data-toggle="dropdown" href="#">Contactos
          <span class="caret"></span></a>
                        <ul class="dropdown-menu">
                            <li><a href="persona.aspx">Personas</a></li>
                            <li class="active"><a href="empresa.aspx">Empresas</a></li>
                        </ul>
                    </li>
                    <li class="dropdown"><a class="dropdown-toggle" data-toggle="dropdown" href="#">Ventas
                        <span class="caret"></span></a>
                        <ul class="dropdown-menu">
                            <li><a href="producto.aspx">Producto</a></li>
                            <li><a href="ventas.aspx">Registro Ventas</a></li>
                            <li><a>Propuestas</a></li>
                        </ul>
                    </li>
                </ul>
                <ul class="nav navbar-nav navbar-right">
                    <li>
                        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="LinkButton1_Click">
           Cerrar Sesión
                        </asp:LinkButton>
                    </li>
                </ul>
            </div>
        </nav>

        <div class="container">
            <div class="container col-xs-12 col-sm-6 col-md-6 col-lg-3">

                <div class="form-group ">
                    <label for="ID">ID:</label>
                    <asp:Label ID="lblId" runat="server" Text="" Visible="false"></asp:Label>
                </div>
                <div class="form-group ">
                    <label for="Nombre">Nombre:</label>
                    <asp:TextBox ID="txtNombre" runat="server" class="form-control" placeholder="Nombre"></asp:TextBox>
                </div>
                <div class="form-group ">
                    <label for="direccion">Dirección:</label>
                    <asp:TextBox ID="txtDireccion" runat="server" placeholder="Direción" class="form-control"
                        TextMode="multiline" Rows="3"></asp:TextBox>
                </div>

                <div class="form-group ">
                    <label for="telefono">Teléfono:</label>
                    <asp:TextBox ID="txtTelefono" runat="server" placeholder="Teléfono" class="form-control"></asp:TextBox>
                </div>
                <div class="form-group ">
                    <asp:Label ID="lblError" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                </div>

                <div class="form-group ">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn-success" OnClick="BtnSubmit_Click" />
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" Enabled="false" CssClass="btn-primary"
                        OnClick="BtnUpdate_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn-danger" OnClick="BtnCancel_Click" />
                </div>
            </div>

            <div class="container col-xs-12 col-sm-12 col-md-12 col-lg-8">
                <div class="form-group ">
                    <h3>
                        <span style="float: left;">
                            <asp:Label ID="lblInfo" runat="server" /></span>
                        <span><small>Total empresas:</small>
                            <asp:Label ID="lbltotalcount" runat="server" CssClass="label label-warning" /></span>
                    </h3>

                    <asp:GridView ID="GridViewEmpresa" runat="server" DataKeyNames="id"
                        OnSelectedIndexChanged="GridViewEmpresa_SelectedIndexChanged"
                        OnRowDeleting="GridViewEmpresa_RowDeleting"
                        CssClass="table table-bordered bs-table">
                        <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#ffffcc" />

                        <Columns>
                            <asp:CommandField HeaderText="Actualizar" ShowSelectButton="True" />
                            <asp:CommandField HeaderText="Eliminar" ShowDeleteButton="True" />
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="form-group ">
                    <div class="row" style="margin-top: 20px;">
                        <div class="col-lg-1" style="text-align: left;">
                            <h5>
                                <asp:Label ID="MessageLabel" Text="Ir a la pág." runat="server" />
                            </h5>
                        </div>
                        <div class="col-lg-1" style="text-align: left;">
                            <asp:DropDownList ID="paginaDropDown" Width="60px" AutoPostBack="true"
                                OnSelectedIndexChanged="CambioPagina"
                                runat="server"
                                CssClass="form-control" />

                        </div>
                        <div class="col-lg-10" style="text-align: right;">
                            <h3>
                                <asp:Label ID="CurrentPageLabel" runat="server" CssClass="label label-warning" />
                            </h3>
                        </div>
                    </div>
                </div>
            </div>
    </form>
</body>
</html>
