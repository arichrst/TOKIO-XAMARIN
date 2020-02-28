using System;
using System.Threading.Tasks;
using Xamanimation;
using Xamarin.Forms;

namespace Tokio.Services
{
    public static class Animator
    {
        public static void SetAnimation(this View control)
        {
            control.GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(async () => {
                    control.IsEnabled = false;
                    await control.Animate(new HeartAnimation()
                    {
                        Duration = 500.ToString(),
                        Easing = EasingType.Linear,
                        Target = control,
                    });
                    await Task.Delay(500);
                    control.IsEnabled = true;
                })
            });
        }

        public static async Task RunAnimation(this View control , int delay = 0)
        {
            await control.Animate(new HeartAnimation()
            {
                Duration = 1000.ToString(),
                Easing = EasingType.Linear,
                Delay = delay,
                Target = control,
            });
        }
    }
}
