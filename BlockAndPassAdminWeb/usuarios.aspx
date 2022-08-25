<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="usuarios.aspx.cs" Inherits="BlockAndPass.AdminWeb.usuarios" %>

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
	        ConsultarEstacionamientosComboModal();
	        ObtenerDatosUsuario();
	        
	        $('#btnConsultar').click();

	        $('#some-name').bootstrapDualListbox({
	            nonSelectedListLabel: 'No Seleccionados',
	            selectedListLabel: 'Seleccionados',
	            filterPlaceHolder: 'Filtro',
	            moveOnSelect: false,
	            showFilterInputs: false,
	            infoText: ''
	        });

	        $('#some-name2').bootstrapDualListbox({
	            nonSelectedListLabel: 'No Seleccionados',
	            selectedListLabel: 'Seleccionados',
	            filterPlaceHolder: 'Filtro',
	            moveOnSelect: false,
	            showFilterInputs: false,
	            infoText: ''
	        });

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
	    function mifuncion() {
	        $("#sampleTable").dataTable({
	            "oLanguage": {
	                "sZeroRecords": "No se encuentra infromacion",
	                "sLoadingRecords": "Cargando...",
	                "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ registros",
	                "sInfoEmpty": "Mostrando 0 a 0 de 0 registros",
	                "sLengthMenu": "Mostrando _MENU_ registros por pagina",
	            },
	            "columnDefs": [{
	                "targets": [4,6,7,8],
	                "visible": false,
	                "searchable": false
	            },
	            {
	                "orderable": false,
	                "targets": -1,
	                "data": null,
	                "defaultContent": "<a class=\"ti-pencil-alt2\" onclick=\"btnEditar_click(this);\"></a>"
	            }, {
	                "orderable": false,
	                "targets": -2,
	                "data": null,
	                "defaultContent": "<a class=\"ti-ruler-alt\" onclick=\"btnPermisos_click(this);\"></a>"
	            },
	            {
	                "orderable": false,
	                "targets": -3,
	                "data": null,
	                "defaultContent": "<a class=\"ti-location-arrow\" onclick=\"btnEstacionamientos_click(this);\"></a>"
	            }, {
	                "orderable": false,
	                "targets": -4,
	                "data": null,
	                "defaultContent": "<a class=\"ti-shopping-cart-full\" onclick=\"btnConvenios_click(this);\"></a>"
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
	            "sAjaxSource": "DataService.asmx/GetItemsUsuarios",
	            //"bJQueryUI": true,
	            //"sPaginationType": "full_numbers",
	            "bDeferRender": true,
	            "fnServerParams": function (aoData) {
	                aoData.push();
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
	        document.getElementById('documentoModal').disabled = true;
	        var index = $(e).closest("tr");
	        var data = $('#sampleTable').dataTable().fnGetData(index);

	        var Documento = data[0];
	        //var IdEstacionamiento = data[1];
	        var Nombres = data[1];
	        var Apellidos = data[2];
	        var Usuario = data[3];
	        var Cargo = data[5];
	        var Estado = data[9];
	        
	        $('#documentoModal').val(Documento);
	        //$('#idEstacionamientoModal').val(IdEstacionamiento);
	        $('#nombresModal').val(Nombres);
	        $('#apellidosModal').val(Apellidos);
	        $('#usuarioModal').val(Usuario);
	        $('#cargoModal').val(Cargo);
	        //alert(Estado);
	        if (Estado == 'True') {
	            $("#activoModal").addClass('checked');
	        } else {
	            $("#activoModal").removeClass('checked');
	        }
	        $('.modal-title').html("Editar");
	        $('#myModal').modal('show');
	    }
	    function btnCrear_click() {
	        document.getElementById('documentoModal').disabled = false;
	        $('#documentoModal').val('');
	        $('#idEstacionamientoModal').val('');
	        $('#nombresModal').val('');
	        $('#apellidosModal').val('');
	        //$('#idEstacionamientoModal').val('');
	        $('#usuarioModal').val('');
	        $('#cargoModal').val('');
	        $("#activoModal").removeClass('checked');

	        $('.modal-title').html("Crear");
	        $('#myModal').modal('show');
	    }	    
	    function btnModalGuardar_click() {
	        if ($('.modal-title').html() == 'Crear') {
	            var activo = $('#activoModal').hasClass("checked");
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/CrearUsuario?documento=" + $('#documentoModal').val() + "&nombres=" + $('#nombresModal').val() + "&apellidos=" + $('#apellidosModal').val() + "&estado=" + activo + "&usuario=" + $('#usuarioModal').val() + "&cargo=" + $('#cargoModal').val(),
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
	                url: "DataService.asmx/ActualizarUsuario?documento=" + $('#documentoModal').val() + "&nombres=" + $('#nombresModal').val() + "&apellidos=" + $('#apellidosModal').val() + "&estado=" + activo + "&usuario=" + $('#usuarioModal').val() + "&cargo=" + $('#cargoModal').val(),
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
	    function btnPermisos_click(e) {
	        var index = $(e).closest("tr");
	        var data = $('#sampleTable').dataTable().fnGetData(index);

	        var Documento = data[0];
	        var Usuario = data[3];

	        $('#documentoModal2').val(Documento);
	        $('#usuarioModal2').val(Usuario);

	        $.ajax({
	            type: "GET",
	            url: "DataService.asmx/ObtenerPermisos1?persona=" + Documento,
	            data: "",
	            contentType: "application/json",
	            dataType: "json",
	            success: function (msg) {
	                CreateArbol(msg.d);
	            },
	            error: function () {
	                alert("Failed to read");
	            }
	        });
	    }
	    function CreateArbol(dataTr) {

	        $('#treeview-checkbox-demo').html("<ul>" +
                                        "<li data-value=\"navPrincipal\">Principal" +
                                            "<ul>" +
                                                "<li data-value=\"cardCuposDisponibles\">Cupos Disponibles</li>" +
                                                "<li data-value=\"cardRecaudoDiario\">Recaudo Diario</li>" +
                                                "<li data-value=\"cardErroresDiarios\">Errores Diarios</li>" +
                                                "<li data-value=\"cardIngresosVehiculosTabla\">Tabla Ingresos Vehiculos</li>" +
                                                "<li data-value=\"cardIngresosDineroTabla\">Tabla Ingresos Dinero</li>" +
                                            "</ul>" +
                                        "</li>" +
                                        "<li data-value=\"navTransacciones\">Transacciones</li>"+
                                        "<li data-value=\"navCargas\">Cargas</li>" +

                                        "<li data-value=\"navBorrarTarjetas\">Eliminar inf tarjetas</li>" +

                                        "<li data-value=\"navArqueos\">Arqueos</li>"+
                                        "<li data-value=\"navAutorizados\">Autorizados" +
                                            "<ul>" +
                                                "<li data-value=\"navPersonasAutorizadas\">Personas Autorizadas</li>" +
                                                "<li data-value=\"navAurotizaciones\">Autorizaciones</li>" +
                                            "</ul>" +
                                        "</li>"+
                                        "<li data-value=\"navReportes\">Reportes" +
                                            "<ul>" +
                                                "<li data-value=\"navReportesOperacion\">Operacion</li>" +
                                                "<li data-value=\"navReportesNegocio\">Negocio</li>" +
                                                //"<li data-value=\"navReportesFuncionales\">Funcionales</li>" +
                                                //"<li data-value=\"navReportesLogs\">Logs</li>" +
                                                "<li data-value=\"navReportesAuditorias\">Auditorias</li>" +
                                                //"<li data-value=\"navReportesAlarmas\">Alarmas</li>" +
                                                "<li data-value=\"navReportesEstadisticos\">Estadisticos</li>" +
                                                //"<li data-value=\"navReportesIContable\">I. Contable</li>" +
                                            "</ul>" +
                                        "</li>" +
                                        //"<li data-value=\"navPPM\">PPM"+
                                        //    "<ul>" +
                                        //        "<li data-value=\"btnLeer\">Leer</li>" +
                                        //        "<li data-value=\"btnPagar\">Pagar</li>" +
                                        //        "<li data-value=\"btnConvenio\">Convenio</li>" +
                                        //        "<li data-value=\"btnCortesia\">Cortesia</li>" +
                                        //        "<li data-value=\"btnLimpiar\">Limpiar</li>" +
                                        //        "<li data-value=\"btnArqueo\">Arqueo</li>" +
                                        //        "<li data-value=\"btnCarga\">Carga</li>" +
                                        //    "</ul>" +
                                        //"</li>" +
                                        "<li data-value=\"navFacturasManuales\">F. Manual</li>" +
                                        "<li data-value=\"navConsignaciones\">Consignaciones</li>" +
                                        "<li data-value=\"navApertura\">Apertura Remota</li>" +
                                        "<li data-value=\"rnavAlarmas\">Alarmas</li>"+
                                        "<li data-value=\"rnavConfiguracion\">Configuracion"+
                                            "<ul>"+
                                                "<li data-value=\"rnavConfiguracion-sistema\">Sistema</li>"+
                                                "<li data-value=\"rnavConfiguracion-usuarios\">Usuarios</li>"+
	                                            "<li data-value=\"rnavConfiguracion-tarifas\">Tarifas</li>"+
	                                            "<li data-value=\"rnavConfiguracion-convenios\">Convenios</li>" +
                                                "<li data-value=\"rnavConfiguracion-eventos\">Eventos</li>" +
	                                            "<li data-value=\"rnavConfiguracion-parametros\">Parametros</li>" +
                                                "<li data-value=\"rnavConfiguracion-facturas\">Facturas</li>" +
                                            "</ul>"+
                                        "</li>" +
                                    "</ul>");
	        $('#treeview-checkbox-demo').treeview({
	            debug: true,
	            data: dataTr
	        });
	        $('#myModal2').modal('show');
	    }
	    function btnGuardarPermisos_Click() {
	        $.ajax({
	            type: "GET",
	            url: "DataService.asmx/UpdatePermisos?documento=" + $('#documentoModal2').val() + "&usuario=" + $('#usuarioModal2').val() + "&permisos=" + $('#treeview-checkbox-demo').treeview('selectedValues'),
	            data: "{}",
	            contentType: "application/json",
	            dataType: "json",
	            success: function (msg) {
	                var obj = jQuery.parseJSON(msg.d);
	                if (obj.Exito == true) {
	                    location.reload();
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
	    function btnEstacionamientos_click(e) {

	        var index = $(e).closest("tr");
	        var data = $('#sampleTable').dataTable().fnGetData(index);

	        var Documento = data[0];
	        var Usuario = data[3];
	        //alert(Documento);
	        $('#documentoModal3').val(Documento);
	        $('#usuarioModal3').val(Usuario);

	        $.ajax({
	            type: "GET",
	            url: "DataService.asmx/GetItemsPermisosUsuarioEstacionamiento?documento=" + Documento,
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
	        $.ajax({
	            type: "GET",
	            url: "DataService.asmx/UpdatePermisosUsuarioEstacionamiento?documento=" + $('#documentoModal3').val() + "&permisos=" + $('#some-name').val(),
	            data: "{}",
	            contentType: "application/json",
	            dataType: "json",
	            success: function (msg) {
	                var obj = jQuery.parseJSON(msg.d);
	                if (obj.Exito == true) {
	                    location.reload();
	                } else {
	                    $('#myModal3').modal('toggle');
	                    confirmaCambios(false, obj.ErrorMessage);
	                }
	            },
	            error: function () {
	                $('#myModal3').modal('toggle');
	                confirmaCambios(false, 'Error al consumir servicio.');
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
	    function btnConvenios_click(e) {
	        var index = $(e).closest("tr");
	        var data = $('#sampleTable').dataTable().fnGetData(index);

	        var Documento = data[0];
	        var Usuario = data[3];
	        //alert(Documento);
	        $('#documentoModal4').val(Documento);
	        $('#usuarioModal4').val(Usuario);

	        $.ajax({
	            type: "GET",
	            url: "DataService.asmx/GetItemsPermisosUsuarioConvenio?documento=" + Documento,
	            data: "{}",
	            contentType: "application/json",
	            dataType: "json",
	            success: function (msg) {
	                fillpermisosconvenios(msg);
	            },
	            error: function () {
	                alert("Failed to load permisos de estacionamiento");
	            }

	        });
	    }
	    function fillpermisosconvenios(datos) {
	        $('#some-name2')
                .find('option')
                .remove()
                .end();

	        $.each(datos.d, function (index, item) {
	            //alert(item.Estado);
	            if (item.Estado == 'true') {
	                $('#some-name2').append('<option value="' + item.IdConvenio + '" selected="selected">' + item.Nombre + '</option>');
	            } else {
	                $('#some-name2').append('<option value="' + item.IdConvenio + '">' + item.Nombre + '</option>');
	            }
	        });

	        $('#some-name2').bootstrapDualListbox('refresh', true);


	        $('#myModal4').modal('show');
	    }
	    function btnGuardarPermisosConvenios_click() {
	        //alert($('#some-name2').val());
	        $.ajax({
	            type: "GET",
	            url: "DataService.asmx/UpdatePermisosUsuarioConvenios?documento=" + $('#documentoModal4').val() + "&permisos=" + $('#some-name2').val(),
	            data: "{}",
	            contentType: "application/json",
	            dataType: "json",
	            success: function (msg) {
	                var obj = jQuery.parseJSON(msg.d);
	                if (obj.Exito == true) {
	                    $('#myModal4').modal('toggle');
	                    confirmaCambios(true, '');
	                } else {
	                    $('#myModal4').modal('toggle');
	                    confirmaCambios(false, obj.ErrorMessage);
	                }
	            },
	            error: function () {
	                $('#myModal4').modal('toggle');
	                confirmaCambios(false, 'Error al consumir servicio.');
	            }
	        });
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
                                <h4 class="title">Usuarios</h4>
                            </div>
                            <div class="content">
                                <%--<div class="row">
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
                                    </div>
                                </div>--%>
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
                                                        <th>Documento</th>
                                                        <th>Nombres</th>
                                                        <th>Apellidos</th>
                                                        <th>Usuario</th>
                                                        <th>Contraseña</th>
                                                        <th>Cargo</th>
                                                        <th>UsuarioCreador</th>
                                                        <th>FechaCreacion</th>
                                                        <th>IdTarjeta</th>
                                                        <th>Estado</th>
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
                                                        <th>Documento</th>
                                                        <th>Nombres</th>
                                                        <th>Apellidos</th>
                                                        <th>Usuario</th>
                                                        <th>Clave</th>
                                                        <th>Cargo</th>
                                                        <th>Creador</th>
                                                        <th>FechaCreacion</th>
                                                        <th>IdTarjeta</th>
                                                        <th>Estado</th>
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
                                    <label>Documento</label>
                                    <input type="text" class="form-control border-input" disabled id="documentoModal">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Usuario</label>
                                    <input type="text" class="form-control border-input" id="usuarioModal">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Nombres</label>
                                    <input type="text" class="form-control border-input" id="nombresModal">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Apellidos</label>
                                    <input type="text" class="form-control border-input" id="apellidosModal">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Cargo</label>
                                    <input type="text" class="form-control border-input" id="cargoModal">
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
                                    <span class="ti-save"></span>Guardar
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
                    <h4 class="modal-title">Permisos</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Documento</label>
                                    <input type="text" class="form-control border-input" disabled id="documentoModal2">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Usuario</label>
                                    <input type="text" class="form-control border-input" disabled id="usuarioModal2">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div id="treeview-checkbox-demo">
                                    <%--<ul>
                                        <li data-value="navTransacciones">Transacciones</li>
                                        <li data-value="navCargas">Cargas</li>

                                        <li data-value="navBorrarTarjetas">Tarjetas</li>

                                        <li data-value="navArqueos">Arqueos</li>
                                        <li data-value="navAutorizados">Autorizados</li>
                                        <li data-value="navReportes">Reportes</li>
                                        <li data-value="navPPM">PPM</li>
                                        <li data-value="rnavAlarmas">Alarmas</li>
                                        <li data-value="rnavConfiguracion">Configuracion
                                            <ul>
                                                <li data-value="rnavConfiguracion-sistema">Sistema</li>
                                                <li data-value="rnavConfiguracion-usuarios">Usuarios</li>
                                                <li data-value="rnavConfiguracion-tarifas">Tarifas</li>
                                                <li data-value="rnavConfiguracion-convenios">Convenios</li>
                                                <li data-value="rnavConfiguracion-parametros">Parametros</li>
                                            </ul>
                                        </li>
                                    </ul>--%>
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
                                <button type="button" id="Button2" class="btn btn-default" onclick="btnGuardarPermisos_Click();">
                                    <span class="ti-save"></span>Guardar
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
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Documento</label>
                                    <input type="text" class="form-control border-input" disabled id="documentoModal3">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Usuario</label>
                                    <input type="text" class="form-control border-input" disabled id="usuarioModal3">
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

    <div id="myModal4" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Convenios</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Documento</label>
                                    <input type="text" class="form-control border-input" disabled id="documentoModal4">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Usuario</label>
                                    <input type="text" class="form-control border-input" disabled id="usuarioModal4">
                                </div>
                            </div>
                        </div>
                        <div id="dual-list-box2" class="form-group row">
                            <div class="col-md-12">
                                <select id="some-name2" multiple="multiple" size="10">
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
                                <button type="button" id="Button6" class="btn btn-default" onclick="btnGuardarPermisosConvenios_click();">
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
                                <button type="button" id="Button3" class="btn btn-default" onclick="btnGuardarModalCambioClave_Click();">
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