namespace SquoundApp_v1.Models
{
    public class AboutUsModel
    {
        public string Image { get; set; }
        public string Headline { get; set; }
        public string SubHeadline { get; set; }
        public string Text { get; set; }

        public AboutUsModel()
        {
            // Default values.
            Image = "squound_logo.png";
            Headline = "Oops!";
            SubHeadline = "";
            Text = "Don't you just hate it when you can't find what you're looking for?";
        }
    }
}
