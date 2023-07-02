
// Button display
var defUserName;
var selectedRows = [];
var selectedIndex = [];
var roesel;

var idToEdit;
var editor = document.getElementById("EditUser");
function HideButtons() {
    if ($('.isCheck:checked').length == 1) {
        document.getElementById("DelUser").disabled = false;
    }
    else if ($('.isCheck:checked').length > 1) {
        document.getElementById("DelUser").disabled = false;
    }
    else {
        document.getElementById("DelUser").disabled = true;
        document.getElementById("EditUser").disabled = true;
    }
}
var tdindexing;
function highlight_row() {
    var table = document.getElementById('display-table');
    var cells = table.getElementsByTagName('td');
    for (var i = 0; i < cells.length; i++) {
        // Take each cell
        var cell = cells[i];
        // do something on onclick event for cell
        cell.onclick = function () {
            // Get the row id where the cell exists
            var rowId = this.parentNode.rowIndex;
            // var data = table.row($(chks[i]).closest('tr')).data();
            document.getElementById("EditUser").disabled = false;
            var rowsNotSelected = table.getElementsByTagName('tr');
            var currentRow = $(this).closest("tr");
            idToEdit = currentRow.find("td:eq(3)").text();
        }
    }
}
function BoxChecker() {
    var table = document.getElementById('display-table');
    var chks = table.getElementsByClassName('isCheck');  
    for (var i = 0; i < chks.length; i++) {
        if (chks[i].checked) {
            HideButtons();
            var data = table.row($(chks[i]).closest('tr')).data();
            selectedRows.push(data[1]);
            selectedIndex.push(data[3]);
            var editdata = [];
            editdata.push(data[1], data[2]);
        }
    }
}
//end of function
// window.onclick = highlight_row;


// Check box 
$(document).ready(function () {
    highlight_row();
    $('#display-table').DataTable({
        'columnDefs': [{
            orderable: false,
            searchable: false,
            scrolly: '200vh',
            "scrollX": true,
            scrollCollapse: true,
            className: 'select-checkboxes',
            targets: 0,
            'className': '',
            'render': function (data, type, full, meta) {
                return '<input type="checkbox" class="isCheck">';
            }
        }], scrollY: '460px',
        scrollCollapse: true,
        'order': [[1, 'asc']]

    });
    var table = $('#display-table').DataTable();
    var tblData = document.getElementById("display-table");
    var chks = tblData.getElementsByClassName("isCheck");
    $(".paginate_button").click(function (e) {
        BoxChecker();
        console.log("PAgin clicked");
        $('.isCheck').change(function () {
            console.log("hello");
            HideButtons();
          
            for (var i = 0; i < chks.length; i++) {
                if (chks[i].checked) {
                    HideButtons();
                    var data = table.row($(chks[i]).closest('tr')).data();
                    selectedRows.push(data[1]);
                    selectedIndex.push(data[3]);
                    console.log(selectedRows);
                    var editdata = [];
                    editdata.push(data[1], data[2]);
                }
            }
        });

    });
    $('#display-table tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
            document.getElementById("EditUser").disabled = true;
        } else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });


    // Multiple check or all selection
    $('#all').change(function (e) {
        selectedRows = [];
        selectedIndex = [];
        $('#display-table tbody :checkbox').prop('checked', $(this).is(':checked'));
        HideButtons();
        for (var i = 0; i < chks.length; i++) {
            if (chks[i].checked) {
                HideButtons();
                document.getElementById("DelUser").disabled = false;
                var data = table.row($(chks[i]).closest('tr')).data();
                selectedRows.push(data[1]);
                selectedIndex.push(data[3]);
                // console.log(data[3]);
            }
        }
    });


    // Manual selection
    $('.isCheck').change(function () {
        console.log("hello");
        HideButtons();
        selectedRows = [];
        selectedIndex = [];
        for (var i = 0; i < chks.length; i++) {
            if (chks[i].checked) {
                HideButtons();
                var data = table.row($(chks[i]).closest('tr')).data();
                selectedRows.push(data[1]);
                selectedIndex.push(data[3]);
                var editdata = [];
                editdata.push(data[1], data[2]);
            }
        }
    });

    // Delete selected users
    $('#DelUser').click(function () {
        var url = "/Home/DeleteUser/";
        console.log(selectedRows);
        let text = "Are you sure you want to delete selected user(s) ?";
        if (confirm(text) == true) {
            $.ajax({
                url: url,
                data: { userid: selectedRows },
                cache: false,
                type: "POST",
                success: function (data) {
                    alert("User deleted !");
                    return window.location.href = "/Home/AdminPage";
                },
                error: function (reponse) {
                    alert("error : " + reponse);
                }
            });
        } else {
            text = "You canceled!";
        }
    });
});

