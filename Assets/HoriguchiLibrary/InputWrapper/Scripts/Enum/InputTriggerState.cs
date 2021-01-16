namespace Horiguchi.InputWrapper
{
    /// <summary>
    /// 入力ステート
    /// </summary>
    public enum InputTriggerState
    {
        Idle = 0b000,
        Released = 0b001,
        HoldingDown = 0b010,
        Pressed = 0b011,
        //          = 0b100,
        Clicked = 0b101,
    }
}
