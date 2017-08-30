if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_poll_getmailidfororg]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_poll_getmailidfororg]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_poll_getmaxsequencenumber]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_poll_getmaxsequencenumber]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_poll_insertpartneroutlet]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_poll_insertpartneroutlet]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_poll_insertpartnertxn]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_poll_insertpartnertxn]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_poll_updatebatchsequencenumber]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_poll_updatebatchsequencenumber]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_poll_updateoffer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_poll_updateoffer]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_poll_validateorg]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_poll_validateorg]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[sp_poll_validatetxn]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[sp_poll_validatetxn]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROC sp_poll_getmailidfororg
@org_number VARCHAR(20)
AS
IF(select count(*) from agency where agency_number=@org_number)>0
BEGIN
	select agency_contact_email_address as org_mail_id from agency where agency_number=@org_number
END
ELSE 
BEGIN
	select partner_contact_email_address as org_mail_id from partner where partner_number=@org_number 
	
END





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROC sp_poll_getmaxsequencenumber
@org_number VARCHAR(20)
AS
IF(select count(*) from agency where agency_number=@org_number)>0
BEGIN
	select agency_last_batch_sequence_number from agency where agency_number=@org_number
END
ELSE 
BEGIN
	select partner_last_batch_sequence_number  from partner where partner_number=@org_number 
	
END





GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROC sp_poll_insertpartneroutlet
@partner_outlet_number nvarchar(20),
@partner_outlet_name nvarchar(30),
@partner_outlet_reference nvarchar(30),
@partner_number nvarchar(20)
AS
DECLARE @agency_crmid uniqueidentifier,@partner_crmid uniqueidentifier,@partner_outlet_crmid uniqueidentifier
select @agency_crmid=agency_crmid,@partner_crmid=partner_crmid from partner where partner_number=@partner_number
if(select count(*) from partner_outlet where partner_outlet_reference=@partner_outlet_reference and partner_crmid=@partner_crmid)=0
begin
	set @partner_outlet_crmid=newid()
	insert into partner_outlet(agency_crmid,partner_crmid,partner_outlet_crmid,partner_outlet_number,partner_outlet_name,partner_outlet_reference,partner_outlet_text_1,partner_outlet_text_2,partner_outlet_text_3)
	values(@agency_crmid,@partner_crmid,@partner_outlet_crmid,@partner_outlet_number,@partner_outlet_name,@partner_outlet_reference,NULL,NULL,NULL)
end




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO







CREATE PROC sp_poll_insertpartnertxn 
@txn_date datetime,
@pos_id varchar(6),
@txn_nbr varchar(5),
@amount_spent decimal(15,2),
@total_points decimal(13,2),
@welcome_points decimal(13,2),
@product_points decimal(13,2),
@extra_points_1 decimal(13,2),
@extra_points_2 decimal(13,2),
@extra_points_3 decimal(13,2),
@amount_spent_at_partner decimal(15,2),
@partner_reference nvarchar(30),
@partner_pos_id nvarchar(30),
@partner_outlet_ref nvarchar(30),
@partner_outlet_name nvarchar(30),
@points_partner_processed_dt datetime,
@partner_number nvarchar(20),
@official_id nvarchar(13),
@card_account_number varchar(20),
@add_skeleton_account bit,
@customer_crmidforoffid varchar(50)=NULL,
@return_code TINYINT OUTPUT
AS
DECLARE @card_account_crmid uniqueidentifier,@customer_crmid uniqueidentifier,@txn_crmid uniqueidentifier,@store_code int,@txn_type_code smallint,@txn_source_code smallint,@txn_reason_code smallint,@offer_crmid uniqueidentifier,@customer_offer_crmid uniqueidentifier,@primary_customer_crmid uniqueidentifier,@cashier_id varchar(14),@user_name nvarchar(50),@partner_outlet_number nvarchar(20),@outlets_to_be_maintained varchar(1),@partner_crmid uniqueidentifier,@usr_crmid uniqueidentifier,@session_crmid uniqueidentifier,@number_of_skeleton_customers int,@partner_text1 nvarchar(30)

SET @return_code=0
set @number_of_skeleton_customers=0
SET @cashier_id =NULL
SET @user_name =NULL
SET @txn_crmid=newid()
SET @partner_outlet_number=NULL
SET @total_points=@extra_points_2
select @store_code=partner_store_code from partner with (nolock) where partner_number=@partner_number
select @offer_crmid=offer_crmid from offer with (nolock) where @points_partner_processed_dt>=collection_start_date and DATEDIFF(day,@points_partner_processed_dt,collection_end_date)>=0
if(@card_account_number is NOT NULL AND @add_skeleton_account=0)
BEGIN
	select @customer_crmid=customer_crmid,@card_account_crmid=card_account_crmid from card_account with (nolock) where card_account_number=@card_account_number
	select @primary_customer_crmid=primary_customer_crmid from customer with (nolock) where customer_crmid=@customer_crmid
