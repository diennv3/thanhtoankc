(function ($) {
    app.modals.CreateOrEditLeaveRequestModal = function () {

        var _leaveRequestsService = abp.services.app.leaveRequests;

        var _modalManager;
        var _$leaveRequestInformationForm = null;

        var _LeaveRequestnguoiBenhLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/LeaveRequests/NguoiBenhLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/LeaveRequests/_LeaveRequestNguoiBenhLookupTableModal.js',
            modalClass: 'NguoiBenhLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$leaveRequestInformationForm = _modalManager.getModal().find('form[name=LeaveRequestInformationsForm]');
            _$leaveRequestInformationForm.validate();

            function toggleHalfDayPart() {
                var typeVal = $('#LeaveRequest_Type').val();
                if (typeVal === '1' || typeVal === 'HalfDay') {
                    $('#halfDayPartRow').show();
                } else {
                    $('#halfDayPartRow').hide();
                    $('#LeaveRequest_HalfDayPart').val('');
                }
            }

            function toggleAllowedMinutesRows() {
                if ($('#LeaveRequest_IsForLateArrival').is(':checked')) {
                    $('#allowedLateRow').show();
                } else {
                    $('#allowedLateRow').hide();
                    $('#LeaveRequest_AllowedLateMinutes').val('');
                }

                if ($('#LeaveRequest_IsForEarlyLeave').is(':checked')) {
                    $('#allowedEarlyRow').show();
                } else {
                    $('#allowedEarlyRow').hide();
                    $('#LeaveRequest_AllowedEarlyMinutes').val('');
                }
            }

            $('#LeaveRequest_Type').change(function () {
                toggleHalfDayPart();
            });

            $('#LeaveRequest_IsForLateArrival').change(function () {
                toggleAllowedMinutesRows();
            });

            $('#LeaveRequest_IsForEarlyLeave').change(function () {
                toggleAllowedMinutesRows();
            });

            toggleHalfDayPart();
            toggleAllowedMinutesRows();
        };

        $('#OpenNguoiBenhLookupTableButton').click(function () {
            var leaveRequest = _$leaveRequestInformationForm.serializeFormToObject();
            _LeaveRequestnguoiBenhLookupTableModal.open({ id: leaveRequest.nguoiBenhId, displayName: leaveRequest.nguoiBenhUserName }, function (data) {
                _$leaveRequestInformationForm.find('input[name=nguoiBenhUserName]').val(data.displayName);
                _$leaveRequestInformationForm.find('input[name=nguoiBenhId]').val(data.id);
            });
        });

        $('#ClearNguoiBenhUserNameButton').click(function () {
            _$leaveRequestInformationForm.find('input[name=nguoiBenhUserName]').val('');
            _$leaveRequestInformationForm.find('input[name=nguoiBenhId]').val('');
        });

        $('#ClearShiftNameButton').click(function () {
            // removed shift handling - noop
        });

        this.save = function () {
            if (!_$leaveRequestInformationForm.valid()) {
                return;
            }

            if ($('#LeaveRequest_NguoiBenhId').prop('required') && $('#LeaveRequest_NguoiBenhId').val() == '') {
                abp.message.error('Người được nghỉ là bắt buộc');
                return;
            }

            var leaveRequest = _$leaveRequestInformationForm.serializeFormToObject();

            leaveRequest.isForEarlyLeave = $('#LeaveRequest_IsForEarlyLeave').is(':checked');
            leaveRequest.isForLateArrival = $('#LeaveRequest_IsForLateArrival').is(':checked');

            var allowedLate = $('#LeaveRequest_AllowedLateMinutes').val();
            leaveRequest.allowedLateMinutes = (allowedLate !== '' && !isNaN(allowedLate)) ? parseInt(allowedLate, 10) : null;
            var allowedEarly = $('#LeaveRequest_AllowedEarlyMinutes').val();
            leaveRequest.allowedEarlyMinutes = (allowedEarly !== '' && !isNaN(allowedEarly)) ? parseInt(allowedEarly, 10) : null;

            _modalManager.setBusy(true);
            _leaveRequestsService.createOrEdit(
                leaveRequest
            ).done(function () {
                abp.notify.info('Lưu thành công');
                _modalManager.close();
                abp.event.trigger('app.createOrEditLeaveRequestModalSaved');
            }).fail(function (err) {
                var msg = (err && err.message) ? err.message : 'Có lỗi xảy ra';
                abp.message.error(msg, 'Lỗi');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);