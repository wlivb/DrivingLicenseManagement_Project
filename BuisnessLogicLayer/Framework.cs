namespace BuisnessLogicLayer
{
    public abstract class clsBaseLogic<T> where T : new()
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public T DTO { get; set; }

        public clsBaseLogic()
        {
            this.DTO = new T();
            this.Mode = enMode.AddNew;
        }

        public clsBaseLogic(T dto)
        {
            this.DTO = dto;
            this.Mode = enMode.Update;
        }

        protected abstract bool _AddNew();
        protected abstract bool _Update();

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return _Update();
            }
            return false;
        }
    }
}
