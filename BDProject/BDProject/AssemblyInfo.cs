using Android.App;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

[assembly: ExportFont("FABrands.otf", Alias = "FA-B")]
[assembly: ExportFont("FARegular.otf", Alias = "FA-R")]
[assembly: ExportFont("FASolid.otf", Alias = "FA-S")]

[assembly: UsesFeature("android.hardware.camera", Required = false)]
[assembly: UsesFeature("android.hardware.camera.autofocus", Required = false)]