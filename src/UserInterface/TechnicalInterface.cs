namespace CSSkinScrapper.UserInterface
{
    public abstract class TechnicalInterface
    {
        protected int IntUiSelector(int max)
        {
            var input = Console.ReadKey();
            string key = "";
            key += input.KeyChar;
            bool success = int.TryParse(key, out int value);

            if (!success | value < 0 | value > max)
            {
                Console.WriteLine(" Enter one of the numbers above:");
                return IntUiSelector(max);
            }

            return value;
        }

        protected bool BoolUiSelector()
        {
            Console.WriteLine(" Type [y] for yes or [n] for no.");
            var rawinput = Console.ReadKey();
            string input = "";
            input += rawinput.KeyChar;

            if (input != "y" && input != "n")
            {
                return BoolUiSelector();
            }
            else
            {
                if (input == "y")
                    return true;
                else
                    return false;
            }
        }
    }
}