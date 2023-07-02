/* myFunction toggles between adding and removing the show class, which is used to hide and show the dropdown content on click of Option button */
function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}

function ShowWarning(elementName) {
    /*Checks whether animation already exists and resets it in the element.*/
    if (elementName.classList.contains("shaking-animation")) {
        elementName.classList.remove("shaking-animation");
        void elementName.offsetWidth;
    }
    elementName.classList.add("shaking-animation");
    elementName.style.display = "block";
}

function changePassword(isUser) {
    var currentPwd = document.getElementById("currentPassword").value;
    var newPwd = document.getElementById("newPassword").value;
    var confirmPwd = document.getElementById("confirmPassword").value;

    


    // Current password validation.
    var isCurrPwdValid = true;
    isCurrPwdValid = IsValidNewPwd(currentPwd);
    if (isCurrPwdValid.isValid == true) {
        errorCurrentPassword.style.display = "none";
        currentPassword.style.border = "2px solid green";
    }
    else {
        currentPassword.style.border = "2px solid red";
        errorCurrentPassword.textContent = isCurrPwdValid.message;
        ShowWarning(errorCurrentPassword);
    }

    // New password validation.
    var isNewPwdValid = true;
    isNewPwdValid = IsValidNewPwd(newPwd);
    if (isNewPwdValid.isValid == true) {
        errorNewPassword.style.display = "none";
        newPassword.style.border = "2px solid green";
    }
    else {
        newPassword.style.border = "2px solid red";
        errorNewPassword.textContent = isNewPwdValid.message;
        ShowWarning(errorNewPassword);
    }

    // Confirm password Validation
    var isConfirmPassMatching = true;

    isConfirmPassMatching = IsValidNewPwd(confirmPwd);
    if (isConfirmPassMatching.isValid == true) {
        if (confirmPwd != newPwd) {
            isConfirmPassMatching.isValid = false;
            errorConfirmPassword.textContent = "Confirm password doesn't match new password";
            ShowWarning(errorConfirmPassword)
        }
        else {
            errorConfirmPassword.style.display = "none";
            confirmPassword.style.border = "2px solid green";
        }
    }
    else {
        confirmPassword.style.border = "2px solid red";
        errorConfirmPassword.textContent = isNewPwdValid.message;
        ShowWarning(errorConfirmPassword);
    }

    //if (confirmPwd != newPassword) {
    //    isConfirmPassMatching = false;
    //    errorConfirmPassword.textContent = "Confirm password doesn't match new password";
    //    ShowWarning(errorConfirmPassword)
    //}
    //else {
    //    ShowWarning(errorConfirmPassword);
    //}

    var allPwdError = true;
    allPwdError = IsValidAllPasswords(currentPwd, newPwd, confirmPwd);

    if (allPwdError.isValid == true) {
        errorAllPassword.style.display = "none";
    }
    else {
        errorAllPassword.textContent = allPwdError.errMsg;
        ShowWarning(errorAllPassword);
    }

    if (allPwdError.isValid == false || isCurrPwdValid.isValid == false || isNewPwdValid.isValid == false || isConfirmPassMatching.isValid == false) {
        return;
    }

    // Perform AJAX request or submit the form to change the password
    // Example: You can use jQuery's $.ajax() method to send the form data to the server

    $.ajax({
        url: "/EmployeeDetails/ChangePassword",
        type: "POST",
        data: {
            enteredPassword: currentPwd,
            newPassword: newPwd
        },
        success: function (result) {
            // Handle success response
            if (result.success == true) {
                alert(result.message);
                goBack(isUser)
                /*window.location.href = "/EmployeeDetails/EmployeeDetails";*/
            }
            else {
                alert(result.message);
            }
        },
        error: function (error) {
            // Handle error response
            alert(error);
        }
    })
}


function IsValidAllPasswords(currentPassword, newPassword, confirmPassword) {
    if (currentPassword === "" || newPassword === "" || confirmPassword === "") {
        return {
            isValid: false,
            errMsg: "Please fill out all the fields"
        };
    }

    //if (newPassword !== confirmPassword) {
    //    //alert("New Password and Confirm Password do not match.");
    //    return "New Password and Confirm Password do not match.";
    //}

    if (newPassword == currentPassword) {
        return {
            isValid: false,
            errMsg: "Current and New passwords are same, please enter different password."
        }; 
    }
    return {
        isValid: true,
        errMsg: ""
    };
}


