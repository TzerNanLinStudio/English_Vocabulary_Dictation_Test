using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config_sharp
{
    public class CustomConfig_EnglishVocaburary_Parameters
    {
        #region "Vocaburary Parameter Declare"
        private int p_TestTime;
        #endregion

        public CustomConfig_EnglishVocaburary_Parameters()
        {
            #region "Vocaburary Parameter Initialize"
            p_TestTime = 0;
            #endregion
        }

        #region "Vocaburary Parameter Set Or Get"
        public int TestTime
        {
            set
            {
                p_TestTime = (value < 0) ? 0 : value;
            }

            get
            {
                return p_TestTime;
            }
        }
        #endregion

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class CustomConfig_EnglishVocaburary : BaseConfig<CustomConfig_EnglishVocaburary>
    {
        private string _version = "UIConfig_202004081500";

        public CustomConfig_EnglishVocaburary_Parameters parameters;

        public CustomConfig_EnglishVocaburary()
        {
            version = _version;

            parameters = new CustomConfig_EnglishVocaburary_Parameters();

            RecipeFullPath = "./recipt.bat"; //Default Path
        }

        public CustomConfig_EnglishVocaburary(string fileFullPath)
        {
            parameters = new CustomConfig_EnglishVocaburary_Parameters();

            RecipeFullPath = fileFullPath;
        }

        protected override bool Write_Object(CustomConfig_EnglishVocaburary config)
        {
            parameters = config.parameters.Clone() as CustomConfig_EnglishVocaburary_Parameters;

            return (this._version == config._version);
        }
    }
}
