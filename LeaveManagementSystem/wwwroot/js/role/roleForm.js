window.onload = function () {
    document.getElementById("nameField").onblur = validateName;
    
    document.getElementById("roleForm").onsubmit = function (event) {
        if (!checkFormValidity()) {
            event.preventDefault(); 
        }
    };
};

function checkFormValidity() {
    const nameValid = validateName(true);
    
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
    event.preventDefault();
    
    const formData = {
        name: document.getElementById('nameField').value,
        id: document.getElementById('idField').value
    };

    try {
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
            const errorResponse = await response.json(); 
            ToastMessage('Error', errorResponse.error.message, 'warning', '#de5b3f');
        }
    } catch (error) {
        console.error('Error:', error);
    }
});