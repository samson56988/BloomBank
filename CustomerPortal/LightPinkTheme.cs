using MudBlazor;

namespace CustomerPortal
{
    public class LightPinkTheme : MudTheme
    {
        public LightPinkTheme()
        {
            Palette = new PaletteLight
            {
                Black = "#000000",
                White = "#FFFFFF",
                Primary = "#FFC0CB", // Light pink (example: Pink)
                Secondary = "#FF69B4", // Deep pink (example: Hot Pink)
                Tertiary = "#FF1493", // Deep pink (example: Deep Pink)
                Success = "#00FFFF",
                Info = "#FFFF00",
                Warning = "#FF00FF",
                Error = "#C0C0C0",
                Dark = "#333333",
                Background = "#FFFFFF",
            };

            PaletteDark = new PaletteDark
            {
                Black = "#000000",
                White = "#FFFFFF",
                Primary = "#FFC0CB", // Light pink (example: Pink)
                Secondary = "#FF69B4", // Deep pink (example: Hot Pink)
                Tertiary = "#FF1493", // Deep pink (example: Deep Pink)
                Success = "#00FFFF",
                Info = "#FFFF00",
                Warning = "#FF00FF",
                Error = "#C0C0C0",
                Dark = "#303030",
                Background = "#303030",
            };
        }
    }

}
