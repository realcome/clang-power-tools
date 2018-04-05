using System;
using System.Runtime.InteropServices;

namespace ClangPowerTools
{
  [CLSCompliant(false)]
  [InterfaceType(ComInterfaceType.InterfaceIsDual)]
  public interface IMyDynamicExtender
  {
    DateTime FileCreated { get; }
    String FileDescription { get; set; }
    String FileType { get; }
  }
}
