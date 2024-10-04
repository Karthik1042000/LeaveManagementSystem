﻿let isModified = false;
const originalName = document.getElementById('name').value;
const originalPassword = document.getElementById('password').value;

document.addEventListener("DOMContentLoaded", function () {
    const saveButton = document.getElementById('saveChangesButton');
    const nameInput = document.getElementById('name');
    const passwordInput = document.getElementById('password');

    // Add event listeners for input changes
    nameInput.addEventListener('input', validateFields);
    passwordInput.addEventListener('input', validateFields);
    
    function validateFields() {
        const nameValue = nameInput.value;
        const passwordValue = passwordInput.value;

        // Check if the values have changed
        const isNameChanged = nameValue !== originalName;
        const isPasswordChanged = passwordValue !== originalPassword;

        const isNameValid = nameValue.length > 3; 
        const isPasswordValid = passwordValue.length > 5;

        if ((isNameChanged && isNameValid) || (isPasswordChanged && isPasswordValid)) {
            saveButton.disabled = false; // Enable Save Changes button
        } else {
            saveButton.disabled = true;
        }
    }
    function initializeForm() {
        saveButton.disabled = true;
    }
    initializeForm();

    // Function to handle form submission
    window.formSubmit = function (event) {
        event.preventDefault(); 

        const userProfile = {
            Id: document.getElementById('employeeId').value,
            Name: nameInput.value,
            Email: document.getElementById('email').value,
            RoleName: document.getElementById('role').value,
            Password: passwordInput.value
        };
        // Submit the data to the server using fetch
        fetch('/employee/saveProfile', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(userProfile)
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log('Success:', data);
                ToastMessage('Success', 'Successfully updated', 'success', 'green');
                setTimeout(function () {
                    window.location.href = '/Employee/Profile?id=' + userProfile.Id;
                }, 2000);
            })
            .catch((error) => {
                console.error('Error:', error);
            });

        return false; // Prevent further form submission handling
    };
});
