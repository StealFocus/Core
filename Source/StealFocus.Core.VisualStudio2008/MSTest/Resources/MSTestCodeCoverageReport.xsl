<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

  <xsl:output method="html" />

  <xsl:variable name="threshold">90</xsl:variable>

  <xsl:template match="/">
    <head>
      <title>MSTest Code Coverage Report</title>
      <style type="text/css">
        h1                    { font: large verdana, arial, helvetica; color:#000000;	}
        h2                    { font: medium verdana, arial, helvetica; color:#000000;	}
        p                     { font: smaller verdana, arial, helvetica; color:#000000;	}
        table                 { font: smaller verdana, arial, helvetica; color:#000000;	}
        thead                 { font: small verdana, arial, helvetica; color:#000000;	}
        td.indentColumn         {  }
        td.percentNumberColumn  { text-align: right; }
        td.greenBar             { background-color: #00FF00; height: 15px; }
        td.amberBar             { background-color: #FFFF00; height: 15px; }
        td.redBar               { background-color: #FF0000; height: 15px; }
      </style>
    </head>
    <body>
      <h1>MSTest Code Coverage Report</h1>
      <p>
        Code coverage report based on MSTest results. The code coverage "threshold" is set to <xsl:value-of select="$threshold" />%.
      </p>
      <xsl:call-template name="ModuleSummary" />
      <br />
      <xsl:apply-templates select="CoverageDSPriv" />
    </body>
  </xsl:template>

  <xsl:template name="ModuleSummary">
    <h2>Summary</h2>
    <table>
      <thead>
        <tr>
          <td>Assembly</td>
          <td colspan="2">Coverage</td>
        </tr>
      </thead>
      <tbody>
      <xsl:for-each select="CoverageDSPriv/Module">
        <tr>
          <td>
            <xsl:value-of select="ModuleName" />
          </td>
          <xsl:call-template name="CoverageInformationColumns" />
        </tr>
      </xsl:for-each>
      </tbody>
    </table>
  </xsl:template>

  <xsl:template match="CoverageDSPriv">
    <h2>Details</h2>
    <table>
      <thead>
        <tr>
          <td colspan="5">Item</td>
          <td colspan="2">Coverage</td>
        </tr>
      </thead>
      <tbody>
        <xsl:apply-templates select="Module" />
      </tbody>
    </table>
  </xsl:template>

  <xsl:template match="Module">
    <xsl:for-each select=".">
      <tr>
        <td>Assembly</td>
        <td colspan="4">
          <xsl:value-of select="ModuleName" />
        </td>
        <xsl:call-template name="CoverageInformationColumns" />
      </tr>
      <xsl:apply-templates select="NamespaceTable" />
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="NamespaceTable">
    <xsl:for-each select=".">
      <tr>
        <td class="indentColumn"></td>
        <td>Namespace</td>
        <td colspan="3">
          <xsl:value-of select="NamespaceName" />
        </td>
        <xsl:call-template name="CoverageInformationColumns" />
      </tr>
      <xsl:apply-templates select="Class" />
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="Class">
    <xsl:for-each select=".">
      <tr>
        <td class="indentColumn"></td>
        <td class="indentColumn"></td>
        <td>Class</td>
        <td colspan="2">
          <xsl:value-of select="ClassName" />
        </td>
        <xsl:call-template name="CoverageInformationColumns" />
      </tr>
      <xsl:apply-templates select="Method" />
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="Method">
    <xsl:for-each select=".">
      <tr>
        <td class="indentColumn"></td>
        <td class="indentColumn"></td>
        <td class="indentColumn"></td>
        <td>Method</td>
        <td colspan="1">
          <xsl:value-of select="MethodFullName" />
        </td>
        <xsl:call-template name="CoverageInformationColumns" />
      </tr>
    </xsl:for-each>
  </xsl:template>

  <!-- "Functions" -->

  <xsl:template name="CoverageInformationColumns">
    <td class="percentNumberColumn">
      <xsl:call-template name="CodeCoveredPercentage" />%
    </td>
    <td>
      <xsl:call-template name="CodeCoverageBar">
        <xsl:with-param name="coveredPercentage">
          <xsl:call-template name="CodeCoveredPercentage" />
        </xsl:with-param>
        <xsl:with-param name="notCoveredPercentage">
          <xsl:call-template name="CodeNotCoveredPercentage" />
        </xsl:with-param>
      </xsl:call-template>
    </td>
  </xsl:template>

  <xsl:template name="CodeCoveredPercentage">
    <xsl:value-of select="round((BlocksCovered div (BlocksCovered + BlocksNotCovered)) * 100)" />
  </xsl:template>

  <xsl:template name="CodeNotCoveredPercentage">
    <xsl:value-of select="100 - (round((BlocksCovered div (BlocksCovered + BlocksNotCovered)) * 100))" />
  </xsl:template>

  <xsl:template name="CodeCoverageBar">
    <xsl:param name="coveredPercentage" />
    <xsl:param name="notCoveredPercentage" />
    <table cellpadding="0" cellspacing="0">
      <tbody>
        <tr>
          <td class="greenBar">
            <xsl:attribute name="width">
              <xsl:value-of select="$coveredPercentage" />
            </xsl:attribute>
          </td>
          <td>
            <xsl:attribute name="class">
              <xsl:if test="$coveredPercentage &gt;= $threshold">amberBar</xsl:if>
              <xsl:if test="$coveredPercentage &lt; $threshold">redBar</xsl:if>
            </xsl:attribute>
            <xsl:attribute name="width">
              <xsl:value-of select="$notCoveredPercentage" />
            </xsl:attribute>
          </td>
        </tr>
      </tbody>
    </table>
  </xsl:template>

</xsl:stylesheet>