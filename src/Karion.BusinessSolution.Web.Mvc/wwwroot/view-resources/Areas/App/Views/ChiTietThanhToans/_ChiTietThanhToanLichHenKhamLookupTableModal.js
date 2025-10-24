(function ($) {
    app.modals.LichHenKhamLookupTableModal = function () {

        var _modalManager;

        var _chiTietThanhToansService = abp.services.app.chiTietThanhToans;
        var _$lichHenKhamTable = $('#LichHenKhamTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$lichHenKhamTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _chiTietThanhToansService.getAllLichHenKhamForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#LichHenKhamTableFilter').val()
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

        $('#LichHenKhamTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getLichHenKham() {
            dataTable.ajax.reload();
        }

        $('#GetLichHenKhamButton').click(function (e) {
            e.preventDefault();
            getLichHenKham();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getLichHenKham();
            }
        });

    };
})(jQuery);

