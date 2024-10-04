window.onload = function () {
    document.getElementById("nameField").onblur = validateName;

    // Attach submit event to the form
    document.getElementById("roleForm").onsubmit = function (event) {
        if (!checkFormValidity()) {
            event.preventDefault(); // Prevent form submission if not valid
        }
    };
};

function checkFormValidity() {
    const nameValid = validateName(true);

    // Enable or disable the button based on the validity of fields
    const submitButton = document.getElementById("submitButton");
    submitButton.disabled = !(nameValid);

    return nameValid;
}
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

document.getElementById('roleForm').addEventListener('submit', async function (event) {
    event.preventDefault(); // Prevent the default form submission

    // Collect the form data
    const formData = {
        name: document.getElementById('nameField').value,
        id: document.getElementById('idField').value
    };

    try {
        // Send a POST request to the server
        const response = await fetch('/Role/SaveRole', {
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
                window.location.href = '/Role/RoleManagement';
            }, 2000);
        } else {
            const errorResponse = await response.json(); // Read the error response as JSON
            ToastMessage('Error', errorResponse.error.message, 'warning', '#de5b3f');
        }
    } catch (error) {
        console.error('Error:', error);
    }
});