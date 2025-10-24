(function ($) {
    app.modals.CreateOrEditNguoiBenhModal = function () {

        var _nguoiBenhsService = abp.services.app.nguoiBenhs;
        var _shiftsService = abp.services.app.shifts;

        var _modalManager;
        var _$nguoiBenhInformationForm = null;

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });
            
            _$nguoiBenhInformationForm = _modalManager.getModal().find('form[name=NguoiBenhInformationsForm]');
            _$nguoiBenhInformationForm.validate();

            if (_shiftsService && typeof _shiftsService.getAllActiveForSelect === 'function') {
                _shiftsService.getAllActiveForSelect().done(function (result) {
                    var $sel = modal.find('#AssignedShiftId');
                    $sel.empty();
                    $sel.append($('<option/>').val('').text('-- Không chọn (dùng cấu hình chung) --'));
                    $.each(result, function (i, s) {
                        var text = s.name;
                        if (s.startTime && s.endTime) {
                            var sShort = s.startTime.length > 5 ? s.startTime.substring(0, 5) : s.startTime;
                            var eShort = s.endTime.length > 5 ? s.endTime.substring(0, 5) : s.endTime;
                            text += ' (' + sShort + ' - ' + eShort + ')';
                        }
                        $sel.append($('<option/>').val(s.id).text(text));
                    });

                    
                    var assigned = modal.find('input[name=id]').length ? modal.find('input[name=id]').val() : null;
                    
                    var cur = modal.find('#AssignedShiftId').data('current');
                    if (!cur) {
                        
                        var hv = modal.find('input[name=AssignedShiftId]').val();
                        if (hv) modal.find('#AssignedShiftId').val(hv);
                    } else {
                        modal.find('#AssignedShiftId').val(cur);
                    }
                }).fail(function () {
                    
                });
            }

            
            modal.find('#ManageShiftsBtn').on('click', function () {
                
                window.location.href = abp.appPath + 'App/Shifts';
            });
        };

        this.save = function () {
            if (!_$nguoiBenhInformationForm.valid()) {
                return;
            }
            var modal = _modalManager.getModal();
            var isEdit = modal.find('input[name=id]').length > 0;
            var nguoiBenh = _$nguoiBenhInformationForm.serializeFormToObject();

            if (nguoiBenh.AssignedShiftId === "" || nguoiBenh.AssignedShiftId === undefined) {
                nguoiBenh.AssignedShiftId = null;
            } else {
                nguoiBenh.AssignedShiftId = parseInt(nguoiBenh.AssignedShiftId) || null;
            }
            
            if (isEdit && (!nguoiBenh.password || nguoiBenh.password.trim() === "")) {
                delete nguoiBenh.password;
            }
            _modalManager.setBusy(true);
            _nguoiBenhsService.createOrEdit(
                nguoiBenh
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditNguoiBenhModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);