const pwdInput = document.getElementById('password');
const show = document.getElementById('show-pwd');

// show.addEventListener('mousedown', showPwd);

function showPwd(){
    if(pwdInput.type === 'password'){
        console.log('hello');
        show.innerHTML = '<i class="far fa-eye-slash"></i>';
        pwdInput.type = 'text';
    } else{
        show.innerHTML = '<i class="far fa-eye"></i>';
        pwdInput.type = 'password';
    }
}

const realFileInput = document.querySelector('.real-file');
const uploadBtn = document.querySelector('.upload-btn');
const fileCallOut = document.querySelector('.file-callout');

uploadBtn.addEventListener('click', function(){
    realFileInput.click();
});
realFileInput.addEventListener('change', function(){
    if(realFileInput.value){
        fileCallOut.innerHTML = realFileInput.value;
    } else{
        fileCallOut.innerHTML = 'No file chosen';
    }
})

