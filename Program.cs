namespace Tips{
    /// <summary>
    /// 定义一个委托
    /// </summary>
    /// <param name="message"></param>
    public delegate void Mydelegate (string message);
    public class Program{
        /// <summary>
        /// 定义一个适合委托的方法
        /// </summary>
        /// <param name="message"></param>
        public static void Dmeth (string message){
            Console.WriteLine (message);
        }
        public static void Dmeth1 (string message){
            Console.WriteLine (message+"111");
        }
        public static void Main(string[] args){
            //实例化委托对象
            Mydelegate mydelegate = Dmeth;
            mydelegate("hanima");
            mydelegate += Dmeth1;
            mydelegate("1");
        }
    }
}
