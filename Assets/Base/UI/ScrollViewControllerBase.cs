using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Private
{
    /// <summary>
    /// スクロールビュー制御ベース
    /// </summary>
    /// <typeparam name="T">スクロールビューボード</typeparam>
    [RequireComponent(typeof(ScrollRect))]
    public abstract class ScrollViewControllerBase<T> : MonoBehaviour where T : ScrollViewBoardControllerBase
    {
        /// <summary>
        /// 生成ボードプレハブ
        /// </summary>
        [SerializeField]
        private T m_CreateBoardPrefab;

		/// <summary>
		/// 一列の個数
		/// </summary>
		[SerializeField]
		private int m_OneLineNumber;

        /// <summary>
        /// ボードサイズ
        /// </summary>
        [SerializeField]
        private Vector2 m_BoardSize;

        /// <summary>
        /// 横に表示するボード数
        /// </summary>
        [SerializeField]
        protected int m_DisplayHorizontalBoardNumber;

        /// <summary>
        /// 縦に表示するボード数
        /// </summary>
        [SerializeField]
        protected int m_DisplayVerticalBoardNumber;

		/// <summary>
		/// 開始余白
		/// </summary>
		[SerializeField]
		protected float m_StartMargin;

		/// <summary>
		/// 終了余白
		/// </summary>
		[SerializeField]
		protected float m_EndMargin;

        /// <summary>
        /// ボードリスト
        /// </summary>
        protected List<T> m_BoardList = new List<T>();

		/// <summary>
		/// スクロールレクト
		/// </summary>
		protected ScrollRect m_ScrollRect;

		/// <summary>
		/// 最大インデックス
		/// </summary>
		protected int m_MaxIndex;

		/// <summary>
		/// 最大開始ライン
		/// </summary>
		protected int m_MaxStartLineNumber;

		private void Awake()
		{
			// スクロールレクト取得
			m_ScrollRect = this.GetComponent<ScrollRect> ();
		}

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="num">要素数</param>
        /// <param name="h_display_number">横に表示するボード数</param>
        /// <param name="v_display_number">縦に表示するボード数</param>
        public virtual void Initialize(int total_content_number, int h_display_number = 0, int v_display_number = 0)
        {
            // ボード数更新
            UpdateBoardNumber(h_display_number, v_display_number);

			// 加算列数算出
			int add_line_number = total_content_number % m_OneLineNumber == 0 ? 0 : 1;

			// 最終ライン数算出
			int last_line_number = (total_content_number / m_OneLineNumber) + add_line_number;

			// スクロールサイズ設定
			SetScrollContentsSizeFromLineNumber (last_line_number);

			// 最大インデックス数設定
			m_MaxIndex = total_content_number - 1;
        }

		/// <summary>
		/// ライン数からスクロールサイズ設定
		/// </summary>
		/// <param name="number">ライン数</param>
		protected abstract void SetScrollContentsSizeFromLineNumber (int number);

		/// <summary>
		/// インデックスからライン数算出
		/// </summary>
		/// <remarks>ライン数は0オリジン</remarks>
		/// <returns>ライン数</returns>
		/// <param name="index">インデックス</param>
		protected abstract int CalculateLineFromIndex (int index);

		/// <summary>
		/// 最大インデックスから最大開始ライン数算出
		/// </summary>
		/// <param name="max_index">最大インデックス</param>
		protected abstract void CalculateMaxStartLineNumberFromMaxIndex (int max_index);
        
		/// <summary>
        /// ボード数更新
        /// </summary>
        /// <param name="h_display_number">横に表示するボード数</param>
        /// <param name="v_display_number">縦に表示するボード数</param>
        private void UpdateBoardNumber(int h_display_number, int v_display_number)
        {
            // 横ボード数
            if (h_display_number > 0)
            {
                m_DisplayHorizontalBoardNumber = h_display_number;
            }

            // 縦ボード数
            if (v_display_number > 0)
            {
                m_DisplayVerticalBoardNumber = v_display_number;
            }

            // 現在ボード数との差を算出
            int diff = (m_DisplayHorizontalBoardNumber * m_DisplayVerticalBoardNumber) - m_BoardList.Count;
		
            if (diff > 0)
            {
                // 不足ボード生成
                for (int i = 0; diff > i; ++i)
                {
                    m_BoardList.Add(CreateBoard());
                }
            }
        }

        /// <summary>
        /// ボード生成
        /// </summary>
        /// <returns>生成ボード</returns>
        private T CreateBoard()
        {
            return Instantiate<T>(m_CreateBoardPrefab);
        }
    }
}
