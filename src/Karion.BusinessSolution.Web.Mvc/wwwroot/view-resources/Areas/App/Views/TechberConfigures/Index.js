(function () {
    $(function () {

        var _$techberConfiguresTable = $('#TechberConfiguresTable');
        var _techberConfiguresService = abp.services.app.techberConfigures;
        var _hanetService = abp.services.app.hanetAppservices;
        
		var _entityTypeFullName = 'Karion.BusinessSolution.TBHostConfigure.TechberConfigure';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.TechberConfigures.Create'),
            edit: abp.auth.hasPermission('Pages.TechberConfigures.Edit'),
            'delete': abp.auth.hasPermission('Pages.TechberConfigures.Delete')
        };

        var _viewTechberConfigureUrl=null;
        _techberConfiguresService.getAuthorizationData().done(function (result) {
            if(result!="hanetClientID configuration is missing [[KEY: HANET_GET_CLIENT_ID]]")
                _viewTechberConfigureUrl=result;
            else 
                abp.message.error("hanetClientID configuration is missing [[KEY: HANET_GET_CLIENT_ID]]");
        });
		
		 
        var _authorizationModal = new app.ModalManager({
            viewUrl: 'https://oauth.hanet.com/oauth2/authorize?response_type=code&client_id=<CLIENT_ID>&redirect_uri=<URL>&scope=full',
            modalClass: 'ViewTechberConfigureModal'
        });

		        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
		        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }
        $(document).ready(function () {
            $("#GetAuthor").on('click',function () {
                window.open(_viewTechberConfigureUrl, "popupWindow", "width=400,height=700,scrollbars=yes");
            });  
            $("#GetAccessToken").on('click',function () {
                _hanetService.hanetWebhookGetAccessToken().done(function (result) {
                    if(result.status){
                        abp.message.success(result.message);
                    }else{
                        abp.message.error(result.message);
                    }
                })
            });
        });
        var dataTable = _$techberConfiguresTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _techberConfiguresService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#TechberConfiguresTableFilter').val(),
					keyFilter: $('#KeyFilterId').val(),
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
                                    window.location="/App/TechberConfigures/ViewTechberConfigure/" + data.record.techberConfigure.id;
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            window.location="/App/TechberConfigures/CreateOrEdit/" + data.record.techberConfigure.id;                                
                            }
                        },
                        {
                            text: app.localize('History'),
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.techberConfigure.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteTechberConfigure(data.record.techberConfigure);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "techberConfigure.key",
						 name: "key"   
					},
					{
						targets: 2,
						 data: "techberConfigure.value",
						 name: "value"   
					}
            ]
        });

        function getTechberConfigures() {
            dataTable.ajax.reload();
        }

        function deleteTechberConfigure(techberConfigure) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _techberConfiguresService.delete({
                            id: techberConfigure.id
                        }).done(function () {
                            getTechberConfigures(true);
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

                

		$('#ExportToExcelButton').click(function () {
            _techberConfiguresService
                .getTechberConfiguresToExcel({
				filter : $('#TechberConfiguresTableFilter').val(),
					keyFilter: $('#KeyFilterId').val(),
					valueFilter: $('#ValueFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditTechberConfigureModalSaved', function () {
            getTechberConfigures();
        });

		$('#GetTechberConfiguresButton').click(function (e) {
            e.preventDefault();
            getTechberConfigures();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getTechberConfigures();
		  }
		});
    });
})();