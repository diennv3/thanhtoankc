(function ($) {
    app.modals.DichVuLookupTableModal = function () {

        var _modalManager;

        var _giaDichVusService = abp.services.app.giaDichVus;
        var _$dichVuTable = $('#DichVuTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$dichVuTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _giaDichVusService.getAllDichVuForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#DichVuTableFilter').val()
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

        $('#DichVuTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getDichVu() {
            dataTable.ajax.reload();
        }

        $('#GetDichVuButton').click(function (e) {
            e.preventDefault();
            getDichVu();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getDichVu();
            }
        });

    };
})(jQuery);

