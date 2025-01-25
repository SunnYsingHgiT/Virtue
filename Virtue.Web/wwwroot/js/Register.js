function validateForm() {
    let isValid = true;

    const firstName = $('#FirstName').val().trim();
    const lastName = $('#LastName').val().trim();
    const email = $('#Email').val().trim();
    const password = $('#Password').val().trim();
    const confirmPassword = $('#ConfirmPassword').val().trim();

    if (firstName === '') {
        $('#errorFirstName').text('First Name is required.');
        isValid = false;
    } else {
        $('#errorFirstName').text('');
    }

    if (lastName === '') {
        $('#errorLastName').text('Last Name is required.');
        isValid = false;
    } else {
        $('#errorLastName').text('');
    }

    if (email === '') {
        $('#errorEmail').text('Email is required.');
        isValid = false;
    } else if (!/^\S+\S+\.\S+$/.test(email)) {
        $('#errorEmail').text('Invalid email format.');
        isValid = false;
    } else {
        $('#errorEmail').text('');
    }

    if (password === '') {
        $('#errorPassword').text('Password is required.');
        isValid = false;
    } else if (password !== confirmPassword) {
        $('#errorConfirmPassword').text('Passwords do not match.');
        isValid = false;
    } else {
        $('#errorPassword').text('');
        $('#errorConfirmPassword').text('');
    }

    if (isValid) {
        $('#registerForm').submit();
    }
}
