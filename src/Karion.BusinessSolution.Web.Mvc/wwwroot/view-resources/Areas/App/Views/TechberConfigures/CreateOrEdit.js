(function () {
    $(function () {
        var _techberConfiguresService = abp.services.app.techberConfigures;

        var _$techberConfigureInformationForm = $('form[name=TechberConfigureInformationsForm]');
        _$techberConfigureInformationForm.validate();

		
   
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });
      
	    

        function save(successCallback) {
            if (!_$techberConfigureInformationForm.valid()) {
                return;
            }

            var techberConfigure = _$techberConfigureInformationForm.serializeFormToObject();
			
			 abp.ui.setBusy();
			 _techberConfiguresService.createOrEdit(
				techberConfigure
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               abp.event.trigger('app.createOrEditTechberConfigureModalSaved');
               
               if(typeof(successCallback)==='function'){
                    successCallback();
               }
			 }).always(function () {
			    abp.ui.clearBusy();
			});
        };
        
        function clearForm(){
            _$techberConfigureInformationForm[0].reset();
        }
        
        $('#saveBtn').click(function(){
            save(function(){
                window.location="/App/TechberConfigures";
            });
        });
        
        $('#saveAndNewBtn').click(function(){
            save(function(){
                if (!$('input[name=id]').val()) {//if it is create page
                   clearForm();
                }
            });
        });
    });
})();