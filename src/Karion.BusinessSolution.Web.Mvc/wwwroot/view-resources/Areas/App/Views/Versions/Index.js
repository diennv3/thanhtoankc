(function () {
    $(function () {

        var _$versionsTable = $('#VersionsTable');
        var _versionsService = abp.services.app.versions;
		var _entityTypeFullName = 'Karion.BusinessSolution.VersionControl.Version';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Versions.Create'),
            edit: abp.auth.hasPermission('Pages.Versions.Edit'),
            'delete': abp.auth.hasPermission('Pages.Versions.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Versions/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Versions/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditVersionModal'
        });       

		 var _viewVersionModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Versions/ViewversionModal',
            modalClass: 'ViewVersionModal'
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

        var dataTable = _$versionsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _versionsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#VersionsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					minVersionFilter: $('#MinVersionFilterId').val(),
					maxVersionFilter: $('#MaxVersionFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val()
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
                                    _viewVersionModal.open({ id: data.record.version.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.version.id });                                
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
                                    entityId: data.record.version.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteVersion(data.record.version);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "version.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "version.version",
						 name: "version"   
					},
					{
						targets: 3,
						 data: "version.isActive",
						 name: "isActive"  ,
						render: function (isActive) {
							if (isActive) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					}
            ]
        });

        function getVersions() {
            dataTable.ajax.reload();
        }

        function deleteVersion(version) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _versionsService.delete({
                            id: version.id
                        }).done(function () {
                            getVersions(true);
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

        $('#CreateNewVersionButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _versionsService
                .getVersionsToExcel({
				filter : $('#VersionsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					minVersionFilter: $('#MinVersionFilterId').val(),
					maxVersionFilter: $('#MaxVersionFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditVersionModalSaved', function () {
            getVersions();
        });

		$('#GetVersionsButton').click(function (e) {
            e.preventDefault();
            getVersions();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getVersions();
		  }
		});
    });
})();