using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClangPowerTools
{
  public class MyDynamicExtender : IMyDynamicExtender, IFilterProperties
  {

    // names to be used when store property values 
    // in EnvDTE.Project.Global
    private const string fileDescPropName = "MyFileDescription";

    // extension manager site
    private EnvDTE.IExtenderSite m_ExtenderSite = null;

    // cookie that identify this extender at extension manager site
    private int m_SiteCookie = 0;

    // project file name.
    private string m_FileName = null;

    private DTE2 mDte;

    public MyDynamicExtender(string fileName, int ExtenderCookie, EnvDTE.IExtenderSite ExtenderSite, DTE2 aDte)
    {
      m_ExtenderSite = ExtenderSite;
      m_SiteCookie = ExtenderCookie;
      m_FileName = fileName;
      mDte = aDte;
    }


    protected void Finalize()
    {
      //Wrap this call in a try-catch to avoid any failure code the
      //Site may return. For instance, since this object is GC'ed,
      //the Site may have already let go of its Cookie.

      try
      {
        if (m_ExtenderSite != null && m_SiteCookie != 0)
        {
          m_ExtenderSite.NotifyDelete(m_SiteCookie);
          System.Diagnostics.Trace.WriteLine(string.Format("Extender cookie deleted 0x{0:X}", m_SiteCookie));
        }


      }
      catch (Exception) { }

      // base.Finalize();

    }


    #region IMyDynamicExtender Implementation 


    public DateTime FileCreated
    {
      get
      {
        return System.IO.File.GetCreationTime(m_FileName);
      }
    }

    public string FileDescription
    {
      get
      {
        EnvDTE.Globals projGlobals = mDte.Solution.Globals; //  applicationObject.Globals;

        if (false == projGlobals.VariableExists[fileDescPropName])
        {
          projGlobals[fileDescPropName] = string.Empty;
          projGlobals.VariablePersists[fileDescPropName] = false;
        }

        return projGlobals[fileDescPropName].ToString();
      }

      set
      {
        EnvDTE.Globals projGlobals = mDte.Solution.Globals; // applicationObject.Globals;
        projGlobals[fileDescPropName] = value;
        projGlobals.VariablePersists[fileDescPropName] = false;
      }

    }

    [DisplayName("File Type")]
    public string FileType
    {
      get
      {
        return "My File Type";
      }
    }

    #endregion


    #region IFilterProperties implementation

    public vsFilterProperties IsPropertyHidden(string PropertyName)
    {
      if (PropertyName == "FileType")
        return vsFilterProperties.vsFilterPropertiesAll;
      else
        return vsFilterProperties.vsFilterPropertiesNone;
    }

    #endregion

  }
}
