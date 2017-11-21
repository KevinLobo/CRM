<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logIn.aspx.cs" Inherits="CRM.Default" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Flat Login Form 3.0</title>

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/meyer-reset/2.0/reset.min.css" />
    <link rel='stylesheet prefetch' href='https://fonts.googleapis.com/css?family=Roboto:400,100,300,500,700,900|RobotoDraft:400,100,300,500,700,900' />
    <link rel='stylesheet prefetch' href='https://maxcdn.bootstrapcdn.com/font-awesome/4.3.0/css/font-awesome.min.css' />

    <link rel="stylesheet" href="css/logIn.css" />


</head>
<body>
    <form id="form1" runat="server">

        <!-- Form Mixin-->
        <!-- Input Mixin-->
        <!-- Button Mixin-->
        <!-- Pen Title-->
        <div class="pen-title">
            <h1>Iniciar Sesión</h1>
        </div>
        <!-- Form Module-->
        <div class="module form-module">
            <div class="toggle">
                <i class="fa fa-times fa-pencil"></i>
            </div>
            <div class="form">
                <h2>Cuenta</h2>

                <asp:TextBox type="text" placeholder="Usuario" ID="userName" runat="server" ValidationGroup="One"></asp:TextBox>
                <asp:TextBox type="password" placeholder="Contraseña" ID="password" runat="server" ValidationGroup="One"></asp:TextBox>
                <asp:Button ID="logIn" runat="server" Text="LogIn" OnClick="logIn_Click " ValidationGroup="One"></asp:Button>
                <asp:Label ID="mensaje" runat="server" Text="" ></asp:Label>

            </div>
            <div class="form">
                <h2>Create an account</h2>
                <asp:Label ID="pore" runat="server" Text="Persona o Empresa: "></asp:Label>
                <asp:DropDownList ID="personaDropdown" runat="server" ValidationGroup="Two"/>
                <asp:TextBox type="text" placeholder="Usuario" ID="txtUsuarioRegistro" runat="server" ValidationGroup="Two"></asp:TextBox>
                <asp:TextBox type="password" placeholder="Contraseña" ID="txtPasswordRegistro" runat="server" ValidationGroup="Two"></asp:TextBox>
                <asp:TextBox type="password" placeholder="Confirmar contraseña" ID="txtPasswordRegistroConfirmar" runat="server" ValidationGroup="Two"></asp:TextBox>
                <asp:Button ID="Button1" runat="server" Text="Registro" OnClick="Register_Click " ValidationGroup="Two"></asp:Button>
                <asp:Label ID="mensajeRegistro" runat="server" Text="" ></asp:Label>
            </div>
        </div>


    </form>
    <script src='http://cdnjs.cloudflare.com/ajax/libs/jquery/2.1.3/jquery.min.js'></script>
    <script src="js/logIn.js"></script>
</body>
</html>
