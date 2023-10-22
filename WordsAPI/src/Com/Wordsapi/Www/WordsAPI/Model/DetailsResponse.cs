using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Com.Wordsapi.Www.WordsAPI.Model {
  public class DetailsResponse {
    /* Phrases to which the original word belongs. */
    public List<string> also = new List<string>();
    /* Words that have the opposite context of the original word. */
    public List<string> antonyms = new List<string>();
    /* The meaning of the word, including it's part of speech. */
    public List<Definition> definitions = new List<Definition>();
    /* Words that are implied by the original word. Usually used for verbs. */
    public List<string> entails = new List<string>();
    /* Categories of the original word. */
    public List<string> hasCategories = new List<string>();
    /* Words that are examples of the original word. */
    public List<string> hasInstances = new List<string>();
    /* Words that belong to the group defined by the original word. */
    public List<string> hasMembers = new List<string>();
    /* Words that are part of the original word. Also known as meronyms. */
    public List<string> hasParts = new List<string>();
    /* Substances that are part of the original word. */
    public List<string> hasSubstances = new List<string>();
    /* Words that are more specific than the original word. Also known as hyponyms. */
    public List<string> hasTypes = new List<string>();
    /* Words that are examples of the domain the original word defines. */
    public List<string> hasUsages = new List<string>();
    /* The domain category to which the original word belongs. */
    public List<string> inCategory = new List<string>();
    /* Regions where the word is used. */
    public List<string> inRegion = new List<string>();
    /* Words that the original word is an example of. */
    public List<string> instance_of = new List<string>();
    /* A group to which the original word belongs. */
    public List<string> memberOf = new List<string>();
    /* Partof */
    public List<string> partOf = new List<string>();
    /* Words to which the original word is relevant */
    public List<string> pertainsTo = new List<string>();
    /* A region where words are used. */
    public List<string> regionOf = new List<string>();
    /* Words that similar to the original word, but are not synonyms. */
    public List<string> similarTo = new List<string>();
    /* Substances to which the original word is a part of. */
    public List<string> substanceOf = new List<string>();
    /* Words that can be interchanged for the original word in the same context. */
    public List<string> synonyms = new List<string>();
    /* Words that are more generic than the original word. Also known as hypernyms. */
    public List<string> typeOf = new List<string>();
    /* Words that the original word is a domain usage of. */
    public List<string> usageOf = new List<string>();
    public List<string> getAlso() {
      return also;
    }
    public void setAlso(List<string> also) {
      this.also = also;
    }

    public List<string> getAntonyms() {
      return antonyms;
    }
    public void setAntonyms(List<string> antonyms) {
      this.antonyms = antonyms;
    }

    public List<Definition> getDefinitions() {
      return definitions;
    }
    public void setDefinitions(List<Definition> definitions) {
      this.definitions = definitions;
    }

    public List<string> getEntails() {
      return entails;
    }
    public void setEntails(List<string> entails) {
      this.entails = entails;
    }

    public List<string> getHasCategories() {
      return hasCategories;
    }
    public void setHasCategories(List<string> hasCategories) {
      this.hasCategories = hasCategories;
    }

    public List<string> getHasInstances() {
      return hasInstances;
    }
    public void setHasInstances(List<string> hasInstances) {
      this.hasInstances = hasInstances;
    }

    public List<string> getHasMembers() {
      return hasMembers;
    }
    public void setHasMembers(List<string> hasMembers) {
      this.hasMembers = hasMembers;
    }

    public List<string> getHasParts() {
      return hasParts;
    }
    public void setHasParts(List<string> hasParts) {
      this.hasParts = hasParts;
    }

    public List<string> getHasSubstances() {
      return hasSubstances;
    }
    public void setHasSubstances(List<string> hasSubstances) {
      this.hasSubstances = hasSubstances;
    }

    public List<string> getHasTypes() {
      return hasTypes;
    }
    public void setHasTypes(List<string> hasTypes) {
      this.hasTypes = hasTypes;
    }

    public List<string> getHasUsages() {
      return hasUsages;
    }
    public void setHasUsages(List<string> hasUsages) {
      this.hasUsages = hasUsages;
    }

    public List<string> getInCategory() {
      return inCategory;
    }
    public void setInCategory(List<string> inCategory) {
      this.inCategory = inCategory;
    }

    public List<string> getInRegion() {
      return inRegion;
    }
    public void setInRegion(List<string> inRegion) {
      this.inRegion = inRegion;
    }

    public List<string> getInstanceOf() {
      return instance_of;
    }
    public void setInstanceOf(List<string> instance_of) {
      this.instance_of = instance_of;
    }

    public List<string> getMemberOf() {
      return memberOf;
    }
    public void setMemberOf(List<string> memberOf) {
      this.memberOf = memberOf;
    }

    public List<string> getPartOf() {
      return partOf;
    }
    public void setPartOf(List<string> partOf) {
      this.partOf = partOf;
    }

    public List<string> getPertainsTo() {
      return pertainsTo;
    }
    public void setPertainsTo(List<string> pertainsTo) {
      this.pertainsTo = pertainsTo;
    }

    public List<string> getRegionOf() {
      return regionOf;
    }
    public void setRegionOf(List<string> regionOf) {
      this.regionOf = regionOf;
    }

    public List<string> getSimilarTo() {
      return similarTo;
    }
    public void setSimilarTo(List<string> similarTo) {
      this.similarTo = similarTo;
    }

    public List<string> getSubstanceOf() {
      return substanceOf;
    }
    public void setSubstanceOf(List<string> substanceOf) {
      this.substanceOf = substanceOf;
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

    public List<string> getUsageOf() {
      return usageOf;
    }
    public void setUsageOf(List<string> usageOf) {
      this.usageOf = usageOf;
    }

    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DetailsResponse {\n");
      sb.Append("  also: ").Append(also).Append("\n");
      sb.Append("  antonyms: ").Append(antonyms).Append("\n");
      sb.Append("  definitions: ").Append(definitions).Append("\n");
      sb.Append("  entails: ").Append(entails).Append("\n");
      sb.Append("  hasCategories: ").Append(hasCategories).Append("\n");
      sb.Append("  hasInstances: ").Append(hasInstances).Append("\n");
      sb.Append("  hasMembers: ").Append(hasMembers).Append("\n");
      sb.Append("  hasParts: ").Append(hasParts).Append("\n");
      sb.Append("  hasSubstances: ").Append(hasSubstances).Append("\n");
      sb.Append("  hasTypes: ").Append(hasTypes).Append("\n");
      sb.Append("  hasUsages: ").Append(hasUsages).Append("\n");
      sb.Append("  inCategory: ").Append(inCategory).Append("\n");
      sb.Append("  inRegion: ").Append(inRegion).Append("\n");
      sb.Append("  instance_of: ").Append(instance_of).Append("\n");
      sb.Append("  memberOf: ").Append(memberOf).Append("\n");
      sb.Append("  partOf: ").Append(partOf).Append("\n");
      sb.Append("  pertainsTo: ").Append(pertainsTo).Append("\n");
      sb.Append("  regionOf: ").Append(regionOf).Append("\n");
      sb.Append("  similarTo: ").Append(similarTo).Append("\n");
      sb.Append("  substanceOf: ").Append(substanceOf).Append("\n");
      sb.Append("  synonyms: ").Append(synonyms).Append("\n");
      sb.Append("  typeOf: ").Append(typeOf).Append("\n");
      sb.Append("  usageOf: ").Append(usageOf).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
  }
  }

