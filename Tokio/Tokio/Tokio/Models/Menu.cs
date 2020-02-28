using System;
using Xamarin.Forms;

namespace Tokio.Models
{
    public class Menu : BaseModel
    {
        public string Name { get; set; }
        public Command Command { get; set; }
        public string ImageUrl { get; set; }
        public UserType AccessedBy { get; set; }
        public double WidthView { get { return Device.Idiom == TargetIdiom.Tablet ? App.ScreenWidth/6 : App.ScreenWidth / 3; } }
        public double HeightView { get { return Device.Idiom == TargetIdiom.Tablet ? (App.ScreenWidth / 6) + 20 : (App.ScreenWidth / 3)+20; } }
        public bool IsVisible { get; set; }

        public Menu(string name,Action action,string imageUrl , UserType accessedBy , User user)
        {
            Name = name;
            Command = new Command(action);
            ImageUrl = imageUrl;
            AccessedBy = accessedBy;
            IsVisible = user.Type >= accessedBy;
        }
    }
}