END
ELSE IF(@card_account_number IS NOT NULL  AND @add_skeleton_account=1)
BEGIN
	--first check whether this was added in an earlier record
	select @customer_crmid=customer_crmid,@card_account_crmid=card_account_crmid from card_account with (nolock) where card_account_number=@card_account_number
	select @primary_customer_crmid=primary_customer_crmid from customer with (nolock) where customer_crmid=@customer_crmid
	set @number_of_skeleton_customers=1
	--Create skeleton account here
	IF (@customer_crmid is NULL)
	BEGIN
		SET @customer_crmid=newid() 
		SET @primary_customer_crmid=@customer_crmid
		SELECT @usr_crmid=usr_crmid from admin_usr where usr_name='ngcadmin'
		SET @card_account_crmid=newid() 
		SET @session_crmid=newid()
		DECLARE @RETURNCODE INT
		EXEC @RETURNCODE = sp_create_customer_skeleton @usr_crmid, @session_crmid, @customer_crmid, '성명없음', @card_account_crmid, @card_account_number, '-1', @txn_date, 'System'
		IF (@RETURNCODE != 0) 
		BEGIN
			SET @return_code=5
			RETURN
		END
	END
END
ELSE IF(@card_account_number IS NULL)
BEGIN
	SET @customer_crmid=CAST(@customer_crmidforoffid as uniqueidentifier)
	select @primary_customer_crmid=primary_customer_crmid  from customer with (nolock) where customer_crmid=@customer_crmid
	select @card_account_crmid=card_account_crmid from card_account with (nolock) where customer_crmid=@customer_crmid and card_account_status_code in(0,1)
	--select @primary_customer_crmid=primary_customer_crmid from customer with (nolock) where customer_crmid=@customer_crmid
END
--select @txn_reason_code=txn_reason_code from txn_reason with (nolock) where txn_reason_description='Partner Auto'
--select @txn_type_code=txn_type_code from txn_type with (nolock) where txn_type_description='Partner Auto'
--select @txn_source_code=txn_source_code from txn_source with (nolock) where txn_source_description='System'

set @txn_reason_code=11
set @txn_type_code=8
set @txn_source_code=5
select @outlets_to_be_maintained=outlets_to_be_maintained,@partner_crmid=partner_crmid,@partner_text1=partner_text_1 from partner with (nolock) where partner_number=@partner_number
IF(@outlets_to_be_maintained=1)
BEGIN
	Select @partner_outlet_number=partner_outlet_number from partner_outlet with (nolock) where partner_crmid=@partner_crmid and partner_outlet_reference=@partner_outlet_ref
END
select @customer_offer_crmid=customer_offer_crmid from customer_offer with (nolock) where customer_crmid=@primary_customer_crmid and offer_crmid=@offer_crmid
IF @customer_offer_crmid IS NULL
BEGIN
	SET @customer_offer_crmid=NEWID()--Insert a record into customer_offer
	INSERT INTO customer_offer(customer_crmid,customer_offer_crmid,offer_crmid,total_points_balance,welcome_points_balance,product_points_balance,extra_points_1_balance,extra_points_2_balance,extra_points_3_balance,mail_date,mailed_flag,amount_rewarded,points_carried_forward,points_brought_forward,points_converted_to_rewards,converted_flag,reissue_requested_date,reissue_requested_by,reissue_date,reissued_flag,requires_reissue_flag,number_of_txns,sign_off_customer_flag,high_reward_flag,reward_customer_flag,txns_archived_flag,redeemed_date,number_of_vouchers_redeemed,value_of_vouchers_redeemed,number_of_partner_txns)
	VALUES (@primary_customer_crmid,@customer_offer_crmid,@offer_crmid,@extra_points_2,0,0,0,@extra_points_2,0,NULL,0,NULL,NULL,NULL,NULL,0,NULL,NULL,NULL,0,0,0,0,0,0,0,NULL,0,0,1)
END
ELSE
BEGIN
	UPDATE customer_offer set total_points_balance=total_points_balance+@extra_points_2,extra_points_2_balance=extra_points_2_balance+@extra_points_2,number_of_partner_txns=CASE WHEN number_of_partner_txns IS NULL THEN 1 ELSE number_of_partner_txns+1 END  where customer_offer_crmid=@customer_offer_crmid
