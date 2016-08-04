/**
 * Functions that handle fetching the invoice items coming from the database
 * @type {{fetchItems: Function,
 *          updatePrice: Function,
 *          updateTotal: Function,
 *          addRow: Function,
 *          deleteRow: Function,
 *          roundNumber: Function,
 *          setPathValue: Function}}
 */

var mioInvoice = {

    /**
     * Fetch items from database using autocomplete method
     * @param $lookupInput - Input selector
     */
    fetchItems: function ($lookupInput) {

        // apply autocomplete method to newly created row
        $lookupInput.autocomplete({
            source: window.filePath,
            minLength: 1,
            select: function (event, ui) {

                if (ui.item.Stock > 0) {
                    var $itemRow = $(this).closest('tr');
                    $itemRow.find('#RepuestoId').val(ui.item.Id);
                    $itemRow.find('#Nombre').val(ui.item.Nombre);
                    $itemRow.find('#PrecioVenta1').val(ui.item.PrecioVenta1);
                    $itemRow.find('#Stock').val(ui.item.Stock);
                    $itemRow.find('#Cantidad').focus();
                    $itemRow.find('#Cantidad').attr("max", ui.item.Stock);
                    return false;
                }
                else {
                    return alert("No hay stock disponible");
                    $itemRow.find('#Nombre').focus();
                }
            }
        });

    },

    /**
     * Update price function
     *  @param $this - Row Object
     */
    updatePrice: function ($this) {
        var $itemRow = $($this).closest('tr');
        // Calculate the price of the row.  Remove any $ so the calculation doesn't break
        var price = $itemRow.find('#PrecioVenta1').val() * $itemRow.find('#Cantidad').val();
        var cantidad = $itemRow.find('#Cantidad').val();
        var stock = $itemRow.find('#Stock').val();
        if (stock >=  cantidad) {
            alert("Stock insuficiente");
        }
        //price = this.roundNumber(price, 0);
        isNaN(price) ? $itemRow.find('#TotalLinea').val("N/A") : $itemRow.find('#TotalLinea').val(price);


        var iva = price / ((parseInt($("#tax").val()) / 100 + 1) * 10);
        var neto = price / (parseInt($("#tax").val()) / 100 + 1);

        $itemRow.find('#TotalLineaNeto').val(this.roundNumber(neto, 0)).number(true, 0);
        $itemRow.find('#TotalLineaIva').val(this.roundNumber(iva, 0)).number(true, 0);

        this.updateSalesTax();
        this.updateTotal();
    },

    /**
     * Handle the total calculation
     */
    updateTotal: function () {

        var total = 0;
        $('input#TotalLinea').each(function (i) {
            price = $(this).val();
            if (!isNaN(price)) total += Number(price);
        });

        //$('#subTotal').html("Gs. " + total);

        //var grdTotal = total + this.updateSalesTax();
        var grdTotal = total;
        $('#Total').val(grdTotal);
        $("select#Metodo").attr('readonly', false);

        $('#grandTotal').html((grdTotal)).number(true, 0);
        $('#subTotal').html(this.roundNumber(grdTotal - this.updateSalesTax(), 0)).number(true, 0);



    },
    updateSalesTax: function () {
        var total = 0;
        $('input#TotalLinea').each(function (i) {
            price = $(this).val();
            if (!isNaN(price)) total += Number(price);
        });

        //var tax = total / $("#tax").val() + 1;
        //var tax = total / ((parseInt($("#tax").val()) / 100 + 1) * 10);
        //$("#salesTax").html("Gs. " + mioInvoice.roundNumber(tax, 0)).number(true, 0);


        var tax = ($("#tax").val() * total * "0.01")/1.1;
        $("#salesTax").html(mioInvoice.roundNumber(tax, 0)).number(true, 0);

        return tax;
    },

    /**
     * Add row to invoice to allow for additional items
     * @param lookupSelector
     */
    addRow: function (lookupSelector) {

        var i = $('.item-row').length;
        var rowTemp = [
    '<tr class="item-row">',
    '<td class="col-lg-1"><button id="deleteRow" class="btn btn-xs btn-danger tip" title="Eliminar"> <i class="im-remove2"></i></button</td>',
    '<td class="col-lg-3"><input id="RepuestoId" name="detallesFacturaCliente[' + i + '].RepuestoId" type="hidden" class="form-control input-sm" placeholder="RepuestoId" value="" readonly="readonly"   />',
    '<div class="has-feedback"><input id="Nombre" autofocus name="detallesFacturaCliente[' + i + '].Nombre" type="text" class="form-control input-sm" value="" placeholder="Buscar Repuesto" /><span class="glyphicon glyphicon-search form-control-feedback text-muted"></span></div></td>',
    '<td class="col-lg-1"><input id="Cantidad" name="detallesFacturaCliente[' + i + '].Cantidad" type="number" class="form-control input-sm" placeholder="Cantidad" value="0" min="1" max="100" required /></td>',
    '<td class="col-lg-2"><input id="PrecioVenta1" name="detallesFacturaCliente[' + i + '].Precio" class="form-control input-sm" placeholder="Precio" type="text"></td>',
    '<td class="col-lg-2"><input id="TotalLineaNeto" class="form-control input-sm" placeholder="Neto" type="text" readonly="readonly" required></td>',
    '<td class="col-lg-1"><input id="TotalLineaIva"  class="form-control input-sm" placeholder="Iva" type="text" readonly="readonly" required></td>',
    '<td class="col-lg-2"><input id="TotalLinea" name="detallesFacturaCliente[' + i + '].Total" class="form-control input-sm" type="text" placeholder="Total" readonly="readonly"></td>',
    '</tr>'
        ].join('');
        i++;
        // Get the table object to use for adding a row at the end of the table
        var $itemsTable = $('#itemsTable');
        var $row = $(rowTemp);

        // Add row after the first row in table
        $('.item-row:last', $itemsTable).after($row);

        // save reference to inputs within row
        var $Nombre = $row.find(lookupSelector);

        $Nombre.focus();
        mioInvoice.fetchItems($Nombre);

        // Update the invoice total on keyup when the user updates the item qty or price input
        // ** Note: This is for the newly created row
        $row.find(itemQtyPriceSelectors).on('keyup', function () {
            // Locate the row we are working with
            var $itemRow = $(this).closest('tr');
            // Update the price.
            mioInvoice.updatePrice($itemRow);
        });


    },

    /**
     * Delete row if we have more than one row in table
     * @param row
     * @returns {boolean}
     */
    deleteRow: function (row) {

        var rowCount = $('#itemsTable tr').length;

        if (rowCount != 2) {
            $(row).parents('.item-row').remove();
            if ($(".item-row").length < 2) $("#deleteRow").hide();
            this.updateSalesTax();
            this.updateTotal();
            return true;
        } else {
            alert('No se puede eliminar esta fila');
            return false;
        }

    },

    /**
     * Function cleans up the number passed and returns the cleaned up value
     * @param number
     * @param decimals
     * @returns {*}
     */
    roundNumber: function (number, decimals) {
        var newString;// The new rounded number
        decimals = Number(decimals);
        if (decimals < 1) {
            newString = (Math.round(number)).toString();
        } else {
            var numString = number.toString();
            if (numString.lastIndexOf(".") == -1) {// If there is no decimal point
                numString += ".";// give it one at the end
            }
            var cutoff = numString.lastIndexOf(".") + decimals;// The point at which to truncate the number
            var d1 = Number(numString.substring(cutoff, cutoff + 1));// The value of the last decimal place that we'll end up with
            var d2 = Number(numString.substring(cutoff + 1, cutoff + 2));// The next decimal, after the last one we want
            if (d2 >= 5) {// Do we need to round up at all? If not, the string will just be truncated
                if (d1 == 9 && cutoff > 0) {// If the last digit is 9, find a new cutoff point
                    while (cutoff > 0 && (d1 == 9 || isNaN(d1))) {
                        if (d1 != ".") {
                            cutoff -= 1;
                            d1 = Number(numString.substring(cutoff, cutoff + 1));
                        } else {
                            cutoff -= 1;
                        }
                    }
                }
                d1 += 1;
            }
            if (d1 == 10) {
                numString = numString.substring(0, numString.lastIndexOf("."));
                var roundedNum = Number(numString) + 1;
                newString = roundedNum.toString() + '.';
            } else {
                newString = numString.substring(0, cutoff) + d1.toString();
            }
        }
        if (newString.lastIndexOf(".") == -1) {// Do this again, to the new string
            newString += ".";
        }
        var decs = (newString.substring(newString.lastIndexOf(".") + 1)).length;
        for (var i = 0; i < decimals - decs; i++) newString += "0";
        //var newNumber = Number(newString);// make it a number if you like
        return newString; // Output the result to the form field (change for your purposes)
    },

    setPathValue: function (path) {
        window.filePath = path
    }

};