// CURD buttons
function display() {
    var viewPopup = document.getElementById("ADDuser");
    viewPopup.style.display = 'block';
    viewPopup.style.animation = "fadeIn 1s"
}
function hide() {
    var viewPopup = document.getElementById("ADDuser");
    viewPopup.style.display = "none";
    document.getElementById('NewUser').value = '';
    document.getElementById('NewUserPassword').value = '';
    document.getElementById('NewUserEmail').value = '';
    document.getElementById('dropDErr').style.display = "none";
    document.getElementById('dropDErr1').style.display = "none";
    document.getElementById('dropDErr2').style.display = "none";
}
function EditThisUser() {
    var viewPopup = document.getElementById("userEdit");
    document.addEventListener("keydown", function (event) {
        if (event.key === "Enter") {
            event.preventDefault(); // Prevent form submission
            UpdateSelectedUser() // Trigger the form submission
        }
    });

    // Edit user details
    var url = "/Home/GetThisUser/";
    $.ajax({
        url: url,
        data: { uid: idToEdit },
        cache: false,
        type: "POST",
        dataType: "JSON",
        success: function (data) {
            console.log('india');
            $.each(data, function (index, model) {
                var selname = document.getElementById("EditUserName").value;
                if (model == false) {
                    viewPopup.style.display = "none";
                    alert(data.error);
                    return window.location.href = "/Home/AdminPage";
                }
                else {
                    defUserName = model.SelectedUser;
                    console.log(model.SelectedUser);
                    document.getElementById("EditUserName").value = model.SelectedUser;
                    document.getElementById("EditUserEmail").value = model.UserEmail;
                    document.getElementById("EditUserPassword").value = model.UserPassword;
                    viewPopup.style.display = 'block';
                    viewPopup.style.animation = "fadeIn 1s"
                }
            });
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
}
function hideEdituser() {
    var viewPopup = document.getElementById("userEdit");
    viewPopup.style.display = "none";

}

// To edit details of existing user
function UpdateSelectedUser() {
    var viewPopup = document.getElementById("userEdit");
    // Userid label
    var usern = document.getElementById("EditErr");
    // user password label
    var upass = document.getElementById("EditErr1");
    // user email label
    var uEmail = document.getElementById("EditErr2");
    var userid = $("#EditUserName").val();
    var password = $("#EditUserPassword").val();
    var uemail = $("#EditUserEmail").val();
    var url = "/Home/UpdateSelectedUser/";
    var emailset = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    var regex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,64})");
    userid = userid.trim();
    if (userid.length < 3 || userid == "") {
        usern.textContent = "*Required"
        viewPopup.style.animation = "shake 0.5s";
        usern.style.display = "block";
        return;
    }
    if (userid.length > 20) {
        usern.textContent = "*Oops! Username conatains max 20 char!";
        viewPopup.style.animation = "shake 0.5s";
        usern.style.display = "block";
        return;
    }
    if (userid.length > 3) {
        usern.style.display = "none";
    }
    // password
    if (password == "") {
        viewPopup.style.animation = "shake 0.5s";
        upass.style.display = "block";
        upass.textContent = "*Required";
        return;
    }
    if (password.length < 8) {
        viewPopup.style.animation = "shake 0.5s";
        upass.style.display = "block";
        upass.textContent = ("Password must be at least 8 characters long.");
        return;
    }

    // Validate password strength (add more conditions as needed)
    if (!/\d/.test(password)) {
        viewPopup.style.animation = "shake 0.5s";
        upass.style.display = "block";
        upass.textContent = ("Password must contain at least one digit.");
        return;
    }
    if (!/[a-z]/.test(password)) {
        viewPopup.style.animation = "shake 0.5s";
        upass.style.display = "block";
        upass.textContent = ("Password must contain at least one lowercase letter.");
        return;
    }

    if (!/[A-Z]/.test(password)) {
        viewPopup.style.animation = "shake 0.5s";
        upass.style.display = "block";
        upass.textContent = ("Password must contain at least one uppercase letter.");
        return;
    }
    if (!/\W/.test(password)) {
        viewPopup.style.animation = "shake 0.5s";
        upass.style.display = "block";
        upass.textContent = ("Password must contain at least one special character.");
        return;
    }
    if (password.match(regex)) {
        upass.style.display = "none";
    }
    // password
    if (!emailset.test(uemail)) {
        viewPopup.style.animation = "shake 0.5s";
        uEmail.style.display = "block";
        uEmail.textContent = "*Email should be like: Acty@gmail.com";
        return;
    }
    else {
        $.ajax({
            url: url,
            data: { uid: idToEdit, SelectedUname: defUserName, newUname: userid, newPassword: password, newEmail: uemail },
            cache: false,
            type: "POST",
            success: function (data) {
                if (data == 1) {
                    alert("Data Updated successfully");
                    return window.location.href = "/Home/AdminPage";
                }
                else {
                    alert("Such username already exist change username")
                }
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
    }
}
$(document).change(function () {
    $(".paginate_button").on("click", function (e) {
        var tblData = document.getElementById("display-table");
        var chks = tblData.getElementsByClassName("isCheck");
        BoxChecker();
        for (var i = 0; i < chks.length; i++) {
            if (chks[i].checked) {
                document.getElementById("DelUser").disabled = false;
                HideButtons();
                var data = table.row($(chks[i]).closest('tr')).data();
                selectedRows.push(data[1]);
                selectedIndex.push(data[3]);
                var editdata = [];
                editdata.push(data[1], data[2]);
            }            
        }
    });
});