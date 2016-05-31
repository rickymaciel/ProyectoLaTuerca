

var numlineas = 0;
var all_impuestos = [];
var all_series = [];
var proveedor = false;
var nueva_compra_url = '';
var kiwimaru_url = '';
var precio_compra = 'coste';
var fin_busqueda1 = true;
var fin_busqueda2 = true;
var siniva = false;
var irpf = 0;
var tiene_recargo = false;


function usar_serie() {
    for (var i = 0; i < all_series.length; i++) {
        if (all_series[i].codserie == $("#codserie").val()) {
            siniva = all_series[i].siniva;
            irpf = all_series[i].irpf;

            for (var j = 0; j < numlineas; j++) {
                if ($("#linea_" + j).length > 0) {
                    if (siniva) {
                        $("#iva_" + j).val(0);
                        $("#recargo_" + j).val(0);
                    }
                }
            }

            break;
        }
    }
}

function recalcular() {
    var l_uds = 0;
    var l_pvp = 0;
    var l_dto = 0;
    var l_neto = 0;
    var l_iva = 0;
    var l_irpf = 0;
    var l_recargo = 0;
    var neto = 0;
    var total_iva = 0;
    var total_irpf = 0;
    var total_recargo = 0;

    for (var i = 0; i < numlineas; i++) {
        if ($("#linea_" + i).length > 0) {
            l_uds = parseFloat($("#Cantidad_" + i).val());
            l_pvp = parseFloat($("#pvp_" + i).val());
            l_dto = parseFloat($("#dto_" + i).val());
            l_neto = l_uds * l_pvp * (100 - l_dto) / 100;
            l_iva = parseFloat($("#iva_" + i).val());
            l_irpf = parseFloat($("#irpf_" + i).val());

            if (tiene_recargo) {
                l_recargo = parseFloat($("#recargo_" + i).val());
            }
            else {
                l_recargo = 0;
                $("#recargo_" + i).val(0);
            }

            $("#neto_" + i).val(l_neto);
            if (numlineas == 1) {
                $("#total_" + i).val(l_neto + l_neto * (l_iva - l_irpf + l_recargo) / 100);
            }
            else {
                $("#total_" + i).val(l_neto + (l_neto * (l_iva - l_irpf + l_recargo) / 100));
            }

            neto += l_neto;
            total_iva += l_neto * l_iva / 100;
            total_irpf += l_neto * l_irpf / 100;
            total_recargo += l_neto * l_recargo / 100;
        }
    }

    $("#aneto").html(neto);
    $("#aiva").html(total_iva);
    $("#are").html(total_recargo);
    $("#airpf").html((total_irpf));
    $("#atotal").val(neto + total_iva - total_irpf + total_recargo);

    if (total_recargo == 0 && !tiene_recargo) {
        $(".recargo").hide();
    }
    else {
        $(".recargo").show();
    }

    if (total_irpf == 0 && irpf == 0) {
        $(".irpf").hide();
    }
    else {
        $(".irpf").show();
    }
}

function ajustar_neto() {
    var l_uds = 0;
    var l_pvp = 0;
    var l_dto = 0;
    var l_neto = 0;

    for (var i = 0; i < numlineas; i++) {
        if ($("#linea_" + i).length > 0) {
            l_uds = parseFloat($("#Cantidad_" + i).val());
            l_pvp = parseFloat($("#pvp_" + i).val());
            l_dto = parseFloat($("#dto_" + i).val());
            l_neto = parseFloat($("#neto_" + i).val());
            if (isNaN(l_neto))
                l_neto = 0;

            if (l_neto <= l_pvp * l_uds) {
                l_dto = 100 - 100 * l_neto / (l_pvp * l_uds);
                if (isNaN(l_dto))
                    l_dto = 0;
            }
            else {
                l_dto = 0;
                l_pvp = 100 * l_neto / (l_uds * (100 - l_dto));
                if (isNaN(l_pvp))
                    l_pvp = 0;
            }

            $("#pvp_" + i).val(l_pvp);
            $("#dto_" + i).val(l_dto);
        }
    }

    recalcular();
}

