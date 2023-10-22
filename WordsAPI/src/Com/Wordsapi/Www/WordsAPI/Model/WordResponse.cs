using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Com.Wordsapi.Www.WordsAPI.Model {
  public class WordResponse {
    /* Results */
    public List<Result> results = new List<Result>();
    public List<Result> getResults() {
      return results;
    }
    public void setResults(List<Result> results) {
      this.results = results;
    }

    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class WordResponse {\n");
      sb.Append("  results: ").Append(results).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }
  }
  }

