(function ($) {
    app.modals.NguoiThanLookupTableModal = function () {

        var _modalManager;

        var _lichHenKhamsService = abp.services.app.lichHenKhams;
        var _$nguoiThanTable = $('#NguoiThanTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$nguoiThanTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _lichHenKhamsService.getAllNguoiThanForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#NguoiThanTableFilter').val()
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" + app.localize('Select') + "' /></div>"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 1,
                    data: "displayName"
                }
            ]
        });

        $('#NguoiThanTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getNguoiThan() {
            dataTable.ajax.reload();
        }

        $('#GetNguoiThanButton').click(function (e) {
            e.preventDefault();
            getNguoiThan();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getNguoiThan();
            }
        });

    };
})(jQuery);

