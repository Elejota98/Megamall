<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="autorizaciones.aspx.cs" Inherits="BlockAndPass.AdminWeb.autorizaciones" %>

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
        <link href="timepicker/timepicker.css" rel="stylesheet" />

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
        <script src="timepicker/bootstrap-timepicker.js"></script>
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
	            ConsultarEstacionamientos(0);
	            ConsultarSedes();
	            ConsultarSedesModal();
	            ConsultarEstacionamientosComboModal(0);
	            ObtenerDatosUsuario();
	            $('#btnConsultar').click();
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
	            $('.selectpicker').selectpicker();
	            $('#timepicker1').timepicker({
	                defaultTime: '12:00 AM',
	                explicitMode: true,
	            });
	            $('#timepicker2').timepicker({
	                defaultTime: '11:59 PM',
	                explicitMode: true,
	            });
	            $('#some-name').bootstrapDualListbox({
	                nonSelectedListLabel: 'No Seleccionados',
	                selectedListLabel: 'Seleccionados',
	                filterPlaceHolder: 'Filtro',
	                moveOnSelect: false,
	                showFilterInputs: false,
	                infoText: ''
	            });
	            getSecurity();
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

	            if (jQuery.inArray('navPersonasAutorizadas', msg.d) >= 0) {

	            } else {
	                if (jQuery.inArray('navAurotizaciones', msg.d) >= 0) {
	                    
	                } else {
	                    window.location.replace("template.aspx");
	                }
	            }
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
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/GetItemsAutorizaciones",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    fillautorizaciones(msg);
	                    //alert('exito');
	                },
	                error: function () {
	                    alert("Failed to load detalle autorizaciones");
	                }
	            });
	        }
	        function fillautorizaciones(msg) {
	            $("#Container2").setTemplateElement("Template-Items2").processTemplate(msg);
	            $('#sampleTable').dataTable({
	                "language": {
	                    "search": "Buscar:",
	                    "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
	                },
	                "columnDefs": [{
	                    "orderable": false,
	                    "targets": -1,
	                    "data": null,
	                    "defaultContent": "<a class=\"ti-pencil-alt2\" onclick=\"btnEditar_click(this);\"></a>"
	                }, {
	                    "orderable": false,
	                    "targets": -2,
	                    "data": null,
	                    "defaultContent": "<a class=\"ti-location-arrow\" onclick=\"btnEstacionamientos_click(this);\"></a>"
	                },
	                {
	                    "targets": [1,2,8],
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
	        function btnEditar_click(e) {
	            var index = $(e).closest("tr");
	            var data = $('#sampleTable').dataTable().fnGetData(index);

	            var idAutorizacion = data[0];
	            var idEstacionamiento = data[1];
	            var regla = data[2];
	            var nombre = data[3];
	            var fechaI = data[4];
	            var fechaF = data[5];
	            var Estado = data[6];
	            var Tipo = data[8];

	            $('#idAutorizacionModal').val(idAutorizacion);
	            $('#nombreModal').val(nombre);
	            $('#idEstacionamientoModal').val(idEstacionamiento);
	            //alert(Tipo);
	            $('#tipoModal').val(Tipo);
	            
	            var amInicialColocar = fechaI.split(' ')[2];
	            var horaInicialColocar = fechaI.split(' ')[1].split(':')[0];
	            var minInicialColocar = fechaI.split(' ')[1].split(':')[1];
	            var fechaInicialColocar = fechaI.split(' ')[0];
	            if (amInicialColocar == 'p.m.') {
	                horaInicialColocar = parseInt(horaInicialColocar) + 12;
	            }

	            $('#fechaIModal').val(fechaInicialColocar + ' ' + horaInicialColocar + ':' + minInicialColocar);

	            var amFinalColocar = fechaF.split(' ')[2];
	            var horaFinalColocar = fechaF.split(' ')[1].split(':')[0];
	            var minFinalColocar = fechaF.split(' ')[1].split(':')[1];
	            var fechaFinalColocar = fechaF.split(' ')[0];
	            //alert(amFinalColocar);
	            if (amFinalColocar == 'p.m.') {
	                horaFinalColocar = parseInt(horaFinalColocar) + 12;
	            }

	            $('#fechaFModal').val(fechaFinalColocar + ' ' + horaFinalColocar + ':' + minFinalColocar);


	            if (Estado == 'true') {
	                $("#activoModal").addClass('checked');
	            } else {
	                $("#activoModal").removeClass('checked');
	            }


	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerReglaAutorizacion?idRegla=" + regla,
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    abrirModalEditar(msg);
	                },
	                error: function () {
	                    alert("Failed to load regla");
	                }
	            });
	        }
	        function abrirModalEditar(data) {
	            document.getElementById('idSedeModal').disabled = true;
	            document.getElementById('idEstacionamientoModal').disabled = true;
	            var custom_arr1 = [];
	            
	            if (data.d[0].Lunes == true) {
	                custom_arr1.push("Lunes")
	            }
	            if (data.d[0].Martes == true) {
	                custom_arr1.push("Martes")
	            }
	            if (data.d[0].Miercoles == true) {
	                custom_arr1.push("Miercoles")
	            }
	            if (data.d[0].Jueves == true) {
	                custom_arr1.push("Jueves")
	            }
	            if (data.d[0].Viernes == true) {
	                custom_arr1.push("Viernes")
	            }
	            if (data.d[0].Sabado == true) {
	                custom_arr1.push("Sabado")
	            }
	            if (data.d[0].Domingo == true) {
	                custom_arr1.push("Domingo")
	            }
	            if (data.d[0].Festivo == true) {
	                custom_arr1.push("Festivo")
	            }
	            

	            if (custom_arr1.length > 0) {
	                $('#selectpp').selectpicker("val", custom_arr1);
	            }

	            var horaInicio = data.d[0].HoraI.split(':')[0];
	            var minutoInicio = data.d[0].HoraI.split(':')[1];
	            var horaFin = data.d[0].HoraF.split(':')[0];
	            var minutoFin = data.d[0].HoraF.split(':')[1];

	            if (horaInicio > 12) {
	                horaInicio = horaInicio - 12;
	                $('#timepicker1').timepicker('setTime', horaInicio+':' + minutoInicio + ' PM');
	            } else {
	                $('#timepicker1').timepicker('setTime', horaInicio + ':' + minutoInicio + ' AM');
	            }

	            if (horaFin > 12) {
	                horaFin = horaFin - 12;
	                $('#timepicker2').timepicker('setTime', horaFin + ':' + minutoFin + ' PM');
	            } else {
	                $('#timepicker2').timepicker('setTime', horaFin + ':' + minutoFin + ' AM');
	            }

	            $('.modal-title').html("Editar");
	            $('#myModal').modal('show');
	        }
	        function btnCrear_click() {
	            document.getElementById('idSedeModal').disabled = false;
	            document.getElementById('idEstacionamientoModal').disabled = false;
	            $('#idAutorizacionModal').val('');
	            $('#nombreModal').val('');
	            $('#tipoModal').val('1');
	            $('#fechaIModal').val('10/10/2006 10:00');
	            $('#fechaFModal').val('10/10/2017 10:00');
	            $("#activoModal").addClass('checked');

	            var custom_arr1 = [];
	            $('#selectpp').selectpicker("val", custom_arr1);

	            $('#timepicker1').timepicker('setTime', '12:00 AM');
	            $('#timepicker2').timepicker('setTime', '11:00 PM');
	            
	            $('.modal-title').html("Crear");
	            $('#myModal').modal('show');
	        }
	        function btnModalGuardar_click() {

	            var idRegla = 0;

	            var horaIni = $('#timepicker1').val().split(':')[0];
	            var minIni = $('#timepicker1').val().split(' ')[0].split(':')[1];
	            if ($('#timepicker1').val().split(' ')[1] == 'PM') {
	                horaIni = parseInt(horaIni) + 12;
	            }

	            var horaFin = $('#timepicker2').val().split(':')[0];
	            var minFin = $('#timepicker2').val().split(' ')[0].split(':')[1];
	            if ($('#timepicker2').val().split(' ')[1] == 'PM') {
	                horaFin = parseInt(horaFin) + 12;
	            }

	            horaIni = horaIni + ':' + minIni;
	            horaFin = horaFin + ':' + minFin;

	            var month = parseInt($("*[name=date]").handleDtpicker('getDate').getMonth() + 1);
	            var month2 = parseInt($("*[name=date2]").handleDtpicker('getDate').getMonth() + 1);

	            var date = $('*[name=date]').handleDtpicker('getDate').getDate() + '-' + month + '-' + $("*[name=date]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date]").handleDtpicker('getDate').getMinutes();
	            var date2 = $('*[name=date2]').handleDtpicker('getDate').getDate() + '-' + month2 + '-' + $("*[name=date2]").handleDtpicker('getDate').getFullYear() + ' ' + $("*[name=date2]").handleDtpicker('getDate').getHours() + ':' + $("*[name=date2]").handleDtpicker('getDate').getMinutes();

	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerIdReglaAutorizacionxCondiciones?horaIni=" + horaIni + "&horaFin=" + horaFin + "&dias=" + $('#selectpp').val(),
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    //alert(msg.d);
	                    idRegla = msg.d;
	                    if (idRegla != 0) {
	                        if ($('.modal-title').html() == 'Crear') {
	                            var activo = $('#activoModal').hasClass("checked");
	                            $.ajax({
	                                type: "GET",
	                                url: "DataService.asmx/CrearAutorizacion?nombre=" + $('#nombreModal').val() + "&fechaInicio=" + date + "&fechaFin=" + date2 + "&estado=" + activo + "&idRegla=" + idRegla + "&idEStacionamiento=" + $('#idEstacionamientoModal').val() + "&tipo=" + $('#tipoModal').val(),
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
	                                    $('#myModal').modal('toggle');
	                                    confirmaCambios(false, 'Error al consumir servicio.');
	                                }
	                            });
	                        } else {
	                            var activo = $('#activoModal').hasClass("checked");
	                            $.ajax({
	                                type: "GET",
	                                url: "DataService.asmx/ActualizarAutorizacion?idAutorizacion=" + $('#idAutorizacionModal').val() + "&nombre=" + $('#nombreModal').val() + "&fechaInicio=" + date + "&fechaFin=" + date2 + "&estado=" + activo + "&idRegla=" + idRegla + "&idEStacionamiento=" + $('#idEstacionamientoModal').val() + "&tipo=" + $('#tipoModal').val(),
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
	                                    $('#myModal').modal('toggle');
	                                    confirmaCambios(false, 'Error al consumir servicio.');
	                                }
	                            });
	                        }
	                    } else {
	                        $('#myModal').modal('toggle');
	                        confirmaCambios(false, 'Regla de autorizacion invalida.');
	                    }
	                },
	                error: function () {
	                    $('#myModal').modal('toggle');
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
	        function cambioComboSedesModal() {
	            ConsultarEstacionamientos($("#idSedeModal").val());
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
	        function ConsultarEstacionamientosComboModal3() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaEstacionamientos?idSede=0",
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#idEstacionamientoModal3").get(0).options.length = 0;

	                    //$("#cbEstacionamiento").get(0).options[$("#cbEstacionamiento").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#idEstacionamientoModal3").get(0).options[$("#idEstacionamientoModal3").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#idEstacionamientoModal3").get(0).options.length = 0;
	                    alert("Failed to load estacionamientos");
	                }

	            });
	        }
	        function btnEstacionamientos_click(e) {
	            //alert('a');
	            ConsultarEstacionamientosComboModal3();
	            var index = $(e).closest("tr");
	            var data = $('#sampleTable').dataTable().fnGetData(index);

	            var idAuto = data[0];
	            var idEstacionamiento = data[1];
	            var nombre = data[3];
	            //alert(Documento);

	            $('#idAutorizacionModal3').val(idAuto);
	            $('#nombreAutorizacionModal3').val(nombre);
	            $('#idEstacionamientoModal3').val(idEstacionamiento);

	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/GetItemsPermisosAutorizacionEstacionamiento?idAutorizacion=" + idAuto + "&idEstacionamiento=" + idEstacionamiento,
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    fillpermisosestacionamiento(msg);
	                },
	                error: function () {
	                    alert("Failed to load permisos de estacionamiento");
	                }
	            });
	        }
	        function fillpermisosestacionamiento(datos) {
	            $('#some-name')
                .find('option')
                .remove()
                .end();

	            $.each(datos.d, function (index, item) {
	                //alert(item.Estado);
	                if (item.Estado == 'true') {
	                    $('#some-name').append('<option value="' + item.IdEstacionamiento + '" selected="selected">' + item.Nombre + '</option>');
	                } else {
	                    $('#some-name').append('<option value="' + item.IdEstacionamiento + '">' + item.Nombre + '</option>');
	                }
	            });

	            $('#some-name').bootstrapDualListbox('refresh', true);


	            $('#myModal3').modal('show');
	        }
	        function btnGuardarPermisosEstacionamientos_click() {
	            ////alert($('#some-name').val());
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/UpdatePermisosAutorizacionEstacionamiento?idAutorizacion=" + $('#idAutorizacionModal3').val() + "&idEstacionamiento=" + $('#idEstacionamientoModal3').val() + "&permisos=" + $('#some-name').val(),
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    //alert(msg.d);
	                    var obj = jQuery.parseJSON(msg.d);
	                    //alert(obj.Exito);
	                    if (obj.Exito == true) {
	                        $('#myModal3').modal('toggle');
	                        confirmaCambios(true, '');
	                    } else {
	                        $('#myModal3').modal('toggle');
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    $('#myModal3').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
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
                        <li id="navAutorizados" class="active">
                            <a>
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
                                    <li id="navPersonasAutorizadas"><a href="autorizados.aspx">Autorizados</a></li>
                                    <li id="navAurotizaciones" class="active"><a href="#">Autorizaciones</a></li>
                                </ul>
                            </div>
                        </nav>
                        <div class="card">
                            <div class="header">
                                <h4 class="title">Autorizaciones</h4>
                            </div>
                            <div class="content">
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
                                            <!-- Templates -->
                                            <p style="display: none">
                                                <textarea id="Template-Items2" rows="0" cols="0"><!--
                                                     <div class="content table-responsive table-full-width">  
                                                        <table id="sampleTable" class="widthFull fontsize10 displayNone">
                                                
                                                            <thead>
                                                                <tr>
                                                                    <th>IdAutorizacion</th>
                                                                    <th>idEstacionamiento</th>
                                                                    <th>IdReglaAutorizacion</th>
                                                                    <th>NombreAutorizacion</th>
                                                                    <th>FechaInicial</th>
                                                                    <th>FechaFinal</th>
                                                                    <th>Estado</th>
                                                                    <th>NombreEstacionamiento</th>
                                                                    <th>IdTipo</th>
                                                                    <th></th>
                                                                    <th></th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                            {#foreach $T.d as post}
                                                            <tr>
                                                                <td>{$T.post.IdAutorizacion}</td>
                                                                <td>{$T.post.idEstacionamiento}</td>
                                                                <td>{$T.post.IdReglaAutorizacion}</td>
                                                                <td>{$T.post.NombreAutorizacion}</td>
                                                                <td>{$T.post.FechaInicial}</td>
                                                                <td>{$T.post.FechaFinal}</td>
                                                                <td>{$T.post.Estado}</td>
                                                                <td>{$T.post.Nombre}</td>
                                                                <td>{$T.post.IdTipo}</td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                            {#/for}
                                                            </tbody>
                                                            <tfoot>
                                                                <tr>
                                                                    <th>IdAutorizacion</th>
                                                                    <th>idEstacionamiento</th>
                                                                    <th>IdReglaAutorizacion</th>
                                                                    <th>NombreAutorizacion</th>
                                                                    <th>FechaInicial</th>
                                                                    <th>FechaFinal</th>
                                                                    <th>Estado</th>
                                                                    <th>NombreEstacionamiento</th>
                                                                    <th>IdTipo</th>
                                                                    <th></th>
                                                                    <th></th>
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
                                        <div class="col-md-10">

                                        </div>
                                        <div class="col-md-2">
                                            <button type="button" class="btn btn-default" onclick="btnCrear_click();">
                                                <span class="ti-plus"></span>Nuevo
                                            </button>
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
                                <button type="button" id="Button3" class="btn btn-default" onclick="btnCerrarConfrimacion_click();">
                                    <span class="ti-close"></span>Cerrar
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="myModal3" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Estacionamientos</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>IdAutorizacion</label>
                                    <input type="text" class="form-control border-input" disabled id="idAutorizacionModal3">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>NombreAutorizacion</label>
                                    <input type="text" class="form-control border-input" disabled id="nombreAutorizacionModal3">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>IdEstacionamiento</label>
                                    <select class="form-control border-input" disabled id="idEstacionamientoModal3">
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div id="dual-list-box" class="form-group row">
                            <div class="col-md-12">
                                <select id="some-name" multiple="multiple" size="10">
                                      <%--<option value="option1">Option 1</option>
                                      <option value="option2">Option 2</option>
                                      <option value="option3" selected="selected">Option 3</option>
                                      <option value="option4">Option 4</option>
                                      <option value="option5">Option 5</option>
                                      <option value="option6" selected="selected">Option 6</option>
                                      <option value="option7">Option 7</option>
                                      <option value="option8">Option 8</option>
                                      <option value="option9">Option 9</option>
                                      <option value="option0">Option 10</option>--%>
                                </select>
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
                                <button type="button" id="Button4" class="btn btn-default" onclick="btnGuardarPermisosEstacionamientos_click();">
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
                                    <label>IdSede</label>
                                    <select class="form-control border-input" id="idSedeModal" onchange="cambioComboSedesModal();">
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>IdEstacionamiento</label>
                                    <select class="form-control border-input" id="idEstacionamientoModal">
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>IdAutorizacion</label>
                                    <input type="text" class="form-control border-input" disabled id="idAutorizacionModal">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>NombreAutorizacion</label>
                                    <input type="text" class="form-control border-input" id="nombreModal">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Tipo</label>
                                    <select class="form-control border-input" id="tipoModal">
                                        <option value="1">Mensualidad</option>
                                        <option value="2">Quincena</option>
                                        <option value="4">Anualidad</option>
                                        <option value="3">Bolsa</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Fecha Inicial
                                    </label>
                                    <input class="form-control border-input" type="text" name="date" value="" id="fechaIModal">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Fecha Final
                                    </label>
                                    <input class="form-control border-input" type="text" name="date2" value="" id="fechaFModal">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Dias</label>
                                    <select class="form-control border-input selectpicker" multiple data-selected-text-format="count > 4" id="selectpp">
                                        <option>Lunes</option>
                                        <option>Martes</option>
                                        <option>Miercoles</option>
                                        <option>Jueves</option>
                                        <option>Viernes</option>
                                        <option>Sabado</option>
                                        <option>Domingo</option>
                                        <option>Festivos</option>
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group border-input">
                                    <label>Hora Inicio</label>
                                    <div class="input-group bootstrap-timepicker timepicker">
                                        <input id="timepicker1" type="text" class="form-control input-small border-input">
                                        <span class="input-group-addon">
                                            <i class="glyphicon glyphicon-time"></i>
                                        </span>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group border-input">
                                    <label>Hora Fin</label>
                                    <div class="input-group bootstrap-timepicker timepicker">
                                        <input id="timepicker2" type="text" class="form-control input-small border-input">
                                        <span class="input-group-addon">
                                            <i class="glyphicon glyphicon-time"></i>
                                        </span>
                                    </div>
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
</html>