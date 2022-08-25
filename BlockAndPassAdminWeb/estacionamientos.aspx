<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="estacionamientos.aspx.cs" Inherits="BlockAndPass.AdminWeb.estacionamientos" %>

<!DOCTYPE html>

<html lang="en">
    <head>
	<meta charset="utf-8" />
	<link rel="icon" type="image/png" sizes="64x64" href="assets/img/faviconparquearse.ico">
	<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

	<title>Block&Pass</title>

	<meta content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=0' name='viewport' />
    <meta name="viewport" content="width=device-width" />

    <%-- CSS --%>
        <link href="assets/css/bootstrap.min.css" rel="stylesheet"/>
        <link href="assets/css/animate.min.css" rel="stylesheet" />
        <link href="assets/css/paper-dashboard.css" rel="stylesheet" />
        <link href="assets/css/demo.css" rel="stylesheet" />
        <link href="assets/css/themify-icons.css" rel="stylesheet">
        <link href="font-awesome-4.7.0/css/font-awesome.min.css" rel="stylesheet">
        <link href='css/css.css?family=Muli:400,300' rel='stylesheet' type='text/css'>
        <link href="DataTables-1.10.13/media/css/jquery.dataTables.min.css" rel="stylesheet">
        <link href="DataTables-1.10.13/extensions/Select/css/select.dataTables.min.css" rel="stylesheet">
        <link href="DataTables-1.10.13/extensions/Buttons/css/buttons.dataTables.min.css" rel="stylesheet">
        <link href="jquery-ui-1.12.1/jquery-ui.css" rel="stylesheet">
        <link href="bootstrap-select-1.12.1/dist/css/bootstrap-select.min.css" rel="stylesheet">  
        <link href="dual-list2/bootstrap-duallistbox.css" type="text/css" rel="stylesheet"> 
        <link href="jquery.simple-dtpicker/jquery.simple-dtpicker.css" rel="stylesheet" />

        <%-- JS --%>
        <script src="assets/js/jquery-1.10.2.js" type="text/javascript"></script>
        <script src="DataTables-1.10.13/media/js/jquery.dataTables.min.js"></script>
        <script src="DataTables-1.10.13/extensions/Select/js/dataTables.select.min.js"></script>
        <script src="DataTables-1.10.13/extensions/Buttons/js/dataTables.buttons.min.js"></script>
        <script src="bootstrap-select-1.12.1/dist/js/bootstrap-select.min.js"></script>
        <script src="dual-list2/jquery.bootstrap-duallistbox.js"></script>
        <script src="jquery.simple-dtpicker/jquery.simple-dtpicker.js" type="text/javascript"></script>
        <script src="jquery-treeview/logger.js"></script>
        <script src="jquery-treeview/treeview.js"></script> 
        <script src="weblibs/currency.js"></script>
        <script src="assets/js/jquery-jtemplates.js" type="text/javascript"></script>
        <script src="assets/js/bootstrap.min.js" type="text/javascript"></script>
        <script src="assets/js/bootstrap-checkbox-radio.js"></script>
        <script src="assets/js/chartist.min.js"></script>
        <script src="assets/js/bootstrap-notify.js"></script>
        <script src="assets/js/paper-dashboard.js"></script>
        <script src="assets/js/demo.js"></script>

        <%-- Style --%>
        <style>
            #myNav {
                z-index:1;
                position:relative;
            }
        </style>

        <%-- Code --%>     
	    <script type="text/javascript">
	        $(function () {
	            'use strict';
	            setInterval(function () {
	                ConsultarAlarmas();
	            }, 300000);
	        });
	        $(document).ready(function () {
	            ConsultarAlarmas();
	            getSecurity();
	            consultarestacionamiento();
	            CargarSedes();
	            ObtenerMAC();
	            ObtenerDatosUsuario();
	        });    
	        function ConsultarAlarmas() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaAlarmasCategoria",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    fillAlarmas(msg);
	                    //alert('exito');
	                },
	                error: function () {
	                    alert("Failed to load alarmas");
	                }

	            });
	        }
	        function fillAlarmas(msg) {
	            $('#ddtNotifications').empty();
	            $('#ddmNotifications').empty();
	            $('#ddtNotifications').append('<i class="ti-bell"></i> <p class="notification">' + msg.d[0].Display + '</p> <p>Alarmas</p> <b class="caret"></b>');
	            $('#ddmNotifications').append('<li class="alert-success"><a href="#"> ' + msg.d[1].Display + ' Alarmas Nivel 1</a></li> <li class="alert-warning"><a href="#">' + msg.d[2].Display + ' Alarmas Nivel 2</a></li> <li class="alert-danger"><a href="#">' + msg.d[3].Display + ' Alarmas Nivel 3</a></li> <li class="divider"></li> <li><a href="alarmas.aspx">Ver Todas</a></li>');
	        }
	        function consultarestacionamiento() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/GetItemsEstacionamientos",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    fillestacionamientos(msg);
	                    //alert('exito');
	                },
	                error: function () {
	                    alert("Failed to load detalle estacionamientos");
	                }
	            });
	        }
	        function fillestacionamientos(msg) {
	            $("#Container2").setTemplateElement("Template-Items2").processTemplate(msg);
	            $('#sampleTable').dataTable({
	                "language": {
	                    "search": "Buscar:",
	                    "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
	                },
	                "columnDefs": [{
	                    "orderable": false,
	                    "className": 'select-checkbox',
	                    "targets": [0],
	                },
	                {
	                    "targets": [1, 6, 9, 13, 3, 10, 11, 12],
	                    "visible": false,
	                    "searchable": false
	                }],
	                "select": {
	                    "style": 'os',
	                    "selector": 'td:first-child'
	                },
	                "scrollY": 200,
	                "scrollX": true,
	                "scrollCollapse": true,
	                "sorting": [[2, "asc"]],
	                "paging": false,
	                "lengthChange": false,
	                "searching": true,
	                "ordering": true,
	                "info": true,
	                "autoWidth": false
	            });
	        }
	        function btnEditar_click() {
	            var table = $('#sampleTable').DataTable();
	            if (table.rows({ selected: true }).count() > 0) {
	                var count = table.rows({ selected: true }).data();
	                //alert(count[0][1]);
	                $('#idEstacionamientoModal').val(count[0][1]);
	                $('#nombreEstacionamientoModal').val(count[0][2]);
	            
	                $('#direccionModal').val(count[0][3]);
	                $('#telefonoModal').val(count[0][4]);
	                $('#personaModal').val(count[0][5]);

	                //$('#ciudadModal').val(count[0][6]);
	                //$('#departamentoModal').val(count[0][7]);
	                $('#emailModal').val(count[0][6]);

	                $('#fechacModal').val(count[0][7]);
	                $('#fechavModal').val(count[0][8]);
	                $('#paquetecModal').val(count[0][9]);
	                $('#numeroModal').val(count[0][10]);
	                $('#ejecutivoModal').val(count[0][11]);
	                $('#idSedeModal').val(count[0][12]);
	            

	                if (count[0][13] == 'true') {
	                    $("#activoModal").addClass('checked');
	                } else {
	                    $("#activoModal").removeClass('checked');
	                }
	                $('.modal-title').html("Editar");
	                $('#myModal').modal('show');
	            }
	        }
	        function CargarSedes() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaSedesCombos",
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#idSedeModal").get(0).options.length = 0;

	                    //$("#cbEstacionamiento").get(0).options[$("#cbEstacionamiento").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#idSedeModal").get(0).options[$("#idSedeModal").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#idSedeModal").get(0).options.length = 0;
	                    alert("Failed to load sedes");
	                }

	            });
	        }
	        function btnModalGuardar_click() {
	            if ($("#idEstacionamientoModal").val() == '') {
	                var activo = $('#activoModal').hasClass("checked");
	                //alert(activo);
	                $.ajax({
	                    type: "GET",
	                    url: "DataService.asmx/CrearEstacionamiento?nombre=" + $("#nombreEstacionamientoModal").val() + "&direccion=" + $("#direccionModal").val() + "&telefono=" + $("#telefonoModal").val() + "&persona=" + $("#personaModal").val() + "&ciudad=" + $("#ciudadModal").val() + "&departamento=" + $("#departamentoModal").val() + "&email=" + $("#emailModal").val() + "&fechac=" + $("#fechacModal").val() + "&fechav=" + $("#fechavModal").val() + "&paquete=" + $("#paquetecModal").val() + "&numero=" + $("#numeroModal").val() + "&ejecutivo=" + $("#ejecutivoModal").val() + "&idSede=" + $("#idSedeModal").val() + "&estado=" + activo,
	                    data: "",
	                    contentType: "application/json",
	                    dataType: "json",
	                    success: function (msg) {
	                        var obj = JSON.parse(msg.d);
	                        if (obj.Exito == true) {
	                            $('#myModal').modal('toggle');
	                            confirmaCambios(true, '');
	                        } else {
	                            $('#myModal').modal('toggle');
	                            confirmaCambios(false, obj.ErrorMessage);
	                        }
	                    },
	                    error: function () {
	                        $('#myModalCambioClave').modal('toggle');
	                        confirmaCambios(false, 'Error al consumir servicio');
	                    }
	                });
	            } else {
	                var activo = $('#activoModal').hasClass("checked");
	                //alert(activo);
	                $.ajax({
	                    type: "GET",
	                    url: "DataService.asmx/ActualizarEstacionamiento?idEstacionamiento=" + $("#idEstacionamientoModal").val() + "&nombre=" + $("#nombreEstacionamientoModal").val() + "&direccion=" + $("#direccionModal").val() + "&telefono=" + $("#telefonoModal").val() + "&persona=" + $("#personaModal").val() + "&ciudad=" + $("#ciudadModal").val() + "&departamento=" + $("#departamentoModal").val() + "&email=" + $("#emailModal").val() + "&fechac=" + $("#fechacModal").val() + "&fechav=" + $("#fechavModal").val() + "&paquete=" + $("#paquetecModal").val() + "&numero=" + $("#numeroModal").val() + "&ejecutivo=" + $("#ejecutivoModal").val() + "&idSede=" + $("#idSedeModal").val() + "&estado=" + activo,
	                    data: "",
	                    contentType: "application/json",
	                    dataType: "json",
	                    success: function (msg) {
	                        var obj = JSON.parse(msg.d);
	                        if (obj.Exito == true) {
	                            $('#myModal').modal('toggle');
	                            confirmaCambios(true, '');
	                        } else {
	                            $('#myModal').modal('toggle');
	                            confirmaCambios(false, obj.ErrorMessage);
	                        }
	                    },
	                    error: function () {
	                        $('#myModalCambioClave').modal('toggle');
	                        confirmaCambios(false, 'Error al consumir servicio');
	                    }
	                });
	            }
	        }
	        function btnCrear_click() {
	            $('#idEstacionamientoModal').val('');
	            $('#nombreEstacionamientoModal').val('');
	            $('#direccionModal').val('');
	            $('#telefonoModal').val('');
	            $('#personaModal').val('');
	            //$('#ciudadModal').val('');
	            //$('#departamentoModal').val('');
	            $('#emailModal').val('');
	            $('#fechacModal').val('');
	            $('#fechavModal').val('');
	            $('#paquetecModal').val('');
	            $('#numeroModal').val('');
	            $('#ejecutivoModal').val('');
	            //$('#idSedeModal').val('');
	            $("#activoModal").removeClass('checked');

	            $('.modal-title').html("Crear");
	            $('#myModal').modal('show');
	        }
	        function getSecurity() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerPermisos?user=1",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    setSecurity(msg);
	                },
	                error: function () {
	                    alert("Failed to load security");
	                }

	            });
	        }
	        function setSecurity(msg) {
	            //$.each(msg.d, function (index, item) {
	            $("li").each(function () {
	                if ($(this).attr("id") != undefined) {
	                    if (jQuery.inArray($(this).attr("id"), msg.d) >= 0) {
	                        $(this).show();
	                    } else {
	                        $(this).hide();
	                    }
	                }
	            });
	            //});
	        }
	        function cambioClave_click() {
	            $('#idClaveDespuesModal').val('');
	            $('#idClaveAntesModal').val('');
	            $('#myModalCambioClave').modal('show');
	        }
	        function btnGuardarModalCambioClave_Click() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/UpdateClaveUsuario?claveNueva=" + $("#idClaveDespuesModal").val() + "&claveVieja=" + $("#idClaveAntesModal").val(),
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        $('#myModalCambioClave').modal('toggle');
	                        window.location.href = "logout.aspx";
	                    } else {
	                        $('#myModalCambioClave').modal('toggle');
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    $('#myModalCambioClave').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
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
	            $('#sampleTable').dataTable().fnDestroy();
	            consultarestacionamiento();
	            $('#myModalConfirmaCambios').modal('toggle');
	        }
	        function ObtenerMAC() {
	            $.ajax({
	                type: "GET",
	                url: "http://localhost:8080/ReaderLocalService.svc/computer/getlocalmacaddress",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    if (msg == "") {
	                        HablitarPPM(false);
	                    } else {
	                        //Verificar si pertenece a ppm registrado
	                        ObtenerPPM(msg);
	                    }
	                },
	                error: function () {
	                    HablitarPPM(false);
	                }

	            });
	        }
	        function ObtenerPPM(mac) {
	            //alert(mac);
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerIdCajeroxMAC?mac=" + mac,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        //alert(obj.Resultado.IdModulo + " - " + obj.Resultado.IdEstacionamiento);
	                        HablitarPPM(true);
	                    } else {
	                        HablitarPPM(false);
	                    }
	                },
	                error: function () {
	                    alert("Failed to load idcajero");
	                }

	            });
	        }
	        function HablitarPPM(bHabilitar) {
	            if (bHabilitar == false) {
	                $('#navPPM').hide();
	            }
	        }
	        function ObtenerDatosUsuario() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerInformacionUsuario",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    //alert(obj.Resultado);
	                    if (obj.Exito == true) {
	                        //alert(obj.Resultado[0].Documento + " - " + obj.Resultado[0].Nombres + " - " + obj.Resultado[0].Usuario + " - " + obj.Resultado[0].Cargo);
	                        $('#encabezadoUsuario').html("¡Bienvenido " + obj.Resultado[0].Usuario + "!");
	                        $('#datosUsuario').html(obj.Resultado[0].Documento + " - " + obj.Resultado[0].Nombres + " - " + obj.Resultado[0].Cargo);
	                    } else {
	                        alert("Failed to load info Usuario actual: " + obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    alert("Failed to load info Usuario actual");
	                }

	            });
	        }
	    </script>
