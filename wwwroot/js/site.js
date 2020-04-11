const pwdInput = document.getElementById('password');
const show = document.getElementById('show-pwd');

function showPwd(){
    if(pwdInput.type === 'password'){
        show.innerHTML = '<i class="far fa-eye-slash"></i>';
        pwdInput.type = 'text';
    } else{
        show.innerHTML = '<i class="far fa-eye"></i>';
        pwdInput.type = 'password';
    }
}

Array.prototype.forEach.call(
    document.querySelectorAll(".upload-btn"),
    function(button) {
        const hiddenInput = button.parentElement.querySelector(
        ".real-file"
        );

        const label = button.parentElement.querySelector(".file-callout");
        const defaultLabelText = "No file selected";

        label.textContent = defaultLabelText;
        label.title = defaultLabelText;

        button.addEventListener("click", function() {
            hiddenInput.click();
        });
        hiddenInput.addEventListener("change", function() {
        const filenameList = Array.prototype.map.call(hiddenInput.files, function(
            file
        ) {
            return file.name;
        });
        label.textContent = filenameList.join(", ") || defaultLabelText;
        label.title = label.textContent;
        });
    }
);