(function ($) {
    app.modals.ShiftLookupTableModal = function () {

        var _modalManager;

        var _leaveRequestsService = abp.services.app.leaveRequests;
        var _$shiftTable = $('#ShiftTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$shiftTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _leaveRequestsService.getAllShiftForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#ShiftTableFilter').val()
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

        $('#ShiftTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getShift() {
            dataTable.ajax.reload();
        }

        $('#GetShiftButton').click(function (e) {
            e.preventDefault();
            getShift();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getShift();
            }
        });

    };
})(jQuery);

