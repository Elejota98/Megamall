<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="alarmas.aspx.cs" Inherits="BlockAndPass.AdminWeb.alarmas" %>

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
                ConsultarEstacionamientos();
                ObtenerDatosUsuario();
                //ConsultarCarriles(0);
                //var dt = new Date();
                //var month = parseInt(dt.getMonth() + 1);
                //$('*[name=date]').appendDtpicker({
                //    "locale": "es",
                //    "minuteInterval": 15,
                //    "current": dt.getDate() + "-" + month + "-" + dt.getFullYear() + " " + "00:00",
                //    "closeOnSelected": true,
                //    "todayButton": false,
                //});
                //$('*[name=date]').change(function () {

                //});
                //$('*[name=date2]').appendDtpicker({
                //    "locale": "es",
                //    "minuteInterval": 15,
                //    "current": dt.getDate() + "-" + month + "-" + dt.getFullYear() + " " + dt.getHours() + ":" + dt.getMinutes(),
                //    //"current": "2012-3-4 12:30",
                //    "closeOnSelected": true,
                //    "todayButton": false,
                //});
                //$('#date2').change(function () {

                //});
                $('#btnConsultar').click();
                $('#btnConsultar2').click();
                $('#btnConsultar3').click();
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
            function ConsultarEstacionamientos() {
                $.ajax({
                    type: "GET",
                    url: "DataService.asmx/ObtenerListaEstacionamientos",
                    data: "{}",
                    contentType: "application/json",
                    dataType: "json",
                    success: function (msg) {
                        $("#cbEstacionamiento").get(0).options.length = 0;
                        $("#cbEstacionamiento2").get(0).options.length = 0;
                        $("#cbEstacionamiento3").get(0).options.length = 0;

                        $("#cbEstacionamiento").get(0).options[$("#cbEstacionamiento").get(0).options.length] = new Option('Todos', 0);
                        $.each(msg.d, function (index, item) {
                            $("#cbEstacionamiento").get(0).options[$("#cbEstacionamiento").get(0).options.length] = new Option(item.Display, item.Value);
                        });

                        $("#cbEstacionamiento2").get(0).options[$("#cbEstacionamiento2").get(0).options.length] = new Option('Todos', 0);
                        $.each(msg.d, function (index, item) {
                            $("#cbEstacionamiento2").get(0).options[$("#cbEstacionamiento2").get(0).options.length] = new Option(item.Display, item.Value);
                        });

                        $("#cbEstacionamiento3").get(0).options[$("#cbEstacionamiento3").get(0).options.length] = new Option('Todos', 0);
                        $.each(msg.d, function (index, item) {
                            $("#cbEstacionamiento3").get(0).options[$("#cbEstacionamiento3").get(0).options.length] = new Option(item.Display, item.Value);
                        });
                    },
                    error: function () {
                        $("#cbEstacionamiento").get(0).options.length = 0;
                        $("#cbEstacionamiento2").get(0).options.length = 0;
                        $("#cbEstacionamiento3").get(0).options.length = 0;
                        alert("Failed to load estacionamientos");
                    }

                });
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
                            "targets": [1,9, 10, 11, 12],
                            "visible": false,
                            "searchable": false
                        },
                        {
                            'searchable': false,
                            'orderable': false,
                            'className': 'dt-body-center',
                            "targets": 0,
                            "data": null,
                            "defaultContent": "<a class=\"ti-search\" onclick=\"funcionclick(this);\"></a>"
                        }
                    ],
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
                    "sAjaxSource": "DataService.asmx/GetItemsAlarmasSinAtender",
                    //"bJQueryUI": true,
                    //"sPaginationType": "full_numbers",
                    "bDeferRender": true,
                    "fnServerParams": function (aoData) {
                        aoData.push({ "name": "iIdEstacionamiento", "value": $("#cbEstacionamiento").val() })
                    },
                    "createdRow": function (row, data, index) {
                        if (data[7] == 1) {
                            $('td', row).addClass('alert-success');
                        }
                        else if (data[7] == 2) {
                            $('td', row).addClass('alert-warning');
                        }
                        else if (data[7] == 3) {
                            $('td', row).addClass('alert-danger');
                        }
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
                $('#cbEstacionamiento').focus()
                $('#cbEstacionamiento').select()
            }
            function mifuncion2() {
                $("#sampleTable2").dataTable({
                    "oLanguage": {
                        "sZeroRecords": "No se encuentra infromacion",
                        "sLoadingRecords": "Cargando...",
                        "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ registros",
                        "sInfoEmpty": "Mostrando 0 a 0 de 0 registros",
                        "sLengthMenu": "Mostrando _MENU_ registros por pagina",
                    },
                    "columnDefs": [
                        {
                            "targets": [1,9, 8, 11, 12],
                            "visible": false,
                            "searchable": false
                        },
                        {
                            "orderable": false,
                            "targets": 0,
                            "data": null,
                            "defaultContent": "<a class=\"ti-search\" onclick=\"funcionclick2(this);\"></a>"
                        }
                    ],
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
                    "sAjaxSource": "DataService.asmx/GetItemsAlarmasSinSolucionar",
                    //"bJQueryUI": true,
                    //"sPaginationType": "full_numbers",
                    "bDeferRender": true,
                    "fnServerParams": function (aoData) {
                        aoData.push({ "name": "iIdEstacionamiento", "value": $("#cbEstacionamiento2").val() })
                    },
                    "createdRow": function (row, data, index) {
                        if (data[7] == 1) {
                            $('td', row).addClass('alert-success');
                        }
                        else if (data[7] == 2) {
                            $('td', row).addClass('alert-warning');
                        }
                        else if (data[7] == 3) {
                            $('td', row).addClass('alert-danger');
                        }
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
                                            $("#sampleTable2").show();
                                        }
                        });
                    }
                });
                $('#cbEstacionamiento2').focus()
                $('#cbEstacionamiento2').select()
            }
            function mifuncion3() {
                $("#sampleTable3").dataTable({
                    "oLanguage": {
                        "sZeroRecords": "No se encuentra infromacion",
                        "sLoadingRecords": "Cargando...",
                        "sInfo": "Mostrando _START_ a _END_ de _TOTAL_ registros",
                        "sInfoEmpty": "Mostrando 0 a 0 de 0 registros",
                        "sLengthMenu": "Mostrando _MENU_ registros por pagina",
                    },
                    "columnDefs": [
                        {
                            "targets": [1,8, 9, 10, 11, 12],
                            "visible": false,
                            "searchable": false
                        },
                        {
                            "orderable": false,
                            "targets": 0,
                            "data": null,
                            "defaultContent": "<a class=\"ti-search\" onclick=\"funcionclick3(this);\"></a>"
                        }
                    ],
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
                    "sAjaxSource": "DataService.asmx/GetItemsAlarmasSolucionadas",
                    //"bJQueryUI": true,
                    //"sPaginationType": "full_numbers",
                    "bDeferRender": true,
                    "fnServerParams": function (aoData) {
                        aoData.push({ "name": "iIdEstacionamiento", "value": $("#cbEstacionamiento2").val() })
                    },
                    "createdRow": function (row, data, index) {
                        if (data[7] == 1) {
                            $('td', row).addClass('alert-success');
                        }
                        else if (data[7] == 2) {
                            $('td', row).addClass('alert-warning');
                        }
                        else if (data[7] == 3) {
                            $('td', row).addClass('alert-danger');
                        }
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
                                            $("#sampleTable3").show();
                                        }
                        });
                    }
                });
                $('#cbEstacionamiento3').focus()
                $('#cbEstacionamiento3').select()
            }
            function funcionclick(e) {
                var index = $(e).closest("tr");
                var data = $('#sampleTable').dataTable().fnGetData(index);

                $('#idAlarmaModal').val(data[1]);
                $('#idModuloModal').val(data[2]);
                $('#idEstacionamientoModal').val(data[3]);
                $('#tipoModal').val(data[4]);
                $('#parteModal').val(data[5]);
                $('#descripcionModal').val(data[6]);
                $('#nivelModal').val(data[7]);
                $('#fechaModal').val(data[8]);
                $('#comment').val('');
                $('#atencionModal').val('');
                $('#usuarioModal').val('');
                $('#solucionModal').val('');

                $('#comment').prop('disabled', false);
                $('#Button1').prop('disabled', false);
                $('#myModal').modal('show');
            }
            function funcionclick2(e) {
                var index = $(e).closest("tr");
                var data = $('#sampleTable2').dataTable().fnGetData(index);

                $('#idAlarmaModal').val(data[1]);
                $('#idModuloModal').val(data[2]);
                $('#idEstacionamientoModal').val(data[3]);
                $('#tipoModal').val(data[4]);
                $('#parteModal').val(data[5]);
                $('#descripcionModal').val(data[6]);
                $('#nivelModal').val(data[7]);
                $('#fechaModal').val(data[8]);
                $('#comment').val(data[9]);
                $('#atencionModal').val(data[10]);
                $('#usuarioModal').val(data[11]);
                $('#solucionModal').val(data[12]);

                $('#comment').prop('disabled', true);
                $('#Button1').prop('disabled', true);
                $('#myModal').modal('show');
            }
            function funcionclick3(e) {
                var index = $(e).closest("tr");
                var data = $('#sampleTable3').dataTable().fnGetData(index);

                $('#idAlarmaModal').val(data[1]);
                $('#idModuloModal').val(data[2]);
                $('#idEstacionamientoModal').val(data[3]);
                $('#tipoModal').val(data[4]);
                $('#parteModal').val(data[5]);
                $('#descripcionModal').val(data[6]);
                $('#nivelModal').val(data[7]);
                $('#fechaModal').val(data[8]);
                $('#comment').val(data[9]);
                $('#atencionModal').val(data[10]);
                $('#usuarioModal').val(data[11]);
                $('#solucionModal').val(data[12]);

                $('#comment').prop('disabled', true);
                $('#Button1').prop('disabled', true);
                $('#myModal').modal('show');
            }
            function AtenderAlarma() {
                if ($("#comment").val() != '') {
                    $.ajax({
                        type: "GET",
                        url: "DataService.asmx/AtenderAlarma?idAlarma=" + $("#idAlarmaModal").val() + "&descripcion=" + $("#comment").val(),
                        data: "",
                        contentType: "application/json",
                        dataType: "json",
                        success: function (msg) {
                            var obj = jQuery.parseJSON(msg.d);
                            if (obj.Exito==true) {
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
                    $('#myModal').modal('toggle');
                    confirmaCambios(false, 'Ingrese un comentario para atender la alarma.');
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
                $('#sampleTable2').dataTable().fnDestroy();
                $('#sampleTable3').dataTable().fnDestroy();
                mifuncion();
                mifuncion2();
                mifuncion3();
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
                            <img src="assets/img/logoparquearse.jpg" class="img-rounded" width="150" height="60">
                        </a>
                    </div>
                    <ul class="nav">
                        <li>
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
                            <a href="reportes.aspx">
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
                        <div class="row">
                            <div class="col-md-12">
                                <div class="card">
                                    <div class="header">
                                        <h4 class="title">Alarmas</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <ul class="nav nav-pills nav-justified">
                                                    <li class="active"><a data-toggle="tab" href="#home">Sin Atender</a></li>
                                                    <li><a data-toggle="tab" href="#menu1">Sin Solucionar</a></li>
                                                    <li><a data-toggle="tab" href="#menu2">Solucionadas</a></li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="tab-content">
                                            <div id="home" class="tab-pane fade in active">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <h3>Alarmas sin atender</h3>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Estacionamientos</label>
                                                            <select class="form-control border-input" id="cbEstacionamiento" onchange="cambioComboEstacionamiento();">
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-8">
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
                                                                    <span class="ti-search"></span>Consultar
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
                                                                        <th></th>
                                                                        <th>IdAlarma</th>
                                                                        <th>IdModulo</th>
                                                                        <th>IdEstacionamiento</th>
                                                                        <th>TipoError</th>
                                                                        <th>Parte</th>
                                                                        <th>Descripcion</th>
                                                                        <th>NivelError</th>
                                                                        <th>FechaRegistro</th>
                                                                        <th>Observacion</th>
                                                                        <th>FechaAtencion</th>
                                                                        <th>UsuarioObservacion</th>
                                                                        <th>FechaSolucion</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                </tbody>
                                                                <tfoot>
                                                                    <tr>
                                                                        <th></th>
                                                                        <th>IdAlarma</th>
                                                                        <th>IdModulo</th>
                                                                        <th>IdEstacionamiento</th>
                                                                        <th>TipoError</th>
                                                                        <th>Parte</th>
                                                                        <th>Descripcion</th>
                                                                        <th>NivelError</th>
                                                                        <th>FechaRegistro</th>
                                                                        <th>Observacion</th>
                                                                        <th>FechaAtencion</th>
                                                                        <th>UsuarioObservacion</th>
                                                                        <th>FechaSolucion</th>
                                                                    </tr>
                                                                </tfoot>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>

                                            </div>
                                            <div id="menu1" class="tab-pane fade">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <h3>Alarmas sin solucionar</h3>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Estacionamientos</label>
                                                            <select class="form-control" id="cbEstacionamiento2" onchange="cambioComboEstacionamiento();">
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-8">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-8">
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <div class="pull-right">
                                                                <label></label>
                                                                <button type="button" id="btnConsultar2" class="btn btn-default" onclick="mifuncion2();">
                                                                    <span class="ti-search"></span>Consultar
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="content table-responsive table-full-width">
                                                            <table id="sampleTable2" class="widthFull fontsize10 displayNone">
                                                                <thead>
                                                                    <tr>
                                                                        <th></th>
                                                                        <th>IdAlarma</th>
                                                                        <th>IdModulo</th>
                                                                        <th>IdEstacionamiento</th>
                                                                        <th>TipoError</th>
                                                                        <th>Parte</th>
                                                                        <th>Descripcion</th>
                                                                        <th>NivelError</th>
                                                                        <th>FechaRegistro</th>
                                                                        <th>Observacion</th>
                                                                        <th>FechaAtencion</th>
                                                                        <th>UsuarioObservacion</th>
                                                                        <th>FechaSolucion</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                </tbody>
                                                                <tfoot>
                                                                    <tr>
                                                                        <th></th>
                                                                        <th>IdAlarma</th>
                                                                        <th>IdModulo</th>
                                                                        <th>IdEstacionamiento</th>
                                                                        <th>TipoError</th>
                                                                        <th>Parte</th>
                                                                        <th>Descripcion</th>
                                                                        <th>NivelError</th>
                                                                        <th>FechaRegistro</th>
                                                                        <th>Observacion</th>
                                                                        <th>FechaAtencion</th>
                                                                        <th>UsuarioObservacion</th>
                                                                        <th>FechaSolucion</th>
                                                                    </tr>
                                                                </tfoot>
                                                            </table>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div id="menu2" class="tab-pane fade">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <h3>Alarmas solucionadas</h3>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <label>Estacionamientos</label>
                                                            <select class="form-control" id="cbEstacionamiento3" onchange="cambioComboEstacionamiento();">
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-8">
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-8">
                                                    </div>
                                                    <div class="col-md-4">
                                                        <div class="form-group">
                                                            <div class="pull-right">
                                                                <label></label>
                                                                <button type="button" id="btnConsultar3" class="btn btn-default" onclick="mifuncion3();">
                                                                    <span class="ti-search"></span>Consultar
                                                                </button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-12">
                                                        <div class="content table-responsive table-full-width">
                                                            <table id="sampleTable3" class="widthFull fontsize10 displayNone">
                                                                <thead>
                                                                    <tr>
                                                                        <th></th>
                                                                        <th>IdAlarma</th>
                                                                        <th>IdModulo</th>
                                                                        <th>IdEstacionamiento</th>
                                                                        <th>TipoError</th>
                                                                        <th>Parte</th>
                                                                        <th>Descripcion</th>
                                                                        <th>NivelError</th>
                                                                        <th>FechaRegistro</th>
                                                                        <th>Observacion</th>
                                                                        <th>FechaAtencion</th>
                                                                        <th>UsuarioObservacion</th>
                                                                        <th>FechaSolucion</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                </tbody>
                                                                <tfoot>
                                                                    <tr>
                                                                        <th></th>
                                                                        <th>IdAlarma</th>
                                                                        <th>IdModulo</th>
                                                                        <th>IdEstacionamiento</th>
                                                                        <th>TipoError</th>
                                                                        <th>Parte</th>
                                                                        <th>Descripcion</th>
                                                                        <th>NivelError</th>
                                                                        <th>FechaRegistro</th>
                                                                        <th>Observacion</th>
                                                                        <th>FechaAtencion</th>
                                                                        <th>UsuarioObservacion</th>
                                                                        <th>FechaSolucion</th>
                                                                    </tr>
                                                                </tfoot>
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
                    </div>
                </div>

                <footer class="footer">
                    <div class="container-fluid">
                        <nav class="pull-left">
                            <ul>

                                <li>
                                    <a>Soporte
                                    </a>
                                </li>
                                <li>
                                    <a>Info
                                    </a>
                                </li>
                                <li>
                                    <a>Licencia
                                    </a>
                                </li>
                            </ul>
                        </nav>
                        <div class="copyright pull-right">
                            &copy;
                            <script>document.write(new Date().getFullYear())</script>
                            , made by Millens
                   
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
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h4 class="modal-title">Detalles</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>IdAlarma</label>
                                    <input type="text" class="form-control border-input" disabled id="idAlarmaModal">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>IdModulo</label>
                                    <input type="text" class="form-control border-input" disabled id="idModuloModal">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>IdEstacionamiento</label>
                                    <input type="text" class="form-control border-input" disabled id="idEstacionamientoModal">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>TipoError</label>
                                    <input type="text" class="form-control border-input" disabled id="tipoModal">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Parte</label>
                                    <input type="text" class="form-control border-input" disabled id="parteModal">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>Nivel</label>
                                    <input type="text" class="form-control border-input" disabled id="nivelModal">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>FechaRegistro</label>
                                    <input type="text" class="form-control border-input" disabled id="fechaModal">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>FechaAtencion</label>
                                    <input type="text" class="form-control border-input" disabled id="atencionModal">
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>UsuarioObservacion</label>
                                    <input type="text" class="form-control border-input" disabled id="usuarioModal">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <div class="form-group">
                                    <label>FechaSolucion</label>
                                    <input type="text" class="form-control border-input" disabled id="solucionModal">
                                </div>
                            </div>
                            <div class="col-md-8">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                    <label>Descipcion</label>
                                    <textarea class="form-control border-input" rows="3" disabled id="descripcionModal"></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <textarea class="form-control border-input" rows="5" id="comment" placeholder="Escriba comentario para atender alarma..."></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="Button1" class="btn btn-default" onclick="AtenderAlarma();">
                                    <span class="ti-check"></span>Atender
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
