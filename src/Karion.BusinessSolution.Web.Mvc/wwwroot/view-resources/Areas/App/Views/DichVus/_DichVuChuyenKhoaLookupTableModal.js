(function ($) {
    app.modals.ChuyenKhoaLookupTableModal = function () {

        var _modalManager;

        var _dichVusService = abp.services.app.dichVus;
        var _$chuyenKhoaTable = $('#ChuyenKhoaTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$chuyenKhoaTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _dichVusService.getAllChuyenKhoaForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#ChuyenKhoaTableFilter').val()
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

        $('#ChuyenKhoaTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getChuyenKhoa() {
            dataTable.ajax.reload();
        }

        $('#GetChuyenKhoaButton').click(function (e) {
            e.preventDefault();
            getChuyenKhoa();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getChuyenKhoa();
            }
        });

    };
})(jQuery);

