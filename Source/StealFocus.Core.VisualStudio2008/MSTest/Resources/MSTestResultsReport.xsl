<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

  <xsl:output method="html"/>

  <xsl:template match="/">
    <xsl:variable name="pass_count" select="/*[local-name()='TestRun']/*[local-name()='ResultSummary']/*[local-name()='Counters']/@passed"/>
    <xsl:variable name="inconclusive_count" select="/*[local-name()='TestRun']/*[local-name()='ResultSummary']/*[local-name()='Counters']/@inconclusive"/>
    <xsl:variable name="failed_count" select="/*[local-name()='TestRun']/*[local-name()='ResultSummary']/*[local-name()='Counters']/@failed"/>
    <xsl:variable name="total_count" select="/*[local-name()='TestRun']/*[local-name()='ResultSummary']/*[local-name()='Counters']/@total"/>
    <head>
      <title>MSTest Results Report</title>
      <style type="text/css">
        h1              { font: large verdana, arial, helvetica; color:#000000;	}
        h2              { font: medium verdana, arial, helvetica; color:#000000;	}
        p               { font: smaller verdana, arial, helvetica; color:#000000;	}
        table           { font: smaller verdana, arial, helvetica; color:#000000;	}
        thead           { font: small verdana, arial, helvetica; color:#000000;	}
        td              { vertical-align:top; }
        td.numberColumn { text-align: right; }
        td.green        { background-color: #00FF00; height: 15px; }
        td.amber        { background-color: #FFFF00; height: 15px; }
        td.red          { background-color: #FF0000; height: 15px; }
        .details        { border-color: Black; }
        .details td     { border-color: Black; }
      </style>
    </head>
    <body>
      <h1>MSTest Results Report</h1>
      <p>
        MSTest results.
      </p>
      <h2>Summary</h2>
      <table>
        <thead>
          <tr>
            <td>Run</td>
            <td>Passed</td>
            <td>Failed</td>
            <td>Inconclusive</td>
            <td>Result</td>
          </tr>
        </thead>
        <tbody>
          <tr>
            <td class="numberColumn">
              <xsl:value-of select="$total_count"/>
            </td>
            <td class="numberColumn">
              <xsl:value-of select="$pass_count"/>
            </td>
            <td class="numberColumn">
              <xsl:value-of select="$failed_count"/>
            </td>
            <td class="numberColumn">
              <xsl:value-of select="$inconclusive_count"/>
            </td>
            <td>
              <xsl:choose>
                <xsl:when test="$total_count=$pass_count">
                  Success
                </xsl:when>
                <xsl:otherwise>
                  Failure
                </xsl:otherwise>
              </xsl:choose>
            </td>
          </tr>
        </tbody>
      </table>
      <h2>Details</h2>
      <table cellspacing="0" border="1" class="details">
        <thead>
          <tr>
            <td>Test Class Name</td>
            <td>Test Method Name</td>
            <td>Result</td>
            <td>Duration</td>
            <td>Standard Output</td>
            <td>Debug Output</td>
          </tr>
        </thead>
        <tbody>
          <xsl:for-each select="/*[local-name()='TestRun']/*[local-name()='Results']/*[local-name()='UnitTestResult']">
            <tr>
              <td>
                <xsl:variable name="id" select="@testId"/>
                <xsl:variable name="fullClassName" select="/*[local-name()='TestRun']/*[local-name()='TestDefinitions']/*[local-name()='UnitTest' and @id=$id]/*[local-name()='TestMethod']/@className" />
                <xsl:value-of select="substring-before($fullClassName, ',')" />
              </td>
              <td>
                <xsl:value-of select="@testName" />
              </td>
              <td>
                <xsl:attribute name="class">
                  <xsl:if test="@outcome = 'Passed'">green</xsl:if>
                  <xsl:if test="@outcome = 'Failed'">red</xsl:if>
                  <xsl:if test="@outcome = 'Inconclusive'">amber</xsl:if>
                </xsl:attribute>
                <xsl:value-of select="@outcome" />
              </td>
              <td>
                <xsl:value-of select="@duration" />hrs
              </td>
              <td>
                <xsl:variable name="standardOutput" select="*[local-name()='Output']/*[local-name()='StdOut']" />
                <pre>
                  <xsl:value-of select="$standardOutput" />
                </pre>
              </td>
              <td>
                <xsl:variable name="debugOutput" select="*[local-name()='debugOutput']/*[local-name()='DebugTrace']" />
                <pre>
                  <xsl:value-of select="$debugOutput" />
                </pre>
              </td>
            </tr>
          </xsl:for-each>
        </tbody>
      </table>
    </body>
  </xsl:template>

</xsl:stylesheet>
