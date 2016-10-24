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
		private int m_LineNumber;

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
        public virtual void Initialize(int num, int h_display_number = 0, int v_display_number = 0)
        {
            // ボード数更新
            UpdateBoardNumber(h_display_number, v_display_number);

			// 加算列数算出
			int add_line_number = num % m_LineNumber == 0 ? 0 : 1;

			SetScrollContentsSize(
        }

		/// <summary>
		/// スクロールサイズ
		/// </summary>
		/// <param name="size">Size.</param>
		protected abstract void SetScrollContentsSize (float size);
        
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
