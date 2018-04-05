using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using EnvDTE80;

namespace ClangPowerTools
{
  [CLSCompliant(false)]
  public class MyDynamicExtenderProvider : IExtenderProvider
  {
    private static string m_DynamicExtenderName = "MyProjectItemExtender";

    private DTE2 mDte;

    public static string DynamicExtenderName
    {
      get
      {
        return m_DynamicExtenderName;
      }
    }


    public MyDynamicExtenderProvider(DTE2 aDte)
    {
      mDte = aDte;
    }

    #region IExtenderProvider Implementation

    public bool CanExtend(string ExtenderCATID, string ExtenderName, object ExtendeeObject)
    {
      bool IfCanExtend = false;

      // check if provider can create extender for the given
      // ExtenderCATID, ExtenderName, and Extendee instance

      System.ComponentModel.PropertyDescriptor extendeeCATIDProp = System.ComponentModel.TypeDescriptor.GetProperties(ExtendeeObject)["ExtenderCATID"];

      // replace guids with the guid of the CATID
      // of the project you extend.

      if (ExtenderName == m_DynamicExtenderName &&
        ExtenderCATID.ToUpper() == VSConstants.CATID.VCProjectNode_string.ToUpper() &&
        extendeeCATIDProp != null &&
        extendeeCATIDProp.GetValue(ExtendeeObject).ToString().ToUpper() == VSConstants.CATID.VCProjectNode_string.ToUpper())
      {
        IfCanExtend = true;
      }
      else
      {
        IfCanExtend = false;

      }

      return IfCanExtend;

    }

    public object GetExtender(string ExtenderCATID, string ExtenderName, object ExtendeeObject, IExtenderSite ExtenderSite, int Cookie)
    {
      MyDynamicExtender dynamicExtender = null;

      if( CanExtend(ExtenderCATID, ExtenderName, ExtendeeObject) )
      {
        // create extender
        dynamicExtender = new MyDynamicExtender(ExtenderName, Cookie, ExtenderSite, mDte);
      }

      return dynamicExtender;
    }

    #endregion

  }
}
