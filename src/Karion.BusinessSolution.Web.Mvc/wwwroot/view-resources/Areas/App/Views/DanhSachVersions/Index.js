(function () {
    $(function () {

        var _$danhSachVersionsTable = $('#DanhSachVersionsTable');
        var _danhSachVersionsService = abp.services.app.danhSachVersions;
		var _entityTypeFullName = 'Karion.BusinessSolution.VersionControl.DanhSachVersion';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.DanhSachVersions.Create'),
            edit: abp.auth.hasPermission('Pages.DanhSachVersions.Edit'),
            'delete': abp.auth.hasPermission('Pages.DanhSachVersions.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DanhSachVersions/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/DanhSachVersions/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditDanhSachVersionModal'
        });       

		 var _viewDanhSachVersionModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/DanhSachVersions/ViewdanhSachVersionModal',
            modalClass: 'ViewDanhSachVersionModal'
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

        var dataTable = _$danhSachVersionsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _danhSachVersionsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#DanhSachVersionsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					minVersionNumberFilter: $('#MinVersionNumberFilterId').val(),
					maxVersionNumberFilter: $('#MaxVersionNumberFilterId').val(),
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
                                    _viewDanhSachVersionModal.open({ id: data.record.danhSachVersion.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.danhSachVersion.id });                                
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
                                    entityId: data.record.danhSachVersion.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteDanhSachVersion(data.record.danhSachVersion);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "danhSachVersion.name",
						 name: "name"   
					},
					{
						targets: 2,
						 data: "danhSachVersion.versionNumber",
						 name: "versionNumber"   
					},
					{
						targets: 3,
						 data: "danhSachVersion.isActive",
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

        function getDanhSachVersions() {
            dataTable.ajax.reload();
        }

        function deleteDanhSachVersion(danhSachVersion) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _danhSachVersionsService.delete({
                            id: danhSachVersion.id
                        }).done(function () {
                            getDanhSachVersions(true);
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

        $('#CreateNewDanhSachVersionButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _danhSachVersionsService
                .getDanhSachVersionsToExcel({
				filter : $('#DanhSachVersionsTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					minVersionNumberFilter: $('#MinVersionNumberFilterId').val(),
					maxVersionNumberFilter: $('#MaxVersionNumberFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditDanhSachVersionModalSaved', function () {
            getDanhSachVersions();
        });

		$('#GetDanhSachVersionsButton').click(function (e) {
            e.preventDefault();
            getDanhSachVersions();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getDanhSachVersions();
		  }
		});
    });
})();