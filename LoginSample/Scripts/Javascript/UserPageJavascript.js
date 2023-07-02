// Get the button, and when the user clicks on it, execute myFunction
//document.getElementById("myBtn").onclick = function () { myFunction() };

/*const { error } = require("jquery");*/
//const { json } = require("modernizr");

/* myFunction toggles between adding and removing the show class, which is used to hide and show the dropdown content on click of Option button */
function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}
var initialAgeValue = $("#DateOfBirth").val();
var changedAgeValue;
var uPhoto = '';
var isPhotoValid = true;
//$(document).ready(function () {
    
//});

// Displays warning.
function ShowWarning(elementName) {
    /*Checks whether animation already exists and resets it in the element.*/
    if (elementName.classList.contains("shaking-animation")) {
        elementName.classList.remove("shaking-animation");
        void requiredName.offsetWidth;
    }
    elementName.classList.add("shaking-animation");
    elementName.style.display = "block";
}


function AddUserDetails(e) {
    var uName = $("#Name").val().trim();
    var uDesignation = $("#Designation").val().trim();
    var uDateOfBirth = $("#DateOfBirth").val();
    var uJoiningDate = $("#JoiningDate").val();
    var uQualifications = $("#Qualification").val().trim();
    var uLanguages = $("#Languages").val().trim();
    var uDatabase = $("#Database").val().trim();
    /*var uSex = $("input[type=radio][name=optradio]:checked").val();*/
    /*var uSex = $('[name="optradio"]:radio:checked').val();*/
    var uSex = "";
    if (document.querySelector('input[name="radioOptionsSex"]:checked')) {
        uSex = document.querySelector('input[name="radioOptionsSex"]:checked').value;
    }

    var uWorkedInJapan = "";
    if (document.querySelector('input[name="radioOptionsJapan"]:checked')) {
        uWorkedInJapan = document.querySelector('input[name="radioOptionsJapan"]:checked').value;
    }


    //Name validation
    var isNameValid = true;
    isNameValid = IsValidName(uName);
    if (isNameValid == true) {
        requiredName.style.display = "none";
        Name.style.border = "2px solid green";
    }
    else {
        Name.style.border = "2px solid red";
        ShowWarning(requiredName);
    }

    //Designation validation
    var isDesignationValid = true;
    isDesignationValid = IsValidDesignation(uDesignation);
    if (isDesignationValid == true) {
        requiredDesignation.style.display = "none";
        Designation.style.border = "2px solid green";
    }
    else {
        Designation.style.border = "2px solid red";
        ShowWarning(requiredDesignation);
    }

    //Date of birth validation
    var isDOBValid = true;
    isDOBValid = isValidDate(uDateOfBirth, false);
    if (isDOBValid == true) {
        requiredDOB.style.display = "none";
        DateOfBirth.style.border = "2px solid green";
    }
    else {
        DateOfBirth.style.border = "2px solid red";
        ShowWarning(requiredDOB);
    }

    //Joining date validation
    var isJoiningDateValid = true;
    isJoiningDateValid = isValidDate(uJoiningDate, true, uDateOfBirth);
    if (isJoiningDateValid == true) {
        errorJoiningDate.style.display = "none";
        JoiningDate.style.border = "2px solid green";
    }
    else {
        JoiningDate.style.border = "2px solid red";
        ShowWarning(errorJoiningDate);
    }

    //Sex validation
    var isSexValid = true;
    isSexValid = IsValidSex(uSex);
    if (isSexValid == false) {
        ShowWarning(requiredSex);
    }
    else {
        requiredSex.style.display = "none";
    }

    //Worked in japan validaion
    var isWorkedInJapanValid = true;
    isWorkedInJapanValid = IsValidWorkedInJapan(uWorkedInJapan);
    if (isWorkedInJapanValid == false) {
        ShowWarning(errorWorkedInJapan);
    }
    else {
        errorWorkedInJapan.style.display = "none";
    }

    /*Checking all validated entries.*/
    if (isNameValid == false || isDesignationValid == false || isDOBValid == false || isJoiningDateValid == false || isSexValid == false || isWorkedInJapanValid == false || isPhotoValid == false) {
        return;
    }
    var modelObj = {
        Name : uName,
        Designation: uDesignation,
        DateOfBirth: new Date(uDateOfBirth),
        Sex: uSex,
        JoiningDate: new Date(uJoiningDate),
        WorkedInJapan: uWorkedInJapan,
        Qualification: uQualifications,
        Languages: uLanguages,
        Database: uDatabase
    }
    SubmitData(modelObj)
}

