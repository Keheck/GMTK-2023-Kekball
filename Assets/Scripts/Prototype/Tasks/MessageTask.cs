public class MessageTask: Task {

    string message;

    public MessageTask(string m): base(0,0) {
        message = m;
    }

    public override string GetDescription() {
        return message;
    }

    public override bool IsSatisfied() {
        return false;
    }

    public override bool IsViolated() {
        return false;
    }
}