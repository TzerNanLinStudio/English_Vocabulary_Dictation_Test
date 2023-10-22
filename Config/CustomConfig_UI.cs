using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config_sharp
{
    public class CustomConfig_UI_Parameters
    {
        #region "UI Parameter Declare"
        private int m_Parameter01;
        #endregion

        public CustomConfig_UI_Parameters()
        {
            #region "UI Parameter Initialize"
            m_Parameter01 = 0;
            #endregion
        }

        #region "UI Parameter Set Or Get"
        public int Parameter01 { get => m_Parameter01; set => m_Parameter01 = value; }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class CustomConfig_UI : BaseConfig<CustomConfig_UI>
    {
        private string _version = "UIConfig_202004081500";

        public CustomConfig_UI_Parameters parameters;

        public CustomConfig_UI()
        {
            version = _version;

            parameters = new CustomConfig_UI_Parameters();
            
            RecipeFullPath = "./recipt.bat"; //Default Path
        }

        public CustomConfig_UI(string fileFullPath)
        {
            parameters = new CustomConfig_UI_Parameters();

            RecipeFullPath = fileFullPath;
        }

        protected override bool Write_Object(CustomConfig_UI config)
        {
            parameters = config.parameters.Clone() as CustomConfig_UI_Parameters;

            return (this._version == config._version);
        }
    }
}

