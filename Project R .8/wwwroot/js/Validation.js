function ValidateForm_Login() {
    let isValid = true;

    isValid = ValidateUsername() && isValid;
    isValid = ValidatePassword() && isValid;

    if (isValid) {
        window.open('/');
    }
    return isValid
}

function ValidateForm_Signup() {
    let isValid = true;

    isValid = ValidateUsername_Signup() && isValid;
    isVlaid = ValidateDisplay_Signup() && isValid;
    isValid = ValidatePassword_Signup() && isValid;
    isValid = ValidateConfirmation_Signup() && isValid;
    isValid = ValidateEmail_Signup() && isValid;
    isValid = ValidatePhoneNum_Signup() && isValid;

    if (isValid) {
        window.open('/');
    }
    return isValid;
}

function ValidateForm_Username() {
    let isValid = true;

    isValid = ValidateUsername_ForgotPass();

    if (isValid) {
        window.open('/cshtml/PasswordReset-Answers');
    }
    return isValid;
}

function ValidateForm_SetNewPassword() {
    let isValid = true;

    isValid = ValidatePassword_SetNewPassword() && isValid;
    isValid = ValidateConfirmation_SetNewPassword() && isValid;

    if (isValid) {
        window.open('/cshtml/Login');
    }
    return isValid;
}


//login Page Validation

function ValidateUsername() {
    const username = document.getElementById("Username_Login").value;
    const usernameError = document.getElementById("Username_LoginError");

    if (!/^[A-Za-z][A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]{2,}$/.test(username)) {
        usernameError.textContent = "Must start with an English letter, be at least 3 characters, and contain no spaces.";
        return false;
    } else {
        usernameError.textContent = "";
        return true;
    }
}

function ValidatePassword() {
    const password = document.getElementById("Password_Login").value;
    const passwordError = document.getElementById("Password_LoginError");

    if (!/^(?!.*(.)\1\1)(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9])[A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]{6,12}$/.test(password)) {
        passwordError.textContent = "6 to 12 characters, English letters only, at least one capital letter, one digit and one special character, no spaces, no three identical characters in a row.";
        return false;
    } else {
        passwordError.textContent = "";
        return true;
    }
}

// Signup Page Validation

function ValidateUsername_Signup() {
    const username = document.getElementById("Username").value;
    const usernameError = document.getElementById("UsernameError");

    if (!/^[A-Za-z][A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]{2,}$/.test(username)) {
        usernameError.textContent = "Must start with an English letter, contain english letters only, be at least 3 characters, and contain no spaces.";
        return false;
    } else {
        usernameError.textContent = "";
        return true;
    }
}

function ValidateDisplay_Signup() {
    const display = document.getElementById("Display").value;
    const displayError = document.getElementById("DisplayError");

    if (!/^[A-Za-z][A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]{2,}$/.test(display)) {
        displayError.textContent = "Must start with an English letter, contain english letters only, be at least 3 characters, and contain no spaces.";
        return false;
    } else {
        displayError.textContent = "";
        return true;
    }
}

function ValidatePassword_Signup() {
    const password = document.getElementById("Password").value;
    const passwordError = document.getElementById("PasswordError");

    if (!/^(?!.*(.)\1\1)(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9])[A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]{6,12}$/.test(password)) {
        passwordError.textContent = "6 to 12 characters, English letters only, at least one capital letter, one digit and one special character, no spaces, no three identical characters in a row.";
        return false;
    } else {
        passwordError.textContent = "";
        return true;
    }
}

function ValidateConfirmation_Signup() {
    const password = document.getElementById("Password").value;
    const confirmation = document.getElementById("Confirmation").value;
    const confirmationError = document.getElementById("ConfirmationError");

    if (password != confirmation) {
        confirmationError.textContent = "Must be identical to the password entered."
        return false;
    } else {
        confirmationError.textContent = "";
        return true;
    }
}

function ValidateEmail_Signup() {
    const email = document.getElementById("Email").value;
    const emailError = document.getElementById("EmailError");

    if (!/^[A-Za-z][A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]{2,}$/.test(email)) {
        emailError.textContent = "Must start with an English letter, contain english letters only, be at least 3 characters, and contain no spaces.";
        return false;
    } else {
        emailError.textContent = "";
        return true;
    }
}

function ValidatePhoneNum_Signup() {
    const phoneNum = document.getElementById("PhoneNum").value;
    const phoneNumError = document.getElementById("PhoneNumError");

    if (!/^(05\d|08)-\d{7}$/.test(phoneNum)) {

        phoneNumError.textContent = "Digits only, must contain a hyphen, start with 05 followed by a digit or 08, then a hyphen, and exactly seven digits.";
        return false;
    }
    else {
        phoneNumError.textContent = "";
        return true;
    }
}

// Forgot Password Page Validation

function ValidateUsername_ForgotPass() {
    const username = document.getElementById("Username_ForgotPass").value;
    const usernameError = document.getElementById("Username_ForgotPassError");

    if (!/^[A-Za-z][A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]{2,}$/.test(username)) {
        usernameError.textContent = "Must start with an English letter, contain english letters only, be at least 3 characters, and contain no spaces.";
        return false;
    } else {
        usernameError.textContent = "";
        return true;
    }
}

// Set New Password Page Validation

function ValidatePassword_SetNewPassword() {
    const password = document.getElementById("newPassword_SetNewPassword").value;
    const passwordError = document.getElementById("NewPassword_SetNewPasswordError");

    if (!/^(?!.*(.)\1\1)(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9])[A-Za-z0-9!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]{6,12}$/.test(password)) {
        passwordError.textContent = "6 to 12 characters, English letters only, at least one capital letter, one digit and one special character, no spaces, no three identical characters in a row.";
        return false;
    } else {
        passwordError.textContent = "";
        return true;
    }
}

function ValidateConfirmation_SetNewPassword() {
    const password = document.getElementById("newPassword_SetNewPassword").value;
    const confirmation = document.getElementById("Confirmation_SetNewPassword").value;
    const confirmationError = document.getElementById("Confirmation_SetNewPasswordError");

    if (password != confirmation) {
        confirmationError.textContent = "Must be identical to the password entered."
        return false;
    } else {
        confirmationError.textContent = "";
        return true;
    }
}