<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="icontable.aspx.cs" Inherits="BlockAndPass.AdminWeb.icontable" %>

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
        <script src="Buttons-1.2.4/css/buttons.dataTables.min.css"></script>
        <link href="jquery-ui-1.12.1/jquery-ui.css" rel="stylesheet">
        <link href="bootstrap-select-1.12.1/dist/css/bootstrap-select.min.css" rel="stylesheet">  
        <link href="dual-list2/bootstrap-duallistbox.css" type="text/css" rel="stylesheet"> 
        <link href="jquery.simple-dtpicker/jquery.simple-dtpicker.css" rel="stylesheet" />

        <%-- JS --%>
        <%--<script src="js/jquery-3.1.1.js" type="text/javascript"></script>--%>
        <script src="assets/js/jquery-1.10.2.js" type="text/javascript"></script>
        <script src="DataTables-1.10.13/media/js/jquery.dataTables.min.js"></script>
        <script src="DataTables-1.10.13/extensions/Select/js/dataTables.select.min.js"></script>
        <script src="DataTables-1.10.13/extensions/Buttons/js/dataTables.buttons.min.js"></script>
        <script src="Buttons-1.2.4/js/buttons.flash.min.js"></script>
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
            /*#mytable10{
                border:1px solid blue;
            }*/
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
	            ObtenerMAC();
	            ObtenerDatosUsuario();
	            ConsultarSedes();
	            ConsultarEstacionamientos(0);


	            var dt = new Date();
	            var month = parseInt(dt.getMonth() + 1);
	            $('*[name=date]').appendDtpicker({
	                "locale": "es",
	                "minuteInterval": 15,
	                "current": dt.getDate() + "-" + month + "-" + dt.getFullYear() + " " + "00:00",
	                "closeOnSelected": true,
	                "todayButton": false,
	            });
	            $('*[name=date]').change(function () {

	            });
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
	                        if (jQuery.inArray($(this).attr("id"), msg.d)>=0) {
	                            $(this).show();
	                        } else {
	                            $(this).hide();
	                        }
	                    }
	                });
	            //});


	            if (jQuery.inArray('navReportesOperacion', msg.d) >= 0) {

	            } else {
	                if (jQuery.inArray('navReportesNegocio', msg.d) >= 0) {
	                    window.location.replace("negocio.aspx");
	                } else {
	                    if (jQuery.inArray('navReportesFuncionales', msg.d) >= 0) {
	                        window.location.replace("funcionales.aspx");
	                    } else {
	                        if (jQuery.inArray('navReportesLogs', msg.d) >= 0) {
	                            window.location.replace("logs.aspx");
	                        } else {
	                            if (jQuery.inArray('navReportesAuditorias', msg.d) >= 0) {
	                                window.location.replace("auditorias.aspx");
	                            } else {
	                                if (jQuery.inArray('navReportesAlarmas', msg.d) >= 0) {
	                                    window.location.replace("alarmas.aspx");
	                                } else {
	                                    if (jQuery.inArray('navReportesComparativo', msg.d) >= 0) {
	                                        window.location.replace("comparativosedes.aspx");
	                                    } else {
	                                        window.location.replace("template.aspx");
	                                    }
	                                }
	                            }
	                        }
	                    }
	                }
	            }
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
	                url: "DataService.asmx/ObtenerIdCajeroxMAC?mac="+mac,
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
	                        $('#encabezadoUsuario').html("¡Bienvenido "+obj.Resultado[0].Usuario + "!");
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
	        function ConsultarEstacionamientos(idSede) {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaEstacionamientos?idSede=" + idSede,
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#cbEstacionamiento").get(0).options.length = 0;

	                    $.each(msg.d, function (index, item) {
	                        $("#cbEstacionamiento").get(0).options[$("#cbEstacionamiento").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#cbEstacionamiento").get(0).options.length = 0;
	                    alert("Failed to load estacionamientos");
	                }

	            });
	        }
	        function ConsultarSedes() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaSedes",
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#cbSedes").get(0).options.length = 0;

	                    $("#cbSedes").get(0).options[$("#cbSedes").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#cbSedes").get(0).options[$("#cbSedes").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#cbSedes").get(0).options.length = 0;
	                    alert("Failed to load sedes");
	                }

	            });
	        }
	        function cambioComboSedes() {
	            ConsultarEstacionamientos($("#cbSedes").val());
	        }
	        function btnConsultar_click() {
	            var month = parseInt($("*[name=date]").handleDtpicker('getDate').getMonth() + 1);
	            var date = $('*[name=date]').handleDtpicker('getDate').getDate() + '-' + month + '-' + $("*[name=date]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date]").handleDtpicker('getDate').getMinutes();
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ConsultarInfoInterfazContable?idEstacionamiento="+$("#cbEstacionamiento").val()+"&fecha=" + date,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    //alert(obj.Resultado);
	                    if (obj.Exito == true) {
	                        var myJsonString = JSON.stringify(obj.Resultado);
	                        //var myJsonString2 = jQuery.parseJSON(obj.Resultado);
	                        //alert(myJsonString);
	                        //alert(myJsonString2);


	                        var response = $.parseJSON(myJsonString);

	                        //var tbl = $("<table/>")
	                        //alert(tbl);
	                        $("#div1").append("<table id=\"mytable10\" class=\"table table-bordered table-hover\">"
                                + "<thead>"
                                + "<tr>"
                                //+ "<th>Empresa</th>"
                                //+ "<th>Documento</th>"
                                //+ "<th>Numero</th>"
                                //+ "<th>Fecha</th>"
                                //+ "<th>Item</th>"
                                + "<th>Concept</th>"
                                //+ "<th>Tercero</th>"
                                //+ "<th>TerceroConsecutivo</th>"
                                + "<th>CuentaLocal</th>"
                                //+ "<th>Proyecto</th>"
                                + "<th>Naturaleza</th>"
                                //+ "<th>Banco</th>"
                                //+ "<th>CuentaBancaria</th>"
                                //+ "<th>Centro</th>"
                                + "<th>Valor</th>"
                                + "<th>Referencia</th>"
                                + "</tr>"
                                + "</thead>"
                                + "<table/>");
	                        
	                        $("#mytable10").append("<tbody>");
	                        $.each(response, function (i, item) {
	                            var tr = "<tr>";
	                            //var td1 = "<td>" + item.empresa + "</td>";
	                            //var td2 = "<td>" + item.documento + "</td>";
	                            //var td3 = "<td>" + item.numero + "</td>";
	                            //var td4 = "<td>" + item.fecha + "</td>";
	                            //var td5 = "<td>" + item.item + "</td>";
	                            var td6 = "<td>" + item.concepto + "</td>";
	                            //var td7 = "<td>" + item.tercero + "</td>";
	                            //var td8 = "<td>" + item.terceroconsecutivo + "</td>";
	                            var td9 = "<td>" + item.cuentalocal + "</td>";
	                            //var td10 = "<td>" + item.proyecto + "</td>";
	                            var td11 = "<td>" + item.naturaleza + "</td>";
	                            //var td12 = "<td>" + item.banco + "</td>";
	                            //var td13 = "<td>" + item.cuentabancaria + "</td>";
	                            //var td14 = "<td>" + item.centro + "</td>";
	                            var td15 = "<td>" + item.valor + "</td>";
	                            var td16 = "<td>" + item.referencia + "</td></tr>";

	                            $("#mytable10").append(tr + td6 + td9 + td11  + td15 + td16 + "</tbody>");
	                        });
	                        $("#mytable10").append("</tbody>");

	                    } else {
	                        alert("Failed to load info Interfaz actual: " + obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    alert("Failed to load info Interfaz actual");
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
                        <li class="active" id="navReportes">
                            <a>
                                <i class="ti-bar-chart"></i>
                                <p>Reportes</p>
                            </a>
                        </li>
                        <li id="navPPM">
                            <a  href="ppm.aspx">
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
                                        <li><a href="#">Ver Todas</a></li>--%>
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
                                                <li id="rnavConfiguracion-sistema"><a href="tarjetas.aspx">Inventario</a></li>
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
                                    <li id="navReportesOperacion"><a href="#">Operacion</a></li>
                                    <li id="navReportesNegocio"><a href="negocio.aspx">Negocio</a></li>
                                    <li id="navReportesFuncionales"><a href="funcionales.aspx">Funcionales</a></li>
                                    <li id="navReportesLogs"><a href="logs.aspx">Logs</a></li>
                                    <li id="navReportesAuditorias"><a href="auditorias.aspx">Auditorias</a></li>
                                    <li id="navReportesAlarmas"><a href="repAlarmas.aspx">Alarmas</a></li>
                                    <li id="navReportesComparativo"><a href="comparativosedes.aspx">Comparativo Sedes</a></li>
                                    <li id="navReportesIContable" class="active"><a href="icontable.aspx">I. Contable</a></li>
                                </ul>
                            </div>
                        </nav>
                        <div class="card">
                            <div class="header">
                                <h4 class="title">Interfaz Contable</h4>
                            </div>
                            <div class="content">
                                <div class="row">
                                    <div class="col-md-4">
                                        <div class="form-group">
                                                <label>Sedes</label>
                                                <select class="form-control border-input" id="cbSedes" onchange="cambioComboSedes();">
                                                </select>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                                <label>Estacionamiento</label>
                                                <select class="form-control border-input" id="cbEstacionamiento">
                                                </select>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <label>Fecha</label>
                                            <input class="form-control border-input" type="text" name="date" value="">
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-8">
                                    </div>
                                    <div class="col-md-4">
                                        <div class="card" onclick="btnConsultar_click()">
                                            <div class="content">
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <div class="icon-big icon-warning">
                                                            <i class="ti-search"></i>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <div class="numbers">
                                                            <p></p>
                                                            Consultar
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="content table-responsive table-full-width" id="div1">  

                                        </div>
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
                                <button type="button" id="Button1" class="btn btn-default" onclick="btnGuardarModalCambioClave_Click();">
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
