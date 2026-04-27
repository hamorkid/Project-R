window.validateSignUpForm = function () {
    let username = document.getElementById('username').value;
    let displayName = document.getElementById('display').value;
    let gmail = document.getElementById('gmail').value;
    let password = document.getElementById('password').value;
    let rePassword = document.getElementById('rePassword').value;
    let city = document.getElementById('city').value;
    let phone = document.getElementById('phone').value;
    let answer1 = document.getElementById('answer1').value;
    let answer2 = document.getElementById('answer2').value;
    let checkbox1 = document.getElementById("q1").value;
    let checkbox2 = document.getElementById("q2").value;


    if (username == "") {
        alert("please fill in username");
        return false;
    }
    else if (displayName == "") {
        alert("please fill in display name");
        return false;
    }
    else if (gmail == "" || gmail == "gmail.com") {
        alert("please fill in gmail");
        return false;
    }
    else if (password == "") {
        alert("please fill in password");
        return false;
    }
    else if (password != rePassword) {
        alert("Your passwords are not matching");
        return false;
    }
    else if (city == "") {
        alert("please fill in city");
        return false;
    }
    else if (phone == "") {
        alert("please fill in phone number");
        return false;
    }
    else if (answer1 == "") {
        alert("please fill in the first answer");
        return false;
    }
    else if (answer2 == "") {
        alert("please fill in the second answer");
        return false;
    }
    else if (checkbox1 == null) {
        alert("please pick a valid question")
        return false;
    }
    else if (checkbox2 == null) {
        alert("please pick a valid question")
        return false;
    }
    else {
        alert("Account created successfully!");
        window.open('HomePage.html');
        return true;
    }
};

window.validateUsernameForm = function () {
    let username = document.getElementById('username').value;

    if (username == "") {
        alert("please fill in username");
        return false;
    }
    else {
        window.open('/cshtml/PasswordReset-Answers');
    }
};

window.validateCreatePasswordForm = function () {
    let password = document.getElementById('password').value;
    let rePassword = document.getElementById('rePassword').value;

    if (password == "") {
        alert("please fill in password");
        return false;
    }
    else if (rePassword == "") {
        alert("please fill in password");
        return false;
    }
    else if (password != rePassword) {
        alert("your passwords are not matching");
        return false;
    }
    else {
        window.open('/cshtml/Login');
    }
}

window.validateAnswersForm = function() {
    let answer1 = document.getElementById('answer1').value;
    let answer2 = document.getElementById('answer2').value;

    if (answer1 == "") {
        alert("please fill in the first answer");
        return false;
    }
    else if (answer2 == "") {
        alert("please fill in the second answer");
        return false;
    }
    else {
        window.open('/cshtml/PasswordReset-CreatePassword');
    }
}

window.validateLoginForm = function() {
    let username = document.getElementById('username').value;
    let gmail = document.getElementById('gmail').value;
    let Password = document.getElementById('Password').value;

    if (username == "") {
        alert("please fill in username");
        return false;
    }
    else if (gmail == "") {
        alert("please fill in your gmail");
        return false;
    }
    else if (Password == "") {
        alert("please fill in your gmail");
        return false;
    }
    else {
        return true;
        window.open('/');
    }
}