function SubmitData(modelObj) {
    $.ajax({
        url: "/EmployeeDetails/AddUserData",
        type: "POST",
        data: JSON.stringify({
            "employeeDetailsModel": modelObj,
            "userPhoto": uPhoto }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            data = JSON.parse(data)
            console.log(data);
            alert("Data submitted successfully!!!");
        },
        error: function (err) {
            console.log(err);
        }
    })
}

/*Gets the photo.*/
function fileCheck(files) {
    //var fileData;
    //var r = new FileReader();
    //r.readAsBinaryString(files.files[0]);
    var fileNameExt = files.files[0].name.substring(
        files.files[0].name.lastIndexOf('.') + 1).toLowerCase();
    var validExtensions = ['jpg', 'png', 'jpeg'];
    if ($.inArray(fileNameExt, validExtensions) == -1) {
        alert("Only these file types are accepted : " + validExtensions.join(', '));
        return;
    }
    var fileSize = files.files[0].size / 1024; // Size in KB.
    if (100 > fileSize || fileSize > 500) {
        isPhotoValid = false;
        alert("Photo should be in range of 100KB to 500KB");
        return;
    }
    convertFileToBase64(files.files[0])
        .then(base64String => {
            /*console.log('Base64 String:', base64String);*/
            uPhoto = base64String;
            var previewImage = URL.createObjectURL(files.files[0]);
            var imageDiv = document.getElementById('imagePreview');
            var newImg = document.createElement('img');
            newImg.style.borderRadius = "50%";
            newImg.style.width = "200px";
            newImg.style.height = "200px";
            imageDiv.innerHTML = '';
            newImg.src = previewImage;
            imageDiv.appendChild(newImg);
        })
        .catch(error => {
            console.error('Error:', error);
        });
}

/*Converts file to a base 64 string*/
function convertFileToBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();

        // Set up the FileReader onload event
        reader.onload = () => {
            const base64String = reader.result.split(',')[1];
            resolve(base64String);
        };

        // Set up the FileReader onerror event
        reader.onerror = error => {
            reject(error);
        };

        // Read the file as Data URL
        reader.readAsDataURL(file);
    });
}

