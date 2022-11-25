using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenaris.Library.Acquisition.Common;
using Tenaris.Library.System.Factory;
using System.Threading;
using System.Configuration;

namespace Tenaris.Fava.Production.Reporting.OplScada
{
    public class OplSubscription
    {
        ITag<UInt16[]> tag;

        public OplSubscription()
        {
            try
            {
                //FactoryProvider instance = FactoryProvider.Instance;
                //IFactory<IAcquisitionSession> factory = instance.CreateFactory<IAcquisitionSession>("AcquisitionConfiguration");
                //AcquisitionSession = factory.Create();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        public List<ITag<Boolean>> tagListForFalseValue;
        public List<ITag<Boolean>> tagListForTrueValue;



        private IAcquisitionSession AcquisitionSession{get;set;}


        public void OpenSession()
        {
            if(!CurrentSession.IsOpen)
                CurrentSession.Open();
        }

        public IAcquisitionSession CurrentSession
        {
            get
            {
                if (AcquisitionSession == null)
                {
                    
                    FactoryProvider instance = FactoryProvider.Instance;
                                       
                    IFactory<IAcquisitionSession> factory = instance.CreateFactory<IAcquisitionSession>("AcquisitionConfiguration");
                    AcquisitionSession = factory.Create();
                    AcquisitionSession.Open();
                }

                return AcquisitionSession;
            }
            set { }
        }

        public void CloseSession()
        {
            CurrentSession.Close();
            
        }

        public UInt16 GetCounterTag(string tagStr)
        {
            
            UInt16 counter = 0;
            try
            {
                
                //
                if(tagStr!=String.Empty)
                {
                    if (!CurrentSession.Tags.Contains(tagStr))
                        tag = CurrentSession.Tags.Add<UInt16[]>(tagStr, null);
                    TagValue<UInt16[]> values = tag.Read();
                    var value = values.Value;
                    counter = value[0];
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return counter;
        }

        public void ResetCounter(string tagStr)
        {
            
            try
            {
                //if (CurrentSession.Tags.Contains("CND_COUNT_OK.ACC"))
                if(tagStr!=String.Empty)
                {
                    if (!CurrentSession.Tags.Contains(tagStr))
                        tag = CurrentSession.Tags.Add<UInt16[]>(tagStr, null);
                    
                    UInt16[] values = new ushort[] {10}; //Pasamos a 1 para que haya transición a vlor 0
                    tag.Write(values);
                    Thread.Sleep(500);
                    values = new ushort[] { 0 };
                    tag.Write(values);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public UInt32 GetTrackingPass(string tagStr)
        {
            UInt16 trackingPass = 0;
            try
            {

                //
                if (tagStr != String.Empty)
                {
                    if (!CurrentSession.Tags.Contains(tagStr))
                        tag = CurrentSession.Tags.Add<UInt16[]>(tagStr, null);
                    TagValue<UInt16[]> values = tag.Read();
                    var value = values.Value;
                    trackingPass = value[2];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return trackingPass;
        }
    }
}
