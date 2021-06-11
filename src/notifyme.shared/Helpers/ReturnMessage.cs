using notifyme.shared.Models;

namespace notifyme.shared.Helpers
{
    public class ReturnMessage : BaseMvvmModel
    {
        public ReturnMessage()
        {
                
        }
        public ReturnMessage(bool initialBool, string initialString)
        {
            _returnBool = initialBool;
            _returnString = initialString;
        }

        private bool _returnBool = true;
        public bool ReturnBool
        {
            get => _returnBool;
            private set => SetValue(ref _returnBool, value);
        }
        
        private string _returnString = "";
        public string ReturnString
        {
            get => _returnString;
            private set => SetValue(ref _returnString, value);
        }

        public void SetNewValues(bool newBool, string newString = "")
        {
            ReturnBool = newBool;
            ReturnString = newString;
        }
    }
}