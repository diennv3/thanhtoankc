(function ($) {
    app.modals.CreateOrEditBacSiChuyenKhoaModal = function () {

        var _bacSiChuyenKhoasService = abp.services.app.bacSiChuyenKhoas;

        var _modalManager;
        var _$bacSiChuyenKhoaInformationForm = null;

        var _BacSiChuyenKhoauserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BacSiChuyenKhoas/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BacSiChuyenKhoas/_BacSiChuyenKhoaUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });
        var _BacSiChuyenKhoachuyenKhoaLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/BacSiChuyenKhoas/ChuyenKhoaLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/BacSiChuyenKhoas/_BacSiChuyenKhoaChuyenKhoaLookupTableModal.js',
            modalClass: 'ChuyenKhoaLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

            var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$bacSiChuyenKhoaInformationForm = _modalManager.getModal().find('form[name=BacSiChuyenKhoaInformationsForm]');
            _$bacSiChuyenKhoaInformationForm.validate();
            
        };
        
        $('#OpenUserLookupTableButton').click(function () {

            var bacSiChuyenKhoa = _$bacSiChuyenKhoaInformationForm.serializeFormToObject();

            _BacSiChuyenKhoauserLookupTableModal.open({
                id: bacSiChuyenKhoa.userId,
                displayName: bacSiChuyenKhoa.userName
            }, function (data) {
                _$bacSiChuyenKhoaInformationForm.find('input[name=userName]').val(data.displayName);
                _$bacSiChuyenKhoaInformationForm.find('input[name=userId]').val(data.id);
            });
        });

        $('#ClearUserNameButton').click(function () {
            _$bacSiChuyenKhoaInformationForm.find('input[name=userName]').val('');
            _$bacSiChuyenKhoaInformationForm.find('input[name=userId]').val('');
        });

        $('#OpenChuyenKhoaLookupTableButton').click(function () {

            var bacSiChuyenKhoa = _$bacSiChuyenKhoaInformationForm.serializeFormToObject();

            _BacSiChuyenKhoachuyenKhoaLookupTableModal.open({
                id: bacSiChuyenKhoa.chuyenKhoaId,
                displayName: bacSiChuyenKhoa.chuyenKhoaTen
            }, function (data) {
                _$bacSiChuyenKhoaInformationForm.find('input[name=chuyenKhoaTen]').val(data.displayName);
                _$bacSiChuyenKhoaInformationForm.find('input[name=chuyenKhoaId]').val(data.id);
            });
        });

        $('#ClearChuyenKhoaTenButton').click(function () {
            _$bacSiChuyenKhoaInformationForm.find('input[name=chuyenKhoaTen]').val('');
            _$bacSiChuyenKhoaInformationForm.find('input[name=chuyenKhoaId]').val('');
        });
        // var hienThiAnh = document.getElementById('image');
        // hienThiAnh.addEventListener('change', function() {
        //     chooseFile(this)
        // });
        // function chooseFile(fileInput) {
        //     console.log(fileInput);
        //     if(fileInput.files && fileInput.files[0]){
        //         var reader = new FileReader();
        //         console.log(reader);
        //         reader.onload  = function (e) {
        //             $('#outImage').attr('src', e.target.result);
        //             console.log($("#image")[0].files);
        //             $('#imageFileName').val($("#image")[0].files[0].name);
        //         };
        //         reader.readAsDataURL(fileInput.files[0]);
        //     }
        // }
        
        this.save = function () {
            if (!_$bacSiChuyenKhoaInformationForm.valid()) {
                return;
            }
            if ($('#BacSiChuyenKhoa_UserId').prop('required') && $('#BacSiChuyenKhoa_UserId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#BacSiChuyenKhoa_ChuyenKhoaId').prop('required') && $('#BacSiChuyenKhoa_ChuyenKhoaId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ChuyenKhoa')));
                return;
            }

            var bacSiChuyenKhoa = _$bacSiChuyenKhoaInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _bacSiChuyenKhoasService.createOrEdit(
                bacSiChuyenKhoa
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                _modalManager.close();
                abp.event.trigger('app.createOrEditBacSiChuyenKhoaModalSaved');
            }).always(function () {
                _modalManager.setBusy(false);
            });
        };
    };
})(jQuery);