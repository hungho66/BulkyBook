var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    //DataTable lấy từ https://datatables.net/ trong phần Example --> Show and Hide
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Category/GetAll"
        },
        "columns": [
            { "data": "name", "width": "60%" },//name ở column hiện là chữ thường, trong database Category thì là Name (chữ hoa) 
            {
                //Sửa và Xóa
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="/Admin/Category/Upsert/${data}" class="btn btn-success text-white" style="cursor: pointer">
                                    <i class="fas fa-edit"></i>
                                </a>
                                <a onclick=Delete("/Admin/Category/Delete/${data}") class="btn btn-danger text-white" style="cursor: pointer">
                                    <i class="fas fa-trash-alt"></i>
                                </a>
                            </div>
                            `;
                }, "width": "40%"
            }
        ]
    });
}

function Delete(url) {
    swal({
        title: "Are you sure want to delete?",
        text: "You will not be able to restore data",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: "DELETE",
                url: url,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}