// Closes the top menu in the event of outside click
window.onclick = function (event) {
    if (!event.target.matches('#myBtn')) {

        var dropdowns =
            document.getElementsByClassName("dropdown-content");

        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
    //UpdateAgeDynamically();
}


/*Validates user name.*/
function IsValidName(uName) {
    if (uName.length == 0) {
        requiredName.textContent = "*Required";
        return false;
    }
    var pattern = /^[a-zA-Z\s]+$/;
    /*var letters = /^[A-Za-z]+$/;*/
    if (!uName.match(pattern)) {
        requiredName.textContent = "Only letters are allowed.";
        return false;
    }
    if (4 > uName.length || uName.length > 20) {
        requiredName.textContent = "Abnormal number of characters.";
        return false;
    }
    
    return true;
}

/*Validates user designation.*/
function IsValidDesignation(uDesignation) {
    if (uDesignation.length == 0) {
        requiredDesignation.textContent = "*Required";
        return false;
    }
    var pattern = /^[a-zA-Z\s]+$/;
    if (!uDesignation.match(pattern)) {
        requiredDesignation.textContent = "Only letters are allowed.";
        return false;
    }
    return true;
}

// Validates sex.
function IsValidSex(uSex) {
    if (0 >= uSex.length || uSex.length > 6) {
        requiredSex.textContent = (uSex.length == 0) ? "*Required" : "Invalid input";
        return false;
    }
    else if (!(uSex == "Male" || uSex == "Female")) {
        requiredSex.textContent = "Invalid input";
        return false;
    }
    return true;
}

// Validates worked in japan.
function IsValidWorkedInJapan(uWorkedInJapan) {
    if (0 >= uWorkedInJapan.length || uWorkedInJapan.length > 3) {
        errorWorkedInJapan.textContent = "*Required";
        return false;
    }
    else if (!(uWorkedInJapan == "Yes" || uWorkedInJapan == "No")) {
        errorWorkedInJapan.textContent = "Invalid input";
        return false;
    }
    return true;
}

function isValidDate(enteredDate, isJoiningDate, uDateOfBirth) {
    var errorMessage = "";
    // Checks empty date input
    if (enteredDate.length == 0) {
        errorMessage = "*Required";
        (isJoiningDate == false) ? requiredDOB.textContent = errorMessage : errorJoiningDate.textContent = errorMessage;
        return false;
    }

    // Check if the input string matches the expected format
    var regex = /^\d{4}-\d{2}-\d{2}$/;
    
    if (!regex.test(enteredDate)) {
        errorMessage = "Invalid date format.";
        (isJoiningDate == false) ? requiredDOB.textContent = errorMessage : errorJoiningDate.textContent = errorMessage;
        return false;
    }

    // Parse the input string and check if it's a valid date
    var parts = enteredDate.split("-");
    var year = parseInt(parts[0], 10);
    var month = parseInt(parts[1], 10);
    var day = parseInt(parts[2], 10);

    var date = new Date(year, month - 1, day); // month is zero-based

    if (
        date.getFullYear() !== year ||
        date.getMonth() + 1 !== month ||
        date.getDate() !== day
    )
    {
        errorMessage = "Invalid date format.";
        (isJoiningDate == false) ? requiredDOB.textContent = errorMessage : errorJoiningDate.textContent = errorMessage;
        return false;
    }

    if (isJoiningDate == false) {
        var age = calculateAge(enteredDate);
        meriUmar = age;
        if (18 > age || age > 60) {
            requiredDOB.textContent = "Age range: 18 - 60 years";
            return false;
        }
    }
    else {
        var joinDate = new Date(enteredDate);
        var birthDate = new Date(uDateOfBirth);
        if (birthDate > joinDate) {
            errorJoiningDate.textContent = "Joining date can't be before birth date";
            return false;
        }
        var ageAtJoining = calculateAgeAtJoining(uDateOfBirth, enteredDate);
        if (18 > ageAtJoining || ageAtJoining > 60) {
            errorJoiningDate.textContent = "Age at joining range: 18 - 60 years";
            return false;
        }
    }
    return true;
}

/*Calculates age of user.*/
function calculateAge(dateString) {
    var today = new Date();
    var birthDate = new Date(dateString);

    var age = today.getFullYear() - birthDate.getFullYear();
    var monthDifference = today.getMonth() - birthDate.getMonth();

    if (monthDifference < 0 || (monthDifference === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }

    return age;
}

/*Calculates age of user at the time of joining.*/
function calculateAgeAtJoining(birthDate, joinDate) {
    var joinDate1 = new Date(joinDate);
    var birthDate1 = new Date(birthDate);

    var ageAtJoining = joinDate1.getFullYear() - birthDate1.getFullYear();
    var monthDifference = joinDate1.getMonth() - birthDate1.getMonth();

    if (monthDifference < 0 || (monthDifference === 0 && joinDate1.getDate() < birthDate1.getDate())) {
        ageAtJoining--;
    }

    return ageAtJoining;
}

///*Calculates base64 size.*/
//function getBase64Size(base64String) {
//    const encoding = "utf-8"; // Specify the character encoding used in the Base64 string
//    const decodedString = atob(base64String);
//    const encoder = new TextEncoder(encoding);
//    const byteLength = encoder.encode(decodedString).byteLength;
//    return byteLength;
//}

function UpdateAgeDynamically() {
    changedAgeValue = $("#DateOfBirth").val();
    if (!(changedAgeValue === initialAgeValue || changedAgeValue == "")) {
        var age = calculateAge(changedAgeValue);
        document.getElementById('Meriumar').innerHTML = (18 > age || age > 60) ? "Age : N/A" :  "Age : "+age;
        initialAgeValue = changedAgeValue;
    }
}

function GreyOutEmptyDates(dateElement) {
    if (dateElement.value == "") {
        dateElement.style.color = "#8898aa";
    }
    else
        dateElement.style.color = "rgb(0,0,0)";
}
$(document).ready(function () {
    GreyOutEmptyDates(document.getElementById('DateOfBirth'));
    GreyOutEmptyDates(document.getElementById('JoiningDate'));
    UpdateAgeDynamically();
});

// Checks element length.
function validateLength(element, maxLength) {
    //var maxLength = element.getAttribute("maxlength");
    if (element.value.length > maxLength) {
        element.value = element.value.slice(0, maxLength);
    }
}