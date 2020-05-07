using System.Collections.Generic;

namespace OnlineShopping.Repository
{
    public class Populator
    {
        public List<PairModel> GetPairModel(string param, long param1 = 0, long param2 = 0)
        {
            List<PairModel> _pairModel = new List<PairModel>();

            switch (param)
            {
                case "IsActive":
                    _pairModel.Add(new PairModel { Key = true, Value = "Active" });
                    _pairModel.Add(new PairModel { Key = false, Value = "In-active" });
                    return _pairModel;
                case "Gender":
                    _pairModel.Add(new PairModel { Key = true, Value = "Male" });
                    _pairModel.Add(new PairModel { Key = false, Value = "Female" });
                    return _pairModel;
                default:
                    return _pairModel;
            }

        }
    }
}