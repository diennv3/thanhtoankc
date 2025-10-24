(function ($) {
    app.modals.HanetTenantPlaceDatasLookupTableModal = function () {

        var _modalManager;

        var _hanetTenantDeviceDatasesService = abp.services.app.hanetTenantDeviceDatases;
        var _$hanetTenantPlaceDatasTable = $('#HanetTenantPlaceDatasTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$hanetTenantPlaceDatasTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _hanetTenantDeviceDatasesService.getAllHanetTenantPlaceDatasForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#HanetTenantPlaceDatasTableFilter').val()
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

        $('#HanetTenantPlaceDatasTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getHanetTenantPlaceDatas() {
            dataTable.ajax.reload();
        }

        $('#GetHanetTenantPlaceDatasButton').click(function (e) {
            e.preventDefault();
            getHanetTenantPlaceDatas();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getHanetTenantPlaceDatas();
            }
        });

    };
})(jQuery);

