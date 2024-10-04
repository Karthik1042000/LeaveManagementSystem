window.onload = function () {
    // Attach blur events to validate on field loss of focus
    document.getElementById("yearField").onblur = validateYear;
    document.getElementById("annualLeaveField").onblur = validateAnnualLeave;
    document.getElementById("casualLeaveField").onblur = validateCasualLeave;
    document.getElementById("restrictedHolidayField").onblur = validateRestrictedHoliday;
    document.getElementById("bonusLeaveField").onblur = validateBonusLeave;
    document.getElementById("roleField").onblur = validateRole;

    // Attach submit event to the form
    document.getElementById("annualLeaveRecordForm").onsubmit = function (event) {
        if (!checkFormValidity()) {
            event.preventDefault(); // Prevent form submission if not valid
        }
    };
};

// Check the validity of the form fields
function checkFormValidity() {
    const yearValid = validateYear(true);
    const annualLeaveValid = validateAnnualLeave(true);
    const casualLeaveValid = validateCasualLeave(true);
    const restrictedHolidayValid = validateRestrictedHoliday(true);
    const bonusLeaveValid = validateBonusLeave(true);
    const roleValid = validateRole(true);

    // Enable or disable the button based on the validity of fields
    const submitButton = document.getElementById("submitButton");
    submitButton.disabled = !(yearValid && annualLeaveValid && casualLeaveValid && restrictedHolidayValid && bonusLeaveValid && roleValid);

    return yearValid && annualLeaveValid && casualLeaveValid && restrictedHolidayValid && bonusLeaveValid && roleValid;
}

// Validation function for Year field
function validateYear(showError = false) {
    const yearField = parseInt(document.getElementById("yearField").value);
    const currentYear = new Date().getFullYear();
    const yearError = document.getElementById("yearError");

    if (isNaN(yearField) || yearField < currentYear) {
        if (showError) yearError.style.display = 'block';
        return false;
    } else {
        yearError.style.display = 'none';
        return true;
    }
}

// Validation function for Annual Leave
function validateAnnualLeave(showError = false) {
    return validateIntegerField("annualLeaveField", "annualLeaveError", showError);
}

// Validation function for Casual Leave
function validateCasualLeave(showError = false) {
    return validateIntegerField("casualLeaveField", "casualLeaveError", showError);
}

// Validation function for Restricted Holiday
function validateRestrictedHoliday(showError = false) {
    return validateIntegerField("restrictedHolidayField", "restrictedHolidayError", showError);
}

// Validation function for Bonus Leave
function validateBonusLeave(showError = false) {
    return validateIntegerField("bonusLeaveField", "bonusLeaveError", showError);
}

// Generic integer validation function
function validateIntegerField(fieldId, errorId, showError = false) {
    const field = parseInt(document.getElementById(fieldId).value);
    const errorElement = document.getElementById(errorId);

    if (isNaN(field) || field < 0) {
        if (showError) errorElement.style.display = 'block';
        return false;
    } else {
        errorElement.style.display = 'none';
        return true;
    }
}

// Validation function for Role field
function validateRole(showError = false) {
    const roleField = document.getElementById("roleField").value;
    const roleError = document.getElementById("roleError");

    if (!roleField.trim()) {
        if (showError) roleError.style.display = 'block';
        return false;
    } else {
        roleError.style.display = 'none';
        return true;
    }
}
document.getElementById('annualLeaveRecordForm').addEventListener('submit', async function (event) {
    event.preventDefault(); // Prevent the default form submission

    // Collect the form data
    const formData = {
        year: document.getElementById('yearField').value,
        annualLeave: document.getElementById('annualLeaveField').value,
        casualLeave: document.getElementById('casualLeaveField').value,
        restrictedHoliday: document.getElementById('restrictedHolidayField').value,
        bonusLeave: document.getElementById('bonusLeaveField').value,
        roleId: document.getElementById('roleField').value,
        id: document.getElementById('idField').value
    };

    try {
        // Send a POST request to the server
        const response = await fetch('/AnnualLeaveRecord/SaveAnnualLeaveRecord', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(formData)
        });

        if (response.ok) {
            const result = await response.json();
            console.log('Success:', result);
            if (formData.id === '0' || formData.id === '') {
                ToastMessage('Success', 'Successfully Created', 'success', 'green');
            }
            else {
                ToastMessage('Success', 'Successfully Updated', 'success', 'green');
            }
            setTimeout(function () {
                window.location.href = '/AnnualLeaveRecord/AnnualRecordManagement';
            }, 2000);
        } else {
            const errorResponse = await response.json(); // Read the error response as JSON
            ToastMessage('Error', errorResponse.error.message, 'warning', '#de5b3f');
        }
    } catch (error) {
        console.error('Error:', error);
    }
});