END
INSERT INTO txn (card_account_crmid,customer_crmid,txn_crmid,store_code,txn_date,txn_type_code,txn_source_code,pos_id,txn_nbr,cashier_id,user_name,amount_spent,txn_reason_code,offer_crmid,customer_offer_crmid,total_points,welcome_points,product_points,extra_points_1,extra_points_2,extra_points_3,amount_spent_at_partner,partner_reference,partner_pos_id,partner_outlet_ref,partner_outlet_name,points_partner_processed_dt,partner_number,partner_outlet_number)
values (@card_account_crmid,@customer_crmid,@txn_crmid,@store_code,@txn_date,@txn_type_code,@txn_source_code,@pos_id,@txn_nbr,@cashier_id,@user_name,@amount_spent,@txn_reason_code,@offer_crmid,@customer_offer_crmid,@total_points,@welcome_points,@product_points,@extra_points_1,@extra_points_2,@extra_points_3,@amount_spent_at_partner,@partner_reference,@partner_pos_id,@partner_outlet_ref,@partner_outlet_name,@points_partner_processed_dt,@partner_number,@partner_outlet_number)
--Update offer to set number_of_partner_transactions to zero if it is null
--declare @last_txn_snapshot_date datetime,@collection_start_date datetime
--select @last_txn_snapshot_date=last_txn_snapshot_date,@collection_start_date=collection_start_date from offer where offer_crmid=@offer_crmid
--IF (@last_txn_snapshot_date IS NULL) SET @last_txn_snapshot_date = @collection_start_date - 1
--if(@txn_date<@last_txn_snapshot_date)
--BEGIN
/*Update Offer 			 
	SET offer.number_of_partner_transactions = 0
	WHERE offer.offer_crmid = @offer_crmid
	And offer.number_of_partner_transactions Is Null
--Update offer to set total_partner_spend to zero if it is null
Update Offer 			 
	SET offer.total_partner_spend = 0
	WHERE offer.offer_crmid = @offer_crmid
	And offer.total_partner_spend Is Null
--Update offer to set offer_partner_points_balance to zero if it is null
Update Offer 			 
	SET offer.offer_partner_points_balance = 0
	WHERE offer.offer_crmid = @offer_crmid
	And offer.offer_partner_points_balance Is Null
--update offer to set the other parameters
UPDATE offer SET offer.offer_partner_points_balance=offer.offer_partner_points_balance+@extra_points_2,
	offer.number_of_partner_transactions =  offer.number_of_partner_transactions + 1, 
	offer.total_partner_spend = offer.total_partner_spend + @amount_spent_at_partner,
	offer.number_of_transactions=offer.number_of_transactions+1,
	offer.amount_spent_balance=	offer.amount_spent_balance+@amount_spent_at_partner,
	offer.number_of_skeleton_customers=offer.number_of_skeleton_customers+@number_of_skeleton_customers
	WHERE offer.offer_crmid = @offer_crmid 
DECLARE @store_day_crmid uniqueidentifier,@partner_store_code INT
SELECT @store_day_crmid =  store_day.store_day_crmid  FROM store_day WHERE  store_day.store_date  =  @txn_date  AND  store_day.store_code  =  @store_code   
IF (@store_day_crmid IS NULL) 
BEGIN
	DECLARE @param1 UNIQUEIDENTIFIER SET @param1=NEWID() 
	INSERT INTO store_day ( update_version, store_day.store_date, store_day.product_points_balance, store_day.number_of_new_hsc_customers, store_day.number_of_normal_customers, store_day.normal_points_balance, store_day.number_of_customers, store_day.welcome_points_balance, store_day.extra_points_1_balance, store_day.number_of_mail_promotion_customers, store_day.manual_total_points_balance, store_day.amount_spent_balance, store_day.extra_points_3_balance, store_day.number_of_third_party_promotion_customers, store_day.number_of_deceased_customers, store_day.number_of_manual_txns, store_day.number_of_banned_customers, store_day.number_of_group_promotion_customers, store_day.extra_points_2_balance, store_day.number_of_primary_customers, store_day.number_of_phone_promotion_customers, store_day.number_of_txns, store_day.store_code, store_day.number_of_primary_normal_customers, store_day.number_of_new_customers, store_day.store_day_crmid, store_day.number_of_left_customers, store_day.number_of_inactive_customers ) VALUES 
			      ( 0, 		@txn_date, 		0,			 		NULL, 					NULL, 				0,					 NULL, 				0,				 0,				 	NULL, 						0,		 		@amount_spent_at_partner, 		0, 			NULL, 						NULL, 						0, 				NULL, 				NULL, 						@extra_points_2, 			NULL, 					NULL, 						1,	 	@store_code, 			NULL, 						NULL, 			   @param1, 			NULL, 					NULL )  
END 
ELSE 
BEGIN
	UPDATE store_day SET update_version = update_version + 1, 
	store_day.amount_spent_balance = store_day.amount_spent_balance + @amount_spent_at_partner, 
	store_day.extra_points_2_balance = store_day.extra_points_2_balance + @extra_points_2, 
	store_day.number_of_txns = store_day.number_of_txns + 1 
	WHERE store_day.store_day_crmid = @store_day_crmid 
END*/
--update partner_text to 1 if null
IF(@partner_text1 IS NULL)
BEGIN
	update partner set partner_text_1='1' where partner_number=@partner_number
