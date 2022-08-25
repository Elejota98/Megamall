<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reportes.aspx.cs" Inherits="BlockAndPass.AdminWeb.reportes" %>

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
	            ConsultarListaReportes();
	            ConsultarListaUsuarios();
	            ConsultarListaModulos();
	            ConsultarSedes();
	            ConsultarEstacionamientos(0);
	            ConsultarCortesias();
	            ConsultarAlarmas();
	            getSecurity();
	            ObtenerMAC();
	            ObtenerDatosUsuario();
	            ConsultarTipoV();
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
	            $('*[name=date2]').appendDtpicker({
	                "locale": "es",
	                "minuteInterval": 15,
	                "current": dt.getDate() + "-" + month + "-" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes(),
	                //"current": "2012-3-4 12:30",
	                "closeOnSelected": true,
	                "todayButton": false,
	            });
	            $('#date2').change(function () {

	            });
	            $('*[name=date3]').appendDtpicker({
	                "locale": "es",
	                "minuteInterval": 15,
	                "current": dt.getDate() + "-" + month + "-" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes(),
	                //"current": "2012-3-4 12:30",
	                "closeOnSelected": true,
	                "todayButton": false,
	            });
	            $('#date3').change(function () {

	            });
	            $('*[name=date4]').appendDtpicker({
	                "locale": "es",
	                "minuteInterval": 15,
	                "current": dt.getDate() + "-" + month + "-" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes(),
	                //"current": "2012-3-4 12:30",
	                "closeOnSelected": true,
	                "todayButton": false,
	            });
	            $('#date4').change(function () {

	            });
	            $('*[name=date5]').appendDtpicker({
	                "locale": "es",
	                "minuteInterval": 15,
	                "current": dt.getDate() + "-" + month + "-" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes(),
	                //"current": "2012-3-4 12:30",
	                "closeOnSelected": true,
	                "todayButton": false,
	            });
	            $('#date5').change(function () {

	            });
	            $('*[name=date6]').appendDtpicker({
	                "locale": "es",
	                "minuteInterval": 15,
	                "current": dt.getDate() + "-" + month + "-" + dt.getFullYear() + " " + "00:00",
	                //"current": "2012-3-4 12:30",
	                "closeOnSelected": true,
	                "todayButton": false,
	            });
	            $('#date6').change(function () {

	            });
	            $('*[name=date7]').appendDtpicker({
	                "locale": "es",
	                "minuteInterval": 15,
	                "current": dt.getDate() + "-" + month + "-" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes(),
	                //"current": "2012-3-4 12:30",
	                "closeOnSelected": true,
	                "todayButton": false,
	            });
	            $('#date7').change(function () {

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
	        function FormarReporte(infoReporte) {

	            var month = parseInt($("*[name=date]").handleDtpicker('getDate').getMonth() + 1);
	            var date = $('*[name=date]').handleDtpicker('getDate').getDate() + '-' + month + '-' + $("*[name=date]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date]").handleDtpicker('getDate').getMinutes();
	            var month2 = parseInt($("*[name=date2]").handleDtpicker('getDate').getMonth() + 1);
	            var datepost = $('*[name=date2]').handleDtpicker('getDate').getDate() + '-' + month2 + '-' + $("*[name=date2]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date2]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date2]").handleDtpicker('getDate').getMinutes();
	            var month3 = parseInt($("*[name=date3]").handleDtpicker('getDate').getMonth() + 1);
	            var date2 = $('*[name=date3]').handleDtpicker('getDate').getDate() + '-' + month3 + '-' + $("*[name=date3]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date3]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date3]").handleDtpicker('getDate').getMinutes();
	            var month4 = parseInt($("*[name=date4]").handleDtpicker('getDate').getMonth() + 1);
	            var datepost2 = $('*[name=date4]').handleDtpicker('getDate').getDate() + '-' + month4 + '-' + $("*[name=date4]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date4]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date4]").handleDtpicker('getDate').getMinutes();
	            var monthFechaSola = parseInt($("*[name=date5]").handleDtpicker('getDate').getMonth() + 1);
	            var dateFechaSola = $('*[name=date5]').handleDtpicker('getDate').getDate() + '-' + monthFechaSola + '-' + $("*[name=date5]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date5]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date5]").handleDtpicker('getDate').getMinutes();
	            var month5 = parseInt($("*[name=date6]").handleDtpicker('getDate').getMonth() + 1);
	            var date3 = $('*[name=date6]').handleDtpicker('getDate').getDate() + '-' + month5 + '-' + $("*[name=date6]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date6]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date6]").handleDtpicker('getDate').getMinutes();
	            var month6 = parseInt($("*[name=date7]").handleDtpicker('getDate').getMonth() + 1);
	            var datepost3 = $('*[name=date7]').handleDtpicker('getDate').getDate() + '-' + month6 + '-' + $("*[name=date7]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date7]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date7]").handleDtpicker('getDate').getMinutes();


	            var oldurl = "DataService.asmx/GetItemsReportes?";

	            if ($('#chbUsuario').is(":checked")) {
	                oldurl = oldurl + "&idusuario=" + $("#cbUsuarios").val();
	            }

	            if ($('#chbModulo').is(":checked")) {
	                oldurl = oldurl + "&idmodulo=" + $("#cdModulo").val();
	            }
	            
	            if ($('#chbUbicacion').is(":checked")) {
	                oldurl = oldurl + "&idsede=" + $("#cbSedes").val() + "&idestacionamiento=" + $("#cbEstacionamiento").val();
	            }

	            if ($('#chbFechaFin').is(":checked")) {
	                oldurl = oldurl + "&sFechaAntesFin=" + date + "&sFechaDespuesFin=" + datepost;
	            }

	            if ($('#chbFechaCortesia').is(":checked")) {
	                oldurl = oldurl + "&sFechaAntesCortesia=" + date2 + "&sFechaDespuesCortesia=" + datepost2;
	            }

	            if ($('#chbMotivoCortesia').is(":checked")) {
	                oldurl = oldurl + "&idMotivo=" + $("#cbCortesias").val();
	            }

	            if ($('#chbFechaSola').is(":checked")) {
	                oldurl = oldurl + "&fechaSola=" + dateFechaSola;
	            }

	            if ($('#chbTipoVehiculo').is(":checked")) {
	                oldurl = oldurl + "&tipoVehiculo=" + $("#cbTipov").val();
	            }

	            if ($('#chbFechaPago').is(":checked")) {
	                oldurl = oldurl + "&fechaPagoAntes=" + date3 + "&fechaPagoDespues=" + datepost3;
	            }

	            
	            if ($.trim($('#sampleTable').html()).length > 0) {
	                $("#sampleTable").dataTable().fnDestroy();
	            }
	            $("#sampleTable").html("");
	            

	            $('#sampleTable').append('<thead>' +
                                                '<tr id=\'encabezado\'>'+
	                                            '</tr>' +
	                                        '</thead>' +
	                                        '<tbody>' +
	                                        '</tbody>');

	            var arrColTabla = infoReporte.ColumnasTabla.split(',');
	            $.each(arrColTabla, function (index, item) {
	                $('#encabezado').append('<th>' + item + '</th>');
	            });

	            //var arrColConsulta = infoReporte.ColumnasConsulta.split(',');
	            //alert(infoReporte.ColumnasConsulta);
	            //alert(infoReporte.FromTabla);
	            //alert(infoReporte.Columna0);

	            $("#sampleTable").dataTable({
	                dom: 'Bflrtip',
	                buttons: [
                        'copyFlash',
                        'csvFlash',
                        'excelFlash',
                        'pdfFlash'
	                ],
	                "oLanguage": {
	                    "sZeroRecords": "No se encuentra infromacion",
	                    "sLoadingRecords": "Cargando...",
	                    "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ registros",
	                    "sInfoEmpty": "Mostrando 0 a 0 de 0 registros",
	                    "sLengthMenu": "Mostrando _MENU_ registros por pagina",
	                },
	                //"aaSorting": [[2, "desc"]],
	                "aLengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
	                "iDisplayLength": 10,
	                "bSortClasses": false,
	                "bStateSave": false,
	                "bPaginate": true,
	                "bAutoWidth": false,
	                "bProcessing": true,
	                "bServerSide": true,
	                "bFilter": false,
	                "bDestroy": true,
	                "sAjaxSource": oldurl,
	                //"sPaginationType": "full_numbers",
	                "bDeferRender": true,
	                "fnServerParams": function (aoData) {
	                    aoData.push({ "name": "columnas", "value": infoReporte.ColumnasConsulta },
	                                { "name": "from", "value": infoReporte.FromTabla },
	                                { "name": "where", "value": "idusuario='1'" },
	                                { "name": "colum0", "value": infoReporte.Columna0 },
                                    { "name": "groupby", "value": infoReporte.Group })
	                },
	                "fnServerData": function (sSource, aoData, fnCallback) {
	                    $.ajax({
	                        "dataType": 'json',
	                        "contentType": "application/json; charset=utf-8",
	                        "type": "GET",
	                        "url": sSource,
	                        "data": aoData,
	                        "success":
	                                    function (msg) {
	                                        var json = jQuery.parseJSON(msg.d);
	                                        //alert(json);
	                                        fnCallback(json);
	                                        $("#sampleTable").show();
	                                    }
	                    });
	                }
	            });
	        }
	        function ConsultarReporte(idReporte) {

	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ConsultarReporte?idReporte="+ idReporte,
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        FormarReporte(obj.Resultado);
	                    } else {
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function ConsultarListaReportes() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaReportes",
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#cbReporte").get(0).options.length = 0;

	                    $.each(msg.d, function (index, item) {
	                        $("#cbReporte").get(0).options[$("#cbReporte").get(0).options.length] = new Option(item.Display, item.Value);
	                    });

	                    ObtenerDescripcionReportes($("#cbReporte").val());
	                    CargarCHBFiltros();
	                },
	                error: function () {
	                    $("#cbReporte").get(0).options.length = 0;
	                    alert("Failed to load lista reportes");
	                }
	            });
	        }
	        function ObtenerDescripcionReportes(idReporte) {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerDescripcionReporte?idReporte="+idReporte,
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#comment").val(msg.d);
	                },
	                error: function () {
	                    alert("Failed to load description reporte");
	                }

	            });
	        }
	        function btnConsultar_click() {
	            ConsultarReporte($('#cbReporte').val());
	            $('#cbReporte').focus();
	        }
	        function onchangeReporte() {
	            ObtenerDescripcionReportes($('#cbReporte').val());
	            CargarCHBFiltros();
	        }
	        function CargarCHBFiltros() {

	            $('#cardUsuario').hide();
	            $('#cardModulo').hide();
	            $('#cardUbicacion').hide();
	            $('#cardFecha').hide();
	            $('#cardFechaCortesia').hide();
	            $('#cardMotivoCortesia').hide();
	            $('#cardTipoVehiculo').hide();
	            $('#cardFechaPago').hide();

	            $('#divChbUsuario').hide();
	            $('#divChbModulo').hide();
	            $('#divChbUbicacion').hide();
	            $('#divChbFechaFin').hide();
	            $('#divChbFechaCortesia').hide();
	            $('#divChbMotivoCortesia').hide();
	            $('#divChbFechaSola').hide();
	            $('#divChbTipoVehiculo').hide();
	            $('#divChbFechaPago').hide();

	            $('#divChbUsuario').removeClass('checked');
	            $('#divChbModulo').removeClass('checked');
	            $('#divChbUbicacion').removeClass('checked');
	            $('#divChbFechaFin').removeClass('checked');
	            $('#divChbFechaCortesia').removeClass('checked');
	            $('#divChbMotivoCortesia').removeClass('checked');
	            $('#divChbFechaSola').removeClass('checked');
	            $('#divChbTipoVehiculo').removeClass('checked');
	            $('#divChbFechaPago').removeClass('checked');


	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerFiltrosReporte?idReporte=" + $('#cbReporte').val(),
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $.each(msg.d, function (index, item) {
	                        $("#"+item.Display).show();
	                    });
	                },
	                error: function () {
	                    alert("Failed to load filtros reportes");
	                }

	            });
	        }
	        function onChangeCheckBoxUsuario() {
	            if ($('#chbUsuario').is(":checked")) {
	                $('#cardUsuario').show();
	            } else {
	                $('#cardUsuario').hide();
	            }
	        }
	        function onChangeCheckBoxModulo() {
	            if ($('#chbModulo').is(":checked")) {
	                $('#cardModulo').show();
	            } else {
	                $('#cardModulo').hide();
	            }
	        }
	        function onChangeCheckBoxUbicacion() {
	            if ($('#chbUbicacion').is(":checked")) {
	                $('#cardUbicacion').show();
	            } else {
	                $('#cardUbicacion').hide();
	            }
	        }
	        function onChangeCheckBoxFechaFin() {
	            if ($('#chbFechaFin').is(":checked")) {
	                $('#cardFechaFin').show();
	            } else {
	                $('#cardFechaFin').hide();
	            }
	        }
	        function onChangeCheckBoxFechaCortesia() {
	            if ($('#chbFechaCortesia').is(":checked")) {
	                $('#cardFechaCortesia').show();
	            } else {
	                $('#cardFechaCortesia').hide();
	            }
	        }
	        function onChangeCheckBoxMotivoCortesia() {
	            if ($('#chbMotivoCortesia').is(":checked")) {
	                $('#cardMotivoCortesia').show();
	            } else {
	                $('#cardMotivoCortesia').hide();
	            }
	        }
	        function onChangeCheckBoxFechaSola() {
	            if ($('#chbFechaSola').is(":checked")) {
	                $('#cardFechaSola').show();
	            } else {
	                $('#cardFechaSola').hide();
	            }
	        }
	        function ConsultarListaUsuarios() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaUsuarios",
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#cbUsuarios").get(0).options.length = 0;

	                    $("#cbUsuarios").get(0).options[$("#cbUsuarios").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#cbUsuarios").get(0).options[$("#cbUsuarios").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#cbUsuarios").get(0).options.length = 0;
	                    alert("Failed to load lista usuarios");
	                }
	            });
	        }
	        function ConsultarListaModulos() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaModulos",
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#cdModulo").get(0).options.length = 0;

	                    $("#cdModulo").get(0).options[$("#cdModulo").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#cdModulo").get(0).options[$("#cdModulo").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#cdModulo").get(0).options.length = 0;
	                    alert("Failed to load lista modulos");
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

	                    $("#cbEstacionamiento").get(0).options[$("#cbEstacionamiento").get(0).options.length] = new Option('Todos', 0);
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
	        function ConsultarCortesias() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaMotivosCortesia",
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#cbCortesias").get(0).options.length = 0;

	                    $("#cbCortesias").get(0).options[$("#cbCortesias").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#cbCortesias").get(0).options[$("#cbCortesias").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#cbCortesias").get(0).options.length = 0;
	                    alert("Failed to load cortesias");
	                }
	            });
	        }
	        function cambioComboSedes() {
	            ConsultarEstacionamientos($("#cbSedes").val());
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
	        function onChangeCheckBoxTipoVehiculo() {
	            if ($('#chbTipoVehiculo').is(":checked")) {
	                $('#cardTipoVehiculo').show();
	            } else {
	                $('#cardTipoVehiculo').hide();
	            }
	        }
	        function ConsultarTipoV() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaTipoVehiculo",
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#cbTipov").get(0).options.length = 0;

	                    $("#cbTipov").get(0).options[$("#cbTipov").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#cbTipov").get(0).options[$("#cbTipov").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#cbTipov").get(0).options.length = 0;
	                    alert("Failed to load cortesias");
	                }
	            });
	        }
	        function onChangeCheckBoxFechaPago() {
	            if ($('#chbFechaPago').is(":checked")) {
	                $('#cardFechaPago').show();
	            } else {
	                $('#cardFechaPago').hide();
	            }
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
                        <div class="row">
                            <div class="col-md-12">
                                <div class="card">
                                    <div class="header">
                                        <h4 class="title">Reporte</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                        <label>Reporte</label>
                                                        <select class="form-control border-input" id="cbReporte" onchange="onchangeReporte();">
                                                        </select>
                                                </div>
                                            </div>
                                            <div class="col-md-2"> 
                                                <div class="checkbox" id="divChbUsuario">
                                                    <label>
                                                        <input type="checkbox" id="chbUsuario" onchange="onChangeCheckBoxUsuario();">Usuario
                                                    </label>
                                                </div>
                                                <div class="checkbox" id="divChbModulo">
                                                    <label>
                                                        <input type="checkbox" id="chbModulo" onchange="onChangeCheckBoxModulo();">Modulo
                                                    </label>
                                                </div>
                                                <div class="checkbox" id="divChbUbicacion">
                                                    <label>
                                                        <input type="checkbox" id="chbUbicacion" onchange="onChangeCheckBoxUbicacion();">Ubicacion
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="col-md-2"> 
                                                <div class="checkbox" id="divChbFechaFin">
                                                    <label>
                                                        <input type="checkbox" id="chbFechaFin" onchange="onChangeCheckBoxFechaFin();">Fecha
                                                    </label>
                                                </div>
                                                <div class="checkbox" id="divChbFechaCortesia">
                                                    <label>
                                                        <input type="checkbox" id="chbFechaCortesia" onchange="onChangeCheckBoxFechaCortesia();">Dia
                                                    </label>
                                                </div>
                                                <div class="checkbox" id="divChbFechaSola">
                                                    <label>
                                                        <input type="checkbox" id="chbFechaSola" onchange="onChangeCheckBoxFechaSola();">Fecha
                                                    </label>
                                                </div>
                                                <div class="checkbox" id="divChbFechaPago">
                                                    <label>
                                                        <input type="checkbox" id="chbFechaPago" onchange="onChangeCheckBoxFechaPago();">Fecha P
                                                    </label>
                                                </div>
                                            </div>
                                            <div class="col-md-2"> 
                                                <div class="checkbox" id="divChbMotivoCortesia">
                                                    <label>
                                                        <input type="checkbox" id="chbMotivoCortesia" onchange="onChangeCheckBoxMotivoCortesia();">Motivo
                                                    </label>
                                                </div>
                                                <div class="checkbox" id="divChbTipoVehiculo">
                                                    <label>
                                                        <input type="checkbox" id="chbTipoVehiculo" onchange="onChangeCheckBoxTipoVehiculo();">Tipo V
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                        <label>Descripcion</label>
                                                        <textarea class="form-control border-input" rows="5" id="comment" readonly></textarea>
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="row">
                                            <div class="col-md-8">
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <div class="pull-right">
                                                        <label></label>
                                                        <button type="button" id="btnConsultar" class="btn btn-default" onclick="btnConsultar_click();">
                                                            <span class="icon-warning ti-search"></span> Consultar
                                                        </button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="cardUsuario" class="col-md-4" style="display: none;">
                                <div class="card">
                                    <div class="header">
                                        <h4 class="title">Usuario</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <select class="form-control border-input" id="cbUsuarios">
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="cardModulo" class="col-md-4" style="display: none;">
                                <div class="card">
                                    <div class="header">
                                        <h4 class="title">Modulo</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <select class="form-control border-input" id="cdModulo">
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="cardUbicacion" class="col-md-12" style="display: none;">
                                <div class="card">
                                    <div class="header">
                                        <h4 class="title">Ubicacion</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Sede</label>
                                                    <select class="form-control border-input" id="cbSedes" onchange="cambioComboSedes();">
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Estacionamiento</label>
                                                    <select class="form-control border-input" id="cbEstacionamiento">
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="cardFechaFin" class="col-md-12" style="display: none;">
                                <div class="card">
                                    <div class="header">
                                        <h4 class="title">Fecha</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Fecha Inicio</label>
                                                    <input class="form-control border-input" type="text" name="date" value="">
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Fecha Fin</label>
                                                    <input class="form-control border-input" type="text" name="date2" value="">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="cardFechaCortesia" class="col-md-12" style="display: none;">
                                <div class="card">
                                    <div class="header">
                                        <h4 class="title">Fecha</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Fecha Inicio</label>
                                                    <input class="form-control border-input" type="text" name="date3" value="">
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Fecha Fin</label>
                                                    <input class="form-control border-input" type="text" name="date4" value="">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="cardFechaPago" class="col-md-12" style="display: none;">
                                <div class="card">
                                    <div class="header">
                                        <h4 class="title">Fecha Pago</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Fecha Inicio</label>
                                                    <input class="form-control border-input" type="text" name="date6" value="">
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <label>Fecha Fin</label>
                                                    <input class="form-control border-input" type="text" name="date7" value="">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="cardMotivoCortesia" class="col-md-4" style="display: none;">
                                <div class="card">
                                    <div class="header">
                                        <h4 class="title">Motivo</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <select class="form-control border-input" id="cbCortesias">
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="cardTipoVehiculo" class="col-md-4" style="display: none;">
                                <div class="card">
                                    <div class="header">
                                        <h4 class="title">Tipo V</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <select class="form-control border-input" id="cbTipov">
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div id="cardFechaSola" class="col-md-4" style="display: none;">
                                <div class="card">
                                    <div class="header">
                                        <h4 class="title">Fecha</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label>Dia</label>
                                                    <input class="form-control border-input" type="text" name="date5" value="">
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-8">
                            </div>
                            <div class="col-md-4">
                                <div class="card" onclick="btnConsultar_click()" id="btnConsultar">
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
                                <div class="card">
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="content table-responsive table-full-width">  
                                                <table id="sampleTable" class="widthFull fontsize10 displayNone">
                                                    
                                                </table>
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
