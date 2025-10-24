(function () {
    $(function () {

        var _$hanetTenantLogsTable = $('#HanetTenantLogsTable');
        var _hanetTenantLogsService = abp.services.app.hanetTenantLogs;
		
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.HanetTenantLogs.Create'),
            edit: abp.auth.hasPermission('Pages.HanetTenantLogs.Edit'),
            'delete': abp.auth.hasPermission('Pages.HanetTenantLogs.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HanetTenantLogs/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/HanetTenantLogs/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditHanetTenantLogModal'
        });       

		 var _viewHanetTenantLogModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HanetTenantLogs/ViewhanetTenantLogModal',
            modalClass: 'ViewHanetTenantLogModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }

        var dataTable = _$hanetTenantLogsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _hanetTenantLogsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#HanetTenantLogsTableFilter').val(),
					valueFilter: $('#ValueFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    width: 120,
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
						{
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewHanetTenantLogModal.open({ id: data.record.hanetTenantLog.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.hanetTenantLog.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteHanetTenantLog(data.record.hanetTenantLog);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "hanetTenantLog.value",
						 name: "value"   
					}
            ]
        });

        function getHanetTenantLogs() {
            dataTable.ajax.reload();
        }

        function deleteHanetTenantLog(hanetTenantLog) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _hanetTenantLogsService.delete({
                            id: hanetTenantLog.id
                        }).done(function () {
                            getHanetTenantLogs(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewHanetTenantLogButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _hanetTenantLogsService
                .getHanetTenantLogsToExcel({
				filter : $('#HanetTenantLogsTableFilter').val(),
					valueFilter: $('#ValueFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditHanetTenantLogModalSaved', function () {
            getHanetTenantLogs();
        });

		$('#GetHanetTenantLogsButton').click(function (e) {
            e.preventDefault();
            getHanetTenantLogs();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getHanetTenantLogs();
		  }
		});
    });
})();