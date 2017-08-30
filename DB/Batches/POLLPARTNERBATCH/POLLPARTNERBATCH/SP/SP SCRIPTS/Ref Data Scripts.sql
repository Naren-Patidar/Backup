insert into txn_reason (txn_reason_crmid,txn_reason_code,txn_reason_description,requires_spend_amount_flag,negative_allowed_flag,postive_allowed_flag,allow_manual_txn_flag,update_version)
values (newid(),11,'Partner Auto',0,1,1,0,1)

insert into txn_type (txn_type_crmid,txn_type_code,txn_type_description,update_version)
values(newid(),6,'Partner Auto',1)