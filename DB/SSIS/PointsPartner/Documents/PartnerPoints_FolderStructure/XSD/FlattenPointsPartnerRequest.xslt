<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text" encoding="utf-8"/>
<xsl:strip-space elements="*"/>
	<xsl:template match="/pptxn">
		<xsl:variable name="fileName" select="/pptxn/file_name"/>
		<xsl:variable name="sendingOrg" select="/pptxn/sending_org"/>
		<xsl:variable name="dateCreated" select="/pptxn/date_created"/>
		<xsl:variable name="timeCreated" select="/pptxn/time_created"/>
		<xsl:variable name="sequenceNumber" select="/pptxn/sequence_number"/>
		<xsl:text>FileName,SendingOrg,DateCreated,TimeCreated,SequenceNumber,PartnerNumber,PartnerOutletRef,PartnerOutletName,CardAccountNumber,TransactionDate,TransactionTime,Spend,PartnerPoints,PartnerReference,PartnerPosId</xsl:text>
    <!--using || as line delimter because SSIS wil not write the line feed char: "|&#10;" -->
    <!-- <xsl:text>&#10;</xsl:text> -->
    <xsl:text>&#13;&#10;</xsl:text>
    <xsl:for-each select="transaction">
			<xsl:value-of select="$fileName"/>,<xsl:value-of select="$sendingOrg"/>,<xsl:value-of select="$dateCreated"/>,<xsl:value-of select="$timeCreated"/>,<xsl:value-of select="$sequenceNumber"/>,<xsl:value-of select="partner_number"/>,<xsl:value-of select="partner_outlet_ref"/>,<xsl:value-of select="partner_outlet_name"/>,<xsl:value-of select="card_account_number"/>,<xsl:value-of select="transaction_date"/>,<xsl:value-of select="transaction_time"/>,<xsl:value-of select="spend"/>,<xsl:value-of select="partner_points"/>,<xsl:value-of select="partner_reference"/>,<xsl:value-of select="partner_pos_id"/>
      <xsl:text>&#13;&#10;</xsl:text>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>
