let globalUserId;
window.onload = function () {
    fetchCachedUserDetails();
};

async function fetchCachedUserDetails() {
    try {
        const response = await fetch('/Home/CachedDetails');
        if (response.ok) {
            const userData = await response.json();
            globalUserId = userData.userId;
        } else {
            console.error('Failed to fetch cached details');
        }
    } catch (error) {
        console.error('Error fetching cached details:', error);
    }
}

async function Approve(id) {
    try {
        const response = await fetch(`/LeaveApplication/LeaveApplicationApprove?id=${id}&approverId=${globalUserId}`, {
            method: "GET",
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
            }
        });
        
        if (response.ok) {
            ToastMessage('Success', 'Successfully Approved', 'success', 'green');
            setTimeout(function () {
                location.reload();
            }, 2000);
        } else {
            const errorResponse = await response.json(); 
            ToastMessage('Error', errorResponse.error.message, 'warning', '#de5b3f');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

async function Reject(id) {
    try {
        const response = await fetch(`/LeaveApplication/LeaveApplicationReject?id=${id}&approverId=${globalUserId}`, {
            method: "GET",
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
            }
        });
        
        if (response.ok) {
            ToastMessage('Success', 'Successfully Rejected', 'success', 'green');
            setTimeout(function () {
                location.reload();
            }, 2000);
        } else {
            const errorResponse = await response.json(); 
            ToastMessage('Error', errorResponse.error.message, 'warning', '#de5b3f');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}

async function Delete(id) {
    try {
        const response = await fetch(`/LeaveApplication/Delete?id=${id}`, {
            method: "DELETE",
            headers: {
                'Content-Type': 'application/json; charset=utf-8',
            }
        });

        if (response.ok) {
            ToastMessage('Success', 'Successfully Deleted', 'success', 'green');
            setTimeout(function () {
                location.reload();
            }, 2000);
        } else {
            const errorResponse = await response.json(); 
            ToastMessage('Error', errorResponse.error.message, 'warning', '#de5b3f');
        }
    } catch (error) {
        console.error('Error:', error);
    }
}