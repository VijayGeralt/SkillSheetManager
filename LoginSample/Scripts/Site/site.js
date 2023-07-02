$(document).ready(function () { 
    $("#SelectedGroup").click(function () {     
        var data = { "userType": $("#SelectedGroup").val() }
        $("select[id$=SelectedUser] > option").remove();
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Home/UserNameBind",
            data: JSON.stringify({ "userType": $("#SelectedGroup").val() }),
            dataType: "json",
            success: function (Result) {
                $.each(Result, function (key, value) {
                    $("#SelectedUser").append($("<option></option>").val
                        (value.Value).html(value.Text));
                });
            }
        })
    })
});