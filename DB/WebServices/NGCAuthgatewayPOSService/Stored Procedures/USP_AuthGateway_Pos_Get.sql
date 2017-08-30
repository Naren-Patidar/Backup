USE [NGCCCOCON]
GO
/****** Object:  StoredProcedure [dbo].[USP_AuthGateway_Pos_Get]    Script Date: 04/21/2011 17:05:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[USP_AuthGateway_Pos_Get]') AND type in (N'P', N'PC'))
BEGIN
	DROP PROCEDURE [dbo].[USP_AuthGateway_Pos_Get]
END
GO
/****** Object:  StoredProcedure [dbo].[USP_AuthGateway_Pos_Get]    Script Date: 04/21/2011 17:04:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[USP_AuthGateway_Pos_Get] 
--@interface_code INT, 
@CultureIsoCode VARCHAR(7), 
@ClubcardID BIGINT, 
--@number_of_txns_limit SMALLINT, 
@TescoStoreID INT,
--Modified for US Loyalty
@AlternateID NVARCHAR(50),
@Status Varchar(2) OUTPUT,
@PrimaryClubcardID BIGINT OUTPUT, 
@CustomerWelcomedFlag VARCHAR(1) OUTPUT, 
@Name1 NVARCHAR(50) OUTPUT, 
@LookupTitlePhrase NVARCHAR(50) OUTPUT, 
@PostalCode NVARCHAR(6) OUTPUT, 
@TotalPointsBalance DECIMAL(13,2) OUTPUT, 
@UpToDate VARCHAR(8) OUTPUT, 
@StatusMsgNo SMALLINT  OUTPUT,
@UniqueNumber VARCHAR(18) OUTPUT, 
@WelcomePointsBalance DECIMAL(13,2) OUTPUT, 
@ExtraPoints1Balance DECIMAL(13,2) OUTPUT, 
@ExtraPoints2Balance DECIMAL(13,2) OUTPUT, 
--Modified for US Loyalty
@BonusPoints DECIMAL(13,2) OUTPUT,
@OperMsgCount Tinyint OUTPUT,
@RcptMsgCount Tinyint OUTPUT,
--Modified for US Loyalty
@GreenPoints DECIMAL(13,2) OUTPUT
AS DECLARE @@THISERROR INT /* INTERFACE CODES 
		1 - Korea		  : Get Converted Points Balance
		2 - Thai ProgreSS : Get Title */ /* RETURN CODES   (Version 2.0)
		0 - Success
		1 - Card Unknown, but valid
		2 - Card Unknown, and invalid
		3 - Card Account is not active
		4 - Card Account Owner is unknown
		5 - Head of Household is unknown
		6 - Card Account Owner is not active
		7 - Head of Household's Primary Card Account is unknown
		8 - Head of Household's Primary Card Account Number is unknown
		9 - Customer Deceased
		10- Customer has left the scheme
		11- Mail status is skeleton
		12- Address in error 
		13- No Current Offer
		14- Current Offer has no collection period number 
		15- Store is unknown */ /* Declare Constants */ 
	DECLARE @return_code INT 

	SET @StatusMsgNo=0 
	SET @OperMsgCount = 0
	SET @RcptMsgCount =0
	SET @Status = 'NK'
	SET @UpToDate = CONVERT(char(8), GETDATE(), 112)

	/* Declare Variables */
	DECLARE @CustClubcardID BIGINT 
	DECLARE @ClubcardStatus SMALLINT 
	DECLARE @CustomerID BIGINT 
	DECLARE @IsPrimaryFlag VARCHAR(1) 
	DECLARE @CustomerUseStatusCode SMALLINT 
	DECLARE @CustomerMailStatusCode SMALLINT 
	DECLARE @PrimaryCustomerId BIGINT 
	DECLARE @CollectionPeriodNumber SMALLINT 
	DECLARE @CurrentOfferId BIGINT 
	DECLARE @PreviousOfferId BIGINT 
	DECLARE @NumberOfTxns SMALLINT 
	DECLARE @WelcomePoints DECIMAL(13,2) 
	DECLARE @LineNumber INT 
	DECLARE @LineText NVARCHAR(30) 
	DECLARE @PrimaryCustomerUseStatusCode SMALLINT
	DECLARE @MessageId BIGINT 
	DECLARE @Rowcount Tinyint

	/* Get Store welcome points */ 
	SELECT @WelcomePoints =  TescoStore.StoreWelcomePointsQty  FROM TescoStore WHERE  TescoStore.TescoStoreID  =  @TescoStoreID  AND IsDeleted = 'N' 
	
	/*
	SET @@THISERROR = @@ERROR  
	IF (@@THISERROR != 0) 
	BEGIN RETURN(256)  
	END 
	*/

	/* Get Welcome Points from the store if custoer_welcome_flag  is false */ 
	IF (@WelcomePoints IS NULL) 
	BEGIN
		SET @StatusMsgNo = 15 --RETURN(15)
		RETURN
	END

	--Modified for US Loyalty
	IF((@ClubcardID = '') OR (@ClubcardID IS NULL))
	BEGIN
		SELECT DISTINCT @ClubcardID = PrimaryClubcardId FROM ClubCard WHERE CustomerID = (SELECT CustomerID FROM CustomerAlternateID WHERE CustomerAlternateID = @AlternateID)
	END
	
	 /* Get Details of Customer, plus Head of Household */ /* Get Details of Swiped Card */ 
	SELECT @CustomerID =  Customer.CustomerID , @PrimaryCustomerId =  Customer.PrimaryCustomerID , 
	@CustomerUseStatusCode =  Customer.CustomerUseStatusID , @CustomerMailStatusCode =  Customer.CustomerMailStatus , 
	@CustomerWelcomedFlag =  Customer.CustomerWelcomedFlag , @LookupTitlePhrase =  Customer.TitleEnglish , 
	@Name1 =  Customer.Name1 , @PostalCode =  Customer.MailingAddressPostCode , 
	@CustClubcardID =  Clubcard.ClubcardID,
	@ClubcardStatus =  Clubcard.ClubcardStatus 
	FROM Customer, Clubcard
	WHERE Clubcard.CustomerID = Customer.CustomerID AND  Clubcard.ClubcardID  =  @ClubcardID 
	
	DECLARE @PriCard BIGINT
	SELECT @PriCard = PrimaryClubcardID FROM Clubcard WHERE ClubcardID = @ClubcardID 
	
	IF (@PriCard = @ClubcardID) BEGIN
		 SET @IsPrimaryFlag ='1'
	END
	ELSE BEGIN
		 SET @IsPrimaryFlag ='0'
	END
	--SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END 

	--Welcome Flag has to be same in UK and Group Countries, so changed from 0 to N and 1 to Y
	IF (@CustomerWelcomedFlag ='N') OR (@CustomerWelcomedFlag IS NULL) BEGIN
		SET @WelcomePointsBalance = @WelcomePoints
	END ELSE BEGIN
		SET @WelcomePointsBalance =0
	END

	IF (@CustClubcardID IS NULL) BEGIN 
		 /* Check Card Number is Valid */ 
		DECLARE @ClubcardNumberID BIGINT 
		SELECT @ClubcardNumberID =  ClubcardRange.ClubcardRangeID  FROM ClubcardRange 
		INNER JOIN ClubcardType on ClubcardRange.ClubcardType = ClubcardType.ClubcardType
		WHERE  ClubcardRange.MinCardNumber  <=  @ClubcardID  
		AND  ClubcardRange.MaxCardNumber  >=  @ClubcardID  
		AND  ClubcardType.CardNumberLength  =  LEN(@ClubcardID)   

		--SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END 
		
		IF (@ClubcardNumberID IS NULL) 
		BEGIN

			SET @StatusMsgNo = 31 --RETURN(15)
			--Modified for US Loyalty
			SET @Status = 'NK'
			--SET @Status = 'OK'
			SET @lookupTitlePhrase = NULL
			SET @Name1 = NULL
			SET @PostalCode = NULL
			RETURN
		END


		EXEC USP_DetermineUniqueNumber @ClubcardID, @UniqueNumber OUTPUT

		--RETURN(1) 
		SET @Status = 'OK'
		SET @StatusMsgNo = 30 
		RETURN
	END

	IF (@LookupTitlePhrase IS NULL) SET @LookupTitlePhrase = ''

	
	IF (@CustomerID IS NULL) 
	BEGIN
		--RETURN(4)
		SET @StatusMsgNo = 4
		SET @LookupTitlePhrase = NULL
		SET @Name1 = NULL
		SET @PostalCode = NULL
		RETURN
	END
	--Modified for US Loyalty
	IF (@ClubcardStatus > 0) 
	--IF (@ClubcardStatus > 1)
	BEGIN
		--RETURN(3)
		SET @StatusMsgNo = 32
		--Modified for US Loyalty
		IF ((SELECT COUNT(*) FROM ClubCard WHERE CustomerID = 
				(SELECT CustomerID FROM ClubCard WHERE ClubcardID = @ClubcardID AND IsDeleted = 'N' AND ClubcardStatus = 0)) > 0)
			BEGIN
				SET @Status = 'OK'
			END
		ELSE
			BEGIN
				SET @Status = 'NK'
			END
		--SET @Status = 'OK'  		
		SET @LookupTitlePhrase = NULL
		SET @Name1 = NULL
		SET @PostalCode = NULL
		RETURN
	END
	IF (@PrimaryCustomerID IS NULL) 
	BEGIN
		--RETURN(5)
		SET @StatusMsgNo = 5
		SET @LookupTitlePhrase = NULL
		SET @Name1 = NULL
		SET @PostalCode = NULL
		RETURN
	END
	IF (@CustomerUseStatusCode = 4) 
	BEGIN
		--RETURN(6)
		SET @StatusMsgNo = 33
		--Modified for US Loyalty
		SET @Status = 'NK'
		--SET @Status = 'OK'  		
		RETURN
	END
	IF (@CustomerUseStatusCode = 3) 
	BEGIN
		--RETURN(9)
		SET @StatusMsgNo = 34
		--Modified for US Loyalty
		SET @Status = 'NK'
		--SET @Status = 'OK'  		
		RETURN
	END

	IF (@CustomerUseStatusCode = 2) 
	BEGIN
		--RETURN(10)
		SET @StatusMsgNo = 35
		--Modified for US Loyalty
		SET @Status = 'NK'
		--SET @Status = 'OK'  		
		RETURN
	END
	IF (@CustomerMailStatusCode = 1) 
	BEGIN
		--SET @return_code=11
		SET @StatusMsgNo = 36
		SET @Status = 'OK'  		
		RETURN
	END
	IF (@CustomerMailStatusCode = 2) 
	BEGIN
		--SET @return_code=12
		SET @StatusMsgNo = 37
		SET @Status = 'OK'  		
		RETURN
	END
	
	 /* If the customer is head of household, and the swiped card is the customer's primary card
	     then we know the primary card account number of the householder, else we have to query
	     the database */ 
	IF (@CustomerID = @PrimaryCustomerID AND @IsPrimaryFlag = 1)
		SET @PrimaryClubcardID = @ClubcardID
	ELSE 
		BEGIN
		 /* Get Primary Card Account of Head of Household */ 
		DECLARE @PrimaryCardNumber BIGINT 
		SELECT @PrimaryClubcardID =  Clubcard.PrimaryClubcardID  FROM Clubcard
		WHERE  Clubcard.CustomerID  =  @PrimaryCustomerID  AND  Clubcard.IsDeleted = 'N'

		--SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END 

		IF (@PrimaryClubcardID IS NULL) 
		BEGIN
			--RETURN(8)
			SET @StatusMsgNo = 8
		RETURN
	END
	END

	 /* Find the current offer to find the household points balance */ 
	SELECT @CurrentOfferID =  offer.OfferID , @CollectionPeriodNumber =  Offer.OfferID  FROM offer 
	WHERE  CASE WHEN (offer.StartDateTime <= GETDATE()) AND (offer.EndDateTime > GETDATE()-1) THEN '1' ELSE '0' END  =  '1'   

	--SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END 

	SET @TotalPointsBalance=0

	IF (@CurrentOfferID IS NULL) 
	BEGIN
		--RETURN(13)
		SET @StatusMsgNo = 13
		--Modified for US Loyalty
		SET @Status = 'NK'
		RETURN
	END

	 /* Get Household Total Points Balance */ 
	SELECT @TotalPointsBalance =  ClubcardOffer.PointsBalanceQty ,-- @number_of_txns =  ClubcardOffer.number_of_txns , 
	@ExtraPoints1Balance =  ClubcardOffer.SKUPointsQty , @ExtraPoints2Balance =  ClubcardOffer.PartnerPointsBalanceQty , 
	--Modified for US Loyalty
	--@ExtraPoints3Balance =  ClubcardOffer.BonusPointsQty  FROM ClubcardOffer  
	@BonusPoints =  ClubcardOffer.BonusPointsQty, @GreenPoints = ClubcardOffer.GreenPointsQty FROM ClubcardOffer 
	WHERE  ClubcardOffer.ClubcardID  =  @PrimaryClubcardID  AND  ClubcardOffer.OfferID = @CurrentOfferID   

	--SET @@THISERROR = @@ERROR  IF (@@THISERROR != 0) BEGIN RETURN(256)  END 

	IF (@TotalPointsBalance IS NULL) SET @TotalPointsBalance=0

	/* Get Primary Customer's Reward and Use status*/
	Select @PrimaryCustomerUseStatusCode =CustomerUseStatusID From Customer Where CustomerID = @PrimaryCustomerID

	/* Get Operator Message*/
		/*
	Select @MessageId = Pos_Message.Message_Crmid 

	From Pos_Message

		Inner Join Message_Destination
			On Message_Destination.message_destination_crmid = Pos_Message.message_destination_crmid
		
		Inner Join Message_Status
			On Message_Status.message_status_crmid = Pos_Message.message_status_crmid
		/*
		Inner Join Message_for_Reward_Status
			On Message_for_Reward_Status.message_crmid = Pos_Message.message_crmid
		
		Inner Join Reward_Status
			On Reward_Status.Reward_Status_Crmid = Message_for_Reward_Status.Reward_Status_Crmid
		*/
		Inner Join Message_for_Use_Status
			On Message_for_Use_Status.Message_Crmid = Pos_Message.message_crmid

		Inner Join customer_use_status
			On customer_use_status.customer_use_status_crmid = Message_for_Use_Status.customer_use_status_crmid

		Inner Join Store
			On Store.Store_Crmid = Pos_Message.Store_Crmid

	Where	Message_Destination.Message_destination_description ='Operator Display'
	And CONVERT(DATETIME,CONVERT(VARCHAR,GETDATE(),111),111) Between Pos_Message.Effective_start_date And Pos_Message.Effective_end_date
	--And 	Reward_status_code = 	--Value to be obtained based upon customer's reward status(from customer_offer)
	And customer_use_status.customer_use_status_code = @primary_customer_use_status_code
	And Message_Status.Message_status_description = 'Approved'
	And Store.Store_Code = @store_code
 	
	Set @Rowcount = @@Rowcount

	If @Rowcount =0 
	Begin	
		Select @Message_Crmid = Pos_Message.Message_Crmid 

		From Pos_Message
			
		Inner Join Message_Destination
			On Message_Destination.message_destination_crmid = Pos_Message.message_destination_crmid
		
		Inner Join Message_Status
			On Message_Status.message_status_crmid = Pos_Message.message_status_crmid
		/*	
		Inner Join Message_for_Reward_Status
			On Message_for_Reward_Status.message_crmid = Pos_Message.message_crmid

		Inner Join Reward_Status
			On Reward_Status.Reward_Status_Crmid = Message_for_Reward_Status.Reward_Status_Crmid
		*/		
		Inner Join Message_for_Use_Status
			On Message_for_Use_Status.Message_Crmid = Pos_Message.message_crmid

		Inner Join customer_use_status
			On customer_use_status.customer_use_status_crmid = Message_for_Use_Status.customer_use_status_crmid



		Where	Message_Destination.Message_destination_description ='Operator Display'
		And CONVERT(DATETIME,CONVERT(VARCHAR,GETDATE(),111),111) Between Pos_Message.Effective_start_date And Pos_Message.Effective_end_date
		--And 	Reward_status_code = 	--Value to be obtained based upon customer's reward status(from customer_offer)
		And customer_use_status.customer_use_status_code = @primary_customer_use_status_code
		And Message_Status.Message_status_description = 'Approved'
		And Store_Crmid IS Null
		
		Set @Rowcount = @@Rowcount

		If (@Rowcount > 1  Or @Rowcount = 0)
			Set @OperMsgCount = 0
		Else	
		Begin
			Select Line_number, Line_text From Message_Line

				Inner Join Pos_Message
					On Message_Line.Message_crmid = Pos_Message.Message_crmid
			Where 	Pos_Message.Message_crmid = @Message_Crmid		
			Order by Line_number
			Set @OperMsgCount = @@Rowcount
		End
	End
	Else
	Begin
		If (@Rowcount > 1  Or @Rowcount = 0)
			Set @OperMsgCount = 0
			
		Else
		Begin	
			Select Line_number, Line_text From Message_Line
				Inner Join Pos_Message
					On Message_Line.Message_crmid = Pos_Message.Message_crmid
			Where 	Pos_Message.Message_crmid = @Message_Crmid		
			Order by Line_number
			Set @OperMsgCount = @@Rowcount
		End
	End				

	/* Get Till Receipt Message*/
	
	Select @Message_Crmid = Pos_Message.Message_Crmid 

	From Pos_Message

		Inner Join Message_Destination
			On Message_Destination.message_destination_crmid = Pos_Message.message_destination_crmid
		
		Inner Join Message_Status
			On Message_Status.message_status_crmid = Pos_Message.message_status_crmid
		/*	
		Inner Join Message_for_Reward_Status
			On Message_for_Reward_Status.message_crmid = Pos_Message.message_crmid

		Inner Join Reward_Status
			On Reward_Status.Reward_Status_Crmid = Message_for_Reward_Status.Reward_Status_Crmid
		*/		
		Inner Join Message_for_Use_Status
			On Message_for_Use_Status.Message_Crmid = Pos_Message.message_crmid

		Inner Join customer_use_status
			On customer_use_status.customer_use_status_crmid = Message_for_Use_Status.customer_use_status_crmid

		Inner Join Store
			On Store.Store_Crmid = Pos_Message.Store_Crmid

	Where	Message_Destination.Message_destination_description ='Till Receipt'
	And CONVERT(DATETIME,CONVERT(VARCHAR,GETDATE(),111),111) Between Pos_Message.Effective_start_date And Pos_Message.Effective_end_date
	--And 	Reward_status_code = 	--Value to be obtained based upon customer's reward status(from customer_offer)
	And customer_use_status.customer_use_status_code = @primary_customer_use_status_code
	And Message_Status.Message_status_description = 'Approved'
	And Store.Store_Code = @store_code
 	
	Set @Rowcount = @@Rowcount

	If @Rowcount =0 
	Begin	
		Select @Message_Crmid = Pos_Message.Message_Crmid 

		From Pos_Message
			
		Inner Join Message_Destination
			On Message_Destination.message_destination_crmid = Pos_Message.message_destination_crmid
		
		Inner Join Message_Status
			On Message_Status.message_status_crmid = Pos_Message.message_status_crmid
		/*	
		Inner Join Message_for_Reward_Status
			On Message_for_Reward_Status.message_crmid = Pos_Message.message_crmid

		Inner Join Reward_Status
			On Reward_Status.Reward_Status_Crmid = Message_for_Reward_Status.Reward_Status_Crmid
		*/		
		Inner Join Message_for_Use_Status
			On Message_for_Use_Status.Message_Crmid = Pos_Message.message_crmid

		Inner Join customer_use_status
			On customer_use_status.customer_use_status_crmid = Message_for_Use_Status.customer_use_status_crmid



		Where	Message_Destination.Message_destination_description ='Till Receipt'
		And CONVERT(DATETIME,CONVERT(VARCHAR,GETDATE(),111),111) Between Pos_Message.Effective_start_date And Pos_Message.Effective_end_date
		--And 	Reward_status_code = 	--Value to be obtained based upon customer's reward status(from customer_offer)
		And customer_use_status.customer_use_status_code = @primary_customer_use_status_code
		And Message_Status.Message_status_description = 'Approved'
		And Store_Crmid IS Null

		Set @Rowcount = @@Rowcount			
	
		If (@Rowcount > 1  Or @Rowcount = 0)
			Set @RcptMsgCount = 0
		Else	
		Begin
			Select Line_number, Line_text From Message_Line

				Inner Join Pos_Message
					On Message_Line.Message_crmid = Pos_Message.Message_crmid
			Where 	Pos_Message.Message_crmid = @Message_Crmid		
			Order by Line_number
			Set @RcptMsgCount = @@Rowcount
		End
	End
	Else
	Begin
		If (@Rowcount > 1  Or @Rowcount = 0)
			Set @RcptMsgCount = 0
		Else
		Begin	
			Select Line_number, Line_text From Message_Line
				Inner Join Pos_Message
					On Message_Line.Message_crmid = Pos_Message.Message_crmid
			Where 	Pos_Message.Message_crmid = @Message_Crmid		
			Order by Line_number
			Set @RcptMsgCount = @@Rowcount
		End
	End				
*/		
	IF @StatusMsgNo = 0 Set @Status = 'OK'  		
	--RETURN(@return_code)
	 RETURN