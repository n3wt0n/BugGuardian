using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTek.BugGuardian.Config
{
    public class BugGuardianSettingCollection : System.Configuration.ConfigurationElementCollection
    {
        public BugGuardianSettingElements this[int index]
        {
            get
            {
                return base.BaseGet(index) as BugGuardianSettingElements;
            }
            set
            {
                if (base.BaseGet(index) != null)
                    base.BaseRemoveAt(index);

                this.BaseAdd(index, value);
            }
        }

        protected override System.Configuration.ConfigurationElement CreateNewElement()
        {
            return new BugGuardianSettingElements();
        }

        protected override object GetElementKey(System.Configuration.ConfigurationElement element)
        {
            return ((BugGuardianSettingElements)element).Key;
        }
    }
}