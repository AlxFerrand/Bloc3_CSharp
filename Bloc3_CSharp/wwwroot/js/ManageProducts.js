let manageProductBtnFilter = document.getElementById('manageProductBtnFilter')

function filterManageProduct() {
    location.replace("/products/index?catId=" + document.getElementById('manageProductCatFilter').value)
}

manageProductBtnFilter.addEventListener('click', filterManageProduct)