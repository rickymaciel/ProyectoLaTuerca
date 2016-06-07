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

                var $itemRow = $(this).closest('tr');

                // Modify this information to match the information coming from assets/ajax-services/fetch-inventory.php
                $itemRow.find('#Id').val(ui.item.Id); // Hidden input on form
                $itemRow.find('#RepuestoId').val(ui.item.Id);
                $itemRow.find('#Nombre').val(ui.item.Nombre);
                $itemRow.find('#PrecioVenta1').val(ui.item.PrecioVenta1);
                $itemRow.find('#Stock').val(ui.item.Stock);

                // Give focus to the next input field to receive input from user
                $itemRow.find('#Cantidad').focus();
                return false;
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
        var price = $itemRow.find('#PrecioVenta1').val().replace("Gs. ", "") * $itemRow.find('#Cantidad').val();
        price = this.roundNumber(price, 0);
        isNaN(price) ? $itemRow.find('#Total').val("N/A") : $itemRow.find('#Total').val(price);
        this.updateSalesTax();
        this.updateTotal();
    },

    /**
     * Handle the total calculation
     */
    updateTotal: function () {

        var total = 0;
        $('input#Total').each(function (i) {
            price = $(this).val().replace("Gs. ", "");
            if (!isNaN(price)) total += Number(price);
        });

        $('#subTotal').html("Gs. " + total);

        var grdTotal = total + this.updateSalesTax();

        grdTotal = this.roundNumber(grdTotal, 2);
        $('#grandTotalTop, #grandTotal').html("Gs. " + grdTotal);

    },

    updateSalesTax: function () {
        var total = 0;

        $('input#Total').each(function (i) {
            price = $(this).val().replace("Gs. ", "");
            if (!isNaN(price)) total += Number(price);
        });

        var tax = $("#tax").val() * total * "0.01";
        $("#salesTax").html("Gs. " + mioInvoice.roundNumber(tax, 2));

        return tax;
    },

    /**
     * Add row to invoice to allow for additional items
     * @param lookupSelector
     */
    addRow: function (lookupSelector) {

        // Get the table object to use for adding a row at the end of the table
        var $itemsTable = $('#itemsTable');
        var $row = $(rowTemp);

        // Add row after the first row in table
        $('.item-row:last', $itemsTable).after($row);

        // save reference to inputs within row
        var $RepuestoId = $row.find(lookupSelector);

        $RepuestoId.focus();
        mioInvoice.fetchItems($RepuestoId);

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
