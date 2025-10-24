(function () {
    $(function () {

        var _$hanetFaceDetectedsTable = $('#HanetFaceDetectedsTable');
        var _hanetFaceDetectedsService = abp.services.app.hanetFaceDetecteds;
		var _entityTypeFullName = 'Karion.BusinessSolution.HanetTenant.HanetFaceDetected';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.HanetFaceDetecteds.Create'),
            edit: abp.auth.hasPermission('Pages.HanetFaceDetecteds.Edit'),
            'delete': abp.auth.hasPermission('Pages.HanetFaceDetecteds.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HanetFaceDetecteds/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/HanetFaceDetecteds/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditHanetFaceDetectedModal'
        });       

		 var _viewHanetFaceDetectedModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/HanetFaceDetecteds/ViewhanetFaceDetectedModal',
            modalClass: 'ViewHanetFaceDetectedModal'
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

        var dataTable = _$hanetFaceDetectedsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _hanetFaceDetectedsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#HanetFaceDetectedsTableFilter').val(),
					placeIdFilter: $('#placeIdFilterId').val(),
					deviceIdFilter: $('#deviceIdFilterId').val(),
					userDetectedIdFilter: $('#userDetectedIdFilterId').val(),
					maskFilter: $('#maskFilterId').val(),
					detectImageUrlFilter: $('#detectImageUrlFilterId').val(),
					minflagFilter: $('#MinflagFilterId').val(),
					maxflagFilter: $('#MaxflagFilterId').val()
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
                                    _viewHanetFaceDetectedModal.open({ id: data.record.hanetFaceDetected.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.hanetFaceDetected.id });                                
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
                                    entityId: data.record.hanetFaceDetected.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteHanetFaceDetected(data.record.hanetFaceDetected);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "hanetFaceDetected.placeId",
						 name: "placeId"   
					},
					{
						targets: 2,
						 data: "hanetFaceDetected.deviceId",
						 name: "deviceId"   
					},
					{
						targets: 3,
						 data: "hanetFaceDetected.userDetectedId",
						 name: "userDetectedId"   
					},
					{
						targets: 4,
						 data: "hanetFaceDetected.mask",
						 name: "mask"   
					},
					{
						targets: 5,
						 data: "hanetFaceDetected.detectImageUrl",
						 name: "detectImageUrl"   
					},
					{
						targets: 6,
						 data: "hanetFaceDetected.flag",
						 name: "flag"   
					}
            ]
        });

        function getHanetFaceDetecteds() {
            dataTable.ajax.reload();
        }

        function deleteHanetFaceDetected(hanetFaceDetected) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _hanetFaceDetectedsService.delete({
                            id: hanetFaceDetected.id
                        }).done(function () {
                            getHanetFaceDetecteds(true);
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

        $('#CreateNewHanetFaceDetectedButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _hanetFaceDetectedsService
                .getHanetFaceDetectedsToExcel({
				filter : $('#HanetFaceDetectedsTableFilter').val(),
					placeIdFilter: $('#placeIdFilterId').val(),
					deviceIdFilter: $('#deviceIdFilterId').val(),
					userDetectedIdFilter: $('#userDetectedIdFilterId').val(),
					maskFilter: $('#maskFilterId').val(),
					detectImageUrlFilter: $('#detectImageUrlFilterId').val(),
					minflagFilter: $('#MinflagFilterId').val(),
					maxflagFilter: $('#MaxflagFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditHanetFaceDetectedModalSaved', function () {
            getHanetFaceDetecteds();
        });

		$('#GetHanetFaceDetectedsButton').click(function (e) {
            e.preventDefault();
            getHanetFaceDetecteds();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getHanetFaceDetecteds();
		  }
		});
    });
})();