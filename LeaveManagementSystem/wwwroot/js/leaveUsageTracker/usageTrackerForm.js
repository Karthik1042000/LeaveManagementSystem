window.onload = function () {
    document.getElementById("annualLeaveField").onblur = validateAnnualLeave;
    document.getElementById("casualLeaveField").onblur = validateCasualLeave;
    document.getElementById("restrictedHolidayField").onblur = validateRestrictedHoliday;
    document.getElementById("bonusLeaveField").onblur = validateBonusLeave;
    
    document.getElementById("leaveTrackForm").onsubmit = function (event) {
        if (!checkFormValidity()) {
            event.preventDefault(); 
        }
    };
};

function checkFormValidity() {
    const annualLeaveValid = validateAnnualLeave(true);
    const casualLeaveValid = validateCasualLeave(true);
    const restrictedHolidayValid = validateRestrictedHoliday(true);
    const bonusLeaveValid = validateBonusLeave(true);
    
    const submitButton = document.getElementById("submitButton");
    submitButton.disabled = !(annualLeaveValid && casualLeaveValid && restrictedHolidayValid && bonusLeaveValid);

    return annualLeaveValid && casualLeaveValid && restrictedHolidayValid && bonusLeaveValid;
}

function validateAnnualLeave(showError = false) {
    return validateIntegerField("annualLeaveField", "annualLeaveError", showError);
}

function validateCasualLeave(showError = false) {
    return validateIntegerField("casualLeaveField", "casualLeaveError", showError);
}

function validateRestrictedHoliday(showError = false) {
    return validateIntegerField("restrictedHolidayField", "restrictedHolidayError", showError);
}

function validateBonusLeave(showError = false) {
    return validateIntegerField("bonusLeaveField", "bonusLeaveError", showError);
}

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
    event.preventDefault(); 
    
    const formData = {
        aLUsed: document.getElementById('annualLeaveField').value,
        cLUsed: document.getElementById('casualLeaveField').value,
        rHUsed: document.getElementById('restrictedHolidayField').value,
        bLUsed: document.getElementById('bonusLeaveField').value,
        id: document.getElementById('idField').value
    };

    try {
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
            const errorResponse = await response.json(); 
            ToastMessage('Error', errorResponse.error.message, 'warning', '#de5b3f');
        }
    } catch (error) {
        console.error('Error:', error);
    }
});
