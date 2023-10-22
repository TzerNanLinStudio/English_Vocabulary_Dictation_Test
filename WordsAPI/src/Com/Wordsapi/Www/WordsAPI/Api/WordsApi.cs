  using System;
  using System.Collections.Generic;
  using Com.Wordsapi.Www.WordsAPI;
  using Com.Wordsapi.Www.WordsAPI.Model;
  namespace Com.Wordsapi.Www.WordsAPI.Api {
    public class WordsApi {
      string basePath;
      private readonly ApiInvoker apiInvoker = ApiInvoker.GetInstance();

      public WordsApi(String basePath = "https://www.wordsapi.com")
      {
        this.basePath = basePath;
      }

      public ApiInvoker getInvoker() {
        return apiInvoker;
      }

      // Sets the endpoint base url for the services being accessed
      public void setBasePath(string basePath) {
        this.basePath = basePath;
      }

      // Gets the endpoint base url for the services being accessed
      public String getBasePath() {
        return basePath;
      }

      /// <summary>
      /// To retrieve a specific set of details of a word. To retrieve a specific set of details of a word, for instance, a word's synonyms, append the detail type to the URL string.
      /// </summary>
      /// <param name="accessToken">API key or token for authorization</param>
      /// <param name="word">Word</param>
      /// <param name="detail">Detail</param>
      /// <returns></returns>
      public DetailsResponse Details (string accessToken, string word, string detail) {
        // create path and map variables
        var path = "/words/{word}/{detail}".Replace("{format}","json").Replace("{" + "word" + "}", apiInvoker.escapeString(word.ToString())).Replace("{" + "detail" + "}", apiInvoker.escapeString(detail.ToString()));

        // query params
        var queryParams = new Dictionary<String, String>();
        var headerParams = new Dictionary<String, String>();
        var formParams = new Dictionary<String, object>();

        // verify required params are set
        if (accessToken == null || word == null || detail == null ) {
           throw new ApiException(400, "missing required params");
        }
        if (accessToken != null){
          string paramStr = (accessToken is DateTime) ? ((DateTime)(object)accessToken).ToString("u") : Convert.ToString(accessToken);
          queryParams.Add("accessToken", paramStr);
		}
        try {
          if (typeof(DetailsResponse) == typeof(byte[])) {
            var response = apiInvoker.invokeBinaryAPI(basePath, path, "GET", queryParams, null, headerParams, formParams);
            return ((object)response) as DetailsResponse;
          } else {
            var response = apiInvoker.invokeAPI(basePath, path, "GET", queryParams, null, headerParams, formParams);
            if(response != null){
               return (DetailsResponse) ApiInvoker.deserialize(response, typeof(DetailsResponse));
            }
            else {
              return null;
            }
          }
        } catch (ApiException ex) {
          if(ex.ErrorCode == 404) {
          	return null;
          }
          else {
            throw ex;
          }
        }
      }
      /// <summary>
      ///   Get
      /// </summary>
      /// <param name="accessToken">API key or token for authorization</param>
      /// <param name="word">Word</param>
      /// <returns></returns>
      public WordResponse Get (string accessToken, string word) {
        // create path and map variables
        var path = "/words/{word}".Replace("{format}","json").Replace("{" + "word" + "}", apiInvoker.escapeString(word.ToString()));

        // query params
        var queryParams = new Dictionary<String, String>();
        var headerParams = new Dictionary<String, String>();
        var formParams = new Dictionary<String, object>();

        // verify required params are set
        if (accessToken == null || word == null ) {
           throw new ApiException(400, "missing required params");
        }
        if (accessToken != null){
          string paramStr = (accessToken is DateTime) ? ((DateTime)(object)accessToken).ToString("u") : Convert.ToString(accessToken);
          queryParams.Add("accessToken", paramStr);
		}
        try {
          if (typeof(WordResponse) == typeof(byte[])) {
            var response = apiInvoker.invokeBinaryAPI(basePath, path, "GET", queryParams, null, headerParams, formParams);
            return ((object)response) as WordResponse;
          } else {
            var response = apiInvoker.invokeAPI(basePath, path, "GET", queryParams, null, headerParams, formParams);
            if(response != null){
               return (WordResponse) ApiInvoker.deserialize(response, typeof(WordResponse));
            }
            else {
              return null;
            }
          }
        } catch (ApiException ex) {
          if(ex.ErrorCode == 404) {
          	return null;
          }
          else {
            throw ex;
          }
        }
      }
      }
    }