END
--END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO



CREATE PROC sp_poll_updatebatchsequencenumber
@org_number VARCHAR(20),
@batch_sequence_number int
AS
IF(select count(*) from agency where agency_number=@org_number)>0
BEGIN
	Update agency set agency_last_batch_sequence_number=@batch_sequence_number where agency_number=@org_number
END
ELSE 
BEGIN
	Update partner set partner_last_batch_sequence_number=@batch_sequence_number where partner_number=@org_number
	
END




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE sp_poll_updateoffer
@points_partner_processed_dt datetime,
@extra_points_2 decimal(20,2),
@number_of_partner_transactions int,
@amount_spent_at_partner decimal(22,2)
AS
declare @offer_crmid uniqueidentifier 
select @offer_crmid=offer_crmid from offer with (nolock) where @points_partner_processed_dt>=collection_start_date and DATEDIFF(day,@points_partner_processed_dt,collection_end_date)>=0
Update Offer 			 
	SET offer.number_of_partner_transactions = 0
	WHERE offer.offer_crmid = @offer_crmid
	And offer.number_of_partner_transactions Is Null
--Update offer to set total_partner_spend to zero if it is null
Update Offer 			 
	SET offer.total_partner_spend = 0
	WHERE offer.offer_crmid = @offer_crmid
	And offer.total_partner_spend Is Null
--Update offer to set offer_partner_points_balance to zero if it is null
Update Offer 			 
	SET offer.offer_partner_points_balance = 0
	WHERE offer.offer_crmid = @offer_crmid
	And offer.offer_partner_points_balance Is Null
--update offer to set the other parameters
UPDATE offer SET offer.offer_partner_points_balance=offer.offer_partner_points_balance+@extra_points_2,
	offer.number_of_partner_transactions =  offer.number_of_partner_transactions + @number_of_partner_transactions, 
	offer.total_partner_spend = offer.total_partner_spend + @amount_spent_at_partner,
	offer.number_of_transactions=offer.number_of_transactions+@number_of_partner_transactions,
	offer.amount_spent_balance=	offer.amount_spent_balance+@amount_spent_at_partner
	WHERE offer.offer_crmid = @offer_crmid

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROC sp_poll_validateorg 
@org_number VARCHAR(20)
AS
DECLARE @Return_Value INT
SET @Return_Value=-1
IF(select count(*) from agency where agency_number=@org_number)>0
BEGIN
	SET @Return_Value=2
END
ELSE 
BEGIN
	IF(select count(*) from partner where partner_number=@org_number and partner_type_code=3)>0
	BEGIN
		SET @Return_Value=1	
	END
END
RETURN @Return_Value


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO








CREATE Procedure sp_poll_validatetxn
@org_number  NVARCHAR(20),
@agency_flag TINYINT,
@Partner_Number NVARCHAR(20),
@card_flag smallint,
@partner_points decimal(13,2),
@partner_outlet_ref NVARCHAR(30) = NULL,
@card_account_number VARCHAR(20) = NULL,
@official_id VARCHAR(13) = NULL,
@return_code Tinyint OUTPUT,
@create_skeleton_flag Tinyint OUTPUT,
@add_partner_outlet TINYINT OUTPUT,
@customer_crmidforoffid uniqueidentifier OUTPUT
As
Begin
/*Return Codes
0 - No Error
1- Invalid organisation
2- No Customer for the official id
3- No card account for the official id
4- The card account is not valid
5- Customer exists for the card account but use status not valid
6- Partner points not allowed
*/
Declare @partner_agency_number NVARCHAR(20)
Declare @customer_crmid uniqueidentifier
Declare @card_account_crmid uniqueidentifier
declare @customer_use_status_code smallint
declare @card_account_status_code smallint
declare @transaction_add_limit decimal(9,2)
declare @transaction_subtract_limit decimal(9,2)
Declare @outlets_to_be_maintained smallint
declare @part_number nvarchar(20)
SET @return_code=0
SET @customer_crmidforoffid=NULL
SET @add_partner_outlet=0
SET @create_skeleton_flag=0
IF(@agency_flag=1)
BEGIN
	Select @partner_agency_number = Partner_Number,@transaction_add_limit=transaction_add_limit,@transaction_subtract_limit=transaction_subtract_limit,@outlets_to_be_maintained=Partner.outlets_to_be_maintained From Partner Where partner_number=@Partner_Number and partner_agency_number=@org_number	
	IF @partner_agency_number IS Null
	Begin
		Set @return_code = 1	
		Return
	END
