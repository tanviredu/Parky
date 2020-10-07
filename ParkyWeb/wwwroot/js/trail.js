﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/nationalPark/GetAllNationalPark",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "50%" },
            { "data": "state", "width": "20%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-center">
                                <a href="/nationalPark/Upsert/${data}" class='btn btn-success text-white'
                                    style='cursor:pointer;'> <i class='far fa-edit'></i></a>
                                    &nbsp;
                                <a onclick=Delete("/nationalPark/Delete/${data}") class='btn btn-danger text-white'
                                    style='cursor:pointer;'> <i class='far fa-trash-alt'></i></a>
                                </div>
                            `;
                }, "width": "30%"
            }
        ]
    });
} 

function Delete(url) {
    swal({
        title: "Are you sure you want to Delete",
        text: "You will not to able to restore the data!",
        icon: "warning",
        buttons: ["Cancel", "Ok"],
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'Delete',
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.erroe(data.message);
                    }
                }
            });
        }
    });
}