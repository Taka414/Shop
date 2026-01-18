//
// (C) 2022 Takap.
//

namespace Takap.Utility
{
    /// <summary>
    /// 指定範囲内でローテーションする数字を管理するクラス
    /// </summary>
    public class RotatableIncrimentNumber
    {
        int _max;
        int _min;
        int _current;

        public RotatableIncrimentNumber(int min, int max)
        {
            _min = min;
            _max = max;
            _current = _min;
        }

        /// <summary>
        /// 範囲内の値をひとつ取得します。
        /// </summary>
        public int Publish()
        {
            if (_current > _max)
            {
                _current = _min;
            }
            return _current++;
        }
    }
}
