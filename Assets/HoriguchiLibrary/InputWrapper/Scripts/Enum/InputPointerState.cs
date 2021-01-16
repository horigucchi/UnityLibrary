namespace Horiguchi.InputWrapper
{
    /// <summary>
    /// 入力ステート
    /// </summary>
    public enum InputPointerState
    {
        Idle = 0b000,
        Stopped = 0b001,
        Moving = 0b010,
        Started = 0b011,
    }
}
