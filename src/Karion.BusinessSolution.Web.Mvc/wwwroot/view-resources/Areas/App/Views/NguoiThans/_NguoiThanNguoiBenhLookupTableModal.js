(function ($) {
    app.modals.NguoiBenhLookupTableModal = function () {

        var _modalManager;

        var _nguoiThansService = abp.services.app.nguoiThans;
        var _$nguoiBenhTable = $('#NguoiBenhTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$nguoiBenhTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _nguoiThansService.getAllNguoiBenhForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#NguoiBenhTableFilter').val()
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

        $('#NguoiBenhTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getNguoiBenh() {
            dataTable.ajax.reload();
        }

        $('#GetNguoiBenhButton').click(function (e) {
            e.preventDefault();
            getNguoiBenh();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getNguoiBenh();
            }
        });

    };
})(jQuery);

