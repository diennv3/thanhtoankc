(function () {
    $(function () {
        var _$modal = $('#UpdateImageProfileForm');
        var nguoiBenhId = _$modal.find('input[name="NguoiBenhId"]').val();
        var _nguoiBenhsService = abp.services.app.nguoiBenhs;

        $('#btnSaveImageProfile').click(function () {
            var fileInput = _$modal.find('input[name="profileImage"]')[0];
            if (fileInput.files.length === 0) {
                abp.message.warn("Vui lòng chọn ảnh.");
                return;
            }

            var file = fileInput.files[0];
            var allowedTypes = ['image/png', 'image/jpeg', 'image/jpg'];
            if (!allowedTypes.includes(file.type)) {
                abp.message.warn("Chỉ chấp nhận ảnh định dạng .jpg, .jpeg, .png");
                return;
            }

            var extension = file.name.split('.').pop().toLowerCase();
            var fileName = 'NB_' + nguoiBenhId + '_' + new Date().getTime() + '.' + extension;

            var reader = new FileReader();
            reader.onload = function (e) {
                var base64Data = e.target.result.split(',')[1];
                var byteArray = Uint8Array.from(atob(base64Data), c => c.charCodeAt(0));

                _nguoiBenhsService.updateImageProfile({
                    nguoiBenhId: parseInt(nguoiBenhId),
                    data: Array.from(byteArray),
                    jpegFileName: fileName
                }).done(function () {
                    abp.notify.success("Cập nhật ảnh thành công!");
                    abp.event.trigger('app.updateImageProfileSaved');
                    $('.modal').modal('hide');
                });
            };
            reader.readAsDataURL(file);
        });
    });
})();