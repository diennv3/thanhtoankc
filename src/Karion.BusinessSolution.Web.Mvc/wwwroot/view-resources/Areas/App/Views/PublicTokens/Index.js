(function () {
    $(function () {

        var _$publicTokensTable = $('#PublicTokensTable');
        var _publicTokensService = abp.services.app.publicTokens;
		var _entityTypeFullName = 'Karion.BusinessSolution.QuanLyDanhMuc.PublicToken';
        
        $('.date-picker').datetimepicker({
            locale: abp.localization.currentLanguage.name,
            format: 'L'
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.PublicTokens.Create'),
            edit: abp.auth.hasPermission('Pages.PublicTokens.Edit'),
            'delete': abp.auth.hasPermission('Pages.PublicTokens.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PublicTokens/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/PublicTokens/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditPublicTokenModal'
        });       

		 var _viewPublicTokenModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/PublicTokens/ViewpublicTokenModal',
            modalClass: 'ViewPublicTokenModal'
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

        var dataTable = _$publicTokensTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _publicTokensService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#PublicTokensTableFilter').val(),
					minTimeSetFilter:  getDateFilter($('#MinTimeSetFilterId')),
					maxTimeSetFilter:  getDateFilter($('#MaxTimeSetFilterId')),
					minTimeExpireFilter:  getDateFilter($('#MinTimeExpireFilterId')),
					maxTimeExpireFilter:  getDateFilter($('#MaxTimeExpireFilterId')),
					tokenFilter: $('#TokenFilterId').val(),
					privateKeyFilter: $('#PrivateKeyFilterId').val(),
					deviceVerificationCodeFilter: $('#DeviceVerificationCodeFilterId').val(),
					lastAccessDeviceVerificationCodeFilter: $('#LastAccessDeviceVerificationCodeFilterId').val(),
					isTokenLockedFilter: $('#IsTokenLockedFilterId').val(),
					nguoiBenhUserNameFilter: $('#NguoiBenhUserNameFilterId').val()
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
                                    _viewPublicTokenModal.open({ id: data.record.publicToken.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.publicToken.id });                                
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
                                    entityId: data.record.publicToken.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deletePublicToken(data.record.publicToken);
                            }
                        }]
                    }
                },
					{
						targets: 1,
						 data: "publicToken.timeSet",
						 name: "timeSet" ,
					render: function (timeSet) {
						if (timeSet) {
							return moment(timeSet).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 2,
						 data: "publicToken.timeExpire",
						 name: "timeExpire" ,
					render: function (timeExpire) {
						if (timeExpire) {
							return moment(timeExpire).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 3,
						 data: "publicToken.token",
						 name: "token"   
					},
					{
						targets: 4,
						 data: "publicToken.privateKey",
						 name: "privateKey"   
					},
					{
						targets: 5,
						 data: "publicToken.deviceVerificationCode",
						 name: "deviceVerificationCode"   
					},
					{
						targets: 6,
						 data: "publicToken.lastAccessDeviceVerificationCode",
						 name: "lastAccessDeviceVerificationCode"   
					},
					{
						targets: 7,
						 data: "publicToken.isTokenLocked",
						 name: "isTokenLocked"  ,
						render: function (isTokenLocked) {
							if (isTokenLocked) {
								return '<div class="text-center"><i class="fa fa-check kt--font-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 8,
						 data: "nguoiBenhUserName" ,
						 name: "nguoiBenhFk.userName" 
					}
            ]
        });

        function getPublicTokens() {
            dataTable.ajax.reload();
        }

        function deletePublicToken(publicToken) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _publicTokensService.delete({
                            id: publicToken.id
                        }).done(function () {
                            getPublicTokens(true);
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

        $('#CreateNewPublicTokenButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _publicTokensService
                .getPublicTokensToExcel({
				filter : $('#PublicTokensTableFilter').val(),
					minTimeSetFilter:  getDateFilter($('#MinTimeSetFilterId')),
					maxTimeSetFilter:  getDateFilter($('#MaxTimeSetFilterId')),
					minTimeExpireFilter:  getDateFilter($('#MinTimeExpireFilterId')),
					maxTimeExpireFilter:  getDateFilter($('#MaxTimeExpireFilterId')),
					tokenFilter: $('#TokenFilterId').val(),
					privateKeyFilter: $('#PrivateKeyFilterId').val(),
					deviceVerificationCodeFilter: $('#DeviceVerificationCodeFilterId').val(),
					lastAccessDeviceVerificationCodeFilter: $('#LastAccessDeviceVerificationCodeFilterId').val(),
					isTokenLockedFilter: $('#IsTokenLockedFilterId').val(),
					nguoiBenhUserNameFilter: $('#NguoiBenhUserNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditPublicTokenModalSaved', function () {
            getPublicTokens();
        });

		$('#GetPublicTokensButton').click(function (e) {
            e.preventDefault();
            getPublicTokens();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getPublicTokens();
		  }
		});
    });
})();