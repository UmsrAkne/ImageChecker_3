namespace ImageChecker_3.Models
{
    public enum TagType
    {
        /// <summary>
        /// タイプなし（デフォルト）
        /// </summary>
        NoType = 0,

        /// <summary>
        /// Image タグ
        /// </summary>
        Image,

        /// <summary>
        /// Draw タグ
        /// </summary>
        Draw,

        /// <summary>
        /// AnimationImage タグ
        /// </summary>
        AnimationImage,

        /// <summary>
        /// AnimationDraw タグ
        /// </summary>
        AnimationDraw,
    }
}