function ajustar_total() {
    var l_uds = 0;
    var l_pvp = 0;
    var l_dto = 0;
    var l_iva = 0;
    var l_irpf = 0;
    var l_recargo = 0;
    var l_neto = 0;
    var l_total = 0;

    for (var i = 0; i < numlineas; i++) {
        if ($("#linea_" + i).length > 0) {
            l_uds = parseFloat($("#Cantidad_" + i).val());
            l_pvp = parseFloat($("#pvp_" + i).val());
            l_dto = parseFloat($("#dto_" + i).val());
            l_iva = parseFloat($("#iva_" + i).val());
            l_recargo = parseFloat($("#recargo_" + i).val());

            l_irpf = irpf;
            if (l_iva <= 0)
                l_irpf = 0;

            l_total = parseFloat($("#total_" + i).val());
            if (isNaN(l_total))
                l_total = 0;

            if (l_total <= l_pvp * l_uds + (l_pvp * l_uds * (l_iva - l_irpf + l_recargo) / 100)) {
                l_neto = 100 * l_total / (100 + l_iva - l_irpf + l_recargo);
                l_dto = 100 - 100 * l_neto / (l_pvp * l_uds);
                if (isNaN(l_dto))
                    l_dto = 0;
            }
            else {
                l_dto = 0;
                l_neto = 100 * l_total / (100 + l_iva - l_irpf + l_recargo);
                l_pvp = l_neto / l_uds;
            }

            $("#pvp_" + i).val(l_pvp);
            $("#dto_" + i).val(l_dto);
        }
    }

    recalcular();
}

function ajustar_iva(num) {
    if ($("#linea_" + num).length > 0) {
        if (proveedor.regimeniva == 'Exento') {
            $("#iva_" + num).val(0);
            $("#recargo_" + num).val(0);

            alert('El proveedor tiene regimen de IVA: ' + proveedor.regimeniva);
        }
        else if (siniva && $("#iva_" + num).val() != 0) {
            $("#iva_" + num).val(0);
            $("#recargo_" + num).val(0);

            alert('La serie selecciona es sin IVA.');
        }
        else if (tiene_recargo) {
            for (var i = 0; i < all_impuestos.length; i++) {
                if ($("#iva_" + num).val() == all_impuestos[i].iva) {
                    $("#recargo_" + num).val(all_impuestos[i].recargo);
                }
            }
        }
    }

    recalcular();
}

function aux_all_impuestos(num, codimpuesto) {
    var iva = 0;
    var recargo = 0;
    if (proveedor.regimeniva != 'Exento' && !siniva) {
        for (var i = 0; i < all_impuestos.length; i++) {
            if (all_impuestos[i].codimpuesto == codimpuesto) {
                iva = all_impuestos[i].iva;
                if (tiene_recargo) {
                    recargo = all_impuestos[i].recargo;
                }
                break;
            }
        }
    }

    var html = "<td><select id=\"iva_" + num + "\" class=\"form-control\" name=\"iva_" + num + "\" onchange=\"ajustar_iva('" + num + "')\">";
    html += "<option value=\"10\" selected=\"selected\">10 %</option>";
    html += "</select></td>";

    html += "<td class=\"recargo\"><input type=\"text\" class=\"form-control text-right\" id=\"recargo_" + num + "\" name=\"recargo_" + num +
          "\" value=\"" + recargo + "\" onclick=\"this.select()\" onkeyup=\"recalcular()\" autocomplete=\"off\"/></td>";

    html += "<td class=\"irpf\"><input type=\"text\" class=\"form-control text-right\" id=\"irpf_" + num + "\" name=\"irpf_" + num +
          "\" value=\"" + irpf + "\" onclick=\"this.select()\" onkeyup=\"recalcular()\" autocomplete=\"off\"/></td>";

    return html;
}

