namespace cattoapi.customResponse
{

    public class CustomResponse<T>
    {
        public CustomResponse()
        {
            responseCode = 0;
            responseMessage = string.Empty;
        }

        public CustomResponse(int responseCode, string responseMessage)
        {
            this.responseCode = responseCode;
            this.responseMessage = responseMessage;
        }

        public CustomResponse(int responseCode,string responseMessage, T data) { 
            this.responseCode = responseCode;
            this.responseMessage = responseMessage;
            this.data = data;
        }
        public int responseCode { get; set; }
        public string responseMessage{ get; set; }
        public T? data { get; set; }

    }
}