</head>
    <body>
        <div class="wrapper">
            <div class="sidebar" data-background-color="white" data-active-color="danger">

            <!--
		        Tip 1: you can change the color of the sidebar's background using: data-background-color="white | black"
		        Tip 2: you can change the color of the active button using the data-active-color="primary | info | success | warning | danger"
	        -->

    	        <div class="sidebar-wrapper">
                    <div class="logo">
                        <a class="simple-text">
                            <img src="assets/img/logoparking.jpg" class="img-rounded" width="150" height="60">
                        </a>
                    </div>
                    <ul class="nav">
                        <li id="navPrincipal">
                            <a href="template.aspx">
                                <i class="ti-panel"></i>
                                <p>Principal</p>
                            </a>
                        </li>
                        <li id="navTransacciones">
                            <a href="transacciones.aspx">
                                <i class="ti-book"></i>
                                <p>Transacciones</p>
                            </a>
                        </li>
                        <li id="navCargas">
                            <a href="cargas.aspx">
                                <i class="ti-ticket"></i>
                                <p>Cargas</p>
                            </a>
                        </li>
                        <li id="navBorrarTarjetas">
                            <a href="borrarinfotarget.aspx">
                                <i class="ti-tablet"></i>
                                <p>Tarjetas Info</p>
                            </a>
                        </li>
                        <li id="navArqueos">
                            <a href="arqueos.aspx">
                                <i class="ti-bag"></i>
                                <p>Arqueos</p>
                            </a>
                        </li>
                        <li id="navAutorizados">
                            <a href="autorizados.aspx">
                                <i class="ti-unlock"></i>
                                <p>Autorizados</p>
                            </a>
                        </li>
                        <li id="navReportes">
                            <a href="reportes2.aspx">
                                <i class="ti-bar-chart"></i>
                                <p>Reportes</p>
                            </a>
                        </li>
                        <li id="navPPM">
                            <a href="ppm.aspx">
                                <i class="ti-money"></i>
                                <p>Punto de Pago</p>
                            </a>
                        </li>
                        <li id="navFacturasManuales">
                            <a href="facturasmanuales.aspx">
                                <i class="ti-pencil-alt"></i>
                                <p>F. Manual</p>
                            </a>
                        </li>
                        <li id="navConsignaciones">
                            <a href="consignaciones.aspx">
                                <i class="ti-shortcode"></i>
                                <p>Consignaciones</p>
                            </a>
                        </li>
                        <li id="navApertura">
                            <a href="apertura.aspx">
                                <i class="ti-upload"></i>
                                <p>Apertura</p>
                            </a>
                        </li>
                    </ul>
    	        </div>
            </div>

            <div class="main-panel">
                <nav class="navbar navbar-default">
                    <div class="container-fluid">
                        <div class="navbar-header">
                            <div class="row">
                                <div class="col-md-12">
                                    <a class="navbar-brand" id="encabezadoUsuario"></a>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12" id="datosUsuario">

                                </div>
                            </div>
                        </div>
                        <div class="collapse navbar-collapse">
                            <ul class="nav navbar-nav navbar-right">
                                <li class="dropdown" id="rnavAlarmas">
                                      <a href="#" class="dropdown-toggle" data-toggle="dropdown" id="ddtNotifications">
                                            <%--<i class="ti-bell"></i>--%>
                                            <%--<p class="notification">2</p>--%>
									        <%--<p>Alarmas</p>--%>
									        <%--<b class="caret"></b>--%>
                                      </a>
                                      <ul class="dropdown-menu" id="ddmNotifications">
                                        <%--<li><a href="#">KYT dañado</a></li>
                                        <li><a href="#">Billete atascado</a></li>
                                        <li class="divider"></li>
                                        <li><a href="alarmas.aspx">Ver Todas</a></li>--%>
                                      </ul>
                                </li>
						        <li class="dropdown" id="rnavConfiguracion">
                                      <a href="#" class="dropdown-toggle" data-toggle="dropdown">
                                            <i class="ti-settings"></i>
									        <p>Configuracion</p>
									        <b class="caret"></b>
                                      </a>
                                      <ul class="dropdown-menu">
                                        <li>
                                            <a href="#">
                                                <i class="ti-star"></i>
                                                Sistema
                                            </a>
                                            <ul>
                                                <li id="rnavConfiguracion-sistema"><a href="inventario.aspx">Inventario</a></li>
                                                <li id="rnavConfiguracion-usuarios"><a href="usuarios.aspx">Usuarios</a></li>
                                                <li id="rnavConfiguracion-tarifas"><a href="tarifas.aspx">Tarifas</a></li>
                                                <li id="rnavConfiguracion-convenios"><a href="convenios.aspx">Convenios</a></li>
                                                <li id="rnavConfiguracion-eventos"><a href="eventos.aspx">Eventos</a></li>
                                                <li id="rnavConfiguracion-parametros"><a href="parametros.aspx">Parametros</a></li>
                                                <li id="rnavConfiguracion-facturas"><a href="facturas.aspx">Facturas</a></li>
                                            </ul>
                                        </li>
                                        <li class="divider"></li>
                                        <li><a onclick="cambioClave_click();">Cambiar Clave</a></li>
                                        <li><a href="logout.aspx">Cerrar Sesion</a></li>
                                      </ul>
                                </li>
                            </ul>
                        </div>
                    </div>
                </nav>

                <div class="content">
                    <div class="container-fluid">
                        <nav class="navbar navbar-default" id="myNav">
                            <div class="collapse navbar-collapse">
                                <ul class="nav navbar-nav nav-tabs-justified">
                                    <li><a href="inventario.aspx">Sedes</a></li>
                                    <li class="active"><a href="#">Estacionamientos</a></li>
                                    <li><a href="modulos.aspx">Modulos</a></li>
                                    <li><a href="
                                        
                                        ">Tarjetas</a></li>
                                </ul>
                            </div>
                        </nav>
                        <div class="card">
                            <div class="header">
                                <h4 class="title">Estacionamientos</h4>
                            </div>
                            <div class="content">
                                <div class="row">
                                    <div class="col-md-12">
                                        <!-- Templates -->
                                        <p style="display: none">
                                            <textarea id="Template-Items2" rows="0" cols="0"><!--
                                                 <div class="content table-responsive table-full-width">  
                                                    <table id="sampleTable" class="widthFull fontsize10 displayNone">
                                                
                                                        <thead>
                                                            <tr>
                                                                <th></th>
                                                                <th>IdEstacionamiento</th>
                                                                <th>Nombre</th>
                                                                <th>Direccion</th>
                                                                <th>TelefonoContacto</th>
                                                                <th>PersonaContacto</th>
                                                                <th>Email</th>
                                                                <th>FechaContratacion</th>
                                                                <th>FechaVencimiento</th>
                                                                <th>PaqueteContratado</th>
                                                                <th>NumeroContrato</th>
                                                                <th>EjecutivoCuenta</th>
                                                                <th>IdSede</th>
                                                                <th>Estado</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                        {#foreach $T.d as post}
                                                        <tr>
                                                            <td></td>
                                                            <td>{$T.post.IdEstacionamiento}</td>
                                                            <td>{$T.post.Nombre}</td>
                                                            <td>{$T.post.Direccion}</td>
                                                            <td>{$T.post.TelefonoContacto}</td>
                                                            <td>{$T.post.PersonaContacto}</td>
                                                            <td>{$T.post.Email}</td>
                                                            <td>{$T.post.FechaContratacion}</td>
                                                            <td>{$T.post.FechaVencimiento}</td>
                                                            <td>{$T.post.PaqueteContratado}</td>
                                                            <td>{$T.post.NumeroContrato}</td>
                                                            <td>{$T.post.EjecutivoCuenta}</td>
                                                            <td>{$T.post.IdSede}</td>
                                                            <td>{$T.post.Estado}</td>
                                                        </tr>
                                                        {#/for}
                                                        </tbody>
                                                        <tfoot>
                                                            <tr>
                                                                <th></th>
                                                                <th>IdEstacionamiento</th>
                                                                <th>Nombre</th>
                                                                <th>Direccion</th>
                                                                <th>TelefonoContacto</th>
                                                                <th>PersonaContacto</th>
                                                                <th>Email</th>
                                                                <th>FechaContratacion</th>
                                                                <th>FechaVencimiento</th>
                                                                <th>PaqueteContratado</th>
                                                                <th>NumeroContrato</th>
                                                                <th>EjecutivoCuenta</th>
                                                                <th>IdSede</th>
                                                                <th>Estado</th>
                                                            </tr>
                                                        </tfoot>
                                                    </table>
                                                </div>
                                                    --></textarea>
                                        </p>
                                        <div id="Container2" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-8">

                                    </div>
                                    <div class="col-md-2">
                                        <button type="button" class="btn btn-default" onclick="btnCrear_click();">
                                            <span class="ti-plus"></span>Agregar
                                        </button>
                                    </div>
                                    <div class="col-md-2">
                                        <button type="button" class="btn btn-default" onclick="btnEditar_click();">
                                            <span class="ti-pencil-alt2"></span>Editar
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <footer class="footer">
                    <div class="container-fluid">
                        
                        <div class="copyright pull-right">
                            &copy; <script>document.write(new Date().getFullYear())</script>, made by Parquearse
                        </div>
                    </div>
                </footer>
            </div>
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

    <div id="myModalCambioClave" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Cambiar clave</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label>Clave anterior</label>
                                    <input type="password" class="form-control border-input" id="idClaveAntesModal">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label>Nueva clave</label>
                                    <input type="password" class="form-control border-input" id="idClaveDespuesModal">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" data-dismiss="modal" class="btn btn-default">
                                    <span class="ti-close"></span>Cerrar
                                </button>
                                <button type="button" id="Button2" class="btn btn-default" onclick="btnGuardarModalCambioClave_Click();">
                                    <span class="ti-save"></span>Guardar
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="myModal" class="modal fade">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title"></h4>
                    </div>
                    <div class="modal-body">
                        <div class="content">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="checkbox" id="activoModal">
                                      <label><input type="checkbox">Activo</label>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>Id</label>
                                        <input type="text" class="form-control border-input" disabled id="idEstacionamientoModal">
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="form-group">
                                        <label>NombreEstacionamiento</label>
                                        <input type="text" class="form-control border-input" id="nombreEstacionamientoModal">
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Direccion</label>
                                        <input type="text" class="form-control border-input" id="direccionModal">
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Telefono</label>
                                        <input type="text" class="form-control border-input" id="telefonoModal">
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Contacto</label>
                                        <input type="text" class="form-control border-input" id="personaModal">
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>FechaContrato</label>
                                        <input type="text" class="form-control border-input" id="fechacModal">
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>FechaVencimiento</label>
                                        <input type="text" class="form-control border-input" id="fechavModal">
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>PaqueteContratado</label>
                                        <input type="text" class="form-control border-input" id="paquetecModal">
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>NumeroContrato</label>
                                        <input type="text" class="form-control border-input" id="numeroModal">
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>EjecutivoCuenta</label>
                                        <input type="text" class="form-control border-input" id="ejecutivoModal">
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>IdSede</label>
                                        <select class="form-control border-input" id="idSedeModal">
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-4">
                                    <div class="form-group">
                                        <label>Email</label>
                                        <input type="text" class="form-control border-input" id="emailModal">
                                    </div>
                                </div>
                                <div class="col-md-8">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="content">
                            <div class="row">
                                <div class="col-md-12">
                                    <button type="button" data-dismiss="modal" class="btn btn-default">
                                        <span class="ti-close"></span>Cerrar
                                    </button>
                                    <button type="button" id="Button1" class="btn btn-default" onclick="btnModalGuardar_click();">
                                        <span class="ti-save"></span>Guardar
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

</html>