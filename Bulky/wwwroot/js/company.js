
//initialisation de la table de données DataTable pour afficher les produits 
var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    console.log("test");
    dataTable = $('#tlbTable').DataTable({
        "ajax": { url:'/admin/product/getall' },
        "columns": [
            //{ data: 'id', "width": "5%" },
            { data: 'title', "width": "15%" },
            { data: 'isbn', "width": "10%" },
            { data: 'author', "width": "10%" },
            { data: 'listPrice', "width": "10%" },
            { data: 'price', "width": "10%" },
            { data: 'price50', "width": "10%" },
            { data: 'price100', "width": "10%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return `
                            <div class="d-flex">
                                    <a class="text-info" href="/Admin/Product/upsert?Id=${data}">
                                        <i class="bi bi-pen-fill"></i>
                                    </a>
                                    <a class="mx-3 text-danger" onClick=Delete('/Admin/Product/delete/${data}')>
                                        <i class="bi bi-trash"></i>
                                    </a>
                                </div>
                    `;
                }, "width": "10%"
            }
        ]
    });
}

//fonction pour supprimer un produit avec une confirmation via SweetAlert
function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
                $.ajax({
                    type: "DELETE",
                    url: url,
                    success: function (data) {
                        //recharger la table
                        dataTable.ajax.reload();
                        //afficher le message de succès
                        toastr.success(data.message);
                    }
                })
                
        }
    });
}
