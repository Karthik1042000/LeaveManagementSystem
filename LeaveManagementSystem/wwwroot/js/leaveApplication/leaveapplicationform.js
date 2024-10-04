let globalUserId;
let globalRole;
window.onload = function () {
    fetchCachedUserDetails();
    document.getElementById("startDateField").onblur = validateStartDate;
    document.getElementById("endDateField").onblur = validateEndDate;
    document.getElementById("leaveTypeField").onblur = validateLeaveType;
    
    document.getElementById("leaveApplicationForm").onsubmit = function (event) {
        if (!checkFormValidity()) {
            event.preventDefault(); 
        }
    };
};

async function fetchCachedUserDetails() {
    try {
        const response = await fetch('/Home/CachedDetails');
        if (response.ok) {
            const userData = await response.json();
            globalUserId = userData.userId;
            globalRole = userData.userRole;
            
        } else {
            console.error('Failed to fetch cached details');
        }
    } catch (error) {
        console.error('Error fetching cached details:', error);
    }
}

function validateStartDate(showError = false) {
    var startDate = new Date(document.getElementById("startDateField").value);
    var today = new Date();
    today.setDate(today.getDate() + 1);
    const startDateError = document.getElementById("startDateError");
    const dateOnly = today.toISOString().split('T')[0]; // "YYYY-MM-DD"
    if (startDate.toISOString().split('T')[0] < dateOnly) {
        if (showError) startDateError.style.display = 'block';
        return false;
    } else {
        startDateError.style.display = 'none';
        return true;
    }
}

function validateEndDate(showError = false) {
    var startDate = new Date(document.getElementById("startDateField").value);
    var endDate = new Date(document.getElementById("endDateField").value);
    const endDateError = document.getElementById("endDateError");

    if (endDate < startDate) {
        if (showError) endDateError.style.display = 'block';
        return false;
    } else {
        endDateError.style.display = 'none';
        return true;
    }
}

function validateLeaveType(showError = false) {
    var leaveType = document.getElementById("leaveTypeField").value;
    const leaveTypeFieldError = document.getElementById("leaveTypeFieldError");

    if (leaveType === "") {
        if (showError) leaveTypeFieldError.style.display = 'block';
        return false;
    } else {
        leaveTypeFieldError.style.display = 'none';
        return true;
    }
}

function checkFormValidity() {
    var isStartDateValid = validateStartDate(true);
    var isEndDateValid = validateEndDate(true);
    var isLeaveTypeValid = validateLeaveType(true);
    
    return isStartDateValid && isEndDateValid && isLeaveTypeValid;
}

document.getElementById('leaveApplicationForm').addEventListener('submit', async function (event) {
    event.preventDefault();

    const formData = {
        startDate: new Date(document.getElementById("startDateField").value),
        endDate: new Date(document.getElementById("endDateField").value),
        leaveType: parseInt(document.getElementById('leaveTypeField').value),
        employeeId: globalUserId,
        employeeName:"",
        id: parseInt($('input[name="Id"]').val()),
        approverId: null,
        approver : null
    };
    console.log(formData);
    try {
        const response = await fetch('/LeaveApplication/SaveLeaveApplication', {
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
                window.location.href = '/Home/Index';
            }, 2000);
        } else {
            const errorResponse = await response.json(); 
            ToastMessage('Error', errorResponse.error.message, 'warning', '#de5b3f');
        }
    } catch (error) {
        console.error('Error:', error);
    }
});




   