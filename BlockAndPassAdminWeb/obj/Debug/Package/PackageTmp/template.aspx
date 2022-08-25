<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="template.aspx.cs" Inherits="BlockAndPass.AdminWeb.template" %>

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
        <link href="chartist-js/chartist.min.css" rel="stylesheet">

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
        <script src="chartist-js/chartist.min.js"></script>
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
            .ct-series-a .ct-line {
                stroke: #f00;
            }
            .ct-series-b .ct-line {
                stroke: #fffa00;
            }
            .ct-series-c .ct-line {
                stroke: #ff6a00;
            }
            .ct-series-a .ct-point {
              /* Size of your points */
              stroke-width: 3px;
              /* Make your points appear as squares */
            }
            .ct-series-b .ct-point {
              /* Size of your points */
              stroke-width: 3px;
              /* Make your points appear as squares */
            }
            .ct-series-c .ct-point {
              /* Size of your points */
              stroke-width: 3px;
              /* Make your points appear as squares */
            }
            #icon1table1 {
                color:#f00
            }
            #icon2table1 {
                color:#fffa00
            }
            #icon3table1 {
                color:#ff6a00
            }
            #icon1table2 {
                color:#f00
            }
            #icon2table2 {
                color:#fffa00
            }
            #icon3table2 {
                color:#ff6a00
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
	            ConsultarSedes();
	            ConsultarEstacionamientos(0);
	            ObtenerMAC();
	            ObtenerDatosUsuario();
	            ObtenerDatosIngresosPP();
	            ObtenerDatosIngresosDineroPP();
	            ObtenerDatosCuposDisponibles();
	            ObtenerDatosRecaudoDia();
	            ObtenerDatosAlarmasDia();
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
	            $("li").each(function () {
	                    if ($(this).attr("id") != undefined) {
	                        if (jQuery.inArray($(this).attr("id"), msg.d)>=0) {
	                            $(this).show();
	                        } else {
	                            $(this).hide();
	                        }
	                    }
	            });

	            $("div.card").each(function () {
	                if ($(this).attr("id") != undefined) {
	                    if (jQuery.inArray($(this).attr("id"), msg.d) >= 0) {
	                        $(this).show();
	                    } else {
	                        $(this).hide();
	                    }
	                }
	            });
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
	        function btnLeer_click() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerValorParametroxNombre?nombre=claveTarjeta&idEstacionamiento=" + $("#cbEstacionamiento").val(),
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = msg.d;
	                    //alert(obj);
	                    if (obj != '') {
	                        $.ajax({
	                            type: "GET",
	                            url: "http://localhost:8080/ReaderLocalService.svc/reader/getcardinfo?password=" + obj,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                tarjetaLeida(msg);
	                            },
	                            error: function () {
	                                confirmaCambios(false, 'Error al consumir servicio LOCAL.');
	                            }

	                        });
	                    } else {
	                        confirmaCambios(false, 'No encuentra Parametro claveTarjeta.');
	                    }
	                }, error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function tarjetaLeida(infoTarjeta) {
	            $("#txtIdTarjeta").val(infoTarjeta.idCard);
	            $("#txtTipo").val(infoTarjeta.tipoTarjeta);
	            $("#txtPlaca").val(infoTarjeta.placa);

	            $("#txtFechaEntrada").val(infoTarjeta.fechEntrada);
	            $("#txtModuloEntrada").val(infoTarjeta.moduloEntrada);
	            $("#txtTipoV").val(infoTarjeta.tipoVehiculo);

	            $("#txtFechaPago").val(infoTarjeta.fechaPago);
	            $("#txtModuloPago").val(infoTarjeta.moduloPago);
	            $("#txtConvenio1").val(infoTarjeta.codeAutorizacion1);
	            $("#txtConvenio2").val(infoTarjeta.codeAutorizacion2);
	            $("#txtConvenio3").val(infoTarjeta.codeAutorizacion3);
	            //$("#txtConvenio4").val(infoTarjeta.tipoVehiculo);

	            //alert(infoTarjeta.cicloActivo);
	            if (infoTarjeta.cicloActivo == true) {
	                $("#chbCicli").addClass('checked');
	            } else {
	                $("#chbCicli").removeClass('checked');
	            }
	            if (infoTarjeta.reposicion == true) {
	                $("#chbRepo").addClass('checked');
	            } else {
	                $("#chbRepo").removeClass('checked');
	            }
	            if (infoTarjeta.cortesia == true) {
	                $("#chbCortesia").addClass('checked');
	            } else {
	                $("#chbCortesia").removeClass('checked');
	            }
	            if (infoTarjeta.valet == true) {
	                $("#chbValet").addClass('checked');
	            } else {
	                $("#chbValet").removeClass('checked');
	            }
	        
	        }
	        function cambioComboSedes() {
	            ConsultarEstacionamientos($("#cbSedes").val());
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

	                    //$("#cbEstacionamiento").get(0).options[$("#cbEstacionamiento").get(0).options.length] = new Option('Todos', 0);
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
	        function ObtenerDatosIngresosPP() {
	            var hoy = [];
	            var ayer = [];
	            var anteayer = [];
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerDatosInformeIngresosPP",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    

	                    var months = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                                    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];

	                    var TodayDate = new Date();
	                    TodayDate.setDate(TodayDate.getDate() - 2);
	                    var m = months[TodayDate.getMonth()];
	                    var d = TodayDate.getDate();

	                    $('#tablaUnoFechaAntier').html('<i class="ti-bar-chart-alt ti-1x" id="icon1table1"></i>&ensp;'+m+ ' ' +d);

	                    if (obj.Exito == true) {
	                        new Chartist.Line('#graficaOcupacion', {
	                            labels: ['7AM', '8AM', '9AM', '10AM', '11AM', '12PM', '1PM', '2PM', '3PM', '4PM', '5PM', '6PM', '7PM', '8PM', '9PM', '10PM'],
	                            series: [obj.Resultado.Anteayer, obj.Resultado.Ayer, obj.Resultado.Hoy]
	                        }, {
	                            fullWidth: true,
	                            chartPadding: {
	                                right: 40
	                            }
	                        });
	                    } else {
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }

	                },
	                error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function ObtenerDatosIngresosDineroPP() {
	            var hoy = [];
	            var ayer = [];
	            var anteayer = [];
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerDatosInformeIngresosDineroPP",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);


	                    var months = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                                    "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];

	                    var TodayDate = new Date();
	                    TodayDate.setDate(TodayDate.getDate() - 2);
	                    var m = months[TodayDate.getMonth()];
	                    var d = TodayDate.getDate();

	                    $('#tablaDosFechaAntier').html('<i class="ti-bar-chart-alt ti-1x" id="icon1table2"></i>&ensp;' + m + ' ' + d);

	                    if (obj.Exito == true) {
	                        new Chartist.Line('#graficaIngresos', {
	                            labels: ['7AM', '8AM', '9AM', '10AM', '11AM', '12PM', '1PM', '2PM', '3PM', '4PM', '5PM', '6PM', '7PM', '8PM', '9PM', '10PM'],
	                            series: [obj.Resultado.Anteayer, obj.Resultado.Ayer, obj.Resultado.Hoy]
	                        }, {
	                            fullWidth: true,
	                            chartPadding: {
	                                right: 40
	                            }
	                        });
	                    } else {
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }

	                },
	                error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function ObtenerDatosCuposDisponibles() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerDatosInformeCuposDisponibles",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        $('#cuposDisponibles').html('<p>Cupos disponibles</p>' + obj.Resultado);
	                    } else {
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
                });
	        }
	        function ObtenerDatosRecaudoDia() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerDatosInformeRecaudoDia",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        $('#recaudoDiario').html('<p>Ingresos diarios</p>$' + numberToCurrency(obj.Resultado));
	                    } else {
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function numberToCurrency(amount) {

	            var thousandsSeparator = ","
	            var currencyNum = "";
	            var amountString = amount.toString();
	            var digits = amountString.split("");

	            var countDigits = digits.length;
	            var revDigits = digits.reverse();

	            for (var i = 0; i < countDigits; i++) {
	                if ((i % 3 == 0) && (i != 0)) {
	                    currencyNum += thousandsSeparator + revDigits[i];
	                } else {
	                    currencyNum += digits[i];
	                }
	            };

	            var revCurrency = currencyNum.split("").reverse().join("");

	            var finalCurrency = revCurrency;

	            return finalCurrency;
	        }
	        function ObtenerDatosAlarmasDia() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerDatosAlarmasDia",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        $('#erroresDiarios').html('<p>Errores diarios</p>' + obj.Resultado);
	                    } else {
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio');
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
                        <li class="active" id="navPrincipal">
                            <a>
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
                        <div class="row">
                            <div class="col-md-4">
                                <div class="card" id="cardCuposDisponibles">
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <div class="icon-big icon-warning text-center">
                                                    <i class="ti-car"></i>
                                                </div>
                                            </div>
                                            <div class="col-xs-7">
                                                <div id="cuposDisponibles">
                                                    <%--<p>Cupos disponibles</p>
                                                    1000--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="footer">
                                            <hr />
                                            <div class="stats">
                                                <i class="ti-reload"></i> Recien actualizado
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="card" id="cardRecaudoDiario">
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <div class="icon-big icon-success text-center">
                                                    <i class="ti-wallet"></i>
                                                </div>
                                            </div>
                                            <div class="col-xs-7">
                                                <div id="recaudoDiario">
                                                    <%--<p>Ingresos diarios</p>
                                                    $1.345.000--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="footer">
                                            <hr />
                                            <div class="stats">
                                                <i class="ti-reload"></i> Recien actualizado
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="card" id="cardErroresDiarios">
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-xs-5">
                                                <div class="icon-big icon-danger text-center">
                                                    <i class="ti-pulse"></i>
                                                </div>
                                            </div>
                                            <div class="col-xs-7">
                                                <div id="erroresDiarios">
                                                    <%--<p>Errors</p>
                                                    23--%>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="footer">
                                            <hr />
                                            <div class="stats">
                                                <i class="ti-reload"></i> Recien actualizado
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="card" id="cardIngresosVehiculosTabla">
                                    <div class="header">
                                        <h4 class="title">Ingresos Vehiculos</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="ct-chart" id="graficaOcupacion"></div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div id="tablaUnoFechaAntier" class="col-md-4">
                                                  <%--<i class="ti-bar-chart-alt ti-1x" id="icon1table1"></i>&ensp;Febrero 4--%>
                                            </div>
                                            <div class="col-md-4">
                                                <i class="ti-bar-chart-alt ti-1x" id="icon2table1"></i>&ensp;Ayer
                                            </div>
                                            <div class="col-md-4">
                                                <i class="ti-bar-chart-alt ti-1x" id="icon3table1"></i>&ensp;Hoy
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="card" id="cardIngresosDineroTabla">
                                    <div class="header">
                                        <h4 class="title">Ingreso ultimos 3 dias</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="ct-chart" id="graficaIngresos"></div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div id="tablaDosFechaAntier" class="col-md-4">
                                                  <%--<i class="ti-bar-chart-alt ti-1x" id="icon1table1"></i>&ensp;Febrero 4--%>
                                            </div>
                                            <div class="col-md-4">
                                                <i class="ti-bar-chart-alt ti-1x" id="icon2table2"></i>&ensp;Ayer
                                            </div>
                                            <div class="col-md-4">
                                                <i class="ti-bar-chart-alt ti-1x" id="icon3table2"></i>&ensp;Hoy
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="card">
                                    <div class="header">
                                        <h4 class="title">Datos Tarjeta</h4>
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
                                                    <label>Estacionamientos</label>
                                                    <select class="form-control border-input" id="cbEstacionamiento">
                                                    </select>
                                            </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <div class="pull-right">
                                                        <label></label>
                                                        <button type="button" id="btnConsultar" class="btn btn-default" onclick="btnLeer_click();">
                                                            <span class="icon-warning ti-credit-card"></span> Leer
                                                        </button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>IdTarjeta</label>
                                                    <input class="form-control border-input" type="text" id="txtIdTarjeta" disabled>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>TipoTarjeta</label>
                                                    <input class="form-control border-input" type="text" id="txtTipo" disabled>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Placa</label>
                                                    <input class="form-control border-input" type="text" id="txtPlaca" disabled>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Fecha Hora Entrada</label>
                                                    <input class="form-control border-input" type="text" id="txtFechaEntrada" disabled>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Modulo Entrada</label>
                                                    <input class="form-control border-input" type="text" id="txtModuloEntrada" disabled>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Tipo Vehiculo</label>
                                                    <input class="form-control border-input" type="text" id="txtTipoV" disabled>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label>Fecha Hora Pago</label>
                                                    <input class="form-control border-input" type="text" id="txtFechaPago" disabled>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label>Modulo Pago</label>
                                                    <input class="form-control border-input" type="text" id="txtModuloPago" disabled>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>Convenio 1</label>
                                                    <input class="form-control border-input" type="text" id="txtConvenio1" disabled>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>Convenio 2</label>
                                                    <input class="form-control border-input" type="text" id="txtConvenio2" disabled>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <label>Convenio 3</label>
                                                    <input class="form-control border-input" type="text" id="txtConvenio3" disabled>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="checkbox" id="chbCicli">
                                                        <label><input type="checkbox" disabled>Ciclo Activo</label>
                                                    </div>
                                                    <div class="checkbox" id="chbRepo">
                                                        <label><input type="checkbox" disabled>Reposicion</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="checkbox" id="chbCortesia">
                                                        <label><input type="checkbox" disabled>Cortesia</label>
                                                    </div>
                                                    <div class="checkbox" id="chbValet">
                                                        <label><input type="checkbox" disabled>Valet</label>
                                                    </div>
                                                </div>
                                            </div>
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
