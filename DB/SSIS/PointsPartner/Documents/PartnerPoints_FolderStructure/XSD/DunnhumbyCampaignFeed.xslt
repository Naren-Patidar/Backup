<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:output method="text" encoding="utf-8"/>
<xsl:strip-space elements="*"/>
	<xsl:template match="/CampaignCustomers | @*">
    <xsl:variable name="DunnhumbyCampaignID" select="DunnhumbyCampaignID"/>
    <xsl:variable name="CampaignDescription" select="CampaignDescription"/>
    <xsl:variable name="NumberOfCustomersWithinCampaign" select="NumberOfCustomersWithinCampaign"/>
    <xsl:text>DunnhumbyCampaignID,CampaignDescription,NumberOfCustomersWithinCampaign,CustomerID,OfferID</xsl:text>
    <xsl:text>&#13;&#10;</xsl:text>
    <xsl:for-each select="Customers/Customer">
      <xsl:value-of select="$DunnhumbyCampaignID"/>,<xsl:value-of select="$CampaignDescription"/>,<xsl:value-of select="$NumberOfCustomersWithinCampaign"/>,<xsl:value-of select="CustomerID"/>,<xsl:value-of select="OfferID"/>
      <xsl:text>&#13;&#10;</xsl:text>
		</xsl:for-each>
	</xsl:template>
</xsl:stylesheet>