function add_articulo(ref, desc, pvp, dto, codimpuesto) {
    desc = Base64.decode(desc);
    $("#lineas_albaran").append("<tr id=\"linea_" + numlineas + "\">\n\
      <td><input type=\"hidden\" name=\"idlinea_"+ numlineas + "\" value=\"-1\"/>\n\
         <input type=\"hidden\" name=\"referencia_"+ numlineas + "\" value=\"" + ref + "\"/>\n\
         <div class=\"form-control\"><small><a target=\"_blank\" href=\"index.php?page=ventas_articulo&ref="+ ref + "\">" + ref + "</a></small></div></td>\n\
      <td><textarea class=\"form-control\" id=\"desc_"+ numlineas + "\" name=\"desc_" + numlineas + "\" rows=\"1\" onclick=\"this.select()\">" + desc + "</textarea></td>\n\
      <td><input type=\"number\" step=\"any\" id=\"Cantidad_"+ numlineas + "\" class=\"form-control text-right\" name=\"Cantidad_" + numlineas +
          "\" onchange=\"recalcular()\" onkeyup=\"recalcular()\" autocomplete=\"off\" value=\"1\"/></td>\n\
      <td><button class=\"btn btn-sm btn-danger\" type=\"button\" onclick=\"$('#linea_"+ numlineas + "').remove();recalcular();\">\n\
         <span class=\"glyphicon glyphicon-trash\"></span></button></td>\n\
      <td><input type=\"text\" class=\"form-control text-right\" id=\"pvp_"+ numlineas + "\" name=\"pvp_" + numlineas + "\" value=\"" + pvp +
          "\" onkeyup=\"recalcular()\" onclick=\"this.select()\" autocomplete=\"off\"/></td>\n\
      <td><input type=\"text\" id=\"dto_"+ numlineas + "\" name=\"dto_" + numlineas + "\" value=\"" + dto +
          "\" class=\"form-control text-right\" onkeyup=\"recalcular()\" onchange=\"recalcular()\" onclick=\"this.select()\" autocomplete=\"off\"/></td>\n\
      <td><input type=\"text\" class=\"form-control text-right\" id=\"neto_"+ numlineas + "\" name=\"neto_" + numlineas +
          "\" onchange=\"ajustar_neto()\" onclick=\"this.select()\" autocomplete=\"off\"/></td>\n\
      "+ aux_all_impuestos(numlineas, codimpuesto) + "\n\
      <td><input type=\"text\" class=\"form-control text-right\" id=\"total_"+ numlineas + "\" name=\"total_" + numlineas +
          "\" onchange=\"ajustar_total()\" onclick=\"this.select()\" autocomplete=\"off\"/></td></tr>");
    numlineas += 1;
    $("#numlineas").val(numlineas);
    recalcular();

    $("#nav_articulos").hide();
    $("#search_results").html('');
    $("#kiwimaru_results").html('');
    $("#nuevo_articulo").hide();
    $("#modal_articulos").modal('hide');

    $("#desc_" + (numlineas - 1)).select();
    return false;
}

