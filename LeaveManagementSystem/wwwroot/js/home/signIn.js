let globalUserId;
let globalRole;
function togglePasswordVisibility() {
    const passwordField = document.getElementById('password');
    const passwordType = passwordField.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordField.setAttribute('type', passwordType);
}

function validateEmployeeId() {
    const employeeIdField = document.getElementById('id');
    const employeeIdError = document.getElementById('employeeIdError');
    const signInButton = document.getElementById('signInButton');

    employeeIdField.addEventListener('focus', () => {
        employeeIdError.style.display = 'none'; 
    });

    employeeIdField.addEventListener('blur', () => {
        if (employeeIdField.value.length !== 9) {
            employeeIdError.style.display = 'block';
            signInButton.disabled = true;
        } else {
            employeeIdError.style.display = 'none'; 
            checkFormValidity(); 
        }
    });
}

function validatePassword() {
    const passwordField = document.getElementById('password');
    const passwordError = document.getElementById('passwordError');
    const signInButton = document.getElementById('signInButton');

    passwordField.addEventListener('focus', () => {
        passwordError.style.display = 'none'; 
    });

    passwordField.addEventListener('blur', () => {
        if (passwordField.value.length <= 5) {
            passwordError.style.display = 'block'; 
            signInButton.disabled = true; 
        } else {
            passwordError.style.display = 'none';
            checkFormValidity();
        }
    });
}
function checkFormValidity() {
    const employeeIdField = document.getElementById('id');
    const passwordField = document.getElementById('password');
    const signInButton = document.getElementById('signInButton');

    if (employeeIdField.value.length === 9 && passwordField.value.length > 5) {
        signInButton.disabled = false; 
    } else {
        signInButton.disabled = true;
    }
}

window.onload = function () {
    validateEmployeeId();
    validatePassword();
};

document.getElementById('signInForm').addEventListener('submit', async function (event) {
    event.preventDefault(); 

    const employeeId = document.getElementById('id').value;
    const password = document.getElementById('password').value;

    try {
        const response = await fetch('/Home/SignIn', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Id: employeeId, Password: password })
        });

        if (response.ok) {
            const employee = await response.json();
            globalUserId = employee.userId; 
            globalRole = employee.userRole; 
            window.location.href = '/Home/Index';
        } else {
            const errorResponse = await response.json(); 
            ToastMessage('Error', errorResponse.error.message, 'warning' , '#de5b3f');
        }
    } catch (error) {
        console.error('Sign In fail:', error);
        ToastMessage('Error', 'error', 'warning', '#de5b3f');
    }
});
