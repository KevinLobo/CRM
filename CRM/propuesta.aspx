﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="propuesta.aspx.cs" Inherits="CRM.propuesta" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Propuesta</title>
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
                <ul class="nav navbar-nav">
                    <li><a href="principal.aspx">Principal</a></li>
                    <li class="dropdown"><a class="dropdown-toggle" data-toggle="dropdown" href="#">Contactos
          <span class="caret"></span></a>
                        <ul class="dropdown-menu">
                            <li><a href="persona.aspx">Personas</a></li>
                            <li><a href="empresa.aspx">Empresas</a></li>
                        </ul>
                    </li>
                    <li class="dropdown active"><a class="dropdown-toggle" data-toggle="dropdown" href="#">Ventas
                        <span class="caret"></span></a>
                        <ul class="dropdown-menu">
                            <li><a href="producto.aspx">Producto</a></li>
                            <li><a href="ventas.aspx">Registro Ventas</a></li>
                            <li class="active"><a href="propuesta.aspx">Propuestas</a></li>
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

<%-- Contenido --%>

        <div class="container">
            <div class="container col-xs-12 col-sm-6 col-md-6 col-lg-3">

                <div class="form-group ">
                    <label>ID:</label>
                    <asp:Label ID="lblIdVenta" runat="server" Visible="false" ></asp:Label>
                </div>
                <div class="form-group ">
                    <label for="ID">Vendedor:</label>
                    <asp:Label ID="lblVendedor" runat="server" ></asp:Label>
                </div>
                <div class="form-group ">
                    <label for="Producto">Producto:</label>
                    <asp:TextBox ID="txtIdProducto" runat="server" class="form-control"
                        onkeypress="return isNumberKey(event,this)" placeholder="ID producto"
                        AutoPostBack="true" OnTextChanged="CambioID" autocomplete="off"></asp:TextBox>
                    <asp:Label ID="lblNombreProducto" runat="server" ></asp:Label>
                </div>

                <div class="form-group ">
                    <label for="fecha">Fecha:</label>
                    <asp:TextBox ID="datetimepicker" runat="server" class="form-control"></asp:TextBox>
                </div>


                <div class="form-group ">
                    <label  style="width:100%;">Cliente:</label>
                        <asp:RadioButton id="rbPersona" GroupName="personaOempresa" Text="Persona"
                             OnCheckedChanged="MostrarPersona" AutoPostBack="True" runat="server" />
                        <asp:RadioButton id="rbEmpresa" GroupName="personaOempresa" Text="Empresa"
                              OnCheckedChanged="MostrarEmpresa" AutoPostBack="True" runat ="server"/>
                    <asp:TextBox ID="txtPersona" runat="server" class="form-control"
                        AutoPostBack="true" OnTextChanged="VerificarPersona" 
                        autocomplete="off" Visible ="false" placeholder="Cédula"></asp:TextBox>
                    <asp:TextBox ID="txtEmpresa" runat="server" class="form-control"
                        AutoPostBack="true" OnTextChanged="VerificarEmpresa"
                        autocomplete="off" Visible ="false" placeholder="Empresa ID"></asp:TextBox>
                    <asp:Label ID="lblCliente" runat="server" ></asp:Label>


                </div>
                <div class="form-group ">
                    <label>Precio:</label>
                    <asp:TextBox ID="txtPrecio" runat="server" class="form-control"
                        Enabled ="false"></asp:TextBox>

                </div>

                <div class="form-group ">
                        <label for="Descuento">Descuento:</label>
                        <div class="form-group ">
                        <div class="col-sm-5" style="text-align: left;">
                            <asp:TextBox ID="txtDescuento" runat="server" class="form-control"
                            onkeypress="return isNumberKey(event,this)" AutoPostBack="true" 
                            OnTextChanged="CambioDescuento" autocomplete="off"></asp:TextBox>
                        </div>
                           <label>%</label>
                    </div>
                </div>

                <div class="form-group ">
                        <label for="comision">Comisión:</label>
                        <div class="form-group ">
                        <div class="col-sm-5" style="text-align: left;">
                            <asp:TextBox ID="txtComision" runat="server" class="form-control"
                            onkeypress="return isNumberKey(event,this)" AutoPostBack="true" 
                            OnTextChanged="CambioComision" autocomplete="off"></asp:TextBox>
                        </div>
                           <label>%</label>
                    </div>
                </div>
                
                <div class="form-group ">
                    <label>Precio final:</label>
                    <asp:Label ID="lblPrecioFinal" runat="server" ></asp:Label>
                </div>

                <div class="form-group ">
                    <label>Estado:</label>
                        <asp:RadioButton id="rbAprovado" GroupName="estadoRespuesta" Text="Aprobado"
                             AutoPostBack="True" runat="server" />
                        <asp:RadioButton id="rbRechazado" GroupName="estadoRespuesta" Text="Rechazado"
                              AutoPostBack="True" runat ="server"/>
                </div>
                <div class="form-group ">
                    <label for="Respuesta">Respuesta:</label>
                    <asp:TextBox ID="txtRespuesta" runat="server" placeholder="Respuesta" class="form-control"
                        TextMode="multiline" Rows="3"></asp:TextBox>
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
                        <span><small>Total ventas:</small>
                            <asp:Label ID="lbltotalcount" runat="server" CssClass="label label-warning" /></span>
                    </h3>

                    <asp:GridView ID="GridViewEmpresa" runat="server" DataKeyNames="id"
                        OnSelectedIndexChanged="GridViewEmpresa_SelectedIndexChanged"
                        OnRowDeleting ="GridViewPropuesta_RowDeleting"
                        CssClass="table table-bordered bs-table">
                        <HeaderStyle BackColor="#337ab7" Font-Bold="True" ForeColor="White" />
                        <EditRowStyle BackColor="#ffffcc" />
                        <Columns>
                            <asp:CommandField HeaderText="Seleccionar" ShowSelectButton="True" />
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
