(function () {
    $(function () {

        var _$nguoiBenhTestsTable = $('#NguoiBenhTestsTable');
        var _nguoiBenhTestsService = abp.services.app.nguoiBenhTests;
		var _entityTypeFullName = 'Karion.BusinessSolution.NguoiBenhTestNS.NguoiBenhTest';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.NguoiBenhTests.Create'),
            edit: abp.auth.hasPermission('Pages.NguoiBenhTests.Edit'),
            'delete': abp.auth.hasPermission('Pages.NguoiBenhTests.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NguoiBenhTests/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/NguoiBenhTests/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditNguoiBenhTestModal'
        });       

		 var _viewNguoiBenhTestModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/NguoiBenhTests/ViewnguoiBenhTestModal',
            modalClass: 'ViewNguoiBenhTestModal'
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

        var dataTable = _$nguoiBenhTestsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _nguoiBenhTestsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#NguoiBenhTestsTableFilter').val(),
					tenFilter: $('#TenFilterId').val()
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
                                    _viewNguoiBenhTestModal.open({ id: data.record.nguoiBenhTest.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.nguoiBenhTest.id });                                
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
                                    entityId: data.record.nguoiBenhTest.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteNguoiBenhTest(data.record.nguoiBenhTest);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "nguoiBenhTest.ten",
						 name: "ten"   
					}
            ]
        });

        function getNguoiBenhTests() {
            dataTable.ajax.reload();
        }

        function deleteNguoiBenhTest(nguoiBenhTest) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _nguoiBenhTestsService.delete({
                            id: nguoiBenhTest.id
                        }).done(function () {
                            getNguoiBenhTests(true);
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

        $('#CreateNewNguoiBenhTestButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _nguoiBenhTestsService
                .getNguoiBenhTestsToExcel({
				filter : $('#NguoiBenhTestsTableFilter').val(),
					tenFilter: $('#TenFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditNguoiBenhTestModalSaved', function () {
            getNguoiBenhTests();
        });

		$('#GetNguoiBenhTestsButton').click(function (e) {
            e.preventDefault();
            getNguoiBenhTests();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getNguoiBenhTests();
		  }
		});
    });
})();