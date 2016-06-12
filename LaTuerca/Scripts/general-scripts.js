// Define variables
var lookupSelector = '#RepuestoId'; // The ID selector to use for the autocomplete function
var lookupInput = $(lookupSelector);

var itemQtyPriceSelectors = '#Cantidad, #PrecioVenta1'; // The ID's used for the Price and Qty binding for updating price
var filePath;

/*
 First, set the path to fetch items from database
 Initialize the lookup for the first input on the page
 */
mioInvoice.setPathValue('http://localhost:53172/Compras/CargarRepuestos');
mioInvoice.fetchItems(lookupInput);

/*
 Create an Array to for adding row in table. You'll want to make sure you have the same number of columns
 You will also want to make sure that your inputs match what's in the html current row.
 Also, be sure you have the correct id's for each input.
 */


var i = $('.item-row').length;
var rowTemp = [
    '<tr class="item-row">',
    '<td><button id="deleteRow" class="btn btn-xs btn-danger tip" title="Eliminar"> <i class="im-remove2"></i></button</td>',
    '<td><input id="Id" type="hidden" value="" placeholder="" />' + // Hidden value to post to DB
        '<input id="RepuestoId" name="detallesFacturaCliente[' + i + '].RepuestoId" type="number" class="form-control input-sm" placeholder="RepuestoId" value="" /> </td>',
    '<td><input id="Nombre" name="detallesFacturaCliente[' + i + '].Nombre" type="text" class="form-control input-sm" value="" placeholder="Descripcion" readonly="readonly" /></td>',
    '<td><input id="Cantidad" name="detallesFacturaCliente[' + i + '].Cantidad" type="number" class="form-control input-sm" placeholder="Cantidad" value="0" min="1" max="100" required /></td>',
    '<td><div class="input-group"><span class="input-group-addon">Gs. </span>' +
        '<input id="PrecioVenta1" name="detallesFacturaCliente[' + i + '].Precio" class="form-control input-sm" placeholder="Precio" type="text"></div></td>',
    '<td><div class="input-group"><span class="input-group-addon">Gs. </span>' +
        '<input id="TotalLinea" name="detallesFacturaCliente[' + i + '].Total" class="form-control input-sm" type="text" placeholder="Total" readonly="readonly"></div></td>' + i++,
    '</tr>'
].join('');



/*
 We are overriding the autocomplete UI menu styles to create our own.
 You can add information from the returned json array as needed
 Just be sure that your array contains the correct value when returned
 You'll want to modify the ajax-services/fetch-inventory.php file for the returned values
 */
$.ui.autocomplete.prototype._renderItem = function (ul, item) {
    return $("<li></li>")
        .data("item.autocomplete", item)

        // This is the autocomplete list that is generated
        .append("<a class='additionalInfo'>" + item.Id + " - " + item.Nombre + " " +

            // This is the hover box that is generated when you hover over an item in the list
            "<span class='additionalInfoColor'>" +
            "<div><h4 class='text-info'>Repuesto Informaci&oacute;n</h4></div>" +
            "<div><strong>En stock: </strong> " + item.Stock + "</div>" +
            "<div><strong>Precio: </strong> Gs. " + item.PrecioVenta1 + "</div>" +
            "</span> </a>")

        .appendTo(ul);
};

/*
 Here's where we start adding rows to the invoice
 Add row to list and allow user to use autocomplete to find items.
 */

$("#addRowBtn").on('click', function (e) {
    mioInvoice.addRow(lookupSelector);
    i++;
    e.preventDefault();
});

/*
 Update invoice total when item Qty or Price inputs have been updated
 */
$(itemQtyPriceSelectors).on('keyup click focus', function () {
    mioInvoice.updatePrice(this);
});

/*
 Update invoice total when invoice tax input has changed
 */
$("#input#Cantidad").on('click', function (e) {
    mioInvoice.updatePrice("#itemQtyPriceSelectors");
    mioInvoice.updateTotal();
    e.preventDefault();

});
$("input#tax").on('keyup', function () {
    mioInvoice.updateTotal();
});


/*
 Remove row when clicked
 */
$(document).on('click', '#deleteRow', function () {
    mioInvoice.deleteRow(this);
});

$('button#saveInvoiceBtn').on('click', function (e) {

    $(window).unbind("beforeunload");

    
    /*  Use this to save the post data to the database using ajax */

//    var postData = $("#invoiceForm").serialize();
//
//    alert(postData);
//
//    $.post("http://localhost:53172/Compras/AjaxCreate", { data: postData })
//        .done(function (data) {
//            alert("Post Data: " + data);
//        });
//
//    e.preventDefault();

});

/*
 We don't want the user to leave the page if they have started working with it so we set the
 onbeforeload method
 */
$('body').on("focus", lookupSelector, function () {
    $(window).bind('beforeunload', function () {
        return "Usted no ha guardado los datos. Seguro que quieres salir de esta pagina sin guardar primero?";
    });
});


/*
 Helpers
 */

var deleteConfirm = function (msg) {
    var a = confirm(msg);
    if (a) {
        return true;
    } else {
        return false;
    }
};


// Show loader when PDF is being generated.
$(document).on("click", '#genPDFbtn', function () {
    $("#loader").show();
});


/*
 Destroy the modal content ** WE NEED THIS ** This prevents the modal from having the same information
 loaded when opening it when separate information.
 */
$(document.body).on('hidden.bs.modal', function () {
    $('#myModal').removeData('bs.modal')
});


/*

 Allow for inline editing

 */

/*
 This saves the values to the hidden input for the inout we are modifying.
 */
$(document).on("keyup", 'input.editbox', function () {
    // Get the value for the selected input on every key up
    var thisValue = $(this, 'input').val();
    var selectedName = $(this, 'input').attr('id');
    $("input[name="+selectedName+"]").val(thisValue);
});


function divClicked() {
    var divHtml = $(this).html();
    var selectName = $(this).attr("id");
    var editableText = $('<input id="'+selectName+'" class="form-control input-sm editbox" type="text">');
    editableText.val(divHtml);
    $(this).replaceWith(editableText);
    editableText.focus();

    // setup the blur event for this new input area
    editableText.blur(function(){
        var html = $(this).val();
        var viewableText = $('<div class="edit" id="'+selectName+'">');
        viewableText.html(html);
        $(this).replaceWith(viewableText);
        // setup the click event for this new div
        viewableText.click(divClicked);
    });
}


$(document).ready(function () {
    $("div.edit").click(divClicked);
});