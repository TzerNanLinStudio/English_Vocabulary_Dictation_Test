using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Com.Wordsapi.Www.WordsAPI.Model {
  public class Result {
    /* Definition */
    public string definition = null;
    /* Derivation */
    public List<string> derivation = new List<string>();
    /* Partofspeech */
    public string partOfSpeech = null;
    /* Synonyms */
    public List<string> synonyms = new List<string>();
    /* Typeof */
    public List<string> typeOf = new List<string>();
    public string getDefinition() {
      return definition;
    }
    public void setDefinition(string definition) {
      this.definition = definition;
    }

    public List<string> getDerivation() {
      return derivation;
    }
    public void setDerivation(List<string> derivation) {
      this.derivation = derivation;
    }

    public string getPartOfSpeech() {
      return partOfSpeech;
    }
    public void setPartOfSpeech(string partOfSpeech) {
      this.partOfSpeech = partOfSpeech;
    }

    public List<string> getSynonyms() {
      return synonyms;
    }
    public void setSynonyms(List<string> synonyms) {
      this.synonyms = synonyms;
    }

    public List<string> getTypeOf() {
      return typeOf;
    }
    public void setTypeOf(List<string> typeOf) {
      this.typeOf = typeOf;
    }

    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Result {\n");
      sb.Append("  definition: ").Append(definition).Append("\n");
      sb.Append("  derivation: ").Append(derivation).Append("\n");
      sb.Append("  partOfSpeech: ").Append(partOfSpeech).Append("\n");
      sb.Append("  synonyms: ").Append(synonyms).Append("\n");
      sb.Append("  typeOf: ").Append(typeOf).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
  }
  }

