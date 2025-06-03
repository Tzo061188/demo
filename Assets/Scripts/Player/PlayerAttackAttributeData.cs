
public class PlayerAttributeData{
  
    public bool isAttackInput;//攻击指令的输入

    public bool isCanSprint = true;//是否可以冲刺
    
    public bool isCanMoveInterrupt; //可以移动中断
    public bool isAttackExecute; //进行攻击执行
    public bool isCanAttack; //可以进行下次攻击
    public AttackContentData currentAttackMode;//当前的一个攻击形式
    public int currentAttackIndex;//当前攻击形式进行的下标
}