<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ppm.aspx.cs" Inherits="BlockAndPass.AdminWeb.ppm" %>

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
            #modal {
                display: none;
                position: absolute;
                top: 45%;
                left: 45%;
                width: 80px;
                height: 80px;
                padding:30px 15px 0px;
                border: 3px solid #ababab;
                box-shadow:1px 1px 10px #ababab;
                border-radius:20px;
                background-color: white;
                z-index: 1002;
                text-align:center;
                overflow: auto;
            }
            #fade {
                display: none;
                position:absolute;
                top: 0%;
                left: 0%;
                width: 100%;
                height: 100%;
                background-color: #ababab;
                z-index: 1001;
                -moz-opacity: 0.8;
                opacity: .70;
                filter: alpha(opacity=80);
            }
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

	            $("div").each(function () {
	                if ($(this).attr("id") != undefined && $(this).attr("id") != 'resModalConfirmaCambios') {
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
	        function ConsultarEstacionamientos(idSede) {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaEstacionamientos?idSede=" + idSede,
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    if (msg.d.length == 0) {
	                        alert("Debe tener permisos sobre el estacionamiento para usar la opcion PPM");
	                        window.location.href = "template.aspx";
	                    } else {
	                        $("#cbEstacionamiento").get(0).options.length = 0;

	                        //$("#cbEstacionamiento").get(0).options[$("#cbEstacionamiento").get(0).options.length] = new Option('Todos', 0);
	                        $.each(msg.d, function (index, item) {
	                            $("#cbEstacionamiento").get(0).options[$("#cbEstacionamiento").get(0).options.length] = new Option(item.Display, item.Value);
	                        });
	                    }
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
	        function btnLeer_click(mensualidad, tipov) {
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
	                            url: "http://localhost:8080/ReaderLocalService.svc/reader/getcardinfo?password="+obj,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                if (msg.error == false) {
	                                    if (msg.cicloActivo == true) {
	                                        tarjetaLeida(msg, mensualidad, tipov);
	                                    } else {
	                                        confirmaCambios(false, 'Tarjeta sin entrada.');
	                                    }
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
	        function tarjetaLeida(infoTarjeta, mensualidad, tipov) {
	                $.ajax({
	                    type: "GET",
	                    url: "DataService.asmx/ConsultarInfoTransaccion?idEstacionamiento=" + $("#cbEstacionamiento").val() + "&idTarjeta=" + infoTarjeta.idCard + "&moduloEntrada=" + infoTarjeta.moduloEntrada,
	                    data: "",
	                    contentType: "application/json",
	                    dataType: "json",
	                    success: function (msg) {
	                        var obj = jQuery.parseJSON(msg.d);
	                        if (obj.Exito == true) {
	                            ConsultarValorPagar(obj.Resultado.IdTransaccion, infoTarjeta, mensualidad, tipov);
	                        } else {
	                            confirmaCambios(false, obj.ErrorMessage);
	                        }
	                    },
	                    error: function () {
	                        confirmaCambios(false, 'Error al consumir servicio');
	                    }
	                });
	        }
	        function ConsultarValorPagar(idTransaccion, infoTarjeta, mensualidad, tipov) {
	            var tipov = 1;
	            var sumTotalPagar = 0;
	            //alert(infoTarjeta.tipoVehiculo);
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ConsultarValorPagar?idTransaccion=" + idTransaccion + "&mensualidad=" + mensualidad + "&repo=" + infoTarjeta.reposicion + "&tipoVehiculo=" + tipov + "&idTarjeta=" + infoTarjeta.idCard,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {

	                        $.each(obj.Resultado, function (index, value) {
	                            sumTotalPagar += value.Total;
	                        });

	                        $("#inputValorPagar").val(numberToCurrency(sumTotalPagar));
	                        $("#inputIdTarjeta").val(infoTarjeta.idCard);
	                        $("#inputIdTransaccion").val(idTransaccion);
	                        $("#inputValorRecibido").val('');
	                        $("#inputValorCambio").val('');
	                        //return obj.Resultado[0].Total;
	                    } else {
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function keyupRecibido(e) {
	            var recibido = $('#inputValorRecibido').val().replace(/\,/g, '');
	            //var primer = recibido.substring(recibido.length - 1, recibido.length);
	            //alert(recibido);
	            
	            
	            $("#inputValorRecibido").val(numberToCurrency(recibido));


	            var cambio = (($("#inputValorPagar").val().length > 0 ? parseInt($("#inputValorPagar").val().replace(/\,/g, '')) : 0) - parseInt($("#inputValorRecibido").val().replace(/\,/g, '')));

	            cambio = cambio > 0 ? 0 : cambio * -1;

	            $("#inputValorCambio").val( numberToCurrency( cambio ));

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
	        function btnPagar_click() {
	            openModal();
	            if ($('#inputValorPagar').val() != '') {
	                if ($('#inputValorPagar').val() != '0') {
	                    if ($('#inputIdTarjeta').val() != '') {
	                        if ($('#inputIdTransaccion').val() != '') {
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
	                                        var $sel = $("#cbPPM");
	                                        var value = $sel.val();
	                                        var text = $("option:selected", $sel).text();
	                                        //alert(text);
	                                        $.ajax({
	                                            type: "GET",
	                                            url: "http://localhost:8080/ReaderLocalService.svc/reader/getcardinfo?password=" + obj,
	                                            data: "",
	                                            contentType: "application/json",
	                                            dataType: "json",
	                                            success: function (msg) {
	                                                if (msg.error == false) {
	                                                    var infromacionTarjeta = msg;
	                                                    $.ajax({
	                                                        type: "GET",
	                                                        url: "http://localhost:8080/ReaderLocalService.svc/reader/paycard?password=" + obj + "&idTarjeta=" + $('#inputIdTarjeta').val() + "&moduloPago=" + text,
	                                                        data: "",
	                                                        contentType: "application/json",
	                                                        dataType: "json",
	                                                        success: function (msg) {
	                                                            if (msg.error == false) {
	                                                                //alert(msg.fechaPago);
	                                                                PagarTransaccionBD(msg.fechaPago, infromacionTarjeta);
	                                                            } else {
	                                                                closeModal();
	                                                                confirmaCambios(false, msg.errorMessage);
	                                                            }
	                                                        },
	                                                        error: function () {
	                                                            closeModal();
	                                                            confirmaCambios(false, 'Error al consumir servicio LOCAL.');
	                                                        }

	                                                    });
	                                                } else {
	                                                    closeModal();
	                                                    confirmaCambios(false, msg.errorMessage);
	                                                }
	                                            },
	                                            error: function () {
	                                                closeModal();
	                                                confirmaCambios(false, 'Error al consumir servicio LOCAL.');
	                                            }

	                                        });
	                                    } else {
	                                        closeModal();
	                                        confirmaCambios(false, 'No encuentra Parametro claveTarjeta.');
	                                    }
	                                }, error: function () {
	                                    closeModal();
	                                    confirmaCambios(false, 'Error al consumir servicio.');
	                                }
	                            });
	                        } else {
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
	                                        var $sel = $("#cbPPM");
	                                        var value = $sel.val();
	                                        var text = $("option:selected", $sel).text();
	                                        //alert(text);
	                                        $.ajax({
	                                            type: "GET",
	                                            url: "http://localhost:8080/ReaderLocalService.svc/reader/getcardinfo?password=" + obj,
	                                            data: "",
	                                            contentType: "application/json",
	                                            dataType: "json",
	                                            success: function (msg) {
	                                                if (msg.error == false) {
	                                                    var infromacionTarjeta = msg;
	                                                    PagarMensualidadBD(msg.fechaPago, infromacionTarjeta);
	                                                } else {
	                                                    closeModal();
	                                                    confirmaCambios(false, msg.errorMessage);
	                                                }
	                                            },
	                                            error: function () {
	                                                closeModal();
	                                                confirmaCambios(false, 'Error al consumir servicio LOCAL.');
	                                            }

	                                        });
	                                    } else {
	                                        closeModal();
	                                        confirmaCambios(false, 'No encuentra Parametro claveTarjeta.');
	                                    }
	                                }, error: function () {
	                                    closeModal();
	                                    confirmaCambios(false, 'Error al consumir servicio.');
	                                }
	                            });
	                        }
	                    } else {
	                        closeModal();
	                        confirmaCambios(false, 'Primero debe leer la tarjeta para proceder al pago.');
	                    }
	                } else {
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
	                                var $sel = $("#cbPPM");
	                                var value = $sel.val();
	                                var text = $("option:selected", $sel).text();
	                                //alert(text);
	                                $.ajax({
	                                    type: "GET",
	                                    url: "http://localhost:8080/ReaderLocalService.svc/reader/getcardinfo?password=" + obj,
	                                    data: "",
	                                    contentType: "application/json",
	                                    dataType: "json",
	                                    success: function (msg) {
	                                        if (msg.error == false) {
	                                            var infromacionTarjeta = msg;
	                                            $.ajax({
	                                                type: "GET",
	                                                url: "http://localhost:8080/ReaderLocalService.svc/reader/paycard?password=" + obj + "&idTarjeta=" + $('#inputIdTarjeta').val() + "&moduloPago=" + text,
	                                                data: "",
	                                                contentType: "application/json",
	                                                dataType: "json",
	                                                success: function (msg) {
	                                                    if (msg.error == false) {
	                                                        $('#inputValorPagar').val('');
	                                                        $('#inputIdTarjeta').val('');
	                                                        $('#inputIdTransaccion').val('');
	                                                        $('#inputValorRecibido').val('');
	                                                        $('#inputValorCambio').val('');
	                                                        closeModal();
	                                                        confirmaCambios(true, '');
	                                                    } else {
	                                                        closeModal();
	                                                        confirmaCambios(false, msg.errorMessage);
	                                                    }
	                                                },
	                                                error: function () {
	                                                    closeModal();
	                                                    confirmaCambios(false, 'Error al consumir servicio LOCAL.');
	                                                }

	                                            });
                                            } else {
                                                closeModal();
                                                confirmaCambios(false, msg.errorMessage);
                                            }
	                                    },
	                                    error: function () {
	                                        closeModal();
	                                        confirmaCambios(false, 'Error al consumir servicio LOCAL.');
	                                    }

	                                });
                                } else {
                                    closeModal();
                                    confirmaCambios(false, 'No encuentra Parametro claveTarjeta.');
                                }
	                        }, error: function () {
	                            closeModal();
	                            confirmaCambios(false, 'Error al consumir servicio.');
	                        }
	                    });
	                }
	            } else {
	                closeModal();
	                confirmaCambios(false, 'Primero debe leer la tarjeta para proceder al pago.');
	            }
	        }
	        function btnConvenio_click() {
	            ConsultarConvenios();
	            $('#myModalConvenio').modal('show');
	        }
	        function btnCortesia_click() {
	            ConsultarMotivosCortesias();
	            $('#myModalCortesia').modal('show');
	        }
	        function confirmaCambios(resultado, mensaje) {
	            //alert(resultado + ' ' + mensaje);
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
	                url: "DataService.asmx/ObtenerIdCajeroxMAC?mac=" + mac,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        $("#cbEstacionamiento").val(obj.Resultado.IdEstacionamiento);
	                        $("#cbPPM").get(0).options[$("#cbPPM").get(0).options.length] = new Option(obj.Resultado.IdModulo, 0);
	                        $("#cbPPM").val(0);
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
	        function ConsultarConvenios() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaConveniosXEstacionamientoXUsuario?idEstacionamiento=" + $("#cbEstacionamiento").val(),
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#cbConveniosModal").get(0).options.length = 0;

	                    //$("#cbSedes").get(0).options[$("#cbSedes").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#cbConveniosModal").get(0).options[$("#cbConveniosModal").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                    ConsultarDescripcionConvenio();
	                },
	                error: function () {
	                    $("#cbConveniosModal").get(0).options.length = 0;
	                    alert("Failed to load convenios");
	                }

	            });
	        }
	        function ConsultarDescripcionConvenio() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ConsultarDescripcionConvenio?idConvenio=" + $("#cbConveniosModal").val(),
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    //alert("OK");
	                    $("#comment").val(msg.d);
	                },
	                error: function () {
	                    alert("Failed to load descripcion");
	                }

	            });
	        }
	        function cambioComboConvenios() {
	            //alert("cambio");
	            ConsultarDescripcionConvenio();
	        }
	        function ConsultarMotivosCortesias(){
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerListaMotivosCortesiaXEstacionamiento?idEstacionamiento=" + $("#cbEstacionamiento").val(),
	                data: "{}",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    $("#cbMotivoCortesia").get(0).options.length = 0;

	                    //$("#cbSedes").get(0).options[$("#cbSedes").get(0).options.length] = new Option('Todos', 0);
	                    $.each(msg.d, function (index, item) {
	                        $("#cbMotivoCortesia").get(0).options[$("#cbMotivoCortesia").get(0).options.length] = new Option(item.Display, item.Value);
	                    });
	                },
	                error: function () {
	                    $("#cbMotivoCortesia").get(0).options.length = 0;
	                    alert("Failed to load motivos cortesias");
	                }

	            });
	        }
	        function btnAplicarConvenio_Click() {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerValorParametroxNombre?nombre=claveTarjeta&idEstacionamiento=" + $("#cbEstacionamiento").val(),
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = msg.d;
	                    if (obj != '') {
	                        $.ajax({
	                            type: "GET",
	                            url: "http://localhost:8080/ReaderLocalService.svc/reader/aplicarconvenio?password=" + obj + "&idConvenio=" + $('#cbConveniosModal').val(),
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                if (msg.error == false) {
	                                    $('#myModalConvenio').modal('toggle');
	                                    confirmaCambios(true, '');
	                                } else {
	                                    $('#myModalConvenio').modal('toggle');
	                                    confirmaCambios(false, msg.errorMessage);
	                                }
	                            },
	                            error: function () {
	                                $('#myModalConvenio').modal('toggle');
	                                confirmaCambios(false, 'Error al consumir servicio LOCAL.');
	                            }

	                        });
	                    } else {
	                        $('#myModalConvenio').modal('toggle');
	                        confirmaCambios(false, 'No encuentra Parametro claveTarjeta.');
	                    }
	                }, error: function () {
	                    $('#myModalConvenio').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }
	            });
	        }
	        function AplicarCortesia_Click() {
	            LeerTarjeta();
	        }
	        function LeerTarjeta() {
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
	                                if (msg.cicloActivo == true) {
	                                    ConsultaInfoTransaccion(msg);
	                                } else {
	                                    $('#myModalCortesia').modal('toggle');
	                                    confirmaCambios(false, 'Tarjeta sin entrada.');
	                                }
	                            },
	                            error: function () {
	                                $('#myModalCortesia').modal('toggle');
	                                confirmaCambios(false, 'Error al consumir servicio LOCAL.');
	                            }
	                        });
	                    } else {
	                        $('#myModalCortesia').modal('toggle');
                            confirmaCambios(false, 'No encuentra Parametro claveTarjeta.');
                        }
	                }, error: function () {
	                    $('#myModalCortesia').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }
	            });
	        }
	        function ConsultaInfoTransaccion(infoTarjeta) {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ConsultarInfoTransaccion?idEstacionamiento=" + $("#cbEstacionamiento").val() + "&idTarjeta=" + infoTarjeta.idCard + "&moduloEntrada=" + infoTarjeta.moduloEntrada,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        AplicarCortesia(obj.Resultado.IdTransaccion);
	                    } else {
	                        $('#myModalCortesia').modal('toggle');
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    $('#myModalCortesia').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function AplicarCortesia(idtransaccion) {
	            //alert(idtransaccion);
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/AplicarCortesia?idEstacionamiento=" + $("#cbEstacionamiento").val() + "&observacion=" + $("#observacion").val() + "&idMotivo=" + $("#cbMotivoCortesia").val() + "&idTransaccion="+idtransaccion,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        EscribirCortesiaTarjeta();
	                    } else {
	                        $('#myModalCortesia').modal('toggle');
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    $('#myModalCortesia').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function EscribirCortesiaTarjeta() {
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
	                            url: "http://localhost:8080/ReaderLocalService.svc/reader/aplicarcortesia?password="+obj,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                if (msg.error == false) {
	                                    $('#myModalCortesia').modal('toggle');
	                                    confirmaCambios(true, '');
	                                } else {
	                                    $('#myModalCortesia').modal('toggle');
	                                    confirmaCambios(false, obj.ErrorMessage);
	                                }
	                            },
	                            error: function () {
	                                $('#myModalCortesia').modal('toggle');
	                                confirmaCambios(false, 'Error al consumir servicio LOCAL');
	                            }

	                        });
	                    } else {
	                        $('#myModalCortesia').modal('toggle');
	                        confirmaCambios(false, 'No encuentra Parametro claveTarjeta.');
	                    }
	                }, error: function () {
	                    $('#myModalCortesia').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function btnLimpiar_click() {
	            $('#inputValorPagar').val('');
	            $('#inputIdTarjeta').val('');
	            $('#inputIdTransaccion').val('');
	            $('#inputValorRecibido').val('');
	            $('#inputValorCambio').val('');
	            //imprimir($('#inputIdTransaccion').val());
	        }
	        function PagarTransaccionBD(fechaPago, infromacionTarjeta) {
	            var $sel = $("#cbPPM");
	            var value = $sel.val();
	            var text = $("option:selected", $sel).text();
	            //?idTransaccion=" + idTransaccion + "&mensualidad=" + mensualidad + "&repo=" + infoTarjeta.reposicion + "&tipoVehiculo=" + tipov,
	            var pagosFinal='';
	            var sumTotalPagar = 0;
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ConsultarValorPagar?idTransaccion=" + $('#inputIdTransaccion').val() + "&mensualidad=" + mensualidades + "&repo=" + infromacionTarjeta.reposicion + "&tipoVehiculo=" + tipovehiculo + "&idTarjeta=" + infromacionTarjeta.idCard,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {

	                        $.each(obj.Resultado, function (index, value) {
	                            
	                            if (!pagosFinal) {
	                            } else {
	                                pagosFinal += ',';
	                            }
	                            sumTotalPagar += value.Total;
	                            pagosFinal += value.Tipo + "-" + value.SubTotal + "-" + value.Iva + "-" + value.Total;
	                        });

	                        $.ajax({
	                            type: "GET",
	                            url: "DataService.asmx/PagarClienteParticular?pagos=" + pagosFinal + "&idTransaccion=" + $('#inputIdTransaccion').val() + "&idEstacionamiento=" + $("#cbEstacionamiento").val() + "&idModulo=" + text + "&fecha=" + fechaPago + "&total=" + sumTotalPagar,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                var obj = jQuery.parseJSON(msg.d);
	                                if (obj.Exito == true) {
	                                    imprimir($('#inputIdTransaccion').val());
	                                } else {
	                                    closeModal();
	                                    confirmaCambios(false, obj.ErrorMessage);
	                                }
	                            },
	                            error: function () {
	                                closeModal();
	                                confirmaCambios(false, 'Error al consumir servicio');
	                            }
	                        });
	                    } else {
	                        closeModal();
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    closeModal();
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function PagarMensualidadBD(fechaPago, infromacionTarjeta) {
	            var $sel = $("#cbPPM");
	            var value = $sel.val();
	            var text = $("option:selected", $sel).text();
	            //?idTransaccion=" + idTransaccion + "&mensualidad=" + mensualidad + "&repo=" + infoTarjeta.reposicion + "&tipoVehiculo=" + tipov,
	            var pagosFinal = '';
	            var sumTotalPagar = 0;
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ConsultarValorPagar?idTransaccion=&mensualidad=true&repo=false&tipoVehiculo=1&idTarjeta=" + infromacionTarjeta.idCard,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {

	                        $.each(obj.Resultado, function (index, value) {

	                            if (!pagosFinal) {
	                            } else {
	                                pagosFinal += ',';
	                            }
	                            sumTotalPagar += value.Total;
	                            pagosFinal += value.Tipo + "-" + value.SubTotal + "-" + value.Iva + "-" + value.Total;
	                        });

	                        $.ajax({
	                            type: "GET",
	                            url: "DataService.asmx/PagarMensualidad?pagos=" + pagosFinal + "&idEstacionamiento=" + $("#cbEstacionamiento").val() + "&idModulo=" + text + "&fecha=" + fechaPago + "&total=" + sumTotalPagar + "&idTarjeta=" + infromacionTarjeta.idCard,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                var obj = jQuery.parseJSON(msg.d);
	                                if (obj.Exito == true) {
	                                    imprimirMensualidad(obj.Resultado.IdTranaccion, obj.Resultado.IdAutorizacion);
	                                } else {
	                                    closeModal();
	                                    confirmaCambios(false, obj.ErrorMessage);
	                                }
	                            },
	                            error: function () {
	                                closeModal();
	                                confirmaCambios(false, 'Error al consumir servicio');
	                            }
	                        });

	                    } else {
	                        closeModal();
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    closeModal();
	                    confirmaCambios(false, 'Error al consumir servicio');
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
	                        //alert(obj.Resultado);
	                        $.ajax({
	                            type: "GET",
	                            url: "http://localhost:8080/ReaderLocalService.svc/computer/printticket?datos=" + msg.d,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                if (msg.error == false) {
	                                    closeModal();
	                                    confirmaCambios(true, '');
	                                    //alert(msg.impresion);
	                                    var sampleArr = base64ToArrayBuffer(msg.impresion);
	                                    saveByteArray("Sample report", sampleArr);
	                                } else {
	                                    closeModal();
	                                    confirmaCambios(false, msg.errorMessage);
	                                }
	                                $('#inputValorPagar').val('');
	                                $('#inputIdTarjeta').val('');
	                                $('#inputIdTransaccion').val('');
	                                $('#inputValorRecibido').val('');
	                                $('#inputValorCambio').val('');
	                            },
	                            error: function () {
	                                closeModal();
	                                confirmaCambios(false, 'Error al consumir servicio LOCAL');
	                            }

	                        });
	                    } else {
	                        closeModal();
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    closeModal();
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function imprimirMensualidad(idtran,idAuto) {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerDatosFacturaMensualidad?idTransaccion=" + idtran + "&idAutorizacion="+idAuto,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        //alert(obj.Resultado);
	                        $.ajax({
	                            type: "GET",
	                            url: "http://localhost:8080/ReaderLocalService.svc/computer/printticketm?datos=" + msg.d,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                if (msg.error == false) {
	                                    closeModal();
	                                    confirmaCambios(true, '');
	                                    //alert(msg.impresion);
	                                    var sampleArr = base64ToArrayBuffer(msg.impresion);
	                                    saveByteArray("Sample report", sampleArr);
	                                } else {
	                                    closeModal();
	                                    confirmaCambios(false, msg.errorMessage);
	                                }
	                                $('#inputValorPagar').val('');
	                                $('#inputIdTarjeta').val('');
	                                $('#inputIdTransaccion').val('');
	                                $('#inputValorRecibido').val('');
	                                $('#inputValorCambio').val('');
	                            },
	                            error: function () {
	                                closeModal();
	                                confirmaCambios(false, 'Error al consumir servicio LOCAL');
	                            }

	                        });
	                    } else {
	                        closeModal();
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    closeModal();
	                    confirmaCambios(false, 'Error al consumir servicio');
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
	        function btnArqueo_click() {
	            var $sel = $("#cbPPM");
	            var value = $sel.val();
	            var text = $("option:selected", $sel).text();

	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/RegistrarArqueo?idEstacionamiento=" + $("#cbEstacionamiento").val() + "&idModulo=" + text,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        //alert(obj.Resultado);
	                        $('#idArqueoCombo').val(obj.Resultado);
	                        $('#myModalArqueo').modal('show');
	                    } else {
	                        confirmaCambios(false, msg.errorMessage);
	                    }
	                },
	                error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }

	            });
	        }
	        function btnAceptarArqueo_Click() {
	            var $sel = $("#cbPPM");
	            var value = $sel.val();
	            var text = $("option:selected", $sel).text();
	            
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ConfirmarArqueo?idEstacionamiento=" + $("#cbEstacionamiento").val() + "&idModulo=" + text + "&idArqueo=" + $('#idArqueoCombo').val(),
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        //alert(obj.Resultado);
	                        //$('#myModalArqueo').modal('toggle');
	                        //confirmaCambios(true, '');
	                        imprimirArqueo($('#idArqueoCombo').val());
	                    } else {
	                        $('#myModalArqueo').modal('toggle');
	                        confirmaCambios(false, msg.errorMessage);
	                    }
	                },
	                error: function () {
	                    $('#myModalArqueo').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }

	            });
	        }
	        function imprimirArqueo(idArqueo) {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerDatosComprobanteArqueo?idArqueo=" + idArqueo,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        //alert(msg.d);
	                        $.ajax({
	                            type: "GET",
	                            url: "http://localhost:8080/ReaderLocalService.svc/computer/printticketArqueo?datos=" + msg.d,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                if (msg.error == false) {
	                                    $('#myModalArqueo').modal('toggle');
	                                    confirmaCambios(true, '');
	                                    //alert(msg.impresion);
	                                    var sampleArr = base64ToArrayBuffer(msg.impresion);
	                                    saveByteArray("Sample report", sampleArr);
	                                } else {
	                                    $('#myModalArqueo').modal('toggle');
	                                    confirmaCambios(false, msg.errorMessage);
	                                }
	                            },
	                            error: function () {
	                                $('#myModalArqueo').modal('toggle');
	                                confirmaCambios(false, 'Error al consumir servicio LOCAL');
	                            }

	                        });
	                    } else {
	                        $('#myModalArqueo').modal('toggle');
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    $('#myModalArqueo').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio');
	                }
	            });
	        }
	        function btnCarga_click() {
	            $('#inputValorCarga').val('');
	            var $sel = $("#cbPPM");
	            var value = $sel.val();
	            var text = $("option:selected", $sel).text();

	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/RegistrarCarga?idEstacionamiento=" + $("#cbEstacionamiento").val() + "&idModulo=" + text,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        //alert(obj.Resultado);
	                        $('#idCargaCombo').val(obj.Resultado);
	                        $('#myModalCarga').modal('show');
	                    } else {
	                        confirmaCambios(false, msg.errorMessage);
	                    }
	                },
	                error: function () {
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }

	            });
	        }
	        function keyupRecibidoCarga(e) {
	            var recibido = $('#inputValorCarga').val().replace(/\,/g, '');
	            //var primer = recibido.substring(recibido.length - 1, recibido.length);
	            //alert(recibido);


	            $("#inputValorCarga").val(numberToCurrency(recibido));
	        }
	        function btnAceptarCarga_Click() {
	            var $sel = $("#cbPPM");
	            var value = $sel.val();
	            var text = $("option:selected", $sel).text();

	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ConfirmarCarga?idEstacionamiento=" + $("#cbEstacionamiento").val() + "&idModulo=" + text + "&idCarga=" + $('#idCargaCombo').val() + "&valor=" + $('#inputValorCarga').val().replace(/\,/g, ''),
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        //alert(obj.Resultado);
	                        //$('#myModalCarga').modal('toggle');
	                        //confirmaCambios(true, '');
	                        imprimirCarga($('#idCargaCombo').val());
	                    } else {
	                        $('#myModalCarga').modal('toggle');
	                        confirmaCambios(false, msg.errorMessage);
	                    }
	                },
	                error: function () {
	                    $('#myModalCarga').modal('toggle');
	                    confirmaCambios(false, 'Error al consumir servicio.');
	                }

	            });
	        }
	        function imprimirCarga(idCarga) {
	            $.ajax({
	                type: "GET",
	                url: "DataService.asmx/ObtenerDatosComprobanteCarga?idCarga=" + idCarga,
	                data: "",
	                contentType: "application/json",
	                dataType: "json",
	                success: function (msg) {
	                    var obj = jQuery.parseJSON(msg.d);
	                    if (obj.Exito == true) {
	                        //alert(msg.d);
	                        $.ajax({
	                            type: "GET",
	                            url: "http://localhost:8080/ReaderLocalService.svc/computer/printticketCarga?datos=" + msg.d,
	                            data: "",
	                            contentType: "application/json",
	                            dataType: "json",
	                            success: function (msg) {
	                                if (msg.error == false) {
	                                    $('#myModalCarga').modal('toggle');
	                                    confirmaCambios(true, '');
	                                    //alert(msg.impresion);
	                                    var sampleArr = base64ToArrayBuffer(msg.impresion);
	                                    saveByteArray("Sample report", sampleArr);
	                                } else {
	                                    $('#myModalCarga').modal('toggle');
	                                    confirmaCambios(false, msg.errorMessage);
	                                }
	                            },
	                            error: function () {
	                                $('#myModalCarga').modal('toggle');
	                                confirmaCambios(false, 'Error al consumir servicio LOCAL');
	                            }

	                        });
	                    } else {
	                        $('#myModalCarga').modal('toggle');
	                        confirmaCambios(false, obj.ErrorMessage);
	                    }
	                },
	                error: function () {
	                    $('#myModalCarga').modal('toggle');
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
	        function antesPagar() {
	            $('#myModalTipoPago').modal('show');
	        }
	        function carro_Click() {
	            $('#myModalTipoPago').modal('toggle');
	            mensualidades = false;
	            tipovehiculo = 1;
	            btnLeer_click(false, 1);
	            
	        }
	        function moto_Click() {
	            $('#myModalTipoPago').modal('toggle');
	            mensualidades = false;
	            tipovehiculo = 2;
	            btnLeer_click(false, 2);
	        }
	        function mensualidad_Click() {
	            $('#myModalTipoPago').modal('toggle');
	            mensualidades = true;
	            tipovehiculo = 1;
	            btnLeerMensualidad_click(true, 1);
	        }
	        var mensualidades = false;
	        var tipovehiculo = 1;
	        function btnLeerMensualidad_click(mensualidad, tipov) {
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
	                                if (msg.error == false) {
	                                    
	                                        ConsultarValorPagar('', msg, true, 1);
	                                    
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
	        function openModal() {
	            document.getElementById('modal').style.display = 'block';
	            document.getElementById('fade').style.display = 'block';
	        }
	        function closeModal() {
	            document.getElementById('modal').style.display = 'none';
	            document.getElementById('fade').style.display = 'none';
	        }
	        function btnAsignarMoto_Click() {
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
	                                if (msg.cicloActivo == true) {
	                                    $.ajax({
	                                        type: "GET",
	                                        url: "DataService.asmx/AplicarEtiquetaMoto?idEstacionamiento=" + $("#cbEstacionamiento").val() + "&idTarjeta=" + msg.idCard + "&moduloEntrada=" + msg.moduloEntrada,
	                                        data: "",
	                                        contentType: "application/json",
	                                        dataType: "json",
	                                        success: function (msg) {
	                                            var obj = jQuery.parseJSON(msg.d);
	                                            if (obj.Exito == true) {
	                                                confirmaCambios(true);
	                                            }else{
	                                                confirmaCambios(false, obj.ErrorMessage);
	                                            }
	                                        },
	                                        error: function () {
	                                            confirmaCambios(false, 'Error al consumir servicio');
	                                        }
	                                    });
	                                } else {
	                                    confirmaCambios(false, 'Tarjeta sin entrada.');
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
	        function isLoaded() {
	            var pdfFrame = window.frames["pdf"];
	            pdfFrame.focus();
	            pdfFrame.print();
	        }
	    </script>
    </head>

    <body>
        <div id="fade"></div>
        <div id="modal">
            <img id="loader" src="images/loading.gif" />
        </div>
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
                        <li id="navPPM" class="active">
                            <a>
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
                                        <h4 class="title">PPM</h4>
                                    </div>
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                        <label>Sedes</label>
                                                        <select class="form-control border-input" id="cbSedes" onchange="cambioComboSedes();" disabled>
                                                        </select>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                        <label>Estacionamientos</label>
                                                        <select class="form-control border-input" id="cbEstacionamiento" disabled>
                                                        </select>
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                        <label>PPM</label>
                                                        <select class="form-control border-input" id="cbPPM" disabled>
                                                        </select>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="card">
                                    <div class="content">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label>IdTransaccion</label>
                                                    <input type="text" class="form-control border-input"  id="inputIdTransaccion" disabled>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Valor a Pagar</label>
                                                    <div class="input-group"> 
                                                        <span class="input-group-addon">$</span>
                                                        <input type="text" class="form-control border-input"  id="inputValorPagar" disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>IdTarjeta</label>
                                                    <input type="text" class="form-control border-input"  id="inputIdTarjeta" disabled>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group"> 
                                                    <label>Valor recibido</label>
                                                    <div class="input-group"> 
                                                        <span class="input-group-addon">$</span>
                                                        <input type="text" class="form-control border-input" onkeyup="keyupRecibido();" id="inputValorRecibido">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label>Cambio</label>
                                                    <div class="input-group"> 
                                                        <span class="input-group-addon">$</span>
                                                        <input type="text" class="form-control border-input" id="inputValorCambio" disabled>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <div class="card" onclick="btnLimpiar_click();" id="btnLimpiar">
                                                    <div class="content">
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                <div class="icon-big icon-warning">
                                                                    <i class="ti-eraser"></i>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-9">
                                                                <div class="numbers">
                                                                    <p></p>
                                                                    Limpiar
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
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="card" onclick="antesPagar()" id="btnLeer">
                                                    <div class="content">
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                <div class="icon-big icon-warning">
                                                                    <i class="ti-credit-card"></i>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-9">
                                                                <div class="numbers">
                                                                    <p></p>
                                                                    Leer
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="card" onclick="btnPagar_click()" id="btnPagar">
                                                    <div class="content">
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                <div class="icon-big icon-success">
                                                                    <i class="ti-money"></i>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-9">
                                                                <div class="numbers">
                                                                    <p></p>
                                                                    Pagar
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="card" onclick="btnConvenio_click()" id="btnConvenio">
                                                    <div class="content">
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                <div class="icon-big icon-info text-center">
                                                                    <i class="ti-shopping-cart-full"></i>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-9">
                                                                <div class="numbers">
                                                                    <p></p>
                                                                    Convenio
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="card" onclick="btnCortesia_click()" id="btnCortesia">
                                                    <div class="content">
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                <div class="icon-big icon-danger text-center">
                                                                    <i class="ti-star"></i>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-9">
                                                                <div class="numbers">
                                                                    <p></p>
                                                                    Cortesia
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="card" onclick="btnCarga_click();" id="btnCarga">
                                                    <div class="content">
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                <div class="icon-big icon-success text-center">
                                                                    <i class="ti-crown"></i>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-9">
                                                                <div class="numbers">
                                                                    <p></p>
                                                                    Carga
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="card" onclick="btnArqueo_click();" id="btnArqueo">
                                                    <div class="content">
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                <div class="icon-big icon-warning text-center">
                                                                    <i class="ti-bag"></i>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-9">
                                                                <div class="numbers">
                                                                    <p></p>
                                                                    Arqueo
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="card" onclick="btnAsignarMoto_Click();">
                                                    <div class="content">
                                                        <div class="row">
                                                            <div class="col-md-3">
                                                                <div class="icon-big icon-warning">
                                                                    <i class="ti-rocket"></i>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-9">
                                                                <div class="numbers">
                                                                    <p></p>
                                                                    Moto
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
                        <div class="row">
                            <div class="col-md-12" id="myTest">
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

    <div id="myModalTipoPago" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Tipo Pago</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="card" onclick="carro_Click()">
                                            <div class="content">
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <div class="text-center">
                                                            <i class="ti-car"></i>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <div>
                                                            Estacionamiento
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--<div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="card" onclick="moto_Click()">
                                            <div class="content">
                                                <div class="row">
                                                    <div class="col-md-12">
                                                            Moto
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>--%>
                            <div class="col-md-6">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="card" onclick="mensualidad_Click()">
                                            <div class="content">
                                                <div class="row">
                                                    <div class="col-md-3">
                                                        <div class="text-center">
                                                            <i class="ti-star"></i>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-9">
                                                        <div>
                                                            Mensualidad
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
            </div>
        </div>
    </div>

    <div id="myModalConvenio" class="modal fade">
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
                                    <label>Convenio</label>
                                    <select class="form-control border-input" id="cbConveniosModal" onchange="cambioComboConvenios();">
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                  <label for="comment">Descripcion:</label>
                                  <textarea class="form-control border-input" rows="5" id="comment" readonly></textarea>
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
                                <button type="button" id="Button1" class="btn btn-default" onclick="btnAplicarConvenio_Click();">
                                    <span class="ti-save"></span>Aplicar
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="myModalArqueo" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Arqueo</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-6">
                                Presione aceptar para confirmar el arqueo:                      
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>IdArqueo:</label>
                                    <input class="form-control border-input" id="idArqueoCombo" disabled>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-12">
                                <button type="button" id="Button4" class="btn btn-default" onclick="btnAceptarArqueo_Click();">
                                    <span class="ti-save"></span>Aceptar
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="myModalCarga" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Carga</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>IdCarga:</label>
                                    <input class="form-control border-input" id="idCargaCombo" disabled>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Valor a Cargar</label>
                                    <div class="input-group"> 
                                        <span class="input-group-addon">$</span>
                                        <input type="text" class="form-control border-input"  id="inputValorCarga" onkeyup="keyupRecibidoCarga();">
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
                                <button type="button" id="Button6" class="btn btn-default" onclick="btnAceptarCarga_Click();">
                                    <span class="ti-save"></span>Aceptar
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="myModalCortesia" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Cortesias</h4>
                </div>
                <div class="modal-body">
                    <div class="content">
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label>Motivo</label>
                                    <select class="form-control border-input" id="cbMotivoCortesia">
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-6">
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="form-group">
                                  <label for="comment">Observacion:</label>
                                  <textarea class="form-control border-input" rows="5" id="observacion"></textarea>
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
                                <button type="button" id="Button3" class="btn btn-default" onclick="AplicarCortesia_Click();">
                                    <span class="ti-save"></span>Aplicar
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