// Shows password my passing the eye element and the password field.
function showPassword(eye, passwordFieldId) {
    var passwordField = document.getElementById(passwordFieldId);

    if ($(eye).hasClass('fa-eye-slash')) {
        $(eye).removeClass('fa-eye-slash');
        $(eye).addClass('fa-eye');
        $(passwordField).attr('type', 'text');
    } else {
        $(eye).removeClass('fa-eye');
        $(eye).addClass('fa-eye-slash');
        $(passwordField).attr('type', 'password');
    }
}


// New password validation

function IsValidNewPwd(newPwd) {
    var regex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,64})");
    
    //---------Password------------
    if (newPwd == "") {

        return {
            isValid: false,
            message: "*Required"
        };
        //errorNewPassword.textContent = "*Required";
        //return false;
    }
    if (newPwd.length < 8) {

        return {
            isValid: false,
            message: "Password must be at least 8 characters long."
        };
        //errorNewPassword.textContent = ("Password must be at least 8 characters long.");
        //return false;
    }

    // Validate password strength (add more conditions as needed)
    if (!/\d/.test(newPwd)) {

        return {
            isValid: false,
            message: "Password must contain at least one digit(0,1,2 etc)."
        };
        //errorNewPassword.textContent = ("Password must contain at least one digit.");
        //return false;
    }

    if (!/[a-z]/.test(newPwd)) {

        return {
            isValid: false,
            message: "Password must contain at least one lowercase letter."
        };
        //errorNewPassword.textContent = ("Password must contain at least one lowercase letter.");
        //return false;
    }

    if (!/[A-Z]/.test(newPwd)) {

        return {
            isValid: false,
            message: "Password must contain at least one uppercase letter."
        };
        //errorNewPassword.textContent = ("Password must contain at least one uppercase letter.");
        //return false;
    }
    if (!/\W/.test(newPwd)) {

        return {
            isValid: false,
            message: "Password must contain at least one special character."
        };
        //errorNewPassword.textContent = ("Password must contain at least one special character.");
        //return false;
    }
    if (newPwd.match(regex)) {

        return {
            isValid: true,
            message: "none"
        };
        //errorNewPassword.style.display = "none";
        //return true;
    }
    //----password end---------
}

// Checks element length.
function validateLength(element, maxLength) {
    //var maxLength = element.getAttribute("maxlength");
    if (element.value.length > maxLength) {
        element.value = element.value.slice(0, maxLength);
    }
}

function redirectToUserPage() {
    window.location.href = "/EmployeeDetails/EmployeeDetails";
}

function onLogClick() {
    return window.location.href = "/Home/logout";
}

function redirectToAdminPage() {
    return window.location.href = "/Home/AdminPage";
}

function goBack(isUser) {
    if (isUser == true) {
        return window.location.href = "/EmployeeDetails/EmployeeDetails";
    }
    else {
        return window.location.href = "/Home/AdminPage";
    }
}

//function showPassword(eye, passwordField) {
//    if (eye.classList.contains("fa-eye-slash")) {
//        eye.classList.removeClass("fa-eye-slash");
//        eye.classList.addClass("fa-eye-slash");
//        element.setAttribute("type", "text");
//    } else {
//        eye.classList.removeClass("fa-eye");
//        eye.classList.addClass("fa-eye-slash");
//        element.setAttribute("type", "password");
//    }
//}



//function showPassword(eye, passwordField) {

//    eye.click(function () {

//        if ($(eye).hasClass('fa-eye-slash')) {

//            $(eye).removeClass('fa-eye-slash');

//            $(eye).addClass('fa-eye');

//            $(passwordField).attr('type', 'text');

//        } else {

//            $(eye).removeClass('fa-eye');

//            $(eye).addClass('fa-eye-slash');

//            $(passwordField).attr('type', 'password');
//        }
//    }
//}