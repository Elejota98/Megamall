<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="autorizados.aspx.cs" Inherits="BlockAndPass.AdminWeb.autorizados" %>

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
	            getSecurity();
	            ConsultarAlarmas();
	            ConsultarEstacionamientos(0);
	            ConsultarSedes();
	            cargarComboAutorizaciones();
	            ConsultarSedesModal();
	            ConsultarEstacionamientosComboModal(0);
	            $('#btnConsultar').click();
	            $('#some-name').bootstrapDualListbox({
	                nonSelectedListLabel: 'No Seleccionados',
	                selectedListLabel: 'Seleccionados',
	                filterPlaceHolder: 'Filtro',
	                moveOnSelect: false,
	                showFilterInputs: false,
	                infoText: ''
	            });
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
	            //navPersonasAutorizadas
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
	                    window.location.replace("autorizaciones.aspx");
	                } else {
	                    window.location.replace("template.aspx");
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
	                url: "DataService.asmx/GetItemsAutorizados",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    fillautorizados(msg);
	                    //alert('exito');
	                },
	                error: function () {
	                    alert("Failed to load detalle autorizados");
	                }
	            });
	        }
	        function fillautorizados(msg) {
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
	                    "targets": [9, 8, 3, 10, 11, 6, 17, 18, 19],
	                    "visible": false,
	                    "searchable": false
	                }, {
	                    "orderable": false,
	                    "targets": -1,
	                    "data": null,
	                    "defaultContent": "<a class=\"ti-credit-card\" onclick=\"reponer(this);\"></a>"
	                },
	                {
	                    "orderable": false,
	                    "targets": -2,
	                    "data": null,
	                    "defaultContent": "<a class=\"ti-location-arrow\" onclick=\"btnEstacionamientos_click(this);\"></a>"
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
	            document.getElementById('numeroDocumentoModal').disabled = true;
	            document.getElementById('idSedeModal').disabled = true;
	            document.getElementById('idEstacionamientoModal').disabled = true;
	            document.getElementById('CongelarModal').disabled = true;
	            document.getElementById('placa1Modal').disabled = true;
	            document.getElementById('placa2Modal').disabled = true;
	            document.getElementById('placa3Modal').disabled = true;
	            document.getElementById('placa4Modal').disabled = true;
	            document.getElementById('placa5Modal').disabled = true;
	            document.getElementById('nombreModal').disabled = true;
	            document.getElementById('valorBolsaModal').disabled = true;
	            document.getElementById('telefonoModal').disabled = true;
	            document.getElementById('emailModal').disabled = true;
	            document.getElementById('autorizacionModal').disabled = true;
	            document.getElementById('EmpresaModal').disabled = true;
	            document.getElementById('NitModal').disabled = true;
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerInformacionCargo",
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        if (obj.Resultado[0].Cargo == "COORDINADOR" || obj.Resultado[0].Cargo == "VIGILANTE" || obj.Resultado[0].Cargo == "SUPERVISOR" || obj.Resultado[0].Cargo == "CAJERO") {
	                            document.getElementById('EmpresaModal').disabled = false;
	                            document.getElementById('NitModal').disabled = false;
	                            document.getElementById('placa1Modal').disabled = false;
	                            document.getElementById('placa2Modal').disabled = false;
	                            document.getElementById('placa3Modal').disabled = false;
	                            document.getElementById('telefonoModal').disabled = false;
	                        }
	                        if (obj.Resultado[0].Cargo == "CONTROL INTERNO" || obj.Resultado[0].Cargo == "COORDINADORA DE OPERACIONES") {
	                            document.getElementById('placa1Modal').disabled = false;
	                            document.getElementById('placa2Modal').disabled = false;
	                            document.getElementById('placa3Modal').disabled = false;
	                            document.getElementById('placa4Modal').disabled = false;
	                            document.getElementById('placa5Modal').disabled = false;
	                            document.getElementById('nombreModal').disabled = false;
	                            document.getElementById('valorBolsaModal').disabled = false;
	                            document.getElementById('telefonoModal').disabled = false;
	                            document.getElementById('emailModal').disabled = false;
	                            document.getElementById('autorizacionModal').disabled = false;
	                            document.getElementById('CongelarModal').disabled = false;
	                            document.getElementById('EmpresaModal').disabled = false;
	                            document.getElementById('NitModal').disabled = false;
	                        }
	                    }
	                },
	                error: function () {
	                    alert("Failed to load info Usuario actual");
	                }

	            });



	            var table = $('#sampleTable').DataTable();
	            if (table.rows({ selected: true }).count() > 0) {
	                var count = table.rows({ selected: true }).data();
	                ////alert(count[0][1]);
	                $('#numeroDocumentoModal').val(count[0][1]);
	                $('#autorizacionModal').val(count[0][2]);
	                $('#idEstacionamientoModal').val(count[0][3]);
	                $('#nombreModal').val(count[0][4]);
	                $('#idTarjetaModal').val(count[0][5]);
	                $('#EmpresaModal').val(count[0][17]);
	                $('#NitModal').val(count[0][18]);
	                $('#telefonoModal').val(count[0][10]);
	                $('#emailModal').val(count[0][11]);
	                $('#placa1Modal').val(count[0][12]);
	                $('#placa2Modal').val(count[0][13]);
	                $('#placa3Modal').val(count[0][14]);
	                $('#placa4Modal').val(count[0][15]);
	                $('#placa5Modal').val(count[0][16]);
	                $('#CongelarModal').val(count[0][19]);

	                if (count[0][8] == 'true') {
	                    $("#activoModal").addClass('checked');
	                } else {
	                    $("#activoModal").removeClass('checked');
	                }
	                $('.modal-title').html("Editar");
	                $('#myModal').modal('show');
	            }
	        }
	        function cargarComboAutorizaciones() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaAutorizaciones",
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#autorizacionModal").get(0).options.length = 0;

	                    //$("#autorizacionModal").get(0).options[$("#autorizacionModal").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#autorizacionModal").get(0).options[$("#autorizacionModal").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#autorizacionModal").get(0).options.length = 0;
	                    alert("Failed to load autorizaciones");
	                }

	            });
	        }
	        function btnCrear_click() {
	            document.getElementById('idSedeModal').disabled = true;
	            document.getElementById('idEstacionamientoModal').disabled = true;
	            document.getElementById('numeroDocumentoModal').disabled = false;
	            document.getElementById('placa1Modal').disabled = false;
	            document.getElementById('placa2Modal').disabled = false;
	            document.getElementById('placa3Modal').disabled = false;
	            document.getElementById('placa4Modal').disabled = false;
	            document.getElementById('placa5Modal').disabled = false;
	            document.getElementById('nombreModal').disabled = false;
	            document.getElementById('valorBolsaModal').disabled = false;
	            document.getElementById('telefonoModal').disabled = false;
	            document.getElementById('emailModal').disabled = false;
	            document.getElementById('autorizacionModal').disabled = false;
	            document.getElementById('CongelarModal').disabled = true;
	            $('#numeroDocumentoModal').val('');
	            //$('#autorizacionModal').val('');
	            $('#nombreModal').val('');
	            $('#telefonoModal').val('');
	            $('#emailModal').val('');
	            $('#idTarjetaModal').val('');
	            $('#EmpresaModal').val('');
	            $('#NitModal').val('');
	            $('#placa1Modal').val('');
	            $('#placa2Modal').val('');
	            $('#placa3Modal').val('');
	            $('#placa4Modal').val('');
	            $('#placa5Modal').val('');
	            $('#CongelarModal').val('');
	            
	            $("#activoModal").removeClass('checked');

	            $('.modal-title').html("Crear");
	            $('#myModal').modal('show');
	        }
	        function btnModalGuardar_click() {
	            if ($('.modal-title').html() == 'Crear') {
	                var activo = $('#activoModal').hasClass("checked");
	                var obj;
	                var obj2;
	                $.ajax({
	                    type: "GET",
	                    url: "DataService.asmx/ObtenerValorParametroxNombre?nombre=claveTarjeta&idEstacionamiento=" + $("#idEstacionamientoModal").val(),
	                    data: "",
	                    contentType: "application/json",
	                    dataType: "json",
	                    success: function (msg) {
	                        obj = msg.d;
	                        //alert(obj);
	                        if (obj != '') {

	                            $.ajax({
	                                type: "GET",
	                                url: "http://localhost:8080/ReaderLocalService.svc/reader/createauthcard?password=" + obj + "&idAutho=" + $("#autorizacionModal").val(),
	                                data: "",
	                                contentType: "application/json",
	                                dataType: "json",
	                                success: function (msg) {
	                                    addAuthotodb(msg.idCard);
	                                },
	                                error: function () {
	                                    $('#myModal').modal('toggle');
	                                    confirmaCambios(false, 'Error al consumir servicio local.');
	                                }
	                            });

	                        } else {
	                            $('#myModal').modal('toggle');
	                            confirmaCambios(false, 'No se encontro parametro de clave de tarjeta.');
	                        }
	                    }, error: function () {
	                        $('#myModal').modal('toggle');
	                        confirmaCambios(false, 'Error al consumir servicio.');
	                    }
	                });
	            }





	            else {

	                var activo = $('#activoModal').hasClass("checked");
	                //alert(activo);
	                $.ajax({
	                    type: "GET",
	                    url: "DataService.asmx/ActualizarAutorizado?documento=" + $('#numeroDocumentoModal').val() + "&nombre=" + $('#nombreModal').val() + "&estado=" + activo + "&idAutorizacion=" + $("#autorizacionModal").val() + "&idEStacionamiento=" + $('#idEstacionamientoModal').val() + "&telefono=" + $('#telefonoModal').val() + "&email=" + $('#emailModal').val() + "&placa1=" + $('#placa1Modal').val() + "&placa2=" + $('#placa2Modal').val() + "&placa3=" + $('#placa3Modal').val() + "&placa4=" + $('#placa4Modal').val() + "&placa5=" + $('#placa5Modal').val() + "&NombreEmpresa=" + $('#EmpresaModal').val() + "&Nit=" + $('#NitModal').val() + "&valorBolsa=" + $('#valorBolsaModal').val() + "&Congelar=" + $('#CongelarModal').val(),
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
	        function addAuthotodb(idcard) {
	            var obj;
	            //alert(idcard);
	            var activo = $('#activoModal').hasClass("checked");
	            //alert(activo);
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/CrearAutorizado?documento=" + $('#numeroDocumentoModal').val() + "&nombre=" + $('#nombreModal').val() + "&estado=" + activo + "&idAutorizacion=" + $("#autorizacionModal").val() + "&idEStacionamiento=" + $('#idEstacionamientoModal').val() + "&idTarjeta=" + idcard + "&NombreEmpresa=" + $('#EmpresaModal').val() + "&Nit=" + $('#NitModal').val() + "&telefono=" + $('#telefonoModal').val() + "&email=" + $('#emailModal').val() + "&placa1=" + $('#placa1Modal').val() + "&placa2=" + $('#placa2Modal').val() + "&placa3=" + $('#placa3Modal').val() + "&placa4=" + $('#placa4Modal').val() + "&placa5=" + $('#placa5Modal').val() + "&valorBolsa=" + $('#valorBolsaModal').val() + "&Congelar=" + $('#CongelarModal').val(),
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = JSON.parse(msg.d);
	                    ////alert(obj);
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
	                    confirmaCambios(false, 'Error al consumir servicio CREAR.');
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

	        var idAuto;
	        var idTarjeta;
	        var idEstacio;

	        function reponer(e) {
	            var index = $(e).closest("tr");
	            var data = $('#sampleTable').dataTable().fnGetData(index);
	            var id = data[1];
	            var name = data[4];
	            var card = data[5];
	            idEstacio = data[3];
	            //alert(data[3]);
	            idAuto = id;
	            idTarjeta = card;
	            //alert(placa);
	            $('#resModalRepo').html('Esta a punto de reponer la tarjeta para el autorizado Id = ' + id + " (" + name + "), esta seguro que desea continuar? (No olvide colocar una nueva tarjeta en el lector.)");

	            $('#myModalRepo').modal('show');
	        }
	        function btnModalGuardarRepo_click() {
	            //alert(idAuto + " - " + idTarjeta);
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerValorParametroxNombre?nombre=claveTarjeta&idEstacionamiento=" + idEstacio,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    obj = msg.d;
	                    //alert(obj);
	                    if (obj != '') {

	                        $.ajax({
	                            type: "GET",
	                            url: "http://localhost:8080/ReaderLocalService.svc/reader/createauthcardrepo?password=" + obj + "&idAutho=" + idAuto,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                repoAuthotodb(msg.idCard);
	                            },
	                            error: function () {
	                                $('#myModalRepo').modal('toggle');
	                                confirmaCambios(false, 'Error al consumir servicio.');
	                            }
	                        });

	                    } else {
	                        $('#myModalRepo').modal('toggle');
	                        confirmaCambios(false, 'No se encontro parametro de clave de tarjeta.');
	                    }
	                }, error: function () {
	                    $('#myModalRepo').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }
	            });
	        }
	        function repoAuthotodb(idcard) {
	            var obj;
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ReponerAutorizado?idTarjetaNew=" + idcard + "&idAuthorizado=" + idAuto + "&idTarjetaOld=" + idTarjeta + "&idEstacionamiento=" + idEstacio,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = JSON.parse(msg.d);
	                    ////alert(obj);
	                    if (obj.Exito == true) {
	                        $('#myModalRepo').modal('toggle');
	                        confirmaCambios(true, '');
	                    } else {
	                        $('#myModalRepo').modal('toggle');
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    $('#myModalRepo').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }
	            });
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

	            var documento = data[1];


	            //alert(Documento);

	            $('#documentoModal3').val(documento);


	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/GetItemsPermisosAutorizadoEstacionamiento?documento=" + documento,
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
	                url: "DataService.asmx/UpdatePermisosAutorizadoEstacionamiento?documento=" + $('#documentoModal3').val() + "&permisos=" + $('#some-name').val(),
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
                                    <li id="navPersonasAutorizadas" class="active"><a href="#">Autorizados</a></li>
                                    <li id="navAurotizaciones"><a href="autorizaciones.aspx">Autorizaciones</a></li>
                                </ul>
                            </div>
                        </nav>
                        <div class="card">
                            <div class="header">
                                <h4 class="title">Autorizados</h4>
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
                                                                <th></th>
                                                                <th>Documento</th>
                                                                <th>IdAutorizacion</th>
                                                                <th>idEstacionamiento</th>
                                                                <th>NombreApellidos</th>
                                                                <th style="display:none;">IdTarjeta</th>
                                                                <th>FechaCreacion</th>
                                                                <th>FechaFin</th>
                                                                <th>Estado</th>
                                                                <th>NombreEstacionamiento</th>
                                                                <th>Telefono</th>
                                                                <th>Email</th>
                                                                <th>Placa1</th>
                                                                <th>Placa2</th>
                                                                <th>Placa3</th>
                                                                <th>Placa4</th>
                                                                <th>Placa5</th>
                                                                <th>NombreEmpresa</th>              
                                                                <th>Nit</th>
                                                                <th>Congelar</th>
                                                                <th></th>
                                                                <th></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                        {#foreach $T.d as post}
                                                        <tr>
                                                            <td></td>
                                                            <td>{$T.post.Documento}</td>
                                                            <td>{$T.post.IdAutorizacion}</td> 
                                                            <td>{$T.post.idEstacionamiento}</td>
                                                            <td>{$T.post.NombreApellidos}</td>
                                                            <td style="display:none;">{$T.post.IdTarjeta}</td>
                                                            <td>{$T.post.FechaCreacion}</td>
                                                            <td>{$T.post.FechaFin}</td>
                                                            <td>{$T.post.Estado}</td>
                                                            <td>{$T.post.Nombre}</td>
                                                            <td>{$T.post.Telefono}</td>
                                                            <td>{$T.post.Email}</td>
                                                            <td>{$T.post.Placa1}</td>
                                                            <td>{$T.post.Placa2}</td>
                                                            <td>{$T.post.Placa3}</td>
                                                            <td>{$T.post.Placa4}</td>
                                                            <td>{$T.post.Placa5}</td>
                                                            <td>{$T.post.NombreEmpresa}</td>
                                                            <td>{$T.post.Nit}</td>
                                                            <td>{$T.post.Congelar}</td>
                                                            <td></td>
                                                            <td></td>
                                                        </tr>
                                                        {#/for}
                                                        </tbody>
                                                        
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

    <div id="myModalRepo" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title"><span class="icon-warning ti-thumb-up"></span> Reponer Autorizado</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-12" id="resModalRepo">
                                
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
                                <button type="button" id="Button4" class="btn btn-default" onclick="btnModalGuardarRepo_click();">
                                    <span class="ti-save"></span>Guardar
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

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
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Numero de Documento</label>
                                    <input type="text" class="form-control border-input" id="numeroDocumentoModal">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Autorizacion</label>
                                    <select class="form-control border-input" id="autorizacionModal">
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Nombre y Apellido</label>
                                    <input type="text" class="form-control border-input" id="nombreModal">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>IdTarjeta</label>
                                    <input type="text" class="form-control border-input" disabled id="idTarjetaModal">
                                </div>
                            </div>
                        </div>

                          <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Nombre Empresa</label>
                                    <input type="text" class="form-control border-input" id="EmpresaModal">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Nit</label>
                                    <input type="text" class="form-control border-input"  id="NitModal">
                                </div>
                            </div>
                        </div>



                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Telefono</label>
                                    <input type="text" class="form-control border-input" id="telefonoModal">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Email</label>
                                    <input type="text" class="form-control border-input" id="emailModal">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Placa1</label>
                                    <input type="text" class="form-control border-input" id="placa1Modal">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Placa2</label>
                                    <input type="text" class="form-control border-input" id="placa2Modal">
                                </div>
                            </div>
                        </div>
                        
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Placa3</label>
                                    <input type="text" class="form-control border-input" id="placa3Modal">
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Placa4</label>
                                    <input type="text" class="form-control border-input" id="placa4Modal">
                                </div>
                            </div>
                        </div>



                         <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Placa5</label>
                                    <input type="text" class="form-control border-input" id="placa5Modal">
                                </div>
                            </div>
                             <div class="col-md-6">
                                <div class="form-group">
                                    <label>Valor Bolsa</label>
                                    <input type="text" class="form-control border-input" id="valorBolsaModal">
                                </div>
                            </div>
                           
                        </div>

                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Congelar</label>
                                    <input type="number" class="form-control border-input" id="CongelarModal">
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

    <div id="myModal3" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Estacionamientos</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label>Documento</label>
                                    <input type="text" class="form-control border-input" disabled id="documentoModal3">
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
                                <button type="button" id="Button5" class="btn btn-default" onclick="btnGuardarPermisosEstacionamientos_click();">
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
                                <button type="button" data-dismiss="modal" class="btn btn-default">
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
</html>--%>