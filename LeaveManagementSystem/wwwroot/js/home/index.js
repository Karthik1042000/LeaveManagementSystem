let globalUserId;
let globalRole;
async function fetchCachedUserDetails() {
    try {
        const response = await fetch('/Home/CachedDetails');
        if (response.ok) {
            const employeeData = await response.json();
            globalUserId = employeeData.userId;
            globalRole = employeeData.userRole;
          
            document.getElementById('employeeId').value = globalUserId;
            document.getElementById('employeeRole').value = globalRole;
            const profileLink = document.getElementById('profileLink');
            if (globalUserId) {
                profileLink.href = `/Employee/Profile?id=${globalUserId}`; 
                historyLink.href = `/LeaveApplication/ApplicationHistory?id=${globalUserId}`;
            }

           
            displayRoleBoxes(globalRole);
        } else {
            console.error('Failed to fetch cached details');
        }
    } catch (error) {
        console.error('Error fetching cached details:', error);
    }
}


function displayRoleBoxes(role) {

    document.getElementById('boxLeaveApply').classList.remove('hidden');
    document.getElementById('boxLeaveHistory').classList.remove('hidden');
    document.getElementById('boxAvailableLeaves').classList.remove('hidden');

 
    if (role === 'Admin') {
        document.getElementById('boxAdditional').classList.remove('hidden');
        document.getElementById('boxEmployeeManagement').classList.remove('hidden');
        document.getElementById('boxRoleManagement').classList.remove('hidden');
        document.getElementById('boxLeaveRecordManagement').classList.remove('hidden');
    }
}


window.onload = function () {
    fetchCachedUserDetails(); 
};
