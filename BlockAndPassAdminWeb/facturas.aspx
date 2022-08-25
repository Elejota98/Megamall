<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="facturas.aspx.cs" Inherits="BlockAndPass.AdminWeb.facturas" %>

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
	            ConsultarSedes();
	            ConsultarEstacionamientos(0);
	            ConsultarCarriles(0, 0);
	            //ConsultarEstacionamientosComboModal();
	            $('#btnConsultar').click();
	            ObtenerMAC();
	            ObtenerDatosUsuario();
	            ConsultarSedesModal();
	            ConsultarEstacionamientosComboModal(0);
	            ConsultarCarrillesComboModal(0, 0);
	            var dt = new Date();
	            var month = parseInt(dt.getMonth() + 1);
	            $('*[name=date1]').appendDtpicker({
	                "locale": "es",
	                "minuteInterval": 15,
	                "current": dt.getDate() + "-" + month + "-" + dt.getFullYear() + " " + "00:00",
	                "closeOnSelected": true,
	                "todayButton": false,
	            });
	            $('*[name=date1]').change(function () {

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
	        function mifuncion() {
	            $("#sampleTable").dataTable({
	                "oLanguage": {
	                    "sZeroRecords": "No se encuentra infromacion",
	                    "sLoadingRecords": "Cargando...",
	                    "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ registros",
	                    "sInfoEmpty": "Mostrando 0 a 0 de 0 registros",
	                    "sLengthMenu": "Mostrando _MENU_ registros por pagina",
	                },
	                "columnDefs": [
                    {
                        "targets": [0,2,4,5,8,10,11],
                        "visible": false,
                        "searchable": false
                    },
                    {
	                    "orderable": false,
	                    "targets": -1,
	                    "data": null,
	                    "defaultContent": "<a class=\"ti-pencil-alt2\" onclick=\"btnEditar_click(this);\"></a>"
	                }],
	                "aaSorting": [[0, "desc"]],
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
	                "sAjaxSource": "DataService.asmx/GetItemsFacturas",
	                //"bJQueryUI": true,
	                //"sPaginationType": "full_numbers",
	                "bDeferRender": true,
	                "fnServerParams": function (aoData) {
	                    aoData.push({ "name": "iIdModulo", "value": $("#cbCarrilles").val() },
                                    { "name": "iIdEstacionamiento", "value": $("#cbEstacionamiento").val() },
	                                { "name": "iIdSede", "value": $("#cbSedes").val() });
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
	                url: "DataService.asmx/ObtenerListaCarrilPagoxEstacionamiento?idEstacionamiento=" + idEsta + "&idSede=" + idSede,
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
	        function ConsultarEstacionamientosComboModal() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaEstacionamientos",
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
	        function btnEditar_click(e) {
	            //document.getElementById('idEstacionamientoModal').disabled = true;
	            //document.getElementById('codigoModal').disabled = true;
	            var index = $(e).closest("tr");
	            var data = $('#sampleTable').dataTable().fnGetData(index);

	            var id = data[0];
	            var modulo = data[1];
	            var estacionamiento = data[2];
	            var prefijo = data[3];
	            var inicial = data[4];
	            var final = data[5];
	            var actual = data[6];
	            var numero = data[7];
	            var fecha = data[8];
	            var estado = data[9];
	            var fechaFin = data[10];
	            var sede = data[11];

	            
	            var fechaInicialColocar = fecha.split(' ')[0];
	            $('#fechaModal').val(fechaInicialColocar);

	            var fechaFinalColocar = fechaFin.split(' ')[0];
	            $('#fechaFinModal').val(fechaFinalColocar);

	            $('#codigoModal').val(id);
	            $('#idModuloModal').val(modulo);
	            $('#idEstacionamientoModal').val(estacionamiento);
	            $('#prefijoModal').val(prefijo);
	            $('#inicialModal').val(inicial);
	            $('#finalModal').val(final);
	            $('#actualModal').val(actual);
	            $('#resolucionModal').val(numero);
	            $('#idSedeModal').val(sede);


	            if (estado == 'True') {
	                $("#activoModal").addClass('checked');
	            } else {
	                $("#activoModal").removeClass('checked');
	            }
	            $('.modal-title').html("Editar");
	            $('#myModal').modal('show');
	        }
	        function btnCrear_click() {
	            //document.getElementById('idEstacionamientoModal').disabled = false;
	            //document.getElementById('codigoModal').disabled = false;

	            $('#codigoModal').val('');
	            $('#prefijoModal').val('');
	            $('#inicialModal').val('');
	            $('#finalModal').val('');
	            $('#resolucionModal').val('');
	            
	            $("#activoModal").removeClass('checked');

	            $('.modal-title').html("Crear");
	            $('#myModal').modal('show');
	        }
	        function btnModalGuardar_click() {
	            if ($('.modal-title').html() == 'Crear') {
	                var activo = $('#activoModal').hasClass("checked");

	                var month = parseInt($("*[name=date1]").handleDtpicker('getDate').getMonth() + 1);
	                var date = $('*[name=date1]').handleDtpicker('getDate').getDate() + '-' + month + '-' + $("*[name=date1]").handleDtpicker('getDate').getFullYear();

	                var month2 = parseInt($("*[name=date2]").handleDtpicker('getDate').getMonth() + 1);
	                var date2 = $('*[name=date2]').handleDtpicker('getDate').getDate() + '-' + month2 + '-' + $("*[name=date2]").handleDtpicker('getDate').getFullYear();
	                
	                $.ajax({
	                    type: "GET",
	                    url: "DataService.asmx/CrearFactura?idEstacionamiento=" + $('#idEstacionamientoModal').val() + "&idModulo=" + $('#idModuloModal').val() + "&prefijo=" + $('#prefijoModal').val() + "&estado=" + activo + "&inicial=" + $('#inicialModal').val() + "&final=" + $('#finalModal').val() + "&numero=" + $('#resolucionModal').val() + "&fecha=" + date + "&fin=" + date2,
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

	                var month = parseInt($("*[name=date1]").handleDtpicker('getDate').getMonth() + 1);
	                var date = $('*[name=date1]').handleDtpicker('getDate').getDate() + '-' + month + '-' + $("*[name=date1]").handleDtpicker('getDate').getFullYear();

	                var month2 = parseInt($("*[name=date2]").handleDtpicker('getDate').getMonth() + 1);
	                var date2 = $('*[name=date2]').handleDtpicker('getDate').getDate() + '-' + month2 + '-' + $("*[name=date2]").handleDtpicker('getDate').getFullYear();

	                $.ajax({
	                    type: "GET",
	                    url: "DataService.asmx/ActualizarFactura?idEstacionamiento=" + $('#idEstacionamientoModal').val() + "&idFacturacion=" + $('#codigoModal').val() + "&idModulo=" + $('#idModuloModal').val() + "&prefijo=" + $('#prefijoModal').val() + "&estado=" + activo + "&inicial=" + $('#inicialModal').val() + "&final=" + $('#finalModal').val() + "&numero=" + $('#resolucionModal').val() + "&fecha=" + date + "&fin=" + date2,
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
	                url: "DataService.asmx/ObtenerListaCarrilPagoxEstacionamiento?idEstacionamiento=" + idEsta + "&idSede=" + idSede,
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
	        function consultarFacturaAnular() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerDatosFacturaxFactura?numFactura=" + $('#numeroFactura').val(),
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        //alert(msg.d);
	                        if (obj.Resultado[0].Tipo == 'Mensualidad') {
	                            $.ajax({
	                                type: "GET",
	                                url: "http://localhost:8080/ReaderLocalService.svc/computer/printticketm?datos=" + msg.d,
	                                data: "",
	                                contentType: "application/json",
	                                dataType: "json",
	                                success: function (msg) {
	                                    if (msg.error == false) {
	                                        //confirmaCambios(true, '');
	                                        var sampleArr = base64ToArrayBuffer(msg.impresion);
	                                        saveByteArray("Sample report", sampleArr, $('#numeroFactura').val());
	                                    } else {
	                                        confirmaCambios(false, msg.errorMessage);
	                                    }
	                                },
	                                error: function () {
	                                    confirmaCambios(false, 'Error al consumir servicio LOCAL');
	                                }

	                            });
	                        } else {
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
	                                        saveByteArray("Sample report", sampleArr, $('#numeroFactura').val());
	                                    } else {
	                                        confirmaCambios(false, msg.errorMessage);
	                                    }
	                                },
	                                error: function () {
	                                    confirmaCambios(false, 'Error al consumir servicio LOCAL');
	                                }

	                            });
	                        }
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
	        function saveByteArray(reportName, byte, numFac) {
	            var file = new Blob([byte], { type: 'application/pdf' });
	            var fileURL = URL.createObjectURL(file);
	            $('#myTest').html("<iframe id='pdf' onload='isLoaded()' name='pdf' src='" + fileURL + "'></iframe>");
	            document.getElementById("rowFacturaAnular").style.visibility = "visible";
	            $('#numFacturaConfirmar').val(numFac);
	            //window.open(fileURL);
	        }
	        function isLoaded() {
	            var pdfFrame = window.frames["pdf"];
	            pdfFrame.focus();
	            //pdfFrame.print();
	        }
	        function btnCancelarAnulacion() {
	            LimpiarPanelAnulacion();
	        }
	        function btnAceptarAnulacion() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/AnularFactura?numFactura=" + $('#numeroFactura').val(),
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        LimpiarPanelAnulacion();
	                        confirmaCambios(true, '');
	                    }else{
	                        confirmaCambios(false, msg.errorMessage);
	                    }
	                },
	                error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function LimpiarPanelAnulacion() {
	            document.getElementById("rowFacturaAnular").style.visibility = "hidden";
	            $('#numFacturaConfirmar').val('');
	            $('#numeroFactura').val('')
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
                        <div class="card">
                            <div class="header">
                                <h4 class="title">Facturas</h4>
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
                                                <select class="form-control border-input" id="cbEstacionamiento" onchange="cambioComboEstacionamiento();">
                                                </select>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                                <label>Carriles</label>
                                                <select class="form-control border-input" id="cbCarrilles">
                                                </select>
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
                                                        <th>Idfacturacion</th>
                                                        <th>IdModulo</th>
                                                        <th>IdEstacionamiento</th>
                                                        <th>Prefijo</th>
                                                        <th>FacturaInicial</th>
                                                        <th>FacturaFinal</th>
                                                        <th>FacturaActual</th>
                                                        <th>NumeroResolucion</th>
                                                        <th>FechaResolucion</th>
                                                        <th>Estado</th>
                                                        <th>FechaFinResolucion</th>
                                                        <th>IdSede</th>
                                                        <th>NombreSede</th>
                                                        <th>NombreEstacionamiento</th>
                                                        <th></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                </tbody>
                                                <tfoot>
                                                    <tr>
                                                        <th>Idfacturacion</th>
                                                        <th>IdModulo</th>
                                                        <th>IdEstacionamiento</th>
                                                        <th>Prefijo</th>
                                                        <th>FacturaInicial</th>
                                                        <th>FacturaFinal</th>
                                                        <th>FacturaActual</th>
                                                        <th>NumeroResolucion</th>
                                                        <th>FechaResolucion</th>
                                                        <th>Estado</th>
                                                        <th>FechaFinResolucion</th>
                                                        <th>IdSede</th>
                                                        <th>NombreSede</th>
                                                        <th>NombreEstacionamiento</th>
                                                        <th></th>
                                                    </tr>
                                                </tfoot>
                                            </table>
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
                                                <button type="button" id="btnNuevo" class="btn btn-default" onclick="btnCrear_click();">
                                                    <span class="ti-plus"></span> Nuevo
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="card">
                            <div class="header">
                                <h4 class="title">Anular Factura</h4>
                            </div>
                            <div class="content">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Numero Factura:</label>
                                            <input type="text" class="form-control border-input" id="numeroFactura">
                                        </div>  
                                    </div>
                                    <div class="col-md-6">
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-8">
                                    </div>
                                    <div class="col-md-4">
                                        <div class="form-group">
                                            <div class="pull-right">
                                                <label></label>
                                                <button type="button" id="btnBuscarFactura" class="btn btn-default" onclick="consultarFacturaAnular();">
                                                    <span class="ti-search"></span> Buscar
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" id="rowFacturaAnular" style='visibility: hidden;'>
                                    <div class="col-md-8" id="myTest">
                                    </div>
                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label>Anular:</label>
                                                    <input type="text" class="form-control border-input" id="numFacturaConfirmar">
                                                </div> 
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="pull-right">
                                                        <label></label>
                                                        <button type="button" class="btn btn-default" onclick="btnCancelarAnulacion();">
                                                            <span class="ti-close"></span> Cancelar
                                                        </button>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <div class="pull-right">
                                                        <label></label>
                                                        <button type="button" class="btn btn-default" onclick="btnAceptarAnulacion();">
                                                            <span class="ti-save"></span> Aceptar
                                                        </button>
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
                                    <label>Codigo</label>
                                    <input type="text" class="form-control border-input" disabled id="codigoModal">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>FacturaActual</label>
                                    <input type="text" class="form-control border-input" disabled id="actualModal">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Prefijo</label>
                                    <input type="text" class="form-control border-input" id="prefijoModal">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>FacturaInicial</label>
                                    <input type="text" class="form-control border-input" id="inicialModal">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>FacturaFinal</label>
                                    <input type="text" class="form-control border-input" id="finalModal">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>NumeroResolucion</label>
                                    <input type="text" class="form-control border-input" id="resolucionModal">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>FechaResolucion
                                    </label>
                                    <input class="form-control border-input" type="text" name="date1" value="" id="fechaModal">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>FechaFin
                                    </label>
                                    <input class="form-control border-input" type="text" name="date2" value="" id="fechaFinModal">
                                </div>
                            </div>
                        </div>
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