<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="transacciones.aspx.cs" Inherits="BlockAndPass.AdminWeb.transacciones" %>

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
        <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.0/jquery.min.js"></script>--%>
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
            .carousel-inner > .item > img,
            .carousel-inner > .item > a > img {
                width: 800px;
                height: 500px;
                margin: auto;
            }
        </style>

        <%-- Code --%>
	    <script type="text/javascript">
	        $(function () {
	            'use strict';
	            setInterval(function () {
	                ConsultarAlarmas();
	            }, 300000);
	            $('#Table1').DataTable({
	                "paging": true,
	                "lengthChange": false,
	                "searching": false,
	                "ordering": false,
	                "info": true,
	                "autoWidth": false
	            });
	        });
	        $(document).ready(function () {
	            //PruebaInicioBorrar();
	            ConsultarAlarmas();
	            getSecurity();
	            ConsultarSedes();
	            ConsultarEstacionamientos(0);
	            ConsultarCarriles(0, 0);
	            ConsultarSedesModal();
	            ConsultarEstacionamientosComboModal(0);
	            ConsultarCarrillesComboModal(0, 0);
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
	            $('#btnConsultar').click();
	            ObtenerMAC();
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
	        function cambioComboEstacionamiento() {
	            ConsultarCarriles($("#cbEstacionamiento").val(), $("#cbSedes").val());
	        }
	        function cambioComboSedes() {
	            ConsultarEstacionamientos($("#cbSedes").val());
	            ConsultarCarriles(0, $("#cbSedes").val());
	        }
	        function ConsultarCarriles(idEsta, idSede) {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaCarrilEntradaxEstacionamiento?idEstacionamiento=" + idEsta + "&idSede=" + idSede,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#cbCarrilles").get(0).options.length = 0;

	                    $("#cbCarrilles").get(0).options[$("#cbCarrilles").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#cbCarrilles").get(0).options[$("#cbCarrilles").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#cbCarrilles").get(0).options.length = 0;
	                    alert("Failed to load carriles");
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
	        function mifuncion() {
	            var month = parseInt($("*[name=date]").handleDtpicker('getDate').getMonth() + 1);
	            var date = $('*[name=date]').handleDtpicker('getDate').getDate() + '-' + month + '-' + $("*[name=date]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date]").handleDtpicker('getDate').getMinutes();
	            var month2 = parseInt($("*[name=date2]").handleDtpicker('getDate').getMonth() + 1);
	            var datepost = $('*[name=date2]').handleDtpicker('getDate').getDate() + '-' + month2 + '-' + $("*[name=date2]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date2]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date2]").handleDtpicker('getDate').getMinutes();
	            $("#sampleTable").dataTable({
	                "oLanguage": {
	                    "sZeroRecords": "No se encuentra informacion",
	                    "sLoadingRecords": "Cargando...",
	                    "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ registros",
	                    "sInfoEmpty": "Mostrando 0 a 0 de 0 registros",
	                    "sLengthMenu": "Mostrando _MENU_ registros por pagina",
	                },
	                "columnDefs": [{
	                    "targets": [0, 4, 7, 9, 10, 11, 12],
	                    "visible": false,
	                    "searchable": false
	                }, {
	                    "orderable": false,
	                    "targets": -1,
	                    "data": null,
	                    "defaultContent": "<a class=\"ti-camera\" onclick=\"btnCamera_Click(this);\"></a>"
	                }, {
	                    "orderable": false,
	                    "targets": -2,
	                    "data": null,
	                    "defaultContent": "<a class=\"ti-credit-card\" onclick=\"reponer(this);\"></a>"
	                }, {
	                    "orderable": false,
	                    "targets": -3,
	                    "data": null,
	                    "defaultContent": "<a class=\"ti-money\" onclick=\"detalle(this);\"></a>"
	                }, {
	                    "orderable": false,
	                    "targets": -4,
	                    "data": null,
	                    "defaultContent": "<a class=\"ti-printer\" onclick=\"btnPrinter_click(this);\"></a>"
	                }],
	                "aaSorting": [[2, "desc"]],
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
	                "sAjaxSource": "DataService.asmx/GetItems",
	                //"bJQueryUI": true,
	                //"sPaginationType": "full_numbers",
	                "bDeferRender": true,
	                "fnServerParams": function (aoData) {
	                    aoData.push({ "name": "iIdEstacionamiento", "value": $("#cbEstacionamiento").val() },
	                                { "name": "sFechaAntes", "value": date },
                                    { "name": "sFechaDespues", "value": datepost },
                                    { "name": "sTipoBusquedaFecha", "value": $("#cbTipoC").val() },
	                                { "name": "sCarril", "value": $("#cbCarrilles").val() },
                                    { "name": "sPlaca", "value": $("#txtPlaca").val() },
                                    { "name": "sIdTarjeta", "value": $("#txtTarjeta").val() },
                                    { "name": "iIdSede", "value": $("#cbSedes").val() },
                                    { "name": "iIdTipoVehiculo", "value": $("#cbTipoVehiculo").val() });
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
                                            //alert(msg);
                                            var json = jQuery.parseJSON(msg.d);
                                            //alert(json);
                                            fnCallback(json);
                                            $("#sampleTable").show();
                                        }
	                    });
	                }
	            });
	            $('#cbSedes').focus()
	            $('#cbSedes').select()
	        }
	        function detalle(e) {
	            var index = $(e).closest("tr");
	            var data = $('#sampleTable').dataTable().fnGetData(index);
	            var idtransaccion = data[7];
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/GetItemsMovimientosPagos?idTransaccion=" + idtransaccion,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    fillDetalles(msg);
	                    //alert('exito');
	                    //$('#myModal').modal('show');
	                },
	                error: function () {
	                    alert("Failed to load detalle transacciones");
	                }

	            });
	        }
	        function fillDetalles(msg) {
	            $("#Container2").setTemplateElement("Template-Items2").processTemplate(msg);
	            $('#Table1').DataTable({
	                "paging": true,
	                "lengthChange": false,
	                "searching": false,
	                "ordering": false,
	                "info": true,
	                "autoWidth": false
	            });
	            $('#myModal').modal('show');
	        }
	        function ReadTarjeta() {
	            //alert('a');
	            $.ajax({
	                type: "GET",
	                url: "http://localhost:8080/ReaderLocalService.svc/reader/getidcard?password=4LT4V1",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    if (msg.error == false) {
	                        $('#txtTarjeta').val(msg.idCard);
	                    } else {
	                        confirmaCambios(false, msg.errorMessage);
	                    }
	                },
	                error: function () {
	                    $('#txtTarjeta').val('');
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }
	            });
	        }
	        function reponer(e) {
	            var index = $(e).closest("tr");
	            var data = $('#sampleTable').dataTable().fnGetData(index);
	            var placa = data[3];
	            var modulo = data[1];
	            var fecha = data[2];

	            var dia = fecha.split(' ')[0].split('/')[0];
	            var mes = fecha.split(' ')[0].split('/')[1];
	            var anho = fecha.split(' ')[0].split('/')[2];

	            var hora = fecha.split(' ')[1].split(':')[0];
	            var min = fecha.split(' ')[1].split(':')[1];
	            var seg = fecha.split(' ')[1].split(':')[2];

	            //alert(fecha.split(' ')[2]);

	            var ampm = fecha.split(' ')[2];

	            var con1 = data[8];
	            var con2 = data[9];
	            var con3 = data[10];

	            var idEsta = data[12];

	            var tipov = data[11];
	            var idtransac = data[7];
	            var idtarjetaNueva;
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerValorParametroxNombre?nombre=claveTarjeta&idEstacionamiento=" + idEsta,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = msg.d;
	                    //alert(obj);
	                    if (obj != '') {
	                        $.ajax({
	                            type: "GET",
	                            url: "http://localhost:8080/ReaderLocalService.svc/reader/replacecard?password=" + obj + "&plate=" + placa + "&modulo=" + modulo + "&dia=" + dia + "&mes=" + mes + "&anho=" + anho + "&hora=" + hora + "&min=" + min + "&seg=" + seg + "&ampm=" + ampm + "&con1=" + con1 + "&con2=" + con2 + "&con3=" + con3 + "&tipov=" + tipov,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                if (msg.error == false) {
	                                    idtarjetaNueva = msg.idCard;
	                                    $.ajax({
	                                        type: "GET",
	                                        url: "DataService.asmx/InactivarTarjeta?idTransaccion=" + idtransac + "&idTarjeta=" + idtarjetaNueva,
	                                        data: "",
	                                        contentType: "application/json",
	                                        dataType: "json",
	                                        success: function (msg) {
	                                            var obj = jQuery.parseJSON(msg.d);
	                                            if (obj.Exito == true) {
	                                                confirmaCambios(true, '');
	                                            } else {
	                                                confirmaCambios(false, obj.ErrorMessage);
	                                            }
	                                        },
	                                        error: function () {
	                                            confirmaCambios(false, 'Error al consumir servicio LOCAL.');
	                                        }
	                                    });
	                                } else {
	                                    confirmaCambios(false, msg.errorMessage);
	                                }
	                            },
	                            error: function () {
	                                confirmaCambios(false, 'Error al consumir servicio LOCAL.');
	                            }
	                        });
	                    } else {
	                        confirmaCambios(false, 'No encuentra Parametro claveTarjeta.');
	                    }
	                }, error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }
	            });
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

	        function btnCrearEntrada_Click() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerInformacionCargo",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        if (obj.Resultado[0].Cargo == "CONTROL INTERNO" || obj.Resultado[0].Cargo == "COORDINADORA DE OPERACIONES") {
	                            $('#myModal2').modal('show');
	                        }
	                    }
	                },
	                error: function () {
	                    alert("Failed to load info Usuario actual");
	                }
	            });
	        }


	        function btnModalGuardar_click() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerValorParametroxNombre?nombre=claveTarjeta&idEstacionamiento=" + $("#idEstacionamientoModal").val(),
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = msg.d;
	                    //alert(obj);
	                    if (obj != '') {
	                        $.ajax({
	                            type: "GET",
	                            url: "http://localhost:8080/ReaderLocalService.svc/reader/getidcard?password=" + obj,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                if (msg.error == false) {
	                                    CrearNuevaEntrada(msg.idCard);
	                                } else {
	                                    $('#myModal2').modal('toggle');
	                                    confirmaCambios(false, msg.errorMessage);
	                                }
	                            },
	                            error: function () {
	                                $('#myModal2').modal('toggle');
	                                confirmaCambios(false, "Error leyendo tarjeta");
	                            }

	                        });
	                    } else {
	                        $('#myModal2').modal('toggle');
	                        confirmaCambios(false, 'Error al leer parametro Clave Tarjeta.');
	                    }
	                },
	                error: function () {
	                    $('#myModal2').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }
	            });
	        }
	        function CrearNuevaEntrada(idCard) {
	            var idEstacionamiento = $("#idEstacionamientoModal").val();
	            var carril = $("#idModuloModal").val();
	            var placa = $("#placaModal").val();
	            var tipovehiculo = $("#tipoVModal").val();
	            var month = parseInt($("*[name=date3]").handleDtpicker('getDate').getMonth() + 1);
	            var date = $('*[name=date3]').handleDtpicker('getDate').getDate() + '-' + month + '-' + $("*[name=date3]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date3]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date3]").handleDtpicker('getDate').getMinutes();

	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/CrearEntrada?idEstacionamiento=" + idEstacionamiento + "&idTarjeta=" + idCard + "&carril=" + carril + "&placa=" + placa + "&fecha=" + date + "&tipov=" + tipovehiculo,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        EscribirEntradaTarjeta(carril, placa, tipovehiculo, date, idCard, idEstacionamiento);
	                    } else {
	                        $('#myModal2').modal('toggle');
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    $('#myModal2').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }

	            });
	        }
	        function EscribirEntradaTarjeta(carril, placa, tipovehiculo, date, idCard, idEsta) {
	            //alert(date);
	            var dia = date.split(' ')[0].split('-')[0];
	            var mes = date.split(' ')[0].split('-')[1];
	            var anho = date.split(' ')[0].split('-')[2];

	            var hora = date.split(' ')[1].split(':')[0];
	            var min = date.split(' ')[1].split(':')[1];

	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerValorParametroxNombre?nombre=claveTarjeta&idEstacionamiento=" + idEsta,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = msg.d;
	                    //alert(obj);
	                    if (obj != '') {
	                        $.ajax({
	                            type: "GET",
	                            url: "http://localhost:8080/ReaderLocalService.svc/reader/createentry?password=" + obj + "&plate=" + placa + "&modulo=" + carril + "&dia=" + dia + "&mes=" + mes + "&anho=" + anho + "&hora=" + hora + "&min=" + min + "&tipov=" + tipovehiculo + "&idCard=" + idCard,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                if (msg.error == false) {
	                                    $('#myModal2').modal('toggle');
	                                    confirmaCambios(true, '');
	                                } else {
	                                    $('#myModal2').modal('toggle');
	                                    confirmaCambios(false, msg.errorMessage);
	                                }
	                            },
	                            error: function () {
	                                $('#myModal2').modal('toggle');
	                                confirmaCambios(false, 'Error al consumir servicio LOCAL.');
	                            }
	                        });
	                    } else {
	                        $('#myModal2').modal('toggle');
	                        confirmaCambios(false, 'No encuentra Parametro claveTarjeta.');
	                    }
	                }, error: function () {
	                    $('#myModal2').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }
	            });
	        }
	        function ConsultarSedesModal() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaSedes",
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#idSedeModal").get(0).options.length = 0;

	                    $("#idSedeModal").get(0).options[$("#idSedeModal").get(0).options.length] = new Option('Todos', 0);
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
	        function ConsultarEstacionamientosComboModal(idSede) {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaEstacionamientos?idSede=" + idSede,
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#idEstacionamientoModal").get(0).options.length = 0;

	                    //$("#cbEstacionamiento").get(0).options[$("#cbEstacionamiento").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#idEstacionamientoModal").get(0).options[$("#idEstacionamientoModal").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#idEstacionamientoModal").get(0).options.length = 0;
	                    alert("Failed to load estacionamientos");
	                }

	            });
	        }
	        function ConsultarCarrillesComboModal(idEsta, idSede) {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaCarrilEntradaxEstacionamiento?idEstacionamiento=" + idEsta + "&idSede=" + idSede,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#idModuloModal").get(0).options.length = 0;

	                    //$("#idModuloModal").get(0).options[$("#idModuloModal").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#idModuloModal").get(0).options[$("#idModuloModal").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#idModuloModal").get(0).options.length = 0;
	                    alert("Failed to load carriles");
	                }

	            });
	        }
	        function cambioComboEstacionamientoModal() {
	            ConsultarCarriles($("#idModuloModal").val(), $("#idSedeModal").val());
	        }
	        function cambioComboSedesModal() {
	            ConsultarEstacionamientos($("#idSedeModal").val());
	            ConsultarCarriles(0, $("#idSedeModal").val());
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
	            mifuncion();
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
	        function btnCamera_Click(e) {
	            var index = $(e).closest("tr");
	            var data = $('#sampleTable').dataTable().fnGetData(index);
	            var idtransaccion = data[7];

	            $('#myCarouselInner').empty();
	            $('#myCarouselslide').empty();

	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerImagenesxTransaccion?idtransaccion=" + idtransaccion,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        if (obj.Resultado.length > 0) {
	                            var i = 0;
	                            $.each(obj.Resultado, function (index, item) {
	                                $('#myCarouselInner').append('<div class="item"><img src="data:image/jpg;base64,' + item.Value + '" alt="Smiley face" height="42" width="42"></div>');
	                                $('#myCarouselslide').append('<li data-target="myCarousel" data-slide-to="' + i + '"></li>');
	                                i++;
	                            });
	                            $('.item').first().addClass('active');
	                            $('.carousel-indicators > li').first().addClass('active');
	                            $('#myCarousel').carousel();
	                            $("#myCarousel").carousel("pause").removeData();
	                            $("#myCarousel").carousel(0);
	                            $('#myModalFotos').modal('show');
	                        } else {
	                            confirmaCambios(false, 'No existen Fotos asociadas.');
	                        }
	                    } else {
	                        confirmaCambios(false, obj.Mensaje);
	                    }
	                },
	                error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }
	            });
	        }
	        function imprimir(idtran) {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerDatosFactura?idTransaccion=" + idtran,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        //alert(msg.d);
	                        $.ajax({
	                            type: "GET",
	                            url: "http://localhost:8080/ReaderLocalService.svc/computer/printticket?datos=" + msg.d,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                if (msg.error == false) {
	                                    //confirmaCambios(true, '');
	                                    var sampleArr = base64ToArrayBuffer(msg.impresion);
	                                    saveByteArray("Sample report", sampleArr);
	                                } else {
	                                    confirmaCambios(false, msg.errorMessage);
	                                }
	                            },
	                            error: function () {
	                                confirmaCambios(false, 'Error al consumir servicio LOCAL');
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
	        function base64ToArrayBuffer(base64) {
	            var binaryString = window.atob(base64);
	            var binaryLen = binaryString.length;
	            var bytes = new Uint8Array(binaryLen);
	            for (var i = 0; i < binaryLen; i++) {
	                var ascii = binaryString.charCodeAt(i);
	                bytes[i] = ascii;
	            }
	            return bytes;
	        }
	        function saveByteArray(reportName, byte) {
	            var file = new Blob([byte], { type: 'application/pdf' });
	            var fileURL = URL.createObjectURL(file);
	            $('#myTest').html("<iframe id='pdf' onload='isLoaded()' name='pdf' src='" + fileURL + "' style='visibility: hidden;'></iframe>");
	            //window.open(fileURL);
	        }
	        function btnPrinter_click(e) {
	            var index = $(e).closest("tr");
	            var data = $('#sampleTable').dataTable().fnGetData(index);
	            var idtransaccion = data[7];
	            imprimir(idtransaccion);
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
	        function isLoaded() {
	            var pdfFrame = window.frames["pdf"];
	            pdfFrame.focus();
	            pdfFrame.print();
	        }
	        function ConsultarTipoV() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaTipoVehiculo",
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#cbTipoVehiculo").get(0).options.length = 0;

	                    $("#cbTipoVehiculo").get(0).options[$("#cbTipoVehiculo").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#cbTipoVehiculo").get(0).options[$("#cbTipoVehiculo").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#cbTipoVehiculo").get(0).options.length = 0;
	                    alert("Failed to load cortesias");
	                }
	            });
	        }
	    </script>
    </head>

    <body">
        <div class="wrapper">
        <div class="sidebar" data-background-color="white"  data-active-color="danger">

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
                    <li class="active" id="navTransacciones">
                        <a>
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
                                    <h4 class="title">Transacciones</h4>
                                </div>
                                <div class="content"> 
                                    <div class="row">
                                        <div class="col-md-8">
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <div class="pull-right">
                                                    <label></label>
                                                    <button type="button" id="btnCrearEntrada" class="btn btn-default" onclick="btnCrearEntrada_Click();">
                                                        <span class="ti-car"></span> CrearEntrada
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
                                                    <select class="form-control border-input" id="cbEstacionamiento" onchange="cambioComboEstacionamiento();">
                                                    </select>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                    <label>Tipo Vehiculo</label>
                                                    <select class="form-control border-input" id="cbTipoVehiculo">
                                                    </select>
                                            </div>
                                        </div>
                                    </div>
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
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label>Tipo Fecha</label>
                                                <select class="form-control border-input" id="cbTipoC">
                                                    <option value="1">Entrada</option>
                                                    <option value="2">Salida</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label>Carril Entrada</label>
                                                <select class="form-control border-input" id="cbCarrilles">
                                                </select>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <label>Placa</label>
                                                <input class="form-control border-input" type="text" id="txtPlaca" placeholder="Digite placa">
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group border-input">
                                                <label>IdTarjeta</label>
                                                <div class='input-group '>
                                                    <input type='text' class="form-control border-input" id="txtTarjeta" placeholder="Leer Tarjeta -> "/>
                                                    <span class="input-group-addon">
                                                        <span class="ti-credit-card" onclick="ReadTarjeta();"></span>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-8">
                                        </div>
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <div class="pull-right">
                                                    <label></label>
                                                    <button type="button" id="btnConsultar" class="btn btn-default" onclick="mifuncion();">
                                                        <span class="ti-search"></span> Consultar
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="content table-responsive table-full-width">  
                                                <table id="sampleTable" class="widthFull fontsize10 displayNone">
                                                    <thead>
                                                        <tr>
                                                            <th>Nombre</th>
                                                            <th>ModEntrada</th>
                                                            <th>FechaEntrada</th>
                                                            <th>PlacaEntrada</th>
                                                            <th>ModSalida</th>
                                                            <th>FechaSalida</th>
                                                            <th>PlacaSalida</th>
                                                            <th>IdTransaccion</th>
                                                            <th>Convenio</th>
                                                            <th>IdConvenio2</th>
                                                            <th>IdConvenio3</th>
                                                            <th>IdTipoVehiculo</th>
                                                            <th>IdEstacionamiento</th>
                                                            <th>Tarjeta</th>
                                                            <th></th>
                                                            <th></th>
                                                            <th></th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                    </tbody>
                                                    <tfoot>
                                                        <tr>
                                                            <th>Nombre</th>
                                                            <th>ModEntrada</th>
                                                            <th>FechaEntrada</th>
                                                            <th>PlacaEntrada</th>
                                                            <th>ModSalida</th>
                                                            <th>FechaSalida</th>
                                                            <th>PlacaSalida</th>
                                                            <th>IdTransaccion</th>
                                                            <th>Convenio</th>
                                                            <th>IdConvenio2</th>
                                                            <th>IdConvenio3</th>
                                                            <th>IdTipoVehiculo</th>
                                                            <th>IdEstacionamiento</th>
                                                            <th>Tarjeta</th>
                                                            <th></th>
                                                            <th></th>
                                                            <th></th>
                                                            <th></th>
                                                        </tr>
                                                    </tfoot>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12" id="myTest">
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

    <div id="myModal2" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Crear Entrada</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>IdSede</label>
                                    <select class="form-control border-input" id="idSedeModal" onchange="cambioComboSedesModal();">
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>IdEstacionamiento</label>
                                    <select class="form-control border-input" id="idEstacionamientoModal" onchange="cambioComboEstacionamientoModal();">
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Carril</label>
                                    <select class="form-control border-input" id="idModuloModal">
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Placa</label>
                                    <input type="text" class="form-control border-input" id="placaModal">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Fecha Entrada
                                    </label>
                                    <input class="form-control border-input" type="text" name="date3" value="">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>TipoVehiculo</label>
                                    <select class="form-control border-input" id="tipoVModal">
                                        <option value="1">Carro</option>
                                        <option value="2">Moto</option>
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-6">
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
                                    <span class="ti-save"></span>Crear
                                </button>
                            </div>
                        </div>  
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="myModalFotos" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Imagenes</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div id="myCarousel" class="carousel slide" data-ride="carousel">
                            <!-- Indicators -->
                            <ol class="carousel-indicators" id="myCarouselslide">
                              <%--<li data-target="#myCarousel" data-slide-to="0" class="active"></li>
                              <li data-target="#myCarousel" data-slide-to="1"></li>
                              <li data-target="#myCarousel" data-slide-to="2"></li>
                              <li data-target="#myCarousel" data-slide-to="3"></li>--%>
                            </ol>

                            <!-- Wrapper for slides -->
                            <div class="carousel-inner" role="listbox" id="myCarouselInner">
                                  <%--<div class="item active">
                                    <img src="https://www.w3schools.com/bootstrap/img_flower2.jpg" alt="Flower" width="460" height="345">
                                  </div>

                                  <div class="item">
                                    <img src="https://www.w3schools.com/bootstrap/img_flower2.jpg" alt="Flower" width="460" height="345">
                                  </div>
    
                                  <div class="item">
                                    <img src="https://www.w3schools.com/bootstrap/img_flower2.jpg" alt="Flower" width="460" height="345">
                                  </div>

                                  <div class="item">
                                    <img src="https://www.w3schools.com/bootstrap/img_flower2.jpg" alt="Flower" width="460" height="345">
                                  </div>--%>
                            </div>

                            <!-- Left and right controls -->
                            <a class="left carousel-control" href="#myCarousel" role="button" data-slide="prev">
                              <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                              <span class="sr-only">Previous</span>
                            </a>
                            <a class="right carousel-control" href="#myCarousel" role="button" data-slide="next">
                              <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                              <span class="sr-only">Next</span>
                            </a>
                          </div>
                    </div>
                </div>
                <div class="modal-footer">
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
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Detalles</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <!-- Templates -->
                        <p style="display: none">
                            <textarea id="Template-Items2" rows="0" cols="0"><!--
                            <table id="Table1" class="table table-bordered table-hover">
                                <thead>
                                <tr>
                                    <th>Parte</th>
                                    <th>Denominacion</th>
                                    <th>Cantidad</th>
                                    <th>Valor</th>
                                </tr>
                                </thead>
                                <tbody>
                                {#foreach $T.d as post}
                                <tr>
                                    <td>{$T.post.Parte}</td>
                                    <td>{$T.post.Denominacion}</td>
                                    <td>{$T.post.Cantidad}</td>
                                    <td>{$T.post.Valor}</td>
                                </tr>
                                {#/for}
                                </tbody>
                                <tfoot>
                                    <tr>
                                        <th>Parte</th>
                                        <th>Denominacion</th>
                                        <th>Cantidad</th>
                                        <th>Valor</th>
                                    </tr>
                                </tfoot>
                            </table>
                            --></textarea>
                        </p>
                        <div id="Container2" />
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="content">
                            
                    </div>
                </div>
            </div>
        </div>
    </div>
</html>
