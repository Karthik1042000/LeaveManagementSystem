window.onload = function () {
    document.getElementById("nameField").onblur = validateName;
    document.getElementById("emailField").onblur = validateEmail;
    document.getElementById("passwordField").onblur = validatePassword;
    document.getElementById("roleField").onblur = validateRole;
    
    document.getElementById("employeeForm").onsubmit = function (event) {
        if (!checkFormValidity()) {
            event.preventDefault(); 
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
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/; 
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
    validateName(true);
    validateEmail(true);
    validatePassword(true);
    validateRole(true);
    
    return validateName(true) && validateEmail(true) && validatePassword(true) && validateRole(true);
}

document.getElementById('employeeForm').addEventListener('submit', async function (event) {
    event.preventDefault(); 
    let idValue = null;
    if (document.getElementById('idField') !== null) {
        idValue = document.getElementById('idField').value;
    }
    const formData = {
        id: idValue,
        name: document.getElementById('nameField').value,
        email: document.getElementById('emailField').value,
        password: document.getElementById('passwordField').value,
        roleId: document.getElementById('roleField').value
    };

    try {
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
            const errorResponse = await response.json(); 
            ToastMessage('Error', errorResponse.error.message, 'warning', '#de5b3f');
        }
    } catch (error) {
        console.error('Error:', error);
    }
});