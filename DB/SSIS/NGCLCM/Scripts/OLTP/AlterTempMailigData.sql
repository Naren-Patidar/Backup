ALTER TABLE TempMailingData ADD EmailAddress NVARCHAR(2000)
ALTER TABLE TempMailingData ADD MobilePhoneNumber VARCHAR(40)
ALTER TABLE TempMailingData ADD TescoGroupMail CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD TescoGroupEmail CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD TescoGroupPhone CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD TescoGroupSMS CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD PartnerThirdPartyMail CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD PartnerThirdPartyEmail CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD PartnerThirdPartyPhone CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD PartnerThirdPartySMS CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD ResearchMail CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD ResearchEmail CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD ResearchPhone CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD ResearchSMS CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD BonusCouponMailing CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD BCMMail CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD BCMEmail CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD BCMPhone CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD BCMSMS CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD TescoProducts CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD TescoPartners CHAR(1) DEFAULT(0)
ALTER TABLE TempMailingData ADD CustomerResearch CHAR(1) DEFAULT(0)