let btnFilter = document.getElementById('btnFilter')


function filterCatalogue()
{
    location.replace("/home/Catalog?catId=" + document.getElementById('catFilter').value)
}
if (btnFilter.is)
btnFilter.addEventListener('click', filterCatalogue)