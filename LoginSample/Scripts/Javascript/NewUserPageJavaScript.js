$(document).ready(function () {
    $("#submit").click(function (e) {
        e.preventDefault();

        //Form data
        var data = new FormData();

        var form_data = $('#myform').serializeArray();

        $.each(form_data, function (key, input) {
            data.append(input.name, input.value);

        }

        );

        //File
        var image = $("#image").val();

        if (image != '') {
            var file_data = $('input[name="image"]')[0].files;
            for (var i = 0; i < file_data.length; i++) {
                data.append("image[]", file_data[i]);
            }
        }

        var request = $.ajax({
            url: 'submit.php',
            type: "post",
            dataType: 'json',
            processData: false,
            contentType: false,
            data: data,
        }
        );

        request.done(function (response, textStatus, jqXHR) {
            alert("done");
            console.log(response);
        }
        );
        request.fail(function (jqXHR, textStatus, errorThrown) {
            console.log(textStatus);
            console.log(errorThrown);
        }
        );

    }
    );
}
);