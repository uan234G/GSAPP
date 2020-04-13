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

window.onload = function () {
    getCovidStats();
}

function getCovidStats() {
    fetch('https://coronavirus-tracker-api.herokuapp.com/v2/locations/225')
        .then(function (resp) { return resp.json() })
        .then(function (data) {
            let population = data.location.country_population;
            let update = data.location.last_updated;
            let confirmedCases = data.location.latest.confirmed;
            let deaths = data.location.latest.deaths;

            document.getElementById('update').innerHTML = update.substr(0, 10);

            document.getElementById('cases').innerHTML = confirmedCases.toLocaleString('en');

            document.getElementById('deaths').innerHTML = deaths.toLocaleString('en');

            document.getElementById('percent').innerHTML = ((Number(deaths) / Number(confirmedCases)) * 100).toLocaleString("en", { minimumFractionDigits: 2, maximumFractionDigits: 2 }) + "%";
            })
            .catch(function () {
                console.log("error");
            })
            setTimeout(getCovidStats, 86400000) // update every 24 hours
}