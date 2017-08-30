

SET IDENTITY_INSERT Preference ON

insert into preference (PreferenceID,PreferenceDescEnglish,PreferenceDescLocal,PreferenceType,InsertDateTime,
InsertBy,AmendDateTime,AmendBy,IsDeleted,SortSeq,PreferenceLevel) values
(49,'Bonus Coupon Mailing','Bonus Coupon Mailing',9,GETDATE(),99,null,null,'Y',6,'A')

insert into preference (PreferenceID,PreferenceDescEnglish,PreferenceDescLocal,PreferenceType,InsertDateTime,
InsertBy,AmendDateTime,AmendBy,IsDeleted,SortSeq,PreferenceLevel) values
(50,'BCM Mail', 'BCM Mail',9,GETDATE(),99,null,null,'Y',NULL,'A')

insert into preference (PreferenceID,PreferenceDescEnglish,PreferenceDescLocal,PreferenceType,InsertDateTime,
InsertBy,AmendDateTime,AmendBy,IsDeleted,SortSeq,PreferenceLevel) values
(51,'BCM EMail', 'BCM EMail',9,GETDATE(),99,null,null,'Y',NULL,'A')

insert into preference (PreferenceID,PreferenceDescEnglish,PreferenceDescLocal,PreferenceType,InsertDateTime,
InsertBy,AmendDateTime,AmendBy,IsDeleted,SortSeq,PreferenceLevel) values
(52,'BCM Phone', 'BCM Phone',9,GETDATE(),99,null,null,'Y',NULL,'A')

insert into preference (PreferenceID,PreferenceDescEnglish,PreferenceDescLocal,PreferenceType,InsertDateTime,
InsertBy,AmendDateTime,AmendBy,IsDeleted,SortSeq,PreferenceLevel) values
(53,'BCM SMS', 'BCM SMS',9,GETDATE(),99,null,null,'Y',NULL,'A')


SET IDENTITY_INSERT Preference OFF