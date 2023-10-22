using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Com.Wordsapi.Www.WordsAPI.Model {
  public class Definition {
    /* Definition */
    public string definition = null;
    /* Partofspeech */
    public string partOfSpeech = null;
    public string getDefinition() {
      return definition;
    }
    public void setDefinition(string definition) {
      this.definition = definition;
    }

    public string getPartOfSpeech() {
      return partOfSpeech;
    }
    public void setPartOfSpeech(string partOfSpeech) {
      this.partOfSpeech = partOfSpeech;
    }

    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Definition {\n");
      sb.Append("  definition: ").Append(definition).Append("\n");
      sb.Append("  partOfSpeech: ").Append(partOfSpeech).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
  }
  }

