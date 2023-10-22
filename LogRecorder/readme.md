# Usage



### 

#### Step 1 Initialize

Declare log object and set path.

Example:

``` c#
private static string LogDirectorRoot = @"C:\AOI_Program\Log\";
private InfoMgr m_InfoMgr = new InfoMgr(LogDirectorRoot + @"General\", LogDirectorRoot + @"Warning\", LogDirectorRoot + @"Error\", LogDirectorRoot + @"Debug\");
```



Setup richbox to log object.

Example: 

``` c#
m_InfoMgr.SetGenLogRTB(richTextBoxGeneral);
m_InfoMgr.SetWarningLogRTB(richTextBoxWarning);
m_InfoMgr.SetErrLogRTB(richTextBoxError);
m_InfoMgr.SetDebugLogRTB(richTextBoxDebug);
```





#### Step 2 Write log

There are 4 function for writing log, according to different mode.(Such as: General, Warning, Error, Debug)

Example:

``` C#
private void Updatelog(string msg, int level)
{
    if (level == 1)
    {
        //UpdateGeneralLog(msg);
        m_InfoMgr.MsgGenLog(msg);
    }
    else if (level == 2)
    {
        m_InfoMgr.MsgWarning(msg);
    }
    else if (level == 3)
    {
        m_InfoMgr.MsgError(msg);
    }
    else if (level == 4)
    {
        m_InfoMgr.MsgDebug(msg);
    }
}
```

