namespace GameCore.Creatures
{
    public enum EThinkingResult
    {
        NORMAL,
        /// <summary>
        /// Существо самоуничтожилось
        /// </summary>
        SHOULD_BE_REMOVED_FROM_QUEUE,
    }
}