function add_linea_libre() {
    codimpuesto = false;

    $("#lineas_albaran").append("<tr id=\"linea_" + numlineas + "\">\n\
      <td><input type=\"hidden\" name=\"idlinea_"+ numlineas + "\" value=\"-1\"/>\n\
         <input type=\"hidden\" name=\"referencia_"+ numlineas + "\"/>\n\
         <div class=\"form-control\"></div></td>\n\
      <td><textarea class=\"form-control\" id=\"desc_"+ numlineas + "\" name=\"desc_" + numlineas + "\" rows=\"1\" onclick=\"this.select()\"></textarea></td>\n\
      <td><input class=\"form-control text-box single-line text-right\" data-val=\"true\" data-val-number=\"The field Cantidad must be a number.\" data-val-range=\"La cantidad debe ser un numero entre 1 y 1000\" data-val-range-max=\"1000\" data-val-range-min=\"1\" data-val-required=\"Debe ingresar una cantidad\" id=\"Cantidad_"+ numlineas + "\" name=\"Cantidad_" + numlineas + "\" type=\"number\" onchange=\"recalcular()\" onkeyup=\"recalcular()\" autocomplete=\"off\" value=\"1\"/></td>\n\
      <td><button class=\"btn btn-sm btn-danger\" type=\"button\" onclick=\"$('#linea_"+ numlineas + "').remove();recalcular();\">\n\
         <span class=\"glyphicon glyphicon-trash\"></span></button></td>\n\
      <td><input type=\"text\" class=\"form-control text-right\" id=\"pvp_"+ numlineas + "\" name=\"pvp_" + numlineas + "\" value=\"0\"\n\
          onkeyup=\"recalcular()\" onclick=\"this.select()\" autocomplete=\"off\"/></td>\n\
      <td><input type=\"text\" id=\"dto_"+ numlineas + "\" name=\"dto_" + numlineas + "\" value=\"0\" class=\"form-control text-right\"\n\
          onkeyup=\"recalcular()\" onclick=\"this.select()\" autocomplete=\"off\"/></td>\n\
      <td><input type=\"text\" class=\"form-control text-right\" id=\"neto_"+ numlineas + "\" name=\"neto_" + numlineas +
          "\" onchange=\"ajustar_neto()\" onclick=\"this.select()\" autocomplete=\"off\"/></td>\n\
      "+ aux_all_impuestos(numlineas, codimpuesto) + "\n\
      <td><input type=\"text\" class=\"form-control text-right\" id=\"total_"+ numlineas + "\" name=\"total_" + numlineas +
          "\" onchange=\"ajustar_total()\" onclick=\"this.select()\" autocomplete=\"off\"/></td></tr>");
    numlineas += 1;
    $("#numlineas").val(numlineas);
    recalcular();

    $("#desc_" + (numlineas - 1)).select();
    return false;
}

function get_precios(ref) {
    if (nueva_compra_url !== '') {
        $.ajax({
            type: 'POST',
            url: nueva_compra_url,
            dataType: 'html',
            data: "referencia4precios=" + ref + "&codproveedor=" + proveedor.codproveedor,
            success: function (datos) {
                $("#nav_articulos").hide();
                $("#search_results").html(datos);
            }
        });
    }
}

function new_articulo() {
    if (nueva_compra_url !== '') {
        $.ajax({
            type: 'POST',
            url: nueva_compra_url + '&new_articulo=TRUE',
            dataType: 'json',
            data: $("form[name=f_nuevo_articulo]").serialize(),
            success: function (datos) {
                if (typeof datos[0] == 'undefined') {
                    if (document.f_nuevo_articulo.referencia.value == '') {
                        alert('Debes escribir una referencia.');
                    }
                    else {
                        alert('Se ha producido un error al crear el artículo.');
                    }
                }
                else {
                    document.f_buscar_articulos.query.value = document.f_nuevo_articulo.referencia.value;
                    $("#nav_articulos li").each(function () {
                        $(this).removeClass("active");
                    });
                    $("#li_mis_articulos").addClass('active');
                    $("#search_results").html('');
                    $("#search_results").show('');
                    $("#kiwimaru_results").hide();
                    $("#nuevo_articulo").hide();

                    if (precio_compra == 'coste') {
                        add_articulo(datos[0].referencia, Base64.encode(datos[0].descripcion), datos[0].coste, 0, datos[0].codimpuesto);
                    }
                    else {
                        add_articulo(datos[0].referencia, Base64.encode(datos[0].descripcion), datos[0].pvp, 0, datos[0].codimpuesto);
                    }
                }
            }
        });
    }
}

