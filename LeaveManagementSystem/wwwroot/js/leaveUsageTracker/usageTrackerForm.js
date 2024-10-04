window.onload = function () {
    // Attach blur events to validate on field loss of focus
    document.getElementById("annualLeaveField").onblur = validateAnnualLeave;
    document.getElementById("casualLeaveField").onblur = validateCasualLeave;
    document.getElementById("restrictedHolidayField").onblur = validateRestrictedHoliday;
    document.getElementById("bonusLeaveField").onblur = validateBonusLeave;

    // Attach submit event to the form
    document.getElementById("leaveTrackForm").onsubmit = function (event) {
        if (!checkFormValidity()) {
            event.preventDefault(); // Prevent form submission if not valid
        }
    };
};

function checkFormValidity() {
    const annualLeaveValid = validateAnnualLeave(true);
    const casualLeaveValid = validateCasualLeave(true);
    const restrictedHolidayValid = validateRestrictedHoliday(true);
    const bonusLeaveValid = validateBonusLeave(true);

    // Enable or disable the button based on the validity of fields
    const submitButton = document.getElementById("submitButton");
    submitButton.disabled = !(annualLeaveValid && casualLeaveValid && restrictedHolidayValid && bonusLeaveValid);

    return annualLeaveValid && casualLeaveValid && restrictedHolidayValid && bonusLeaveValid;
}

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

document.getElementById('leaveTrackForm').addEventListener('submit', async function (event) {
    event.preventDefault(); // Prevent the default form submission

    // Collect the form data
    const formData = {
        aLUsed: document.getElementById('annualLeaveField').value,
        cLUsed: document.getElementById('casualLeaveField').value,
        rHUsed: document.getElementById('restrictedHolidayField').value,
        bLUsed: document.getElementById('bonusLeaveField').value,
        id: document.getElementById('idField').value
    };

    try {
        // Send a POST request to the server
        const response = await fetch('/LeaveUsageTracker/SaveLeaveUsageTracker', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(formData)
        });

        if (response.ok) {
            const result = await response.json();
            console.log('Success:', result);
            ToastMessage('Success', 'Successfully Updated', 'success', 'green');
            setTimeout(function () {
                window.location.href = '/LeaveUsageTracker/LeaveUsageTrackerManagement';
            }, 2000);
        } else {
            const errorResponse = await response.json(); // Read the error response as JSON
            ToastMessage('Error', errorResponse.error.message, 'warning', '#de5b3f');
        }
    } catch (error) {
        console.error('Error:', error);
    }
});
