<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="BlockAndPass.AdminWeb.login" %>

<!DOCTYPE html>

<html lang="en">
    <head>
	    <meta charset="utf-8" />
	    <link rel="icon" type="image/png" sizes="64x64" href="assets/img/faviconparquearse.ico">
	    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

	    <title>Block&Pass</title>

	    <meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0' name='viewport' />
        <meta name="viewport" content="width=device-width" />

        <link rel="stylesheet" href="assets/css/bootstrap.min.css"/>
        <link href="weblibs/jquery-ui.css" rel="stylesheet"> 
        <link rel="stylesheet" type="text/css" href="login/style.css" />
       

        <script src="weblibs/jquery.js" type="text/javascript"></script>
        <script src="assets/js/bootstrap.min.js" type="text/javascript"></script>
        <script src="weblibs/jquery.validate.min.js" type="text/javascript"></script>


        <script>
            $(document).ready(function () {
                var paramPass = getUrlParameter('invalidpassword');
                var paramUser = getUrlParameter('nofinduser');
                //alert(paramPass + " - " + paramUser);
                if (paramPass == 'true') {
                    confirmaCambios(false, 'Contraseña Invalida.');
                } else if (paramUser == 'true') {
                    confirmaCambios(false, 'Usuario no existe.');
                }
            });
            var getUrlParameter = function getUrlParameter(sParam) {
                var sPageURL = decodeURIComponent(window.location.search.substring(1)),
                    sURLVariables = sPageURL.split('&'),
                    sParameterName,
                    i;

                for (i = 0; i < sURLVariables.length; i++) {
                    sParameterName = sURLVariables[i].split('=');

                    if (sParameterName[0] === sParam) {
                        return sParameterName[1] === undefined ? true : sParameterName[1];
                    }
                }
            };
            function confirmaCambios(resultado, mensaje) {
                if (resultado == true) {
                    $('.modal-title-confirmacion').html('<span class="icon-warning ti-thumb-up"></span> Exito');
                    $('#resModalConfirmaCambios').html('Se ha realizado la operacion con exito.');
                } else {
                    $('.modal-title-confirmacion').html('<span class="icon-danger ti-thumb-down"></span> Error');
                    $('#resModalConfirmaCambios').html(mensaje);
                }

                $('#myModalConfirmaCambios').modal('show');
            }
            function btnCerrarConfrimacion_click() {
                $('#myModalConfirmaCambios').modal('toggle');
                window.location.href = "login.aspx";
            }
            function validateForm() {
                var x = $('#emailcamp').val();
                var y = $('#passcamp').val();
                //alert(x);
                if (x == "") {
                    confirmaCambios(false, 'Campo Usuario requerido.');
                    return false;
                }else if(y==""){
                    confirmaCambios(false, 'Campo Clave requerido.');
                    return false;
                }
            }
        </script>
    </head>
    <body>
        <div class="container">
			<header>
                <div class="logo">
                    <a class="simple-text">
                        <img src="assets/img/logoparking.jpg" class="img-rounded" width="150" height="60">
                    </a>
                </div>
            </header>
			<section class="main">
				<form class="form-1" name="myForm" onsubmit="return validateForm()">
					<p class="field">
						<input type="text" name="login" placeholder="Usuario" id="emailcamp" runat="server">
						<i class="icon-user icon-large"></i>
					</p>
						<p class="field">
							<input type="password" name="password" placeholder="Clave" id="passcamp" runat="server">
							<i class="icon-lock icon-large"></i>
					</p>
					<p class="submit">
						<button type="submit" name="submit"><i class="icon-arrow-right icon-large"></i></button>
					</p>
				</form>
			</section>
        </div>
    </body>

    <div id="myModalConfirmaCambios" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title-confirmacion"></h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-12" id="resModalConfirmaCambios">
                                
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="Button5" class="btn btn-default" onclick="btnCerrarConfrimacion_click();">
                                    <span class="ti-close"></span>Cerrar
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</html>
