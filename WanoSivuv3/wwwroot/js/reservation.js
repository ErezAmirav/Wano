window.onload = function () {
    var tablenum = document.querySelector(".reservation__qty");

    var minus = document.getElementById("minus");

    var plus = document.getElementById("plus");

    function addNum() {
        var number = tablenum.innerHTML;
        if (number < 8) {
            number++;
            tablenum.innerHTML = number;
            disableButton();
        }
    }

    function removeNum() {
        var number = tablenum.innerHTML;
        if (number > 2) {
            number--;
            tablenum.innerHTML = number;
            disableButton();
        }
    }

    plus.addEventListener('click', addNum);
    minus.addEventListener('click', removeNum);

    function disableButton() {
        if (tablenum.innerHTML > 2) {
            minus.classList.remove("reservation__btn--disabled");
        } else {
            minus.classList.add("reservation__btn--disabled");
        }

        if (tablenum.innerHTML >= 8) {
            plus.classList.add("reservation__btn--disabled");
        } else {
            plus.classList.remove("reservation__btn--disabled");
        }
    }

    
}
function orderDone() {
    alert("Reservation Completed, Thank you!");
    window.location.href = "/Home/Index";  

}