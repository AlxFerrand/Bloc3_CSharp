let btnJohnConnect = document.getElementById('johnConnect')
let inputEmail = document.getElementById('Input_Email')
let inputPass = document.getElementById('Input_Password')

function autoConnect() {
    inputEmail.value = "JohnDoe@Test.fr"
    inputPass.value = "JohnDoe01%"
}

btnJohnConnect.addEventListener('click',autoConnect)