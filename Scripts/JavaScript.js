function DeleteCustomer() {
    var result = confirm("Voulez-vous supprimmer le client ?");
    if (result) {
        // Delete logic goes here
    }
}

function DeleteBroker() {
    var result = confirm("Voulez-vous supprimmer le courtier ?");
    if (result) {
        // Delete logic goes here
    }
}

$(function () {
    $.validator.methods.date = function (value, element) {
        Globalize.culture("fr");
        return this.optional(element) || Globalize.parseDate(value) !== null;
    }
});
$(function () {
    moment.locale('fr');
    $("#datetimepickerIndex").datetimepicker({
        locale: 'fr',
        icons: {
            time: "fas fa-clock",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down"
        },
        dateFormat: "dd/MM/yyyy",
        daysOfWeekDisabled: [0, 6],
        minDate: new Date(),
        disabledHours: [0, 1, 2, 3, 4, 5, 6, 7, 8, 20, 21, 22, 23, 24],
        disabledTimeIntervals: [[moment({ h: 0 }), moment({ h: 8 })], [moment({ h: 20 }), moment({ h: 24 })]]
    });
    $('#datetimepicker1').datetimepicker({
        locale: 'fr',
        icons: {
            time: "fas fa-clock",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down"
        },
        dateFormat: "dd/MM/yyyy",
        timeFormat: "HH:mm",
        daysOfWeekDisabled: [0, 6],
        minDate: new Date(),
        disabledHours: [0, 1, 2, 3, 4, 5, 6, 7, 8, 20, 21, 22, 23, 24],
        disabledTimeIntervals: [[moment({ h: 0 }), moment({ h: 8 })], [moment({ h: 20 }), moment({ h: 24 })]]
    });
});