
// Add new user
function AddNewUser() {
    var viewPopup = document.getElementById("ADDuser");
    var userid = $("#NewUser").val();
    var password = $("#NewUserPassword").val();
    var userEmail = $("#NewUserEmail").val();
    var usern = document.getElementById("dropDErr");
    var upass = document.getElementById("dropDErr1");
    var uEmail = document.getElementById("dropDErr2");
    var url = "/Home/AddNewUser/";

    $("#btnUpdate").val('Plesae wait..');
    var emailset = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    var regex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,64})");
    userid = userid.trim();
    if (userid.length < 3 || userid == "") {
        viewPopup.style.animation = "shake 0.5s";
        usern.textContent = "*Required"
        usern.style.display = "block";
        return;
    }
    if (userid.length > 20) {
        viewPopup.style.animation = "shake 0.5s";
        usern.textContent = "*Oops! Username conatains max 20 char!";
        usern.style.display = "block";
        return;
    }
    if (userid.length > 3) {
        usern.style.display = "none";
    }
    //---------Password------------
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
    //----password end---------
    if (!emailset.test(userEmail)) {
        viewPopup.style.animation = "shake 0.5s";
        uEmail.style.display = "block";
        uEmail.textContent = "*Email should be like: hello@gmail.com";
        return;
    }
    else {
        uEmail.style.display = "none";
        $.ajax({
            url: url,
            data: { userId: userid, pass: password, userEmail: userEmail },
            cache: false,
            type: "POST",
            success: function (data) {
                if (data == "1") {
                    alert("succefully entered");
                    return window.location.href = "/Home/AdminPage";
                }
                else {
                    alert("You can't use this username! , Find unique one");
                }
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
        $("#btnUpadate").val('Add');
    }
}
document.addEventListener("keydown", function (event) {
    if (event.key === "Enter") {
        event.preventDefault(); // Prevent form submission
        AddNewUser(); // Trigger the form submission
    }
});

function onChangepass() {
    return window.location.href = "/EmployeeDetails/ChangePasswordView";
}

function redirectToOverview() {
    return window.location.href = "/EmployeeDetails/GetOverview";
}