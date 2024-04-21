window.showDialog = function (dialogId) {
    console.log('Showing dialog:', dialogId);
    const dialog = document.getElementById(dialogId);
    if (dialog) {
        dialog.style.display = 'flex'; // Show the dialog
    }
};

window.hideDialog = function (dialogId) {
    console.log('Hiding dialog:', dialogId);
    const dialog = document.getElementById(dialogId);
    if (dialog) {
        dialog.style.display = 'none'; // Hide the dialog
    }
};