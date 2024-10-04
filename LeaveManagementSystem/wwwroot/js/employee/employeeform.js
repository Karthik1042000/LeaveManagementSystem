window.onload = function () {
    // Attach blur events to validate on field loss of focus
    document.getElementById("nameField").onblur = validateName;
    document.getElementById("emailField").onblur = validateEmail;
    document.getElementById("passwordField").onblur = validatePassword;
    document.getElementById("roleField").onblur = validateRole;

    // Attach submit event to the form
    document.getElementById("employeeForm").onsubmit = function (event) {
        if (!checkFormValidity()) {
            event.preventDefault(); // Prevent form submission if not valid
        }
    };
};

function validateName(showError = false) {
    const nameField = document.getElementById("nameField");
    const errorElement = document.getElementById("nameError");
    if (nameField.value.trim().length < 3) {
        if (showError) errorElement.style.display = 'block';
        return false;
    } else {
        errorElement.style.display = 'none';
        return true;
    }
}

function validateEmail(showError = false) {
    const emailField = document.getElementById("emailField");
    const errorElement = document.getElementById("emailError");
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; // Simple email regex pattern
    if (!re.test(String(emailField.value).toLowerCase())) {
        if (showError) errorElement.style.display = 'block';
        return false;
    } else {
        errorElement.style.display = 'none';
        return true;
    }
}

function validatePassword(showError = false) {
    const passwordField = document.getElementById("passwordField");
    const errorElement = document.getElementById("passwordError");
    if (passwordField.value.trim().length < 5) {
        if (showError) errorElement.style.display = 'block';
        return false;
    } else {
        errorElement.style.display = 'none';
        return true;
    }
}

function validateRole(showError = false) {
    const roleField = document.getElementById("roleField");
    const errorElement = document.getElementById("roleError");
    if (roleField.value === "") {
        if (showError) errorElement.style.display = 'block';
        return false;
    } else {
        errorElement.style.display = 'none';
        return true;
    }
}

function checkFormValidity() {
    // Check all fields' validity
    validateName(true);
    validateEmail(true);
    validatePassword(true);
    validateRole(true);

    // Check if any error message is displayed
    return validateName(true) && validateEmail(true) && validatePassword(true) && validateRole(true);
}

document.getElementById('employeeForm').addEventListener('submit', async function (event) {
    event.preventDefault(); // Prevent the default form submission
    let idValue = null;
    if (document.getElementById('idField') !== null) {
        idValue = document.getElementById('idField').value;
    }
    // Collect the form data
    const formData = {
        id: idValue,
        name: document.getElementById('nameField').value,
        email: document.getElementById('emailField').value,
        password: document.getElementById('passwordField').value,
        roleId: document.getElementById('roleField').value
    };

    try {
        // Send a POST request to the server
        const response = await fetch('/Employee/SaveEmployee', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(formData)
        });

        if (response.ok) {
            const result = await response.json();
            console.log('Success:', result);
            if (formData.id === null) {
                ToastMessage('Success', 'Successfully Created', 'success', 'green');
            }
            else {
                ToastMessage('Success', 'Successfully Updated', 'success', 'green');
            }
            setTimeout(function () {
                window.location.href = '/Employee/EmployeeManagement';
            }, 2000);
        } else {
            const errorResponse = await response.json(); // Read the error response as JSON
            ToastMessage('Error', errorResponse.error.message, 'warning', '#de5b3f');
        }
    } catch (error) {
        console.error('Error:', error);
    }
});