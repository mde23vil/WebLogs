using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LogAnalyzer.Helpers
{
  public static class EntityTypes
  {
    public static IEnumerable<string> GetDocumentTypes()
    {
      return new List<string>()
      {
        //Contracts
        "IContract",
        "IContractStatement",
        "IIncomingInvoice",
        "ISupAgreement",
        //Docflow
        "IAddendum",
        "IMemo",
        "IMinutes",
        "ISimpleDocument",
        //RecordManagement
        "IIncomingLetter",
        "IOrder",
        "IOutgoingLetter",
        //Projects
        "IProjectDocument"
      };
    }
  }
}