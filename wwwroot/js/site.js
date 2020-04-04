const pwdInput = document.getElementById('password');
const show = document.getElementById('show-pwd');

// show.addEventListener('mousedown', showPwd);

function showPwd(){
    if(pwdInput.type = 'password'){
        console.log('hello');
        show.innerHTML = '<i class="far fa-eye-slash"></i>';
        pwdInput.type = 'text';
    }
}

