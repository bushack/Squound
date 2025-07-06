namespace SquoundApp_v1.Models
{
    public class AboutModel
    {
        public string Image { get; set; }
        public string Headline { get; set; }
        public string SubHeadline { get; set; }
        public string Text { get; set; }

        public AboutModel()
        {
            // Default values.
            Image = "squound_logo.png";
            Headline = "Sorry!";
            SubHeadline = "It's not you - it's us";
            Text = "We can't find what we're looking for.";
        }
    }
}