function buscar_articulos() {
    document.f_nuevo_articulo.referencia.value = document.f_buscar_articulos.query.value;
    document.f_nuevo_articulo.refproveedor.value = document.f_buscar_articulos.query.value;

    if (document.f_buscar_articulos.query.value == '') {
        $("#nav_articulos").hide();
        $("#search_results").html('');
        $("#kiwimaru_results").html('');
        $("#nuevo_articulo").hide();

        fin_busqueda1 = true;
        fin_busqueda2 = true;
    }
    else {
        $("#nav_articulos").show();

        if (nueva_compra_url !== '') {
            fin_busqueda1 = false;
            $.getJSON(nueva_compra_url, $("form[name=f_buscar_articulos]").serialize(), function (json) {
                var items = [];
                var insertar = false;
                $.each(json, function (key, val) {
                    var descripcion = Base64.encode(val.descripcion);
                    var descripcion_visible = val.descripcion;
                    if (val.codfamilia) {
                        descripcion_visible += ' <span class="label label-default" title="Familia: ' + val.codfamilia + '">'
                                + val.codfamilia + '</span>';
                    }
                    if (val.codfabricante) {
                        descripcion_visible += ' <span class="label label-default" title="Fabricante: ' + val.codfabricante + '">'
                                + val.codfabricante + '</span>';
                    }

                    var precio = val.coste;
                    if (precio_compra == 'pvp') {
                        precio = val.pvp;
                    }

                    var tr_aux = '<tr>';
                    if (val.bloqueado) {
                        tr_aux = "<tr class=\"danger\">";
                    }
                    else if (val.stockfis < val.stockmin) {
                        tr_aux = "<tr class=\"warning\">";
                    }
                    else if (val.stockfis > val.stockmax) {
                        tr_aux = "<tr class=\"success\">";
                    }

                    if (val.secompra) {
                        items.push(tr_aux + "<td><a href=\"#\" onclick=\"get_precios('" + val.referencia + "')\" title=\"más detalles\">\n\
                     <span class=\"glyphicon glyphicon-eye-open\"></span></a>\n\
                     &nbsp; <a href=\"#\" onclick=\"return add_articulo('"
                                + val.referencia + "','" + descripcion + "','" + precio + "','" + val.dtopor + "','" + val.codimpuesto + "')\">"
                                + val.referencia + '</a> ' + descripcion_visible + "</td>\n\
                     <td class=\"text-right\"><a href=\"#\" onclick=\"return add_articulo('"
                                + val.referencia + "','" + descripcion + "','" + val.coste + "','" + val.dtopor + "','" + val.codimpuesto + "')\">"
                                + val.coste + "</a></td>\n\
                     <td class=\"text-right\"><a href=\"#\" onclick=\"return add_articulo('"
                                + val.referencia + "','" + descripcion + "','" + val.pvp + "','0','" + val.codimpuesto + "')\" title=\"actualizado el "
                                + val.factualizado + "\">" + val.pvp + "</a></td>\n\
                     <td class=\"text-right\">"+ val.stockfis + "</td></tr>");
                    }

                });

            });

        }
    }
}

$(document).ready(function () {
    $("#i_new_line").click(function () {
        $("#i_new_line").val("");
        $("#nav_articulos li").each(function () {
            $(this).removeClass("active");
        });
        $("#li_mis_articulos").addClass('active');
        $("#nav_articulos").hide();
        $("#search_results").html('');
        $("#search_results").show('');
        $("#kiwimaru_results").html('');
        $("#kiwimaru_results").hide();
        $("#nuevo_articulo").hide();
        $("#modal_articulos").modal('show');
    });

    $("#i_new_line").keyup(function () {
        document.f_buscar_articulos.query.value = $("#i_new_line").val();
        $("#i_new_line").val('');
        $("#nav_articulos li").each(function () {
            $(this).removeClass("active");
        });
        $("#li_mis_articulos").addClass('active');
        $("#nav_articulos").hide();
        $("#search_results").html('');
        $("#search_results").show('');
        $("#kiwimaru_results").html('');
        $("#kiwimaru_results").hide();
        $("#nuevo_articulo").hide();
        $("#modal_articulos").modal('show');
    });

});