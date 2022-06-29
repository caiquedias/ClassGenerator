namespace ClassGenerator_BETA_.DTO

{
    public class ResultDto
    {
        public ResultDto(bool _success, object _objectReturn)
        {
            SetSucess(_success);
            SetObjectReturn(_objectReturn);
        }

        private bool sucess;
        private object objectReturn;

        public bool GetSucess()
        {
            return sucess;
        }

        private void SetSucess(bool value)
        {
            sucess = value;
        }

        public object GetObjectReturn()
        {
            return objectReturn;
        }

        private void SetObjectReturn(object value)
        {
            objectReturn = value;
        }
    }
}