END
ELSE
BEGIN
	SELECT @part_number=partner_number,@transaction_add_limit=transaction_add_limit,@transaction_subtract_limit=transaction_subtract_limit,@outlets_to_be_maintained=Partner.outlets_to_be_maintained from partner where partner_number=@Partner_Number
	IF(@part_number is null)
	BEGIN
		Set @return_code = 1	
		Return
	END
END
	If @partner_outlet_ref IS Not Null
	BEGIN
		IF @outlets_to_be_maintained=1
		BEGIN
			If Not Exists(Select partner_number From Partner 
				Inner Join partner_outlet
				On Partner.Partner_Crmid = partner_outlet.Partner_Crmid
				Where partner_outlet.partner_outlet_reference= @partner_outlet_ref and 
				partner.partner_number=@Partner_Number)
			BEGIN
				Set @add_partner_outlet = 1	
			END
		END
	END
IF(@card_flag=0)
BEGIN
	select @customer_crmid=customer_crmid from customer where official_id=@official_id-- and customer_use_status_code not in(2,3,4)	
	SET @customer_crmidforoffid=@customer_crmid
	IF(@customer_crmid IS NULL)
	Begin
		SET @return_code=2
		RETURN
	END
	ELSE
	BEGIN
		select @customer_use_status_code=customer_use_status_code from customer where customer_crmid=@customer_crmid
		if(@customer_use_status_code in (2,3,4))
		BEGIN
			SET @return_code=5
			RETURN	
		END
		ELSE
		BEGIN
			Select @card_account_crmid=card_account_crmid,@card_account_status_code=card_account_status_code from card_account where customer_crmid=@customer_crmid --and card_account_status_code not in(2,3,4,5)
			IF(@card_account_crmid IS NULL or @card_account_status_code in(2,3,4,5))
			Begin
				SET @return_code=3
			end
		END
	END
END
ELSE
BEGIN
	select @customer_crmid=customer_crmid,@card_account_status_code=card_account_status_code from card_account where card_account_number=@card_account_number --and card_account_status_code not in(2,3,4,5)
	IF(@customer_crmid IS NULL)
		IF NOT EXISTS(SELECT card_account_type.card_account_type_code  FROM card_account_type, card_account_number WHERE card_account_number.card_account_type_crmid = card_account_type.card_account_type_crmid AND  card_account_number.min_card_account_number  <=  @card_account_number  AND  card_account_number.max_card_account_number  >=  @card_account_number  AND  card_account_number.card_account_number_length  =  LEN(@card_account_number) )  
		BEGIN
			SET @return_code=7
			RETURN
		END
		ELSE
		BEGIN
			SET @create_skeleton_flag=1
		END
	ELSE
	BEGIN
		--select @customer_crmid=customer_crmid from card_account where card_account_number=@card_account_number and card_account_status_code not in(2,3,4,5)
		--IF(@customer_crmid IS NULL)
		if(@card_account_status_code in (2,3,4,5))
		BEGIN
			SET @return_code=4
			RETURN 
		END
		ELSE
		BEGIN
			--IF NOT EXISTS (select customer_crmid from customer where customer_crmid=@customer_crmid and customer_use_status_code not in(2,3,4))
			select @customer_crmid=customer_crmid,@customer_use_status_code=customer_use_status_code from customer where customer_crmid=@customer_crmid
			if(@customer_crmid is NULL or @customer_use_status_code in(2,3,4))
			BEGIN
				SET @return_code=5
				RETURN
			END
		END
	END
END
IF (@partner_points >@transaction_add_limit or @partner_points < @transaction_subtract_limit)
	SET @return_code = 6
End
RETURN



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

