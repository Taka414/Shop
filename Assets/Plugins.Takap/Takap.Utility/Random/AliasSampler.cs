// 
// (C) 2022 Takap.
//

using System.Collections.Generic;

namespace Takap.Utility
{
    /// <summary>
    /// 重みづけに基づいた抽選(復元抽出)をインデックスペースで行います。
    /// </summary>
    public class AliasSampler
    {
        //
        // Fields
        // - - - - - - - - - - - - - - - - - - - -

        private readonly List<float> _weights = new List<float>();
        private float _totalWeight;
        private float[] _normalized;
        private int[] _aliases;

        //
        // Props
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 現在抽選対象にしているオブジェクトの個数を取得します。
        /// </summary>
        public int Count => _weights.Count;

        /// <summary>
        /// 合計の重みづけ量を取得します。
        /// </summary>
        public float TotalWeight => _totalWeight;

        //
        // Constructors
        // - - - - - - - - - - - - - - - - - - - -

        public AliasSampler()
        {
            // nop
        }

        /// <summary>
        /// 抽選条件を指定してオブジェクトを初期化します。
        /// </summary>
        public AliasSampler(IEnumerable<float> weights) => AddWeights(weights);

        //
        // Public Methods
        // - - - - - - - - - - - - - - - - - - - -

        /// <summary>
        /// 指定した要素の重みを取得します。
        /// </summary>
        public float GetWeight(int index)
        {
            return _weights[index];
        }

        /// <summary>
        /// 条件をひとつだけ追加します。
        /// </summary>
        public void AddWeight(float weight, bool skipRefresh = false)
        {
            addWeightInner(weight);

            if (!skipRefresh)
            {
                Refresh();
            }
        }

        /// <summary>
        /// 指定したリストに従って重みづけの条件を設定します。
        /// </summary>
        /// <remarks>
        /// 
        /// 例えば以下のように指定してSample()を実行すると
        /// 0=10%, 1=30%, 2=60%の確率で抽選したインデックスが取得できるようになる
        /// AliasSampler engine = new AliasSampler();
        /// var list = new List<float>()
        /// {
        ///     0.1f, // 10%
        ///     0.3f, // 30%
        ///     0.6f, // 60%
        /// };
        /// engine.AddWeights(list);
        /// 
        /// </remarks>
        public void AddWeights(IEnumerable<float> weights, bool skipRefresh = false)
        {
            if (weights is ICollection<float> collection)
            {
                // 容量更新
                if (_weights.Capacity < _weights.Count + collection.Count)
                {
                    _weights.Capacity = _weights.Count + collection.Count;
                }
            }

            foreach (float weight in weights)
            {
                addWeightInner(weight);
            }

            if (!skipRefresh)
            {
                Refresh();
            }
        }

        /// <summary>
        /// 現在管理している条件をすべてクリアします。
        /// </summary>
        public void Clear()
        {
            Clear();
            _normalized = null;
            _aliases = null;
        }

        /// <summary>
        /// O(N)で内部リスト構築します。
        /// </summary>
        public void Refresh()
        {
            int length = Count;

            _normalized = new float[length];
            _aliases = new int[length];
            int[] indexes = new int[length];

            // 重みの合計を算出
            float _totalWeight = 0.0f;
            for (int i = 0; i < length; ++i)
            {
                float weight = GetWeight(i);
                if (weight > 0.0f)
                {
                    _totalWeight += weight;
                }
            }

            float normalizeRatio = length / _totalWeight;

            int left = -1;
            int right = length;

            for (int i = 0; i < length; ++i)
            {
                // エイリアス初期化
                _aliases[i] = i;

                // 重みを要素数で正規化
                float weight = GetWeight(i);
                if (weight > 0.0f)
                {
                    weight *= normalizeRatio;
                }
                else
                {
                    weight = 0.0f;
                }
                _normalized[i] = weight;

                // 重みが1ブロックに収まるかどうかで前方・後方に振り分け
                if (weight < 1.0f)
                {
                    indexes[++left] = i;
                }
                else
                {
                    indexes[--right] = i;
                }
            }

            // 少なくとも1つは重みが1でなければエイリアス設定
            if (left >= 0 && right < length)
            {
                left = 0;
                while (left < length && right < length)
                {
                    int leftIndex = indexes[left];
                    int rightIndex = indexes[right];

                    // エイリアス設定
                    _aliases[leftIndex] = rightIndex;

                    // rightIndexに紐づく重みからleftIndex分を減算
                    float leftWeight = _normalized[leftIndex];
                    float rightWeight = _normalized[rightIndex] + leftWeight - 1.0f;
                    _normalized[rightIndex] = rightWeight;

                    // 重みが1未満になったら後方リストの先頭を後ろにずらす
                    if (rightWeight < 1.0f)
                    {
                        ++right;
                    }

                    ++left;
                }
            }
        }

        /// <summary>
        /// O(1)で復元抽出を行います。
        /// </summary>
        public int Sample()
        {
            int count = Count;
            if (count == 0)
                return -1;

            float random = nextRandom() * count;

            int index = (int)random;
            float weight = random - index;

            // 末尾に丸める
            if (index >= count)
            {
                index = count - 1;
                weight = 1.0f;
            }

            // 重みを超えたらエイリアスに差し替える
            if (_normalized[index] <= weight)
            {
                index = _aliases[index];
            }

            return index;
        }

        //
        // Other Methods
        // - - - - - - - - - - - - - - - - - - - -

        //private readonly Random r = new Random();
        //private float nextRandom() => this.r.Next(0, 100000) / 100000f;
        private float nextRandom() => UniRandom.value;

        private float addWeightInner(float weight)
        {
            if (weight > 0.0f)
            {
                _weights.Add(weight);
                _totalWeight += weight;
                return weight;
            }

            _weights.Add(0.0f);
            return 0.0f;
        }
